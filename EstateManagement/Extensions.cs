namespace EstateManagement;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.EventStore.EventHandling;
using Shared.EventStore.SubscriptionWorker;
using Shared.General;
using Shared.Logger;

[ExcludeFromCodeCoverage]
/// <summary>
/// 
/// </summary>
public static class Extensions
{
    public static IServiceCollection AddInSecureEventStoreClient(
        this IServiceCollection services,
        Uri address,
        Func<HttpMessageHandler>? createHttpMessageHandler = null)
    {
        return services.AddEventStoreClient((Action<EventStoreClientSettings>)(options =>
                                                                               {
                                                                                   options.ConnectivitySettings.Address = address;
                                                                                   options.ConnectivitySettings.Insecure = true;
                                                                                   options.CreateHttpMessageHandler = createHttpMessageHandler;
                                                                               }));
    }

    static Action<TraceEventType, String, String> log = (tt, subType, message) => {
                                                            String logMessage = $"{subType} - {message}";
                                                            switch (tt)
                                                            {
                                                                case TraceEventType.Critical:
                                                                    Logger.LogCritical(new Exception(logMessage));
                                                                    break;
                                                                case TraceEventType.Error:
                                                                    Logger.LogError(new Exception(logMessage));
                                                                    break;
                                                                case TraceEventType.Warning:
                                                                    Logger.LogWarning(logMessage);
                                                                    break;
                                                                case TraceEventType.Information:
                                                                    Logger.LogInformation(logMessage);
                                                                    break;
                                                                case TraceEventType.Verbose:
                                                                    Logger.LogDebug(logMessage);
                                                                    break;
                                                            }
                                                        };

    static Action<TraceEventType, String> concurrentLog = (tt, message) => Extensions.log(tt, "CONCURRENT", message);
    static Action<TraceEventType, String> orderedLog = (tt, message) => Extensions.log(tt, "ORDERED", message);

    public static void PreWarm(this IApplicationBuilder applicationBuilder)
    {
        Startup.LoadTypes();

        var internalSubscriptionService = Boolean.Parse(ConfigurationReader.GetValue("InternalSubscriptionService"));

        if (internalSubscriptionService)
        {
            IConfigurationSection subscriptionConfigSection = Startup.Configuration.GetSection("AppSettings:SubscriptionConfig");
            SubscriptionConfigRoot subscriptionConfigRoot = new SubscriptionConfigRoot();
            subscriptionConfigSection.Bind(subscriptionConfigRoot);

            String eventStoreConnectionString = ConfigurationReader.GetValue("EventStoreSettings", "ConnectionString");
            Int32 cacheDuration = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "InternalSubscriptionServiceCacheDuration"));

            ISubscriptionRepository subscriptionRepository = SubscriptionRepository.Create(eventStoreConnectionString, cacheDuration);
            ((SubscriptionRepository)subscriptionRepository).Trace += (sender,
                                                                       s) => Extensions.log(TraceEventType.Information, "REPOSITORY", s);

            // init our SubscriptionRepository
            subscriptionRepository.PreWarm(CancellationToken.None).Wait();

            if (subscriptionConfigRoot.Concurrent.IsEnabled)
            {
                SubscriptionWorker concurrentSubscriptions = ConfigureConcurrentSubscriptions(subscriptionRepository, subscriptionConfigRoot.Concurrent);
                concurrentSubscriptions.StartAsync(CancellationToken.None).Wait();
            }

            if (subscriptionConfigRoot.Ordered.IsEnabled)
            {
                SubscriptionWorker orderedSubscriptions = ConfigureOrderedSubscriptions(subscriptionRepository, subscriptionConfigRoot.Ordered);
                orderedSubscriptions.StartAsync(CancellationToken.None).Wait();
            }
        }
    }

    private static SubscriptionWorker ConfigureConcurrentSubscriptions(ISubscriptionRepository subscriptionRepository, SubscriptionConfig concurrent)
    {
        IDomainEventHandlerResolver eventHandlerResolver = Startup.Container.GetInstance<IDomainEventHandlerResolver>("Concurrent");

        Int32 inflightMessages = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "InflightMessages"));
        Int32 persistentSubscriptionPollingInSeconds = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "PersistentSubscriptionPollingInSeconds"));

        SubscriptionWorker concurrentSubscriptions = SubscriptionWorker.CreateConcurrentSubscriptionWorker(Startup.EventStoreClientSettings, eventHandlerResolver, subscriptionRepository,
                                                                                                           inflightMessages, persistentSubscriptionPollingInSeconds);

        concurrentSubscriptions.Trace += (_, args) => Extensions.concurrentLog(TraceEventType.Information, args.Message);
        concurrentSubscriptions.Warning += (_, args) => Extensions.concurrentLog(TraceEventType.Warning, args.Message);
        concurrentSubscriptions.Error += (_, args) => Extensions.concurrentLog(TraceEventType.Error, args.Message);

        if (!String.IsNullOrEmpty(concurrent.Ignore))
        {
            concurrentSubscriptions = concurrentSubscriptions.IgnoreSubscriptions(concurrent.Ignore);
        }

        if (!String.IsNullOrEmpty(concurrent.Filter))
        {
            //NOTE: Not overly happy with this design, but;
            //the idea is if we supply a filter, this overrides ignore
            concurrentSubscriptions = concurrentSubscriptions.FilterSubscriptions(concurrent.Filter);
            //.IgnoreSubscriptions(null);

        }

        if (!String.IsNullOrEmpty(concurrent.StreamName))
        {
            concurrentSubscriptions = concurrentSubscriptions.FilterByStreamName(concurrent.StreamName);
        }

        return concurrentSubscriptions;
    }

    private static SubscriptionWorker ConfigureOrderedSubscriptions(ISubscriptionRepository subscriptionRepository, SubscriptionConfig ordered)
    {
        IDomainEventHandlerResolver eventHandlerResolver = Startup.Container.GetInstance<IDomainEventHandlerResolver>("Ordered");

        Int32 persistentSubscriptionPollingInSeconds = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "PersistentSubscriptionPollingInSeconds"));

        SubscriptionWorker orderedSubscriptions =
            SubscriptionWorker.CreateOrderedSubscriptionWorker(Startup.EventStoreClientSettings,
                                                               eventHandlerResolver,
                                                               subscriptionRepository,
                                                               persistentSubscriptionPollingInSeconds);

        orderedSubscriptions.Trace += (_, args) => Extensions.orderedLog(TraceEventType.Information, args.Message);
        orderedSubscriptions.Warning += (_, args) => Extensions.orderedLog(TraceEventType.Warning, args.Message);
        orderedSubscriptions.Error += (_, args) => Extensions.orderedLog(TraceEventType.Error, args.Message);

        if (!String.IsNullOrEmpty(ordered.Ignore))
        {
            orderedSubscriptions = orderedSubscriptions.IgnoreSubscriptions(ordered.Ignore);
        }

        if (!String.IsNullOrEmpty(ordered.Filter))
        {
            //NOTE: Not overly happy with this design, but;
            //the idea is if we supply a filter, this overrides ignore
            orderedSubscriptions = orderedSubscriptions.FilterSubscriptions(ordered.Filter)
                                                       .IgnoreSubscriptions(null);

        }

        if (!String.IsNullOrEmpty(ordered.StreamName))
        {
            orderedSubscriptions = orderedSubscriptions.FilterByStreamName(ordered.StreamName);
        }

        return orderedSubscriptions;
    }
}

public class SubscriptionConfigRoot
{
    public SubscriptionConfig Ordered { get; set; }
    public SubscriptionConfig Concurrent { get; set; }
}

public class SubscriptionConfig
{
    public Boolean IsEnabled { get; set; }
    public String Filter { get; set; }
    public String Ignore { get; set; }
    public String StreamName { get; set; }
}
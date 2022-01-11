namespace EstateManagement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Abstractions;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using BusinessLogic.Common;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Events;
    using BusinessLogic.Manger;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using Contract.DomainEvents;
    using Estate.DomainEvents;
    using EstateReporting.Database;
    using MediatR;
    using Merchant.DomainEvents;
    using MerchantStatement.DomainEvents;
    using MessagingService.Client;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;
    using Models.Factories;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Repository;
    using SecurityService.Client;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EntityFramework;
    using Shared.EntityFramework.ConnectionStringConfiguration;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using Shared.EventStore.EventStore;
    using Shared.EventStore.Extensions;
    using Shared.EventStore.SubscriptionWorker;
    using Shared.General;
    using Shared.Logger;
    using Shared.Repositories;
    using Swashbuckle.AspNetCore.Filters;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        static Action<TraceEventType, String, String> log = (tt,
                                                             subType,
                                                             message) =>
                                                            {
                                                                String logMessage = $"{subType} - {message}";
                                                                switch(tt)
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

        static Action<TraceEventType, String> concurrentLog = (tt,
                                                               message) => Extensions.log(tt, "CONCURRENT", message);

        public static void PreWarm(this IApplicationBuilder applicationBuilder)
        {
            Startup.LoadTypes();

            var internalSubscriptionService = Boolean.Parse(ConfigurationReader.GetValue("InternalSubscriptionService"));

            if (internalSubscriptionService)
            {
                String eventStoreConnectionString = ConfigurationReader.GetValue("EventStoreSettings", "ConnectionString");
                Int32 inflightMessages = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "InflightMessages"));
                Int32 persistentSubscriptionPollingInSeconds = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "PersistentSubscriptionPollingInSeconds"));
                String filter = ConfigurationReader.GetValue("AppSettings", "InternalSubscriptionServiceFilter");
                String ignore = ConfigurationReader.GetValue("AppSettings", "InternalSubscriptionServiceIgnore");
                String streamName = ConfigurationReader.GetValue("AppSettings", "InternalSubscriptionFilterOnStreamName");
                Int32 cacheDuration = Int32.Parse(ConfigurationReader.GetValue("AppSettings", "InternalSubscriptionServiceCacheDuration"));

                ISubscriptionRepository subscriptionRepository = SubscriptionRepository.Create(eventStoreConnectionString, cacheDuration);

                ((SubscriptionRepository)subscriptionRepository).Trace += (sender,
                                                                           s) => Extensions.log(TraceEventType.Information, "REPOSITORY", s);

                // init our SubscriptionRepository
                subscriptionRepository.PreWarm(CancellationToken.None).Wait();

                var eventHandlerResolver = Startup.ServiceProvider.GetService<IDomainEventHandlerResolver>();

                SubscriptionWorker concurrentSubscriptions =
                    SubscriptionWorker.CreateConcurrentSubscriptionWorker(eventStoreConnectionString,
                                                                          eventHandlerResolver,
                                                                          subscriptionRepository,
                                                                          inflightMessages,
                                                                          persistentSubscriptionPollingInSeconds);

                concurrentSubscriptions.Trace += (_,
                                                  args) => Extensions.concurrentLog(TraceEventType.Information, args.Message);
                concurrentSubscriptions.Warning += (_,
                                                    args) => Extensions.concurrentLog(TraceEventType.Warning, args.Message);
                concurrentSubscriptions.Error += (_,
                                                  args) => Extensions.concurrentLog(TraceEventType.Error, args.Message);

                if (!String.IsNullOrEmpty(ignore))
                {
                    concurrentSubscriptions = concurrentSubscriptions.IgnoreSubscriptions(ignore);
                }

                if (!String.IsNullOrEmpty(filter))
                {
                    //NOTE: Not overly happy with this design, but
                    //the idea is if we supply a filter, this overrides ignore
                    concurrentSubscriptions = concurrentSubscriptions.FilterSubscriptions(filter).IgnoreSubscriptions(null);

                }

                if (!String.IsNullOrEmpty(streamName))
                {
                    concurrentSubscriptions = concurrentSubscriptions.FilterByStreamName(streamName);
                }

                concurrentSubscriptions.StartAsync(CancellationToken.None).Wait();
            }
        }
    }
}
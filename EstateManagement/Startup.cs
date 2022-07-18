namespace EstateManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Security;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Bootstrapper;
    using BusinessLogic.Common;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Events;
    using BusinessLogic.Manger;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using Common;
    using Contract.DomainEvents;
    using Controllers;
    using Estate.DomainEvents;
    using EstateReporting.Database;
    using EventStore.Client;
    using HealthChecks.UI.Client;
    using Lamar;
    using MediatR;
    using Merchant.DomainEvents;
    using MerchantStatement.DomainEvents;
    using MessagingService.Client;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Microsoft.Win32;
    using Models.Factories;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Repository;
    using SecurityService.Client;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EntityFramework;
    using Shared.EntityFramework.ConnectionStringConfiguration;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using Shared.EventStore.EventStore;
    using Shared.EventStore.Extensions;
    using Shared.EventStore.SubscriptionWorker;
    using Shared.Extensions;
    using Shared.General;
    using Shared.Logger;
    using Shared.Repositories;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using AuthenticationFailedContext = Microsoft.AspNetCore.Authentication.JwtBearer.AuthenticationFailedContext;
    using ConnectionStringType = Shared.Repositories.ConnectionStringType;
    using ILogger = Microsoft.Extensions.Logging.ILogger;
    using TokenValidatedContext = Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="webHostEnvironment">The web host environment.</param>
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
                                                                      .AddJsonFile("/home/txnproc/config/appsettings.json", true, true)
                                                                      .AddJsonFile($"/home/txnproc/config/appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true)
                                                                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                      .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                                                                      .AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the web host environment.
        /// </summary>
        /// <value>
        /// The web host environment.
        /// </value>
        public static IWebHostEnvironment WebHostEnvironment { get; set; }

        public static EventStoreClientSettings EventStoreClientSettings;

        public static IServiceProvider ServiceProvider { get; set; }

        public static void ConfigureEventStoreSettings(EventStoreClientSettings settings = null)
        {
            if (settings == null)
            {
                settings = new EventStoreClientSettings();
            }

            if (Startup.EventStoreClientSettings != null) {
                settings = Startup.EventStoreClientSettings;
                return;
            }
            
            settings.ConnectivitySettings = EventStoreClientConnectivitySettings.Default;
            settings.ConnectivitySettings.Address = new Uri(Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString"));
            settings.ConnectivitySettings.Insecure = Startup.Configuration.GetValue<Boolean>("EventStoreSettings:Insecure");


            settings.DefaultCredentials = new UserCredentials(Startup.Configuration.GetValue<String>("EventStoreSettings:UserName"),
                                                              Startup.Configuration.GetValue<String>("EventStoreSettings:Password"));
            Startup.EventStoreClientSettings = settings;
        }

        public static Container Container;

        public void ConfigureContainer(ServiceRegistry services)
        {
            ConfigurationReader.Initialise(Startup.Configuration);

            Startup.ConfigureEventStoreSettings();

            services.IncludeRegistry<MiddlewareRegistry>();
            services.IncludeRegistry<MediatorRegistry>();
            services.IncludeRegistry<RepositoryRegistry>();
            services.IncludeRegistry<DomainServiceRegistry>();
            services.IncludeRegistry<ClientsRegistry>();
            services.IncludeRegistry<MiscRegistry>();
            services.IncludeRegistry<EventHandlerRegistry>();

            Startup.LoadTypes();

            Startup.Container = new Container(services);
        }
       

        public static void LoadTypes()
        {
            ContractCreatedEvent c = new ContractCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "");
            MerchantCreatedEvent m = new MerchantCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), "", DateTime.Now);
            EstateCreatedEvent e = new EstateCreatedEvent(Guid.NewGuid(), "");
            CallbackReceivedEnrichedEvent ce = new CallbackReceivedEnrichedEvent(Guid.NewGuid());
            TransactionHasBeenCompletedEvent t =
                new TransactionHasBeenCompletedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "", "", true, DateTime.MinValue, null);
            MerchantFeeSettledEvent f = new MerchantFeeSettledEvent(Guid.NewGuid(),
                                                                    Guid.NewGuid(),
                                                                    Guid.NewGuid(),
                                                                    Guid.NewGuid(),
                                                                    0,
                                                                    0,
                                                                    Guid.NewGuid(),
                                                                    0,
                                                                    DateTime.MinValue);
            StatementCreatedEvent s = new StatementCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.MinValue);
            TypeProvider.LoadDomainEventsTypeDynamically();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="provider">The provider.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            String nlogConfigFilename = "nlog.config";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.ConfigureNLog(Path.Combine(env.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("EstateManagement");

            Logger.Initialise(logger);

            Action<String> loggerAction = message =>
                                          {
                                              Logger.LogInformation(message);
                                          };
            Startup.Configuration.LogConfiguration(loggerAction);

            ConfigurationReader.Initialise(Startup.Configuration);

            app.AddRequestLogging();
            app.AddResponseLogging();
            app.AddExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                                 endpoints.MapHealthChecks("health", new HealthCheckOptions()
                                                                      {
                                                                          Predicate = _ => true,
                                                                          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                                                      });
                             });
            app.UseSwagger();

            app.UseSwaggerUI();

            app.PreWarm();
        }

    }
    
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

        static Action<TraceEventType, String> concurrentLog = (tt, message) => log(tt, "CONCURRENT", message);

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

                ((SubscriptionRepository)subscriptionRepository).Trace += (sender, s) => Extensions.log(TraceEventType.Information, "REPOSITORY", s);

                // init our SubscriptionRepository
                subscriptionRepository.PreWarm(CancellationToken.None).Wait();

                var eventHandlerResolver = applicationBuilder.ApplicationServices.GetService<IDomainEventHandlerResolver>();

                SubscriptionWorker concurrentSubscriptions = SubscriptionWorker.CreateConcurrentSubscriptionWorker(Startup.EventStoreClientSettings, eventHandlerResolver, subscriptionRepository, inflightMessages, persistentSubscriptionPollingInSeconds);

                concurrentSubscriptions.Trace += (_, args) => concurrentLog(TraceEventType.Information, args.Message);
                concurrentSubscriptions.Warning += (_, args) => concurrentLog(TraceEventType.Warning, args.Message);
                concurrentSubscriptions.Error += (_, args) => concurrentLog(TraceEventType.Error, args.Message);

                if (!String.IsNullOrEmpty(ignore))
                {
                    concurrentSubscriptions = concurrentSubscriptions.IgnoreSubscriptions(ignore);
                }

                if (!String.IsNullOrEmpty(filter))
                {
                    //NOTE: Not overly happy with this design, but
                    //the idea is if we supply a filter, this overrides ignore
                    concurrentSubscriptions = concurrentSubscriptions.FilterSubscriptions(filter)
                                                                     .IgnoreSubscriptions(null);

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

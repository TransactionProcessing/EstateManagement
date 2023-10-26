namespace EstateManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Bootstrapper;
    using BusinessLogic.Events;
    using Contract.DomainEvents;
    using Estate.DomainEvents;
    using EventStore.Client;
    using FileProcessor.File.DomainEvents;
    using FileProcessor.FileImportLog.DomainEvents;
    using HealthChecks.UI.Client;
    using Lamar;
    using Merchant.DomainEvents;
    using MerchantStatement.DomainEvents;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using Shared.EventStore.Aggregate;
    using Shared.Extensions;
    using Shared.General;
    using Shared.Logger;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

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

        public static void ConfigureEventStoreSettings(EventStoreClientSettings settings)
        {
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

            services.IncludeRegistry<MediatorRegistry>();
            services.IncludeRegistry<RepositoryRegistry>();
            services.IncludeRegistry<MiddlewareRegistry>();
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
                                                                    DateTime.MinValue,
                                                                    DateTime.MinValue);
            StatementCreatedEvent s = new StatementCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.MinValue);
            VoucherGeneratedEvent g = new VoucherGeneratedEvent(Guid.NewGuid(),
                                                                Guid.NewGuid(),
                                                                Guid.NewGuid(),
                                                                DateTime.Now,
                                                                String.Empty,
                                                                0,
                                                                String.Empty,
                                                                DateTime.Now,
                                                                String.Empty);
            ImportLogCreatedEvent i = new ImportLogCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            FileCreatedEvent file = new FileCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), String.Empty, DateTime.Now);

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
                                                                          ResponseWriter = Shared.HealthChecks.HealthCheckMiddleware.WriteResponse
                                                                      });
                                 endpoints.MapHealthChecks("healthui", new HealthCheckOptions()
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
}

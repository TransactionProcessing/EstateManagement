namespace EstateManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using BusinessLogic.CommandHandlers;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Manger;
    using BusinessLogic.Services;
    using Common;
    using EventStore.ClientAPI;
    using Lamar;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models.Factories;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.Extensions;
    using Shared.General;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                                      .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public static IContainer Container { get; set; }

        public static IConfigurationRoot Configuration { get; set; }

        public static IWebHostEnvironment WebHostEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            this.ConfigureMiddlewareServices(services);

            services.AddControllers().AddNewtonsoftJson(options =>
                                                        {
                                                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                            options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                                                            options.SerializerSettings.Formatting = Formatting.Indented;
                                                            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                                        });

            Startup.GetConfiguredContainer(services, Startup.WebHostEnvironment);
        }

        private void ConfigureMiddlewareServices(IServiceCollection services)
        {
            services.AddApiVersioning(
                                      options =>
                                      {
                                          // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                                          options.ReportApiVersions = true;
                                          options.DefaultApiVersion = new ApiVersion(1, 0);
                                          options.AssumeDefaultVersionWhenUnspecified = true;
                                          options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                                      });

            services.AddVersionedApiExplorer(
                                             options =>
                                             {
                                                 // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                                                 // note: the specified format code will format the version as "'v'major[.minor][-status]"
                                                 options.GroupNameFormat = "'v'VVV";

                                                 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                                                 // can also be used to control the format of the API version in route templates
                                                 options.SubstituteApiVersionInUrl = true;
                                             });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();
                c.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
                              IApiVersionDescriptionProvider provider)
        {
            String nlogConfigFilename = "nlog.config";

            if (env.IsDevelopment())
            {
                nlogConfigFilename = $"nlog.{env.EnvironmentName}.config";
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.ConfigureNLog(Path.Combine(env.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("EstateManagement");

            Logger.Initialise(logger);

            ConfigurationReader.Initialise(Startup.Configuration);

            app.AddRequestLogging();
            app.AddResponseLogging();
            app.AddExceptionHandler();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(
                             options =>
                             {
                                 // build a swagger endpoint for each discovered API version
                                 foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                                 {
                                     options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                                 }
                             });

            if (String.Compare(ConfigurationReader.GetValue("EventStoreSettings", "START_PROJECTIONS"),
                               Boolean.TrueString,
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                app.PreWarm(true).Wait();
            }
            else
            {
                app.PreWarm();
            }
        }

        public static IContainer GetConfiguredContainer(ServiceRegistry services,
                                                        IWebHostEnvironment webHostEnvironment)
        {
            Container container = new Container(services);

            Dictionary<String, String[]> handlerEventTypesToSilentlyHandle = new Dictionary<String, String[]>();

            if (Startup.Configuration != null)
            {
                IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle");

                if (section != null)
                {
                    Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle").Bind(handlerEventTypesToSilentlyHandle);
                }
            }

            DomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle = new DomainEventTypesToSilentlyHandle(handlerEventTypesToSilentlyHandle);

            //Can we create a static method in this class that returns IContainer?
            services.AddSingleton<IDomainEventTypesToSilentlyHandle>(eventTypesToSilentlyHandle);

            services.IncludeRegistry<CommonRegistry>();

            container.Configure(services);

            Startup.Container = container;

            return container;
        }
    }

    [ExcludeFromCodeCoverage]
    public class CommonRegistry : ServiceRegistry
    {
        public CommonRegistry()
        {
            String connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");
            String connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            Int32 httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            this.For<IEventStoreContext>().Use<EventStoreContext>().Singleton().Ctor<EventStoreConnectionSettings>().Is(settings);

            //Func<String, IEventStoreContext> eventStoreContextFunc = (connectionString) =>
            //                                                         {
            //                                                             EventStoreConnectionSettings connectionSettings = EventStoreConnectionSettings.Create(connectionString, connectionName, httpPort);
                                                                         
            //                                                             ExplicitArguments args = new ExplicitArguments().Set(connectionSettings);
                                                                         
            //                                                             return Startup.Container.GetInstance<IEventStoreContext>();
            //                                                         };

            Func<EventStoreConnectionSettings, IEventStoreConnection> eventStoreConnectionFunc = (connectionSettings) =>
                                                                                                 {
                                                                                                     return EventStoreConnection.Create(connectionSettings.ConnectionString);
                                                                                                 };

            this.For<Func<EventStoreConnectionSettings, IEventStoreConnection>>().Use(eventStoreConnectionFunc);

            //this.For<ESLogger.ILogger>().Use<ESLogger.Common.Log.ConsoleLogger>().Singleton();
            this.For<ICommandRouter>().Use<CommandRouter>().Singleton();
            this.For<IAggregateRepository<EstateAggregate.EstateAggregate>>()
                .Use<AggregateRepository<EstateAggregate.EstateAggregate>>().Singleton();
            this.For<IAggregateRepository<MerchantAggregate.MerchantAggregate>>()
                .Use<AggregateRepository<MerchantAggregate.MerchantAggregate>>().Singleton();
            this.For<IMerchantDomainService>().Use<MerchantDomainService>().Singleton();
            this.For<IEstateManagementManager>().Use<EstateManagementManager>();
            this.For<IModelFactory>().Use<ModelFactory>();
            this.For<Factories.IModelFactory>().Use<Factories.ModelFactory>();
        }
    }

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    public static class StartupExtensions
    {
        #region Methods

        /// <summary>
        /// Pres the warm.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="startProjections">if set to <c>true</c> [start projections].</param>
        /// <returns></returns>
        public static async Task PreWarm(this IApplicationBuilder applicationBuilder,
                                         Boolean startProjections = false)
        {
            try
            {
                if (startProjections)
                {
                    await StartupExtensions.StartEventStoreProjections();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Starts the event store projections.
        /// </summary>
        /// <returns></returns>
        private static async Task StartEventStoreProjections()
        {
            // TODO: Refactor
            // This method is a brutal way of getting projections to be run when the API starts up
            // This needs refactored into a more pleasant function
            String connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");
            String connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            Int32 httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            // Do continuous first
            String continuousProjectionsFolder = ConfigurationReader.GetValue("EventStoreSettings", "ContinuousProjectionsFolder");
            String[] files = Directory.GetFiles(continuousProjectionsFolder, "*.js");

            // TODO: Improve this later to get status of projection before trying to create
            //foreach (String file in files)
            //{
            //    String withoutExtension = Path.GetFileNameWithoutExtension(file);
            //    String projectionBody = File.ReadAllText(file);
            //    Boolean emitEnabled = continuousProjectionsFolder.ToLower().Contains("emitenabled");

            //    await Retry.For(async () =>
            //                    {
            //                        DnsEndPoint d = new DnsEndPoint(settings.IpAddress, settings.HttpPort);

            //                        ProjectionsManager projectionsManager = new ProjectionsManager(new ConsoleLogger(), d, TimeSpan.FromSeconds(5));

            //                        UserCredentials userCredentials = new UserCredentials(settings.UserName, settings.Password);

            //                        String projectionStatus = await projectionsManager.GetStatusAsync(withoutExtension, userCredentials);

            //                        await projectionsManager.CreateContinuousAsync(withoutExtension, projectionBody, userCredentials);
            //                        Logger.LogInformation("After CreateContinuousAsync");
            //                        if (emitEnabled)
            //                        {
            //                            await projectionsManager.AbortAsync(withoutExtension, userCredentials);
            //                            await projectionsManager.UpdateQueryAsync(withoutExtension, projectionBody, emitEnabled, userCredentials);
            //                            await projectionsManager.EnableAsync(withoutExtension, userCredentials);
            //                        }
            //                    });
            //}
        }

        #endregion
    }
}

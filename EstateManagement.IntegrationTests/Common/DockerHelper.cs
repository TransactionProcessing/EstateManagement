namespace EstateManagement.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateReporting.Database;
    using EventStore.Client;
    using global::Shared.Logger;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using Shouldly;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.IntegrationTesting.DockerHelper" />
    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        #region Fields

        /// <summary>
        /// The estate client
        /// </summary>
        public IEstateClient EstateClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;

        /// <summary>
        /// The test bank account number
        /// </summary>
        public static String TestBankAccountNumber = "12345678";

        /// <summary>
        /// The test bank sort code
        /// </summary>
        public static String TestBankSortCode = "112233";

        /// <summary>
        /// The test host client
        /// </summary>
        public HttpClient TestHostClient;

        /// <summary>
        /// The test identifier
        /// </summary>
        public Guid TestId;

        /// <summary>
        /// The callback handler container name
        /// </summary>
        protected String CallbackHandlerContainerName;

        /// <summary>
        /// The callback handler port
        /// </summary>
        protected Int32 CallbackHandlerPort;

        /// <summary>
        /// The containers
        /// </summary>
        protected List<IContainerService> Containers;

        
        /// <summary>
        /// The estate management port
        /// </summary>
        protected Int32 EstateManagementPort;

        /// <summary>
        /// The estate reporting port
        /// </summary>
        protected Int32 EstateReportingPort;
        
        /// <summary>
        /// The event store HTTP port
        /// </summary>
        protected Int32 EventStoreHttpPort;

        /// <summary>
        /// The security service port
        /// </summary>
        protected Int32 SecurityServicePort;
        
        /// <summary>
        /// The test host service port
        /// </summary>
        protected Int32 TestHostServicePort;

        /// <summary>
        /// The test networks
        /// </summary>
        protected List<INetworkService> TestNetworks;

        /// <summary>
        /// The testing context
        /// </summary>
        private readonly TestingContext TestingContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="testingContext">The testing context.</param>
        public DockerHelper(NlogLogger logger,
                            TestingContext testingContext)
        {
            this.Logger = logger;
            this.TestingContext = testingContext;
            this.Containers = new List<IContainerService>();
            this.TestNetworks = new List<INetworkService>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the subscription service configuration.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        public async Task PopulateSubscriptionServiceConfiguration(String estateName)
        {
            EventStorePersistentSubscriptionsClient client =
                new EventStorePersistentSubscriptionsClient(DockerHelper.ConfigureEventStoreSettings(this.EventStoreHttpPort));

            PersistentSubscriptionSettings settings = new PersistentSubscriptionSettings(resolveLinkTos:true, StreamPosition.Start);
            await client.CreateAsync(estateName.Replace(" ", ""), "Reporting", settings);
            await client.CreateAsync($"EstateManagementSubscriptionStream_{estateName.Replace(" ", "")}", "Estate Management", settings);
        }

        /// <summary>
        /// Setups the callback handler container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="networkServices">The network services.</param>
        /// <param name="hostFolder">The host folder.</param>
        /// <param name="dockerCredentials">The docker credentials.</param>
        /// <param name="eventStoreAddress">The event store address.</param>
        /// <param name="forceLatestImage">if set to <c>true</c> [force latest image].</param>
        /// <param name="additionalEnvironmentVariables">The additional environment variables.</param>
        /// <returns></returns>
        public IContainerService SetupCallbackHandlerContainer(String imageName,
                                                               List<INetworkService> networkServices,
                                                               Boolean forceLatestImage = false,
                                                               List<String> additionalEnvironmentVariables = null)
        {
            this.Logger.LogInformation("About to Start Callback Handler Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"EventStoreSettings:ConnectionString={this.GenerateEventStoreConnectionString()}");

            if (additionalEnvironmentVariables != null)
            {
                environmentVariables.AddRange(additionalEnvironmentVariables);
            }

            ContainerBuilder callbackHandlerContainer = new Builder().UseContainer().WithName(this.CallbackHandlerContainerName).WithEnvironment(environmentVariables.ToArray())
                                                                     .UseImage(imageName, forceLatestImage).ExposePort(DockerHelper.CallbackHandlerDockerPort)
                                                                     .UseNetwork(networkServices.ToArray());

            callbackHandlerContainer = MountHostFolder(callbackHandlerContainer);
            callbackHandlerContainer = SetDockerCredentials(callbackHandlerContainer);

            // Now build and return the container                
            IContainerService builtContainer = callbackHandlerContainer.Build().Start().WaitForPort($"{DockerHelper.CallbackHandlerDockerPort}/tcp", 30000);

            this.Logger.LogInformation("Callback Handler Container Started");

            return builtContainer;
        }

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            this.HostTraceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            this.SecurityServiceContainerName = $"securityservice{testGuid:N}";
            this.EstateManagementContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            this.EstateReportingContainerName = $"estatereporting{testGuid:N}";
            this.TestHostContainerName = $"testhosts{testGuid:N}";
            this.CallbackHandlerContainerName = $"callbackhandler{testGuid:N}";

            this.DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");
            this.SqlServerDetails = (Setup.SqlServerContainerName, Setup.SqlUserName, Setup.SqlPassword);
            this.ClientDetails = ("serviceClient", "Secret1");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);

            IContainerService eventStoreContainer =
                this.SetupEventStoreContainer("eventstore/eventstore:21.2.0-buster-slim", testNetwork);
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint($"{DockerHelper.EventStoreHttpDockerPort}/tcp").Port;
            
            String insecureEventStoreEnvironmentVariable = "EventStoreSettings:Insecure=true";
            String persistentSubscriptionPollingInSeconds = "AppSettings:PersistentSubscriptionPollingInSeconds=10";
            String internalSubscriptionServiceCacheDuration = "AppSettings:InternalSubscriptionServiceCacheDuration=0";

            IContainerService estateManagementContainer = this.SetupEstateManagementContainer("estatemanagement",
                                                                                                      new List<INetworkService>
                                                                                                      {
                                                                                                          testNetwork,
                                                                                                          Setup.DatabaseServerNetwork
                                                                                                      },
                                                                                                      additionalEnvironmentVariables:new List<String>
                                                                                                          {
                                                                                                              insecureEventStoreEnvironmentVariable,
                                                                                                              persistentSubscriptionPollingInSeconds,
                                                                                                              internalSubscriptionServiceCacheDuration
                                                                                                          });

            IContainerService securityServiceContainer = this.SetupSecurityServiceContainer("stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    true);

            IContainerService estateReportingContainer = this.SetupEstateReportingContainer("stuartferguson/estatereporting",
                                                                                            new List<INetworkService>
                                                                                            {
                                                                                                testNetwork,
                                                                                                Setup.DatabaseServerNetwork
                                                                                            },
                                                                                            additionalEnvironmentVariables:new List<String>
                                                                                                {
                                                                                                    persistentSubscriptionPollingInSeconds,
                                                                                                    internalSubscriptionServiceCacheDuration
                                                                                                },
                                                                                            forceLatestImage:true);


            var callbackHandlerContainer = this.SetupCallbackHandlerContainer("stuartferguson/callbackhandler",
                                                                                      new List<INetworkService>
                                                                                      {
                                                                                          testNetwork
                                                                                      },
                                                                                      true,
                                                                                      additionalEnvironmentVariables: new List<String>
                                                                                          {
                                                                                              insecureEventStoreEnvironmentVariable
                                                                                          });

            var testHostContainer = this.SetupTestHostContainer("stuartferguson/testhosts",
                                                                        new List<INetworkService>
                                                                        {
                                                                            testNetwork,
                                                                            Setup.DatabaseServerNetwork
                                                                        },
                                                                        true);

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         estateReportingContainer,
                                         callbackHandlerContainer,
                                         testHostContainer
                                     });

            // Cache the ports
            this.EstateManagementPort = estateManagementContainer.ToHostExposedEndpoint($"{DockerHelper.EstateManagementDockerPort}/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint($"{DockerHelper.SecurityServiceDockerPort}/tcp").Port;
            this.EstateReportingPort = estateReportingContainer.ToHostExposedEndpoint($"{DockerHelper.EstateReportingDockerPort}/tcp").Port;
            this.TestHostServicePort = testHostContainer.ToHostExposedEndpoint($"{DockerHelper.TestHostPort}/tcp").Port;
            this.CallbackHandlerPort = callbackHandlerContainer.ToHostExposedEndpoint($"{DockerHelper.CallbackHandlerDockerPort}/tcp").Port;

            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"https://127.0.0.1:{this.SecurityServicePort}";

            HttpClientHandler clientHandler = new HttpClientHandler
                                              {
                                                  ServerCertificateCustomValidationCallback = (message,
                                                                                               certificate2,
                                                                                               arg3,
                                                                                               arg4) =>
                                                                                              {
                                                                                                  return true;
                                                                                              }
                                              };
            HttpClient httpClient = new HttpClient(clientHandler);
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            this.TestHostClient = new HttpClient();
            this.TestHostClient.BaseAddress = new Uri($"http://127.0.0.1:{this.TestHostServicePort}");

            // TODO: Load up the projections
            await this.LoadEventStoreProjections().ConfigureAwait(false);

            String callbackUrl = $"http://{this.CallbackHandlerContainerName}:{DockerHelper.CallbackHandlerDockerPort}/api/callbacks";
            await this.ConfigureTestBank(DockerHelper.TestBankSortCode, DockerHelper.TestBankAccountNumber, callbackUrl);
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun()
        {
            await this.RemoveEstateReadModel().ConfigureAwait(false);

            if (this.Containers.Any())
            {
                foreach (IContainerService containerService in this.Containers)
                {
                    containerService.StopOnDispose = true;
                    containerService.RemoveOnDispose = true;
                    containerService.Dispose();
                }
            }

            if (this.TestNetworks.Any())
            {
                foreach (INetworkService networkService in this.TestNetworks)
                {
                    networkService.Stop();
                    networkService.Remove(true);
                }
            }
        }

        /// <summary>
        /// Configures the event store settings.
        /// </summary>
        /// <param name="eventStoreHttpPort">The event store HTTP port.</param>
        /// <returns></returns>
        private static EventStoreClientSettings ConfigureEventStoreSettings(Int32 eventStoreHttpPort)
        {
            String connectionString = $"http://127.0.0.1:{eventStoreHttpPort}";

            EventStoreClientSettings settings = new EventStoreClientSettings();
            settings.CreateHttpMessageHandler = () => new SocketsHttpHandler
                                                      {
                                                          SslOptions =
                                                          {
                                                              RemoteCertificateValidationCallback = (sender,
                                                                                                     certificate,
                                                                                                     chain,
                                                                                                     errors) => true,
                                                          }
                                                      };
            settings.ConnectionName = "Specflow";
            settings.ConnectivitySettings = new EventStoreClientConnectivitySettings
                                            {
                                                Address = new Uri(connectionString),
                                            };

            settings.DefaultCredentials = new UserCredentials("admin", "changeit");
            return settings;
        }

        /// <summary>
        /// Configures the test bank.
        /// </summary>
        /// <param name="sortCode">The sort code.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        private async Task ConfigureTestBank(String sortCode,
                                             String accountNumber,
                                             String callbackUrl)
        {
            var hostConfig = new
                             {
                                 sort_code = sortCode,
                                 account_number = accountNumber,
                                 callback_url = callbackUrl
                             };

            await Retry.For(async () =>
                            {
                                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/testbank/configuration");
                                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(hostConfig), Encoding.UTF8, "application/json");
                                var responseMessage = await this.TestHostClient.SendAsync(requestMessage);
                                responseMessage.IsSuccessStatusCode.ShouldBeTrue();
                            });
        }

        /// <summary>
        /// Loads the event store projections.
        /// </summary>
        private async Task LoadEventStoreProjections()
        {
            //Start our Continous Projections - we might decide to do this at a different stage, but now lets try here
            String projectionsFolder = "../../../projections/continuous";
            IPAddress[] ipAddresses = Dns.GetHostAddresses("127.0.0.1");

            if (!string.IsNullOrWhiteSpace(projectionsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(projectionsFolder);

                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();

                    EventStoreProjectionManagementClient projectionClient =
                        new EventStoreProjectionManagementClient(DockerHelper.ConfigureEventStoreSettings(this.EventStoreHttpPort));

                    foreach (FileInfo file in files)
                    {
                        String projection = File.ReadAllText(file.FullName);
                        String projectionName = file.Name.Replace(".js", string.Empty);

                        try
                        {
                            this.Logger.LogInformation($"Creating projection [{projectionName}] from file [{file.FullName}]");
                            await projectionClient.CreateContinuousAsync(projectionName, projection, trackEmittedStreams:true).ConfigureAwait(false);
                        }
                        catch(Exception e)
                        {
                            this.Logger.LogError(new Exception($"Projection [{projectionName}] error", e));
                        }
                    }
                }
            }

            this.Logger.LogInformation("Loaded projections");
        }

        /// <summary>
        /// Removes the estate read model.
        /// </summary>
        private async Task RemoveEstateReadModel()
        {
            List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

            foreach (Guid estateId in estateIdList)
            {
                String databaseName = $"EstateReportingReadModel{estateId}";

                // Build the connection string (to master)
                String connectionString = Setup.GetLocalConnectionString(databaseName);
                await Retry.For(async () =>
                                {
                                    EstateReportingSqlServerContext context = new EstateReportingSqlServerContext(connectionString);
                                    await context.Database.EnsureDeletedAsync(CancellationToken.None);
                                },
                                retryFor:TimeSpan.FromMinutes(2),
                                retryInterval:TimeSpan.FromSeconds(30));
            }
        }

        #endregion

        #region Others

        /// <summary>
        /// The callback handler docker port
        /// </summary>
        public const Int32 CallbackHandlerDockerPort = 5010;

        #endregion
    }
}
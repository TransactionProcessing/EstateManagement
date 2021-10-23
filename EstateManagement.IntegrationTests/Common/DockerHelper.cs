using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Resources;
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
    using Gherkin;
    using global::Shared.Logger;
    using global::Shared.IntegrationTesting;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using Shouldly;

    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        private readonly NlogLogger Logger;

        private readonly TestingContext TestingContext;

        public DockerHelper(NlogLogger logger, TestingContext testingContext)
        {
            this.Logger = logger;
            this.TestingContext = testingContext;
            this.Containers = new List<IContainerService>();
            this.TestNetworks =new List<INetworkService>();
        }
        
        public Guid TestId;

        protected List<IContainerService> Containers;

        protected List<INetworkService> TestNetworks;

        protected String SecurityServiceContainerName;

        protected String EstateManagementContainerName;

        protected String EventStoreContainerName;

        protected String EstateReportingContainerName;

        protected String TestHostContainerName;

        protected String CallbackHandlerContainerName;

        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

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

            String eventStoreAddress = $"http://{this.EventStoreContainerName}";

            (String, String,String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);

            IContainerService eventStoreContainer = DockerHelper.SetupEventStoreContainer(this.EventStoreContainerName, this.Logger, "eventstore/eventstore:20.10.0-buster-slim", testNetwork, traceFolder);
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint($"{DockerHelper.EventStoreHttpDockerPort}/tcp").Port;
            
            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(this.EstateManagementContainerName, this.Logger,
                                                                                                      "estatemanagement", new List<INetworkService>
                                                                                                                          {
                                                                                                                              testNetwork,
                                                                                                                              Setup.DatabaseServerNetwork
                                                                                                                          }, traceFolder, null,
                                                                                                      this.SecurityServiceContainerName,
                                                                                                      eventStoreAddress,
                                                                                                      (Setup.SqlServerContainerName,
                                                                                                      "sa",
                                                                                                      "thisisalongpassword123!"),
                                                                                                      ("serviceClient", "Secret1"));

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(this.SecurityServiceContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials,
                                                                                                    true);

            IContainerService estateReportingContainer = DockerHelper.SetupEstateReportingContainer(this.EstateReportingContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/estatereporting",
                                                                                                    new List<INetworkService>
                                                                                                    {
                                                                                                        testNetwork,
                                                                                                        Setup.DatabaseServerNetwork
                                                                                                    },
                                                                                                    traceFolder,
                                                                                                    dockerCredentials,
                                                                                                    this.SecurityServiceContainerName,
                                                                                                    eventStoreAddress,
                                                                                                    (Setup.SqlServerContainerName,
                                                                                                    "sa",
                                                                                                    "thisisalongpassword123!"),
                                                                                                    ("serviceClient", "Secret1"),
                                                                                                    true);

            var callbackHandlerContainer = SetupCallbackHandlerContainer(this.CallbackHandlerContainerName,
                                                                         this.Logger,
                                                                         "stuartferguson/callbackhandler",
                                                                         new List<INetworkService>
                                                                         {
                                                                             testNetwork
                                                                         },
                                                                         traceFolder,
                                                                         dockerCredentials,
                                                                         eventStoreAddress,
                                                                         true);

            var testHostContainer = SetupTestHostContainer(this.TestHostContainerName,
                                                           this.Logger,
                                                           "stuartferguson/testhosts",
                                                           new List<INetworkService>
            {
                testNetwork,
                Setup.DatabaseServerNetwork
            }, traceFolder,dockerCredentials, (Setup.SqlServerContainerName,
                                                               "sa",
                                                               "thisisalongpassword123!"), true);


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
            this.EstateReportingPort= estateReportingContainer.ToHostExposedEndpoint($"{DockerHelper.EstateReportingDockerPort}/tcp").Port;
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
            await ConfigureTestBank(DockerHelper.TestBankSortCode, DockerHelper.TestBankAccountNumber, callbackUrl);
        }

        public static String TestBankSortCode = "112233";

        public static String TestBankAccountNumber = "12345678";
        private async Task ConfigureTestBank(String sortCode, String accountNumber, String callbackUrl)
        {
            var hostConfig = new
                             {
                                 sort_code = sortCode,
                                 account_number = accountNumber,
                                 callback_url = callbackUrl
                             };

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/testbank/configuration");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(hostConfig), Encoding.UTF8, "application/json");
            await Retry.For(async () =>
                            {
                                var responseMessage = await this.TestHostClient.SendAsync(requestMessage);
                                responseMessage.IsSuccessStatusCode.ShouldBeTrue();
                            });
        }

        public static IContainerService SetupCallbackHandlerContainer(String containerName, ILogger logger, String imageName,
                                                               List<INetworkService> networkServices,
                                                               String hostFolder,
                                                               (String URL, String UserName, String Password)? dockerCredentials,
                                                               String eventStoreAddress,
                                                               Boolean forceLatestImage = false,
                                                               List<String> additionalEnvironmentVariables = null)
        {
            logger.LogInformation("About to Start Callback Handler Container");

            List<String> environmentVariables = new List<String>();
            environmentVariables.Add($"EventStoreSettings:ConnectionString={eventStoreAddress}:{DockerHelper.EventStoreHttpDockerPort}");
            
            if (additionalEnvironmentVariables != null)
            {
                environmentVariables.AddRange(additionalEnvironmentVariables);
            }

            ContainerBuilder callbackHandlerContainer = new Builder().UseContainer().WithName(containerName)
                                                                       .WithEnvironment(environmentVariables.ToArray())
                                                              .UseImage(imageName, forceLatestImage).ExposePort(DockerHelper.CallbackHandlerDockerPort)
                                                              .UseNetwork(networkServices.ToArray());

            if (String.IsNullOrEmpty(hostFolder) == false)
            {
                callbackHandlerContainer = callbackHandlerContainer.Mount(hostFolder, "/home/txnproc/trace", MountType.ReadWrite);
            }

            if (dockerCredentials.HasValue)
            {
                callbackHandlerContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
            }

            // Now build and return the container                
            IContainerService builtContainer = callbackHandlerContainer.Build().Start().WaitForPort($"{DockerHelper.CallbackHandlerDockerPort}/tcp", 30000);

            logger.LogInformation("Callback Handler Container Started");

            return builtContainer;
        }

        public const int CallbackHandlerDockerPort = 5010;

        private async Task LoadEventStoreProjections()
        {
            //Start our Continous Projections - we might decide to do this at a different stage, but now lets try here
            String projectionsFolder = "../../../projections/continuous";
            IPAddress[] ipAddresses = Dns.GetHostAddresses("127.0.0.1");

            if (!String.IsNullOrWhiteSpace(projectionsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(projectionsFolder);

                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();

                    EventStoreProjectionManagementClient projectionClient = new EventStoreProjectionManagementClient(ConfigureEventStoreSettings(this.EventStoreHttpPort));

                    foreach (FileInfo file in files)
                    {
                        String projection = File.ReadAllText(file.FullName);
                        String projectionName = file.Name.Replace(".js", String.Empty);

                        try
                        {
                            Logger.LogInformation($"Creating projection [{projectionName}] from file [{file.FullName}]");
                            await projectionClient.CreateContinuousAsync(projectionName, projection, trackEmittedStreams:true).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(new Exception($"Projection [{projectionName}] error", e));
                        }
                    }
                }
            }
            
            Logger.LogInformation("Loaded projections");
        }

        public IEstateClient EstateClient;

        public ISecurityServiceClient SecurityServiceClient;

        public HttpClient TestHostClient;

        protected Int32 EstateManagementPort;

        protected Int32 EstateReportingPort;

        protected Int32 SecurityServicePort;

        protected Int32 TestHostServicePort;

        protected Int32 CallbackHandlerPort;

        protected Int32 EventStoreHttpPort;

        public override async Task StopContainersForScenarioRun()
        {
            await RemoveEstateReadModel().ConfigureAwait(false);

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
                                    EstateReportingContext context = new EstateReportingContext(connectionString);
                                    await context.Database.EnsureDeletedAsync(CancellationToken.None);
                                }, retryFor:TimeSpan.FromMinutes(2),retryInterval:TimeSpan.FromSeconds(30));
                
            }
        }

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

        public async Task PopulateSubscriptionServiceConfiguration(String estateName)
        {
            EventStorePersistentSubscriptionsClient client = new EventStorePersistentSubscriptionsClient(ConfigureEventStoreSettings(this.EventStoreHttpPort));

            PersistentSubscriptionSettings settings = new PersistentSubscriptionSettings(resolveLinkTos: true, StreamPosition.Start);
            await client.CreateAsync(estateName.Replace(" ", ""), "Reporting", settings);
            await client.CreateAsync($"EstateManagementSubscriptionStream_{estateName.Replace(" ", "")}", "Estate Management", settings);
        }
    }
}

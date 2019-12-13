using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using global::Shared.General;
    using global::Shared.Logger;
    using Microsoft.Extensions.Logging;
    using SecurityService.Client;

    public class DockerHelper
    {
        private readonly NlogLogger Logger;

        protected INetworkService TestNetwork;
        
        protected Int32 EstateManagementApiPort;
        protected Int32 EventStorePort;

        public IContainerService EstateManagementApiContainer;
        protected IContainerService EventStoreContainer;

        public IEstateClient EstateClient;

        protected HttpClient HttpClient;

        protected String EventStoreConnectionString;

        protected String EstateManagementApiContainerName;
        protected String EventStoreContainerName;

        public DockerHelper(NlogLogger logger)
        {
            this.Logger = logger;
        }

        private void SetupTestNetwork()
        {
            this.Logger.LogInformation("Starting Network Setup");
            // Build a network
            this.TestNetwork = new Ductus.FluentDocker.Builders.Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();

            this.Logger.LogInformation("Network Setup Complete");
        }

        private void SetupEventStoreContainer(String traceFolder)
        {
            this.Logger.LogInformation("About to Start Event Store Container");
            // Event Store Container
            this.EventStoreContainer = new Ductus.FluentDocker.Builders.Builder()
                                       .UseContainer()
                                       .UseImage("eventstore/eventstore:release-5.0.2")
                                       .ExposePort(2113)
                                       .ExposePort(1113)
                                       .WithName(this.EventStoreContainerName)
                                       .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                                       .UseNetwork(this.TestNetwork)
                                       .Mount(traceFolder, "/var/log/eventstore", MountType.ReadWrite)
                                       .Build()
                                       .Start().WaitForPort("2113/tcp", 30000);

            this.Logger.LogInformation("Event Store Container Started");
        }

        public Guid TestId;
        
        public ISecurityServiceClient SecurityServiceClient;

        public async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            this.SecurityServiceContainerName = $"securityservice{testGuid:N}";
            this.EstateManagementApiContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            
            this.EventStoreConnectionString =
                $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{this.EventStoreContainerName}:1113;VerboseLogging=true;";
            
            this.SetupTestNetwork();
            this.SetupEventStoreContainer(traceFolder);
            this.SetupEstateManagementContainer(traceFolder);
            this.SetupSecurityServiceContainer(traceFolder);
            
            // Cache the ports
            this.EstateManagementApiPort= this.EstateManagementApiContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = this.SecurityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;

            // Setup the base address resolvers
            Func<String, String> baseAddressResolver = api => $"http://127.0.0.1:{this.EstateManagementApiPort}";
            Func<String, String> securityServiceBaseAddressResolver = api => $"http://127.0.0.1:{this.SecurityServicePort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(baseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(securityServiceBaseAddressResolver, httpClient);

            //this.HttpClient = new HttpClient();
            //this.HttpClient.BaseAddress = new Uri(baseAddressResolver(String.Empty));
        }

        public Int32 SecurityServicePort;

        public async Task StopContainersForScenarioRun()
        {
            try
            {
                if (this.EstateManagementApiContainer != null)
                {
                    this.EstateManagementApiContainer.StopOnDispose = true;
                    this.EstateManagementApiContainer.RemoveOnDispose = true;
                    this.EstateManagementApiContainer.Dispose();
                }

                if (this.SecurityServiceContainer != null)
                {
                    this.SecurityServiceContainer.StopOnDispose = true;
                    this.SecurityServiceContainer.RemoveOnDispose = true;
                    this.SecurityServiceContainer.Dispose();
                }

                if (this.EventStoreContainer != null)
                {
                    this.EventStoreContainer.StopOnDispose = true;
                    this.EventStoreContainer.RemoveOnDispose = true;
                    this.EventStoreContainer.Dispose();
                }

                if (this.TestNetwork != null)
                {
                    this.TestNetwork.Stop();
                    this.TestNetwork.Remove(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public IContainerService SecurityServiceContainer;

        public String SecurityServiceContainerName;

        private void SetupSecurityServiceContainer(String traceFolder)
        {
            this.Logger.LogInformation("About to Start Security Container");

            this.SecurityServiceContainer = new Builder().UseContainer().WithName(this.SecurityServiceContainerName)
                                                         .WithEnvironment("ASPNETCORE_ENVIRONMENT=IntegrationTest",
                                                                          $"ServiceOptions:PublicOrigin=http://{this.SecurityServiceContainerName}:5001",
                                                                          $"ServiceOptions:IssuerUrl=http://{this.SecurityServiceContainerName}:5001",
                                                                          "urls=http://*:5001")
                                                         .WithCredential("https://www.docker.com", "stuartferguson", "Sc0tland")
                                                         .UseImage("stuartferguson/securityservice").ExposePort(5001).UseNetwork(new List<INetworkService>
                                                                                                                  {
                                                                                                                      this.TestNetwork
                                                                                                                  }.ToArray())
                                                         .Mount(traceFolder, "/home/txnproc/trace", MountType.ReadWrite).Build().Start().WaitForPort("5001/tcp", 30000);
            Thread.Sleep(20000);

            this.Logger.LogInformation("Security Service Container Started");

        }

        private void SetupEstateManagementContainer(String traceFolder)
        {
            this.Logger.LogInformation("About to Start Estate Management Container");

            this.EstateManagementApiContainer = new Builder()
                                          .UseContainer()
                                          .WithName(this.EstateManagementApiContainerName)
                                          .WithEnvironment(this.EventStoreConnectionString,
                                                           $"AppSettings:SecurityService=http://{this.SecurityServiceContainerName}:5001",
                                                           "urls=http://*:5000")
                                                           //"AppSettings:MigrateDatabase=true",
                                                           //"EventStoreSettings:START_PROJECTIONS=true",
                                                           //"EventStoreSettings:ContinuousProjectionsFolder=/app/projections/continuous")
                                          .UseImage("estatemanagement")
                                          .ExposePort(5000)
                                          .UseNetwork(new List<INetworkService> { this.TestNetwork, Setup.DatabaseServerNetwork }.ToArray())
                                          .Mount(traceFolder, "/home/txnproc/trace", MountType.ReadWrite)
                                          .Build()
                                          .Start().WaitForPort("5000/tcp", 30000);

            this.Logger.LogInformation("Estate Management Container Started");
        }
    }
}

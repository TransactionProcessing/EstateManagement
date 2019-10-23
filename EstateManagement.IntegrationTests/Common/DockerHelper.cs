using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;

    public class DockerHelper
    {
        protected INetworkService TestNetwork;

        protected Int32 EstateManagementApiPort;
        protected Int32 EventStorePort;

        protected IContainerService EstateManagementApiContainer;
        protected IContainerService EventStoreContainer;

        public IEstateClient EstateClient;
        protected HttpClient HttpClient;

        protected String EventStoreConnectionString;

        protected String EstateManagementApiContainerName;
        protected String EventStoreContainerName;

        private void SetupTestNetwork()
        {
            // Build a network
            this.TestNetwork = new Ductus.FluentDocker.Builders.Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();
        }

        private void SetupEventStoreContainer(String traceFolder)
        {
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

            Console.Out.WriteLine("Started Event Store");
        }

        public Guid TestId;

        public async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = $"/home/ubuntu/estatemanagement/trace/{scenarioName}/";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            // Setup the container names
            this.EstateManagementApiContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            
            this.EventStoreConnectionString =
                $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{this.EventStoreContainerName}:1113;VerboseLogging=true;";
            
            this.SetupTestNetwork();
            this.SetupEventStoreContainer(traceFolder);
            this.SetupEstateManagementApiContainer(traceFolder);

            // Cache the ports
            this.EstateManagementApiPort= this.EstateManagementApiContainer.ToHostExposedEndpoint("5000/tcp").Port;

            Console.Out.WriteLine($"Estate management port [{this.EstateManagementApiPort}]");

            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;

            Console.Out.WriteLine($"Event Store port [{this.EventStorePort}]");

            // Setup the base address resolver
            Func<String, String> baseAddressResolver = api => $"http://127.0.0.1:{this.EstateManagementApiPort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(baseAddressResolver, httpClient);

            //this.HttpClient = new HttpClient();
            //this.HttpClient.BaseAddress = new Uri(baseAddressResolver(String.Empty));
        }

        public async Task StopContainersForScenarioRun()
        {
            try
            {
                var log = this.EstateManagementApiContainer.Logs(true);
                var gimp = log.TryRead(10000);

                Console.Out.WriteLine(gimp);

                if (this.EstateManagementApiContainer != null)
                {
                    this.EstateManagementApiContainer.StopOnDispose = true;
                    this.EstateManagementApiContainer.RemoveOnDispose = true;
                    this.EstateManagementApiContainer.Dispose();
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

        private void SetupEstateManagementApiContainer(String traceFolder)
        {
            // Management API Container
            this.EstateManagementApiContainer = new Builder()
                                          .UseContainer()
                                          .WithName(this.EstateManagementApiContainerName)
                                          .WithEnvironment(this.EventStoreConnectionString) //,
                                                           //"AppSettings:MigrateDatabase=true",
                                                           //"EventStoreSettings:START_PROJECTIONS=true",
                                                           //"EventStoreSettings:ContinuousProjectionsFolder=/app/projections/continuous")
                                          .UseImage("estatemanagement")
                                          .ExposePort(5000)
                                          .UseNetwork(new List<INetworkService> { this.TestNetwork, Setup.DatabaseServerNetwork }.ToArray())
                                          .Mount(traceFolder, "/home", MountType.ReadWrite)
                                          .Build()
                                          .Start().WaitForPort("5000/tcp", 30000);

            var log = this.EstateManagementApiContainer.Logs(true);
            var gimp = log.TryRead(10000);
            
            Console.Out.WriteLine(gimp);
            Console.Out.WriteLine("Started Estate Management");
        }
    }
}

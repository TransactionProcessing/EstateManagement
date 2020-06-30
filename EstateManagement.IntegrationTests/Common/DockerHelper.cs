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
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateReporting.Database;
    using Gherkin;
    using global::Shared.Logger;
    using global::Shared.IntegrationTesting;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Internal;
    using SecurityService.Client;
    using EventStore.ClientAPI.Projections;
    using EventStore.ClientAPI.Common.Log;
    using EventStore.ClientAPI.SystemData;

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

        protected String SubscriptionServiceContainerName;
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
            this.SubscriptionServiceContainerName = $"subscription{testGuid:N}";

            (String, String,String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);

            IContainerService eventStoreContainer = DockerHelper.SetupEventStoreContainer(this.EventStoreContainerName, this.Logger, "eventstore/eventstore:20.6.0-buster-slim", testNetwork, traceFolder, usesEventStore2006OrLater: true);


            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(this.EstateManagementContainerName, this.Logger,
                                                                                                      "estatemanagement", new List<INetworkService>
                                                                                                                          {
                                                                                                                              testNetwork,
                                                                                                                              Setup.DatabaseServerNetwork
                                                                                                                          }, traceFolder, null,
                                                                                                      this.SecurityServiceContainerName,
                                                                                                      this.EventStoreContainerName,
                                                                                                      (Setup.SqlServerContainerName,
                                                                                                      "sa",
                                                                                                      "thisisalongpassword123!"),
                                                                                                      ("serviceClient", "Secret1"));

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(this.SecurityServiceContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials);

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
                                                                                                    (Setup.SqlServerContainerName,
                                                                                                    "sa",
                                                                                                    "thisisalongpassword123!"),
                                                                                                    ("serviceClient", "Secret1"),
                                                                                                    true);

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         estateReportingContainer,
                                     });

            // Cache the ports
            this.EstateManagementPort = estateManagementContainer.ToHostExposedEndpoint($"{DockerHelper.EstateManagementDockerPort}/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint($"{DockerHelper.SecurityServiceDockerPort}/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint($"{DockerHelper.EventStoreHttpDockerPort}/tcp").Port;
            this.EstateReportingPort= estateReportingContainer.ToHostExposedEndpoint($"{DockerHelper.EstateReportingDockerPort}/tcp").Port;

            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"http://127.0.0.1:{this.SecurityServicePort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);

            // TODO: Load up the projections
            await this.LoadEventStoreProjections().ConfigureAwait(false);

            await this.PopulateSubscriptionServiceConfiguration().ConfigureAwait(false);

            IContainerService subscriptionServiceContainer = DockerHelper.SetupSubscriptionServiceContainer(this.SubscriptionServiceContainerName,
                                                                                                            this.Logger,
                                                                                                            "stuartferguson/subscriptionservicehost",
                                                                                                            new List<INetworkService>
                                                                                                            {
                                                                                                                testNetwork,
                                                                                                                Setup.DatabaseServerNetwork
                                                                                                            },
                                                                                                            traceFolder,
                                                                                                            dockerCredentials,
                                                                                                            this.SecurityServiceContainerName,
                                                                                                            (Setup.SqlServerContainerName,
                                                                                                            "sa",
                                                                                                            "thisisalongpassword123!"),
                                                                                                            this.TestId,
                                                                                                            ("serviceClient", "Secret1"),
                                                                                                            true);

            this.Containers.Add(subscriptionServiceContainer);
        }

        private async Task LoadEventStoreProjections()
        {
            //Start our Continous Projections - we might decide to do this at a different stage, but now lets try here
            String projectionsFolder = "../../../projections/continuous";
            IPAddress[] ipAddresses = Dns.GetHostAddresses("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(ipAddresses.First(), this.EventStoreHttpPort);

            if (!String.IsNullOrWhiteSpace(projectionsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(projectionsFolder);

                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();

                    // TODO: possibly need to change timeout and logger here
                    ProjectionsManager projectionManager = new ProjectionsManager(new ConsoleLogger(), endpoint, TimeSpan.FromSeconds(30));

                    foreach (FileInfo file in files)
                    {
                        String projection = File.ReadAllText(file.FullName);
                        String projectionName = file.Name.Replace(".js", String.Empty);

                        try
                        {
                            Logger.LogInformation($"Creating projection [{projectionName}]");
                            await projectionManager.CreateContinuousAsync(projectionName, projection, new UserCredentials("admin", "changeit")).ConfigureAwait(false);
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

        protected Int32 EstateManagementPort;

        protected Int32 EstateReportingPort;

        protected Int32 SecurityServicePort;
        
        protected Int32 EventStoreHttpPort;

        public override async Task StopContainersForScenarioRun()
        {
            await CleanUpSubscriptionServiceConfiguration().ConfigureAwait(false);

            await RemoveEstateReadModel().ConfigureAwait(false);

            if (EnumerableExtensions.Any(this.Containers))
            {
                foreach (IContainerService containerService in this.Containers)
                {
                    containerService.StopOnDispose = true;
                    containerService.RemoveOnDispose = true;
                    containerService.Dispose();
                }
            }

            if (EnumerableExtensions.Any(this.TestNetworks))
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

        protected async Task PopulateSubscriptionServiceConfiguration()
        {
            String connectionString = Setup.GetLocalConnectionString("SubscriptionServiceConfiguration");

            await using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(CancellationToken.None).ConfigureAwait(false);
                
                    // Create an Event Store Server
                    await this.InsertEventStoreServer(connection,this.EventStoreContainerName).ConfigureAwait(false);

                    String endPointUri = $"http://{this.EstateReportingContainerName}:5005/api/domainevents";
                    // Add Route for Estate Aggregate Events
                    await this.InsertSubscription(connection, "$ce-EstateAggregate", "Reporting", endPointUri).ConfigureAwait(false);

                    // Add Route for Merchant Aggregate Events
                    await this.InsertSubscription(connection, "$ce-MerchantAggregate", "Reporting", endPointUri).ConfigureAwait(false);

                    await connection.CloseAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        protected async Task CleanUpSubscriptionServiceConfiguration()
        {
            String connectionString = Setup.GetLocalConnectionString("SubscriptionServiceConfiguration");

            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                // Delete the Event Store Server
                await this.DeleteEventStoreServer(connection).ConfigureAwait(false);

                // Delete the Subscriptions
                await this.DeleteSubscriptions(connection).ConfigureAwait(false);
                
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }

        protected async Task InsertEventStoreServer(SqlConnection openConnection, String eventStoreContainerName)
        {
            String esConnectionString = $"ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:{DockerHelper.EventStoreTcpDockerPort};VerboseLogging=true;";
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"INSERT INTO EventStoreServer(EventStoreServerId, ConnectionString,Name) SELECT '{this.TestId}', '{esConnectionString}', 'TestEventStore'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }
        
        protected async Task DeleteEventStoreServer(SqlConnection openConnection)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"DELETE FROM EventStoreServer WHERE EventStoreServerId = '{this.TestId}'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task DeleteSubscriptions(SqlConnection openConnection)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"DELETE FROM Subscription WHERE EventStoreId = '{this.TestId}'";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }

        protected async Task InsertSubscription(SqlConnection openConnection, String streamName, String groupName, String endPointUri)
        {
            SqlCommand command = openConnection.CreateCommand();
            command.CommandText = $"INSERT INTO subscription(SubscriptionId, EventStoreId, StreamName, GroupName, EndPointUri, StreamPosition) SELECT '{Guid.NewGuid()}', '{this.TestId}', '{streamName}', '{groupName}', '{endPointUri}', null";
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}

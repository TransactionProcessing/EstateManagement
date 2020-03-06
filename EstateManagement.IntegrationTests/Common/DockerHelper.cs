using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Data;
    using System.Net.Http;
    using System.Resources;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using Gherkin;
    using global::Shared.Logger;
    using global::Shared.IntegrationTesting;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Internal;
    using SecurityService.Client;

    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        private readonly NlogLogger Logger;

        public DockerHelper(NlogLogger logger)
        {
            this.Logger = logger;
            this.Containers = new List<IContainerService>();
            this.TestNetworks =new List<INetworkService>();
        }
        
        public Guid TestId;

        protected List<IContainerService> Containers;

        protected List<INetworkService> TestNetworks;

        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");
            
            // Setup the container names
            String securityServiceContainerName = $"securityservice{testGuid:N}";
            String estateManagementContainerName = $"estate{testGuid:N}";
            String eventStoreContainerName = $"eventstore{testGuid:N}";
            String estateReportingContainerName = $"estatereporting{testGuid:N}";
            String subscriptionServiceContainerName = $"subscription{testGuid:N}";

            (String, String,String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);

            IContainerService eventStoreContainer = DockerHelper.SetupEventStoreContainer(eventStoreContainerName, this.Logger,
                                                                                          "eventstore/eventstore:release-5.0.2",
                                                                                          testNetwork, traceFolder);


            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(estateManagementContainerName, this.Logger,
                                                                                                      "estatemanagement", new List<INetworkService>
                                                                                                                          {
                                                                                                                              testNetwork,
                                                                                                                              Setup.DatabaseServerNetwork
                                                                                                                          }, traceFolder, null,
                                                                                                      securityServiceContainerName,
                                                                                                      eventStoreContainerName,
                                                                                                      ("serviceClient", "Secret1"));

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(securityServiceContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials);

            IContainerService estateReportingContainer = DockerHelper.SetupEstateReportingContainer(estateReportingContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/estatereporting",
                                                                                                    new List<INetworkService>
                                                                                                    {
                                                                                                        testNetwork,
                                                                                                        Setup.DatabaseServerNetwork
                                                                                                    },
                                                                                                    traceFolder,
                                                                                                    dockerCredentials,
                                                                                                    securityServiceContainerName,
                                                                                                    Setup.SqlServerContainerName,
                                                                                                    "sa",
                                                                                                    "thisisalongpassword123!",
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

            await PopulateSubscriptionServiceConfiguration(eventStoreContainerName).ConfigureAwait(false);

            IContainerService subscriptionServiceContainer = DockerHelper.SetupSubscriptionServiceContainer(subscriptionServiceContainerName,
                                                                                                            this.Logger,
                                                                                                            "stuartferguson/subscriptionservicehost",
                                                                                                            new List<INetworkService>
                                                                                                            {
                                                                                                                testNetwork,
                                                                                                                Setup.DatabaseServerNetwork
                                                                                                            },
                                                                                                            traceFolder,
                                                                                                            dockerCredentials,
                                                                                                            securityServiceContainerName,
                                                                                                            Setup.SqlServerContainerName,
                                                                                                            "sa",
                                                                                                            "thisisalongpassword123!",
                                                                                                            this.TestId,
                                                                                                            ("serviceClient", "Secret1"),
                                                                                                            true);

            this.Containers.Add(subscriptionServiceContainer);
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

        protected async Task PopulateSubscriptionServiceConfiguration(String eventStoreContainerName)
        {
            String connectionString = Setup.GetLocalConnectionString("SubscriptionServiceConfiguration");

            await using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(CancellationToken.None).ConfigureAwait(false);
                
                    // Create an Event Store Server
                    await this.InsertEventStoreServer(connection,eventStoreContainerName).ConfigureAwait(false);

                    String endPointUri = $"http://localhost:{this.EstateReportingPort}/api/DomainEvent";
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

using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Net.Http;
    using System.Resources;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using Gherkin;
    using global::Shared.Logger;
    using global::Shared.IntegrationTesting;
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
            String estateManagementApiContainerName = $"estate{testGuid:N}";
            String eventStoreContainerName = $"eventstore{testGuid:N}";
            
            (String, String,String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);
            IContainerService eventStoreContainer = DockerHelper.SetupEventStoreContainer(eventStoreContainerName, this.Logger,
                                                                                          "eventstore/eventstore:release-5.0.2",
                                                                                          testNetwork, traceFolder);


            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(estateManagementApiContainerName, this.Logger,
                                                                                                      "estatemanagement", new List<INetworkService>
                                                                                                                          {
                                                                                                                              testNetwork
                                                                                                                          }, traceFolder, null,
                                                                                                      securityServiceContainerName,
                                                                                                      eventStoreContainerName);

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(securityServiceContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials);

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer
                                     });

            // Cache the ports
            this.EstateManagementApiPort = estateManagementContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;

            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementApiPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"http://127.0.0.1:{this.SecurityServicePort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
        }

        public IEstateClient EstateClient;

        public ISecurityServiceClient SecurityServiceClient;

        protected Int32 EstateManagementApiPort;

        protected Int32 SecurityServicePort;
        
        protected Int32 EventStoreHttpPort;

        public override async Task StopContainersForScenarioRun()
        {
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
    }
}

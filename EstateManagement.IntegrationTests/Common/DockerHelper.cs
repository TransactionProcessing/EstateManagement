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
    using global::Shared.IntegrationTesting;
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
        /// The testing context
        /// </summary>
        private readonly TestingContext TestingContext;

        #endregion

        #region Constructors
        
        public DockerHelper()
        {
            this.TestingContext = new TestingContext();
        }

        #endregion

        #region Methods
        
        public override async Task StartContainersForScenarioRun(String scenarioName) {

            await base.StartContainersForScenarioRun(scenarioName);

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

            String callbackUrl = $"http://{this.CallbackHandlerContainerName}:{DockerPorts.CallbackHandlerDockerPort}/api/callbacks";
            await this.ConfigureTestBank(DockerHelper.TestBankSortCode, DockerHelper.TestBankAccountNumber, callbackUrl);
        }
        
        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun()
        {
            await this.RemoveEstateReadModel().ConfigureAwait(false);

            await base.StopContainersForScenarioRun();
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
    }
}
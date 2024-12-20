﻿namespace EstateManagement.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Database.Contexts;
    using EventStore.Client;
    using global::Shared.IntegrationTesting;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using Shouldly;
    using TransactionProcessor.Client;

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

        public ITransactionProcessorClient TransactionProcessorClient;

        public EventStoreProjectionManagementClient ProjectionManagementClient;

        #endregion

        #region Constructors

        public DockerHelper()
        {
            this.TestingContext = new TestingContext();
        }

        #endregion

        #region Methods

        private void SetHostTraceFolder(String scenarioName)
        {
            String ciEnvVar = Environment.GetEnvironmentVariable("CI");
            DockerEnginePlatform engineType = DockerHelper.GetDockerEnginePlatform();

            // We are running on linux (CI or local ok)
            // We are running windows local (can use "C:\\home\\txnproc\\trace\\{scenarioName}")
            // We are running windows CI (can use "C:\\Users\\runneradmin\\trace\\{scenarioName}")

            Boolean isCI = (String.IsNullOrEmpty(ciEnvVar) == false && String.Compare(ciEnvVar, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase) == 0);

            this.HostTraceFolder = (engineType, isCI) switch
            {
                (DockerEnginePlatform.Windows, false) => $"C:\\home\\txnproc\\trace\\{scenarioName}",
                (DockerEnginePlatform.Windows, true) => $"C:\\Users\\runneradmin\\txnproc\\trace\\{scenarioName}",
                _ => $"/home/txnproc/trace/{scenarioName}"
            };

            if (engineType == DockerEnginePlatform.Windows && isCI)
            {
                if (Directory.Exists(this.HostTraceFolder) == false)
                {
                    this.Trace($"[{this.HostTraceFolder}] does not exist");
                    Directory.CreateDirectory(this.HostTraceFolder);
                    this.Trace($"[{this.HostTraceFolder}] created");
                }
                else
                {
                    this.Trace($"[{this.HostTraceFolder}] already exists");
                }
            }

            this.Trace($"HostTraceFolder is [{this.HostTraceFolder}]");
        }

        public override async Task CreateSubscriptions(){
            List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new();
            subscriptions.AddRange(MessagingService.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());
            subscriptions.AddRange(EstateManagement.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());

            // TODO: get from the transacition processor nuget
            subscriptions.Add(("$ce-EstateAggregate","Transaction Processor - Ordered",2));
            subscriptions.Add(("$ce-MerchantAggregate", "Transaction Processor - Ordered", 2));
            subscriptions.Add(("$ce-MerchantDepositListAggregate", "Transaction Processor - Ordered", 2));
            subscriptions.Add(("$ce-SettlementAggregate", "Transaction Processor", 0));
            subscriptions.Add(("$ce-TransactionAggregate", "Transaction Processor", 0));
            subscriptions.Add(("$ce-TransactionAggregate", "Transaction Processor - Ordered", 2));
            subscriptions.Add(("$ce-VoucherAggregate", "Transaction Processor", 0));
            subscriptions.Add(("$ce-VoucherAggregate", "Transaction Processor - Ordered", 2));

            foreach ((String streamName, String groupName, Int32 maxRetries) subscription in subscriptions){
                var x = subscription;
                x.maxRetries = 2;
                await this.CreatePersistentSubscription(x);
            }
        }

        public override async Task StartContainersForScenarioRun(String scenarioName, DockerServices dockerServices) {

            await base.StartContainersForScenarioRun(scenarioName, dockerServices);

            this.Trace($"Estate Management Port is [{this.EstateManagementPort}]");
            this.Trace($"Security Service Port is [{this.SecurityServicePort}]");
            
            // Setup the base address resolvers
            String estateAddress = $"http://127.0.0.1:{this.EstateManagementPort}";
            String securityAddress = $"https://127.0.0.1:{this.SecurityServicePort}";

            String EstateManagementBaseAddressResolver(String api) => estateAddress;
            String SecurityServiceBaseAddressResolver(String api) => securityAddress;
            String TransactionProcessorBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorPort}";

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
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient, 2);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            this.TransactionProcessorClient = new TransactionProcessorClient(TransactionProcessorBaseAddressResolver, httpClient);
            this.TestHostClient = new HttpClient();
            this.TestHostClient.BaseAddress = new Uri($"http://127.0.0.1:{this.TestHostServicePort}");

            this.Trace("About to configure Test Bank");
            String callbackUrl = $"http://{this.CallbackHandlerContainerName}:{DockerPorts.CallbackHandlerDockerPort}/api/callbacks";
            await this.ConfigureTestBank(DockerHelper.TestBankSortCode, DockerHelper.TestBankAccountNumber, callbackUrl);
            this.Trace("Test Bank Configured");

            this.ProjectionManagementClient = new EventStoreProjectionManagementClient(ConfigureEventStoreSettings());
        }
        
        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun(DockerServices sharedDockerServices)
        {
            await this.RemoveEstateReadModel().ConfigureAwait(false);

            await base.StopContainersForScenarioRun(sharedDockerServices);
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
            this.Trace(this.TestHostClient.BaseAddress.ToString());

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
                                    EstateManagementSqlServerContext context = new EstateManagementSqlServerContext(connectionString);
                                    await context.Database.EnsureDeletedAsync(CancellationToken.None);
                                },
                                retryFor:TimeSpan.FromMinutes(2),
                                retryInterval:TimeSpan.FromSeconds(30));
            }
        }

        protected override List<String> GetRequiredProjections()
        {
            List<String> requiredProjections = new List<String>();

            requiredProjections.Add("CallbackHandlerEnricher.js");
            requiredProjections.Add("EstateAggregator.js");
            requiredProjections.Add("MerchantAggregator.js");
            requiredProjections.Add("MerchantBalanceCalculator.js");
            requiredProjections.Add("MerchantBalanceProjection.js");

            return requiredProjections;
        }

        #endregion
    }
}
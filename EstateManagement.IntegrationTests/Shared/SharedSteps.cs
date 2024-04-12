using System;
using System.Collections.Generic;

namespace EstateManagement.IntegrationTests.Shared
{
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using DataTransferObjects.Requests.Contract;
    using DataTransferObjects.Requests.Estate;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Estate;
    using DataTransferObjects.Responses.Merchant;
    using global::Shared.IntegrationTesting;
    using IntegrationTesting.Helpers;
    using Newtonsoft.Json.Linq;
    using Reqnroll;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using SecurityService.IntegrationTesting.Helpers;
    using Shouldly;
    using TransactionProcessor.DataTransferObjects;
    using TransactionProcessor.IntegrationTesting.Helpers;
    using ClientDetails = Common.ClientDetails;
    using DockerHelper = Common.DockerHelper;
    using MerchantOperatorResponse = DataTransferObjects.Responses.Merchant.MerchantOperatorResponse;
    using MerchantResponse = DataTransferObjects.Responses.Merchant.MerchantResponse;
    using ReqnrollExtensions = IntegrationTesting.Helpers.ReqnrollExtensions;
    using ReqnrollTableHelper = global::Shared.IntegrationTesting.ReqnrollTableHelper;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        private readonly SecurityServiceSteps SecurityServiceSteps;

        private readonly EstateManagementSteps EstateManagementSteps;

        private readonly TransactionProcessorSteps TransactionProcessorSteps;
        
        public SharedSteps(ScenarioContext scenarioContext,
                           TestingContext testingContext) {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
            this.SecurityServiceSteps = new SecurityServiceSteps(testingContext.DockerHelper.SecurityServiceClient);
            this.EstateManagementSteps = new EstateManagementSteps(testingContext.DockerHelper.EstateClient, this.TestingContext.DockerHelper.TestHostClient);
            this.TransactionProcessorSteps = new TransactionProcessorSteps(testingContext.DockerHelper.TransactionProcessorClient, testingContext.DockerHelper.TestHostClient);
        }

        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(DataTable table) {
            List<CreateEstateRequest> requests = table.Rows.ToCreateEstateRequests();
            
            List<EstateResponse> verifiedEstates = await this.EstateManagementSteps.WhenICreateTheFollowingEstates(this.TestingContext.AccessToken, requests);

            foreach (EstateResponse verifiedEstate in verifiedEstates){
                this.TestingContext.AddEstateDetails(verifiedEstate.EstateId, verifiedEstate.EstateName, verifiedEstate.EstateReference);
                this.TestingContext.Logger.LogInformation($"Estate {verifiedEstate.EstateName} created with Id {verifiedEstate.EstateId}");
            }
        }

        [Given(@"I have created the following operators")]
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(DataTable table) {
            List<(EstateDetails estate, CreateOperatorRequest request)> requests = table.Rows.ToCreateOperatorRequests(this.TestingContext.Estates);

            List<(Guid, EstateOperatorResponse)> results = await this.EstateManagementSteps.WhenICreateTheFollowingOperators(this.TestingContext.AccessToken, requests);

            foreach ((Guid, EstateOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} created with Id {result.Item2.OperatorId} for Estate {result.Item1}");
            }
        }

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(DataTable table) {
            List<(EstateDetails estate, CreateMerchantRequest)> requests = table.Rows.ToCreateMerchantRequests(this.TestingContext.Estates);

            List<MerchantResponse> verifiedMerchants = await this.EstateManagementSteps.WhenICreateTheFollowingMerchants(this.TestingContext.AccessToken, requests);

            foreach (MerchantResponse verifiedMerchant in verifiedMerchants)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(verifiedMerchant.EstateId);
                estateDetails.AddMerchant(verifiedMerchant);
                this.TestingContext.Logger.LogInformation($"Merchant {verifiedMerchant.MerchantName} created with Id {verifiedMerchant.MerchantId} for Estate {estateDetails.EstateName}");
            }
        }

        [Given(@"I have assigned the following operator to the merchants")]
        [When(@"I assign the following operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(DataTable table) {
            List<(EstateDetails, Guid, AssignOperatorRequest)> requests = table.Rows.ToAssignOperatorRequests(this.TestingContext.Estates);

            List<(EstateDetails, MerchantOperatorResponse)> results = await this.EstateManagementSteps.WhenIAssignTheFollowingOperatorToTheMerchants(this.TestingContext.AccessToken, requests);

            foreach ((EstateDetails, MerchantOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} assigned to Estate {result.Item1.EstateName}");
            }
        }

        [When(@"I create the following security users")]
        [Given("I have created the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(DataTable table){
            List<CreateNewUserRequest> createUserRequests = table.Rows.ToCreateNewUserRequests(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenICreateTheFollowingSecurityUsers(this.TestingContext.AccessToken, createUserRequests, this.TestingContext.Estates);
        }

        [Given(@"the following security roles exist")]
        public async Task GivenTheFollowingSecurityRolesExist(DataTable table) {
            List<CreateRoleRequest> requests = table.Rows.ToCreateRoleRequests();
            List<(String, Guid)> responses = await this.SecurityServiceSteps.GivenICreateTheFollowingRoles(requests, CancellationToken.None);
        }

        [Given(@"I make the following manual merchant deposits")]
        [When(@"I make the following manual merchant deposits")]
        public async Task WhenIMakeTheFollowingManualMerchantDeposits(DataTable table){
            List<(EstateDetails, Guid, MakeMerchantDepositRequest)> requests = table.Rows.ToMakeMerchantDepositRequest(this.TestingContext.Estates);

            foreach ((EstateDetails, Guid, MakeMerchantDepositRequest) request in requests){
                Decimal previousMerchantBalance = await this.GetMerchantBalance(request.Item2);

                await this.EstateManagementSteps.GivenIMakeTheFollowingManualMerchantDeposits(this.TestingContext.AccessToken, request);

                await Retry.For(async () => {
                                    Decimal currentMerchantBalance = await this.GetMerchantBalance(request.Item2);

                                    currentMerchantBalance.ShouldBe(previousMerchantBalance + request.Item3.Amount);

                                    this.TestingContext.Logger.LogInformation($"Deposit Reference {request.Item3.Reference} made for Merchant Id {request.Item2}");
                                });
            }
        }

        private async Task<Decimal> GetMerchantBalance(Guid merchantId){
            JsonElement jsonElement = (JsonElement)await this.TestingContext.DockerHelper.ProjectionManagementClient.GetStateAsync<dynamic>("MerchantBalanceProjection", $"MerchantBalance-{merchantId:N}");
            JObject jsonObject = JObject.Parse(jsonElement.GetRawText());
            decimal balanceValue = jsonObject.SelectToken("merchant.balance").Value<decimal>();
            return balanceValue;
        }

        [When(@"I make the following merchant withdrawals")]
        public async Task WhenIMakeTheFollowingMerchantWithdrawals(DataTable table){
            List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> requests = table.Rows.ToMakeMerchantWithdrawalRequest(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIMakeTheFollowingMerchantWithdrawals(this.TestingContext.AccessToken, requests);
        }

        [Given(@"I create the following api scopes")]
        public async Task GivenICreateTheFollowingApiScopes(DataTable table) {
            List<CreateApiScopeRequest> requests = table.Rows.ToCreateApiScopeRequests();
            await this.SecurityServiceSteps.GivenICreateTheFollowingApiScopes(requests);
        }
        
        [Given(@"the following api resources exist")]
        public async Task GivenTheFollowingApiResourcesExist(DataTable table) {
            List<CreateApiResourceRequest> requests = table.Rows.ToCreateApiResourceRequests();
            await this.SecurityServiceSteps.GivenTheFollowingApiResourcesExist(requests);
        }

        [Given(@"I create the following identity resources")]
        public async Task GivenICreateTheFollowingIdentityResources(DataTable table) {
            List<CreateIdentityResourceRequest> requests = table.Rows.ToCreateIdentityResourceRequest();
            List<CreateIdentityResourceResponse> responses = await this.SecurityServiceSteps.GivenICreateTheFollowingIdentityResources(requests, CancellationToken.None);
        }
        
        [Given(@"the following clients exist")]
        public async Task GivenTheFollowingClientsExist(DataTable table) {
            List<CreateClientRequest> requests = table.Rows.ToCreateClientRequests();
            List<(String clientId, String secret, List<String> allowedGrantTypes)> clients = await this.SecurityServiceSteps.GivenTheFollowingClientsExist(requests);
            foreach ((String clientId, String secret, List<String> allowedGrantTypes) client in clients)
            {
                this.TestingContext.AddClientDetails(client.clientId, client.secret, String.Join(",", client.allowedGrantTypes));
            }
        }

        [Given(@"I have a token to access the estate management resource")]
        [Given(@"I have a token to access the estate management and transaction processor resources")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementResource(DataTable table) {
            DataTableRow firstRow = table.Rows.First();
            String clientId = ReqnrollTableHelper.GetStringRowValue(firstRow, "ClientId");
            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            this.TestingContext.AccessToken = await this.SecurityServiceSteps.GetClientToken(clientDetails.ClientId,clientDetails.ClientSecret, CancellationToken.None);
        }

        [Given(@"I am logged in as ""(.*)"" with password ""(.*)"" for Estate ""(.*)"" with client ""(.*)""")]
        public async Task GivenIAmLoggedInAsWithPasswordForEstate(String username,
                                                                  String password,
                                                                  String estateName,
                                                                  String clientId) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            String tokenResponse = await this.SecurityServiceSteps
                                             .GetPasswordToken(clientId, clientDetails.ClientSecret, username, password, CancellationToken.None).ConfigureAwait(false);

            estateDetails.SetEstateUserToken(tokenResponse);
        }


        [When(@"I get the estate ""(.*)"" the estate details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateDetailsAreReturnedAsFollows(String estateName,
                                                                                DataTable table){
            List<String> estateDetails = table.Rows.ToEstateDetails();
            
            await this.EstateManagementSteps.WhenIGetTheEstateTheEstateDetailsAreReturnedAsFollows(this.TestingContext.AccessToken, estateName, this.TestingContext.Estates, estateDetails);
        }

        [When(@"I get the estate ""(.*)"" the estate operator details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateOperatorDetailsAreReturnedAsFollows(String estateName,
                                                                                        DataTable table){
            List<String> operators = table.Rows.ToOperatorDetails();
            await this.EstateManagementSteps.WhenIGetTheEstateTheEstateOperatorDetailsAreReturnedAsFollows(this.TestingContext.AccessToken, estateName, this.TestingContext.Estates, operators);
        }

        [When(@"I get the estate ""(.*)"" the estate security user details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateSecurityUserDetailsAreReturnedAsFollows(String estateName,
                                                                                            DataTable table) {

            List<String> securityUsers = table.Rows.ToSecurityUsersDetails();
            await this.EstateManagementSteps.WhenIGetTheEstateTheEstateSecurityUserDetailsAreReturnedAsFollows(this.TestingContext.AccessToken, estateName, this.TestingContext.Estates, securityUsers);
        }
        
        [Given(@"I have assigned the following devices to the merchants")]
        [When(@"I add the following devices to the merchant")]
        public async Task WhenIAddTheFollowingDevicesToTheMerchant(DataTable table) {
            List<(EstateDetails, Guid, AddMerchantDeviceRequest)> requests = table.Rows.ToAddMerchantDeviceRequests(this.TestingContext.Estates);

            List<(EstateDetails, MerchantResponse, String)> results = await this.EstateManagementSteps.GivenIHaveAssignedTheFollowingDevicesToTheMerchants(this.TestingContext.AccessToken, requests);
            foreach ((EstateDetails, MerchantResponse, String) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Device {result.Item3} assigned to Merchant {result.Item2.MerchantName} Estate {result.Item1.EstateName}");
            }
        }

        [When(@"I swap the merchant device the device is swapped")]
        public async Task WhenISwapTheMerchantDeviceTheDeviceIsSwapped(DataTable table) {
            var requests = table.Rows.ToSwapMerchantDeviceRequests(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenISwapTheMerchantDeviceTheDeviceIsSwapped(this.TestingContext.AccessToken, requests);
        }
        
        [When(@"I get the merchants for '(.*)' then (.*) merchants will be returned")]
        public async Task WhenIGetTheMerchantsForThenMerchantsWillBeReturned(String estateName,
                                                                             Int32 expectedMerchantCount){
            await this.EstateManagementSteps.WhenIGetTheMerchantsForThenMerchantsWillBeReturned(this.TestingContext.AccessToken,
                                                                                                estateName,
                                                                                                this.TestingContext.Estates,
                                                                                                expectedMerchantCount);
        }

        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(DataTable table) {
            List<(EstateDetails, CreateContractRequest)> requests = table.Rows.ToCreateContractRequests(this.TestingContext.Estates);
            List<ContractResponse> responses = await this.EstateManagementSteps.GivenICreateAContractWithTheFollowingValues(this.TestingContext.AccessToken, requests);
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(DataTable table) {
            List<(EstateDetails, Contract, AddProductToContractRequest)> requests = table.Rows.ToAddProductToContractRequest(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenICreateTheFollowingProducts(this.TestingContext.AccessToken, requests);
        }

        [When(@"I add the following Transaction Fees")]
        public async Task WhenIAddTheFollowingTransactionFees(DataTable table) {
            List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> requests = table.Rows.ToAddTransactionFeeForProductToContractRequests(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIAddTheFollowingTransactionFees(this.TestingContext.AccessToken, requests);
        }

        [When(@"I add the following contracts to the following merchants")]
        public async Task WhenIAddTheFollowingContractsToTheFollowingMerchants(DataTable table)
        {
            List<(EstateDetails, Guid, Guid)> requests = table.Rows.ToAddContractToMerchantRequests(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIAddTheFollowingContractsToTheFollowingMerchants(this.TestingContext.AccessToken, requests);
        }
        
        [Then(@"I get the Contracts for '(.*)' the following contract details are returned")]
        public async Task ThenIGetTheContractsForTheFollowingContractDetailsAreReturned(String estateName,
                                                                                        DataTable table){
            List<(String, String)> contractDetails = table.Rows.ToContractDetails();
            await this.EstateManagementSteps.ThenIGetTheContractsForTheFollowingContractDetailsAreReturned(this.TestingContext.AccessToken, estateName, this.TestingContext.Estates, contractDetails);
        }

        [Then(@"I get the Merchant Contracts for '(.*)' for '(.*)' the following contract details are returned")]
        public async Task ThenIGetTheMerchantContractsForForTheFollowingContractDetailsAreReturned(String merchantName,
                                                                                                   String estateName,
                                                                                                   DataTable table){
            List<(String, String)> contractDetails = table.Rows.ToContractDetails();
            await this.EstateManagementSteps.ThenIGetTheMerchantContractsForForTheFollowingContractDetailsAreReturned(this.TestingContext.AccessToken, estateName, merchantName, this.TestingContext.Estates, contractDetails);
        }

        [Then(@"I get the Transaction Fees for '(.*)' on the '(.*)' contract for '(.*)' the following fees are returned")]
        public async Task ThenIGetTheTransactionFeesForOnTheContractForTheFollowingFeesAreReturned(String productName,
                                                                                                   String contractName,
                                                                                                   String estateName,
                                                                                                   DataTable table) {
           List<(CalculationType, String, Decimal?, FeeType)> transactionFees = table.Rows.ToContractTransactionFeeDetails();
           await this.EstateManagementSteps.ThenIGetTheTransactionFeesForOnTheContractForTheFollowingFeesAreReturned(this.TestingContext.AccessToken,
                                                                                                                     estateName,
                                                                                                                     contractName,
                                                                                                                     productName,
                                                                                                                     this.TestingContext.Estates, transactionFees);

            
        }

        [When(@"I get the estate ""(.*)"" an error is returned")]
        public async Task WhenIGetTheEstateAnErrorIsReturned(String estateName) {
            await this.EstateManagementSteps.WhenIGetTheEstateAnErrorIsReturned(this.TestingContext.AccessToken, estateName, this.TestingContext.Estates);
        }

        [When(@"I get the merchant ""(.*)"" for estate ""(.*)"" an error is returned")]
        public async Task WhenIGetTheMerchantForEstateAnErrorIsReturned(String merchantName,
                                                                  String estateName) {
            await this.EstateManagementSteps.WhenIGetTheMerchantForEstateAnErrorIsReturned(this.TestingContext.AccessToken, estateName,merchantName,this.TestingContext.Estates);
        }

        [When(@"I set the merchants settlement schedule")]
        public async Task WhenISetTheMerchantsSettlementSchedule(DataTable table){
            List<(EstateDetails, Guid, SetSettlementScheduleRequest)> requests = table.Rows.ToSetSettlementScheduleRequests(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenISetTheMerchantsSettlementSchedule(this.TestingContext.AccessToken, requests);
        }

        [When(@"I make the following automatic merchant deposits")]
        public async Task WhenIMakeTheFollowingAutomaticMerchantDeposits(DataTable table){
            var results = table.Rows.ToAutomaticDepositRequests(this.TestingContext.Estates, DockerHelper.TestBankSortCode, DockerHelper.TestBankAccountNumber);
            await this.EstateManagementSteps.WhenIMakeTheFollowingAutomaticMerchantDeposits(results);
        }
        
        [When(@"I make the following manual merchant deposits the deposit is rejected")]
        [When(@"I make the following automatic merchant deposits the deposit is rejected")]
        public async Task WhenIMakeTheFollowingMerchantDepositsTheDepositIsRejected(DataTable table) {
            List<(EstateDetails, Guid, MakeMerchantDepositRequest)> requests = table.Rows.ToMakeMerchantDepositRequest(this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIMakeTheFollowingMerchantDepositsTheDepositIsRejected(this.TestingContext.AccessToken, requests);
        }

        [When(@"I perform the following transactions")]
        public async Task WhenIPerformTheFollowingTransactions(DataTable table) {
            List<(EstateDetails, Guid, String, SerialisedMessage)> serialisedMessages = TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions.ToSerialisedMessages(table.Rows, this.TestingContext.Estates);

            await this.TransactionProcessorSteps.WhenIPerformTheFollowingTransactions(this.TestingContext.AccessToken, serialisedMessages);
        }

        [Then(@"transaction response should contain the following information")]
        public void ThenTransactionResponseShouldContainTheFollowingInformation(DataTable table) {
            List<(SerialisedMessage, String, String, String)> transactions = TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions.GetTransactionDetails(table.Rows,this.TestingContext.Estates);
            this.TransactionProcessorSteps.ValidateTransactions(transactions);
        }
        
        [When(@"I get the pending settlements the following information should be returned")]
        public async Task WhenIGetThePendingSettlementsTheFollowingInformationShouldBeReturned(DataTable table) {
            List<(EstateDetails, Guid, DateTime, Int32)> requests = TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions.ToPendingSettlementRequests(table.Rows, this.TestingContext.Estates);
            await this.TransactionProcessorSteps.WhenIGetThePendingSettlementsTheFollowingInformationShouldBeReturned(this.TestingContext.AccessToken, requests);
        }

        [When(@"I process the settlement for '([^']*)' on Estate '([^']*)' for Merchant '([^']*)' then (.*) fees are marked as settled and the settlement is completed")]
        public async Task WhenIProcessTheSettlementForOnEstateForMerchantThenFeesAreMarkedAsSettledAndTheSettlementIsCompleted(String dateString,
                                                                                                                               String estateName,
                                                                                                                               String merchantName,
                                                                                                                               Int32 numberOfFeesSettled){
            TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions.ProcessSettlementRequest processSettlementRequest = TransactionProcessor.IntegrationTesting.Helpers.ReqnrollExtensions.ToProcessSettlementRequest(dateString, estateName, merchantName, this.TestingContext.Estates);
            await this.TransactionProcessorSteps.WhenIProcessTheSettlementForOnEstateThenFeesAreMarkedAsSettledAndTheSettlementIsCompleted(this.TestingContext.AccessToken, processSettlementRequest, numberOfFeesSettled);

        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' with the Start Date '([^']*)' and the End Date '([^']*)' the following data is returned")]
        public async Task WhenIGetTheEstateSettlementReportForEstateWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(string estateName,
                                                                                                                            string startDateString,
                                                                                                                            string endDateString,
                                                                                                                            Table table){
            DateTime stateDate = ReqnrollTableHelper.GetDateForDateString(startDateString, DateTime.UtcNow.Date);
            DateTime endDate = ReqnrollTableHelper.GetDateForDateString(endDateString, DateTime.UtcNow.Date);

            ReqnrollExtensions.SettlementDetails settlementDetails = table.Rows.ToSettlementDetails(estateName, this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(this.TestingContext.AccessToken, stateDate, endDate, settlementDetails);
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' for Merchant '([^']*)' with the Start Date '([^']*)' and the End Date '([^']*)' the following data is returned")]
        public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(string estateName,
            string merchantName,
            string startDateString,
            string endDateString,
            Table table){

            DateTime stateDate = ReqnrollTableHelper.GetDateForDateString(startDateString, DateTime.UtcNow.Date);
            DateTime endDate = ReqnrollTableHelper.GetDateForDateString(endDateString, DateTime.UtcNow.Date);
            ReqnrollExtensions.SettlementDetails settlementDetails = table.Rows.ToSettlementDetails(estateName, merchantName, this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(this.TestingContext.AccessToken,
                                                                                                                                                          stateDate, endDate,
                                                                                                                                                          settlementDetails);
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' for Merchant '([^']*)' with the Date '([^']*)' the following fees are settled")]
        public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheDateTheFollowingFeesAreSettled(string estateName,
            string merchantName,
            string settlementDateString,
            Table table){

            List<ReqnrollExtensions.SettlementFeeDetails> settlementFeeDetailsList = table.Rows.ToSettlementFeeDetails(estateName, merchantName, settlementDateString, this.TestingContext.Estates);
            await this.EstateManagementSteps.WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheDateTheFollowingFeesAreSettled(this.TestingContext.AccessToken, settlementFeeDetailsList);
        }
    }

    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Converts to datetime.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Guid guid) {
            var bytes = guid.ToByteArray();

            Array.Resize(ref bytes, 8);

            return new DateTime(BitConverter.ToInt64(bytes));
        }

        /// <summary>
        /// Converts to guid.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static Guid ToGuid(this DateTime dt) {
            var bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }

        #endregion
    }

    public static class Helpers
    {
        public static Guid CalculateSettlementAggregateId(DateTime settlementDate,
                                                          Guid merchantId,
                                                          Guid estateId) {
            Guid aggregateId = GuidCalculator.Combine(estateId, merchantId, settlementDate.ToGuid());
            return aggregateId;
        }
    }

    public static class GuidCalculator
    {
        #region Methods

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Byte offset) {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++) {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid) {
            return GuidCalculator.Combine(firstGuid, secondGuid, 0);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid,
                                   Byte offset) {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();
            Byte[] thirdAsBytes = thirdGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++) {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + thirdAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid) {
            return GuidCalculator.Combine(firstGuid, secondGuid, thirdGuid, 0);
        }

        #endregion
    }
}

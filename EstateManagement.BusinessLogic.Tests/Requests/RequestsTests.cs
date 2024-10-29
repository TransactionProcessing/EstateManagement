namespace EstateManagement.BusinessLogic.Tests.Commands
{
    using Models;
    using Models.Contract;
    using Requests;
    using Shouldly;
    using Testing;
    using Xunit;

    public class RequestsTests
    {
        #region Methods
        
        [Fact]
        public void AddSettledFeeToMerchantStatementRequest_CanBeCreated_IsCreated() {
            AddSettledFeeToMerchantStatementRequest addSettledFeeToMerchantStatementRequest =
                AddSettledFeeToMerchantStatementRequest.Create(TestData.EstateId,
                                                               TestData.MerchantId,
                                                               TestData.SettledFeeDateTime1,
                                                               TestData.SettledFeeAmount1,
                                                               TestData.TransactionId1,
                                                               TestData.SettledFeeId1);

            addSettledFeeToMerchantStatementRequest.ShouldNotBeNull();
            addSettledFeeToMerchantStatementRequest.SettledDateTime.ShouldBe(TestData.SettledFeeDateTime1);
            addSettledFeeToMerchantStatementRequest.SettledFeeId.ShouldBe(TestData.SettledFeeId1);
            addSettledFeeToMerchantStatementRequest.SettledAmount.ShouldBe(TestData.SettledFeeAmount1);
            addSettledFeeToMerchantStatementRequest.TransactionId.ShouldBe(TestData.TransactionId1);
            addSettledFeeToMerchantStatementRequest.EstateId.ShouldBe(TestData.EstateId);
            addSettledFeeToMerchantStatementRequest.MerchantId.ShouldBe(TestData.MerchantId);
        }

        

        [Fact]
        public void AddTransactionToMerchantStatementRequest_CanBeCreated_IsCreated() {
            AddTransactionToMerchantStatementRequest addTransactionToMerchantStatementRequest =
                AddTransactionToMerchantStatementRequest.Create(TestData.EstateId,
                                                                TestData.MerchantId,
                                                                TestData.TransactionDateTime1,
                                                                TestData.TransactionAmount1,
                                                                TestData.IsAuthorisedTrue,
                                                                TestData.TransactionId1);

            addTransactionToMerchantStatementRequest.ShouldNotBeNull();
            addTransactionToMerchantStatementRequest.IsAuthorised.ShouldBe(TestData.IsAuthorisedTrue);
            addTransactionToMerchantStatementRequest.TransactionAmount.ShouldBe(TestData.TransactionAmount1);
            addTransactionToMerchantStatementRequest.TransactionDateTime.ShouldBe(TestData.TransactionDateTime1);
            addTransactionToMerchantStatementRequest.TransactionId.ShouldBe(TestData.TransactionId1);
            addTransactionToMerchantStatementRequest.EstateId.ShouldBe(TestData.EstateId);
            addTransactionToMerchantStatementRequest.MerchantId.ShouldBe(TestData.MerchantId);
        }

        
        [Fact]
        public void EmailMerchantStatementRequest_CanBeCreated_IsCreated(){
            EmailMerchantStatementRequest emailMerchantStatementRequest =
                EmailMerchantStatementRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.MerchantStatementId);

            emailMerchantStatementRequest.ShouldNotBeNull();
            emailMerchantStatementRequest.EstateId.ShouldBe(TestData.EstateId);
            emailMerchantStatementRequest.MerchantId.ShouldBe(TestData.MerchantId);
            emailMerchantStatementRequest.MerchantStatementId.ShouldBe(TestData.MerchantStatementId);
        }

        #endregion
    }
}
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
        public void AddOperatorToEstateRequest_CanBeCreated_IsCreated() {
            AddOperatorToEstateRequest addOperatorToEstateRequest = AddOperatorToEstateRequest.Create(TestData.EstateId,
                                                                                                      TestData.OperatorId,
                                                                                                      TestData.OperatorName,
                                                                                                      TestData.RequireCustomMerchantNumberTrue,
                                                                                                      TestData.RequireCustomTerminalNumberTrue);

            addOperatorToEstateRequest.ShouldNotBeNull();
            addOperatorToEstateRequest.EstateId.ShouldBe(TestData.EstateId);
            addOperatorToEstateRequest.OperatorId.ShouldBe(TestData.OperatorId);
            addOperatorToEstateRequest.Name.ShouldBe(TestData.OperatorName);
            addOperatorToEstateRequest.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberTrue);
            addOperatorToEstateRequest.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberTrue);
        }

        [Fact]
        public void AddProductToContractRequest_WithFixedValue_CanBeCreated_IsCreated() {
            AddProductToContractRequest addProductToContractRequest =
                AddProductToContractRequest.Create(TestData.ContractId,
                                                   TestData.EstateId,
                                                   TestData.ProductId,
                                                   TestData.ProductName,
                                                   TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue, TestData.ProductTypeMobileTopup);

            addProductToContractRequest.ShouldNotBeNull();
            addProductToContractRequest.ContractId.ShouldBe(TestData.ContractId);
            addProductToContractRequest.EstateId.ShouldBe(TestData.EstateId);
            addProductToContractRequest.ProductId.ShouldBe(TestData.ProductId);
            addProductToContractRequest.ProductName.ShouldBe(TestData.ProductName);
            addProductToContractRequest.DisplayText.ShouldBe(TestData.ProductDisplayText);
            addProductToContractRequest.Value.ShouldBe(TestData.ProductFixedValue);
            addProductToContractRequest.ProductType.ShouldBe(TestData.ProductTypeMobileTopup);
        }

        [Fact]
        public void AddProductToContractRequest_WithVariableValue_CanBeCreated_IsCreated() {
            AddProductToContractRequest addProductToContractRequest =
                AddProductToContractRequest.Create(TestData.ContractId, TestData.EstateId, TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, null, TestData.ProductTypeMobileTopup);

            addProductToContractRequest.ShouldNotBeNull();
            addProductToContractRequest.ContractId.ShouldBe(TestData.ContractId);
            addProductToContractRequest.EstateId.ShouldBe(TestData.EstateId);
            addProductToContractRequest.ProductId.ShouldBe(TestData.ProductId);
            addProductToContractRequest.ProductName.ShouldBe(TestData.ProductName);
            addProductToContractRequest.DisplayText.ShouldBe(TestData.ProductDisplayText);
            addProductToContractRequest.Value.ShouldBeNull();
            addProductToContractRequest.ProductType.ShouldBe(TestData.ProductTypeMobileTopup);
        }

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

        [Theory]
        [InlineData(CalculationType.Percentage, FeeType.Merchant)]
        [InlineData(CalculationType.Fixed, FeeType.Merchant)]
        [InlineData(CalculationType.Percentage, FeeType.ServiceProvider)]
        [InlineData(CalculationType.Fixed, FeeType.ServiceProvider)]
        public void AddTransactionFeeForProductToContractRequest_CanBeCreated_IsCreated(CalculationType calculationType,
                                                                                        FeeType feeType) {
            AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest =
                AddTransactionFeeForProductToContractRequest.Create(TestData.ContractId,
                                                                    TestData.EstateId,
                                                                    TestData.ProductId,
                                                                    TestData.TransactionFeeId,
                                                                    TestData.TransactionFeeDescription,
                                                                    calculationType,
                                                                    feeType,
                                                                    TestData.TransactionFeeValue);

            addTransactionFeeForProductToContractRequest.ShouldNotBeNull();
            addTransactionFeeForProductToContractRequest.ContractId.ShouldBe(TestData.ContractId);
            addTransactionFeeForProductToContractRequest.EstateId.ShouldBe(TestData.EstateId);
            addTransactionFeeForProductToContractRequest.ProductId.ShouldBe(TestData.ProductId);
            addTransactionFeeForProductToContractRequest.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
            addTransactionFeeForProductToContractRequest.Description.ShouldBe(TestData.TransactionFeeDescription);
            addTransactionFeeForProductToContractRequest.CalculationType.ShouldBe(calculationType);
            addTransactionFeeForProductToContractRequest.FeeType.ShouldBe(feeType);
            addTransactionFeeForProductToContractRequest.Value.ShouldBe(TestData.TransactionFeeValue);
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
        public void CreateContractRequest_CanBeCreated_IsCreated() {
            CreateContractRequest createContractRequest =
                CreateContractRequest.Create(TestData.ContractId, TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            createContractRequest.ShouldNotBeNull();
            createContractRequest.ContractId.ShouldBe(TestData.ContractId);
            createContractRequest.EstateId.ShouldBe(TestData.EstateId);
            createContractRequest.OperatorId.ShouldBe(TestData.OperatorId);
            createContractRequest.Description.ShouldBe(TestData.ContractDescription);
        }
        
        [Fact]
        public void DisableTransactionFeeForProductRequest_CanBeCreated_IsCreated() {
            DisableTransactionFeeForProductRequest disableTransactionFeeForProductRequest =
                DisableTransactionFeeForProductRequest.Create(TestData.ContractId, TestData.EstateId, TestData.ProductId, TestData.TransactionFeeId);

            disableTransactionFeeForProductRequest.ShouldNotBeNull();
            disableTransactionFeeForProductRequest.ContractId.ShouldBe(TestData.ContractId);
            disableTransactionFeeForProductRequest.EstateId.ShouldBe(TestData.EstateId);
            disableTransactionFeeForProductRequest.ProductId.ShouldBe(TestData.ProductId);
            disableTransactionFeeForProductRequest.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
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
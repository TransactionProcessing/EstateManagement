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
        public void AddMerchantDeviceRequest_CanBeCreted_IsCreated() {
            AddMerchantDeviceRequest addMerchantDeviceRequest =
                AddMerchantDeviceRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.DeviceId, TestData.DeviceIdentifier);

            addMerchantDeviceRequest.ShouldNotBeNull();
            addMerchantDeviceRequest.EstateId.ShouldBe(TestData.EstateId);
            addMerchantDeviceRequest.MerchantId.ShouldBe(TestData.MerchantId);
            addMerchantDeviceRequest.DeviceId.ShouldBe(TestData.DeviceId);
            addMerchantDeviceRequest.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        }

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
        public void AssignOperatorToMerchantRequest_CanBeCreated_IsCreated() {
            AssignOperatorToMerchantRequest assignOperatorToMerchantRequest = AssignOperatorToMerchantRequest.Create(TestData.EstateId,
                TestData.MerchantId,
                TestData.OperatorId,
                TestData.OperatorMerchantNumber,
                TestData.OperatorTerminalNumber);

            assignOperatorToMerchantRequest.ShouldNotBeNull();
            assignOperatorToMerchantRequest.EstateId.ShouldBe(TestData.EstateId);
            assignOperatorToMerchantRequest.MerchantId.ShouldBe(TestData.MerchantId);
            assignOperatorToMerchantRequest.OperatorId.ShouldBe(TestData.OperatorId);
            assignOperatorToMerchantRequest.MerchantNumber.ShouldBe(TestData.OperatorMerchantNumber);
            assignOperatorToMerchantRequest.TerminalNumber.ShouldBe(TestData.OperatorTerminalNumber);
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
        public void CreateEstateRequest_CanBeCreated_IsCreated() {
            CreateEstateRequest createEstateRequest = CreateEstateRequest.Create(TestData.EstateId, TestData.EstateName);

            createEstateRequest.ShouldNotBeNull();
            createEstateRequest.EstateId.ShouldBe(TestData.EstateId);
            createEstateRequest.Name.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public void CreateEstateUserRequest_CanBeCreated_IsCreated() {
            CreateEstateUserRequest createEstateUserRequest = CreateEstateUserRequest.Create(TestData.EstateId,
                                                                                             TestData.EstateUserEmailAddress,
                                                                                             TestData.EstateUserPassword,
                                                                                             TestData.EstateUserGivenName,
                                                                                             TestData.EstateUserMiddleName,
                                                                                             TestData.EstateUserFamilyName);

            createEstateUserRequest.ShouldNotBeNull();
            createEstateUserRequest.EstateId.ShouldBe(TestData.EstateId);
            createEstateUserRequest.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
            createEstateUserRequest.Password.ShouldBe(TestData.EstateUserPassword);
            createEstateUserRequest.GivenName.ShouldBe(TestData.EstateUserGivenName);
            createEstateUserRequest.MiddleName.ShouldBe(TestData.EstateUserMiddleName);
            createEstateUserRequest.FamilyName.ShouldBe(TestData.EstateUserFamilyName);
        }

        [Fact]
        public void CreateMerchantRequest_CanBeCreated_IsCreated() {
            CreateMerchantRequest createMerchantRequest = CreateMerchantRequest.Create(TestData.EstateId,
                                                                                       TestData.MerchantId,
                                                                                       TestData.MerchantName,
                                                                                       TestData.MerchantAddressLine1,
                                                                                       TestData.MerchantAddressLine2,
                                                                                       TestData.MerchantAddressLine3,
                                                                                       TestData.MerchantAddressLine4,
                                                                                       TestData.MerchantTown,
                                                                                       TestData.MerchantRegion,
                                                                                       TestData.MerchantPostalCode,
                                                                                       TestData.MerchantCountry,
                                                                                       TestData.MerchantContactName,
                                                                                       TestData.MerchantContactPhoneNumber,
                                                                                       TestData.MerchantContactEmailAddress,
                                                                                       TestData.SettlementSchedule,
                                                                                       TestData.DateMerchantCreated);

            createMerchantRequest.ShouldNotBeNull();
            createMerchantRequest.EstateId.ShouldBe(TestData.EstateId);
            createMerchantRequest.MerchantId.ShouldBe(TestData.MerchantId);
            createMerchantRequest.Name.ShouldBe(TestData.MerchantName);
            createMerchantRequest.AddressLine1.ShouldBe(TestData.MerchantAddressLine1);
            createMerchantRequest.AddressLine2.ShouldBe(TestData.MerchantAddressLine2);
            createMerchantRequest.AddressLine3.ShouldBe(TestData.MerchantAddressLine3);
            createMerchantRequest.AddressLine4.ShouldBe(TestData.MerchantAddressLine4);
            createMerchantRequest.Town.ShouldBe(TestData.MerchantTown);
            createMerchantRequest.Region.ShouldBe(TestData.MerchantRegion);
            createMerchantRequest.PostalCode.ShouldBe(TestData.MerchantPostalCode);
            createMerchantRequest.Country.ShouldBe(TestData.MerchantCountry);
            createMerchantRequest.ContactName.ShouldBe(TestData.MerchantContactName);
            createMerchantRequest.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
            createMerchantRequest.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
            createMerchantRequest.SettlementSchedule.ShouldBe(TestData.SettlementSchedule);
            createMerchantRequest.CreateDateTime.ShouldBe(TestData.DateMerchantCreated);
        }

        [Fact]
        public void CreateMerchantUserRequest_CanBeCreated_IsCreated() {
            CreateMerchantUserRequest createMerchantUserRequest = CreateMerchantUserRequest.Create(TestData.EstateId,
                                                                                                   TestData.MerchantId,
                                                                                                   TestData.EstateUserEmailAddress,
                                                                                                   TestData.EstateUserPassword,
                                                                                                   TestData.EstateUserGivenName,
                                                                                                   TestData.EstateUserMiddleName,
                                                                                                   TestData.EstateUserFamilyName);

            createMerchantUserRequest.ShouldNotBeNull();
            createMerchantUserRequest.EstateId.ShouldBe(TestData.EstateId);
            createMerchantUserRequest.MerchantId.ShouldBe(TestData.MerchantId);
            createMerchantUserRequest.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
            createMerchantUserRequest.Password.ShouldBe(TestData.EstateUserPassword);
            createMerchantUserRequest.GivenName.ShouldBe(TestData.EstateUserGivenName);
            createMerchantUserRequest.MiddleName.ShouldBe(TestData.EstateUserMiddleName);
            createMerchantUserRequest.FamilyName.ShouldBe(TestData.EstateUserFamilyName);
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
        public void GenerateMerchantStatementRequest_CanBeCreated_IsCreated() {
            GenerateMerchantStatementRequest generateMerchantStatementRequest =
                GenerateMerchantStatementRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);

            generateMerchantStatementRequest.ShouldNotBeNull();
            generateMerchantStatementRequest.EstateId.ShouldBe(TestData.EstateId);
            generateMerchantStatementRequest.MerchantId.ShouldBe(TestData.MerchantId);
            generateMerchantStatementRequest.StatementDate.ShouldBe(TestData.StatementCreateDate);
        }

        [Theory]
        [InlineData(MerchantDepositSource.Manual)]
        [InlineData(MerchantDepositSource.Automatic)]
        public void MakeMerchantDepositRequest_CanBeCreated_IsCreated(MerchantDepositSource merchantDepositSource) {
            MakeMerchantDepositRequest makeMerchantDepositRequest = MakeMerchantDepositRequest.Create(TestData.EstateId,
                                                                                                      TestData.MerchantId,
                                                                                                      merchantDepositSource,
                                                                                                      TestData.DepositReference,
                                                                                                      TestData.DepositDateTime,
                                                                                                      TestData.DepositAmount.Value);

            makeMerchantDepositRequest.ShouldNotBeNull();
            makeMerchantDepositRequest.EstateId.ShouldBe(TestData.EstateId);
            makeMerchantDepositRequest.MerchantId.ShouldBe(TestData.MerchantId);
            makeMerchantDepositRequest.Source.ShouldBe(merchantDepositSource);
            makeMerchantDepositRequest.Amount.ShouldBe(TestData.DepositAmount.Value);
            makeMerchantDepositRequest.DepositDateTime.ShouldBe(TestData.DepositDateTime);
            makeMerchantDepositRequest.Reference.ShouldBe(TestData.DepositReference);
        }

        [Fact]
        public void MakeMerchantWithdrawalRequest_CanBeCreated_IsCreated() {
            MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest = MakeMerchantWithdrawalRequest.Create(TestData.EstateId,
                                                                                                               TestData.MerchantId,
                                                                                                               TestData.WithdrawalDateTime,
                                                                                                               TestData.WithdrawalAmount.Value);

            makeMerchantWithdrawalRequest.ShouldNotBeNull();
            makeMerchantWithdrawalRequest.EstateId.ShouldBe(TestData.EstateId);
            makeMerchantWithdrawalRequest.MerchantId.ShouldBe(TestData.MerchantId);
            makeMerchantWithdrawalRequest.Amount.ShouldBe(TestData.WithdrawalAmount.Value);
            makeMerchantWithdrawalRequest.WithdrawalDateTime.ShouldBe(TestData.WithdrawalDateTime);
        }

        [Fact]
        public void SetMerchantSettlementScheduleRequest_CanBeCreated_IsCreated() {
            SetMerchantSettlementScheduleRequest setMerchantSettlementScheduleRequest =
                SetMerchantSettlementScheduleRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.SettlementSchedule);

            setMerchantSettlementScheduleRequest.ShouldNotBeNull();
            setMerchantSettlementScheduleRequest.EstateId.ShouldBe(TestData.EstateId);
            setMerchantSettlementScheduleRequest.MerchantId.ShouldBe(TestData.MerchantId);
            setMerchantSettlementScheduleRequest.SettlementSchedule.ShouldBe(TestData.SettlementSchedule);
        }

        [Fact]
        public void EmailMerchantStatementRequest_CanBeCreated_IsCreated()
        {
            EmailMerchantStatementRequest emailMerchantStatementRequest =
                EmailMerchantStatementRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.MerchantStatementId);

            emailMerchantStatementRequest.ShouldNotBeNull();
            emailMerchantStatementRequest.EstateId.ShouldBe(TestData.EstateId);
            emailMerchantStatementRequest.MerchantId.ShouldBe(TestData.MerchantId);
            emailMerchantStatementRequest.MerchantStatementId.ShouldBe(TestData.MerchantStatementId);
        }

        [Fact]
        public void AddContractToMerchantRequest_CanBeCreated_IsCreated(){
            AddMerchantContractRequest addMerchantContractRequest = AddMerchantContractRequest.Create(TestData.EstateId,
                                                                                                      TestData.MerchantId,
                                                                                                      TestData.ContactId);
            addMerchantContractRequest.ShouldNotBeNull();
            addMerchantContractRequest.EstateId.ShouldBe(TestData.EstateId);
            addMerchantContractRequest.MerchantId.ShouldBe(TestData.MerchantId);
            addMerchantContractRequest.ContractId.ShouldBe(TestData.ContactId);
        }

        #endregion
    }
}
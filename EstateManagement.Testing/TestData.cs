namespace EstateManagement.Testing{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BusinessLogic.Events;
    using BusinessLogic.Requests;
    using Contract.DomainEvents;
    using ContractAggregate;
    using Database.Entities;
    using DataTransferObjects.Requests.Estate;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Requests.Operator;
    using Estate.DomainEvents;
    using EstateAggregate;
    using FileProcessor.File.DomainEvents;
    using FileProcessor.FileImportLog.DomainEvents;
    using Merchant.DomainEvents;
    using MerchantAggregate;
    using MerchantStatement.DomainEvents;
    using MerchantStatementAggregate;
    using Models;
    using Models.Contract;
    using Models.File;
    using Models.Merchant;
    using Models.MerchantStatement;
    using Newtonsoft.Json;
    using Operator.DomainEvents;
    using OperatorAggregate;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.ValueObjects;
    using TransactionProcessor.DataTransferObjects;
    using TransactionProcessor.Float.DomainEvents;
    using TransactionProcessor.Reconciliation.DomainEvents;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;
    using Address = DataTransferObjects.Requests.Merchant.Address;
    using AssignOperatorRequest = DataTransferObjects.Requests.Merchant.AssignOperatorRequest;
    using Contact = DataTransferObjects.Requests.Merchant.Contact;
    using Contract = Database.Entities.Contract;
    using Deposit = CallbackHandler.DataTransferObjects.Deposit;
    using Estate = Database.Entities.Estate;
    using OperatorEntity = Database.Entities.Operator;
    using File = Models.File.File;
    using Merchant = Database.Entities.Merchant;
    using MerchantDepositSource = Models.MerchantDepositSource;
    using Operator = Models.Estate.Operator;
    using Transaction = Models.File.Transaction;
    using TransactionFeeModel = Models.Contract.TransactionFee;

    public class TestData{
        #region Fields

        /// <summary>
        /// The address identifier
        /// </summary>
        public static Guid AddressId = Guid.Parse("B1C68246-F867-43CC-ACA9-37D15D6437C6");

        /// <summary>
        /// The address line1
        /// </summary>
        public static String AddressLine1 = "AddressLine1";

        /// <summary>
        /// The address line2
        /// </summary>
        public static String AddressLine2 = "AddressLine2";

        /// <summary>
        /// The address line3
        /// </summary>
        public static String AddressLine3 = "AddressLine3";

        /// <summary>
        /// The address line4
        /// </summary>
        public static String AddressLine4 = "AddressLine4";

        public static String AuthorisationCode = "ABCD1234";

        public static Decimal AvailableBalance = 1000.00m;

        public static Decimal AvailableBalance2 = 2000.00m;

        public static Decimal Balance = 1000.00m;

        public static Decimal Balance2 = 2200.00m;

        public static Guid BalanceEventId1 = Guid.Parse("269331F9-B20E-4E20-9D75-FDE4CE014EF2");

        public static Guid BalanceEventId2 = Guid.Parse("BC604870-3451-4BBE-B798-D1B21E3E9996");

        public static String BalanceRecordReference = "Transaction Completed";

        public static String BalanceReference1 = "Transaction Completed";

        public static String BalanceReference2 = "Transaction Fee";

        public static Decimal CalculatedValue = 2.95m;

        public static Guid CallbackId = Guid.Parse("ABC603D3-360E-4F58-8BB9-827EE7A1CB03");

        public static String CallbackMessage = "Message1";

        public static Int32 CallbackMessageFormat = 1;

        public static String CallbackReference = "Estate1-Merchant1";

        public static String CallbackTypeString = "CallbackHandler.DataTransferObjects.Deposit";

        public static Decimal ChangeAmount = 100.00m;

        public static Decimal ChangeAmount1 = -10m;

        public static Decimal ChangeAmount2 = -0.05m;

        /// <summary>
        /// The contact email
        /// </summary>
        public static String ContactEmail = "testcontact1@testmerchant1.co.uk";

        public static String ContactEmailUpdate = "testcontact1@testmerchant1.com";

        /// <summary>
        /// The contact identifier
        /// </summary>
        public static Guid ContactId = Guid.Parse("B1C68246-F867-43CC-ACA9-37D15D6437C6");

        /// <summary>
        /// The contact name
        /// </summary>
        public static String ContactName = "Test Contact";

        public static String ContactNameUpdate = "Test Contact Update";

        /// <summary>
        /// The contact phone
        /// </summary>
        public static String ContactPhone = "123456789";

        public static String ContactPhoneUpdate = "1234567890";

        public static String ContractDescription = "Test Contract";

        public static String ContractDescription2 = "Test Contract 2";

        public static Guid ContractId = Guid.Parse("3C50EDAB-0718-4666-8BEB-1BD5BF08E1D7");

        public static Guid ContractId2 = Guid.Parse("086C2FC0-DB29-4A75-8983-3A3A78628A2A");

        public static String ProductDisplayText = "100 KES";

        public static Decimal ProductFixedValue = 100.00m;

        public static Guid ProductId3 = Guid.Parse("08657CB3-3737-4113-8BB1-C8118B3EEA06");

        public static String ProductName = "Product 1";

        public static ContractProduct ContractProductEntity3 = new ContractProduct{
                                                                                      Value = TestData.ProductFixedValue,
                                                                                      ProductId = TestData.ProductId3,
                                                                                      ProductName = TestData.ProductName,
                                                                                      DisplayText = TestData.ProductDisplayText
                                                                                  };

        /// <summary>
        /// The country
        /// </summary>
        public static String Country = "Country";

        public static String CurrencyCode = "KES";

        public static DateTime DateMerchantCreated = new DateTime(2019, 11, 16);

        public static String DeclinedOperatorResponseCode = "400";

        public static String DeclinedOperatorResponseMessage = "Topup Failed";

        public static String DeclinedResponseCode = "0001";

        public static String DeclinedResponseMessage = "DeclinedResponseMessage";

        public static String DepositAccountNumber = "12345678";

        public static PositiveMoney DepositAmount = PositiveMoney.Create(Money.Create(1000.00m));

        public static PositiveMoney DepositAmount2 = PositiveMoney.Create(Money.Create(1200.00m));

        public static DateTime DepositDateTime = new DateTime(2019, 11, 16);

        public static DateTime DepositDateTime2 = new DateTime(2019, 11, 16);

        public static Guid DepositHostIdentifier = Guid.Parse("1D1BD9F0-D953-4B2A-9969-98D3C0CDFA2A");

        public static Guid DepositId = Guid.Parse("A15460B1-9665-4C3E-861D-3B65D0EBEF19");

        public static String DepositReference = "Test Deposit 1";

        public static String DepositReference2 = "Test Deposit 2";

        public static String DepositSortCode = "112233";

        public static Guid DeviceId = Guid.Parse("B434EA1A-1684-442F-8BEB-21D84C4F53B3");

        public static String DeviceIdentifier = "EMULATOR123456";

        /// <summary>
        /// The email address
        /// </summary>
        public static String EmailAddress = "testuser1@testestate1.co.uk";

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static EstateAggregate EmptyEstateAggregate = EstateAggregate.Create(TestData.EstateId);

        public static MerchantDepositListAggregate EmptyMerchantDepositListAggregate = new MerchantDepositListAggregate();

        public static String EndDate = "20210105";

        public static DateTime EntryDateTime = new DateTime(2021, 2, 18);

        public static String EntryType1 = "D";

        public static String EntryType2 = "C";

        public static String EstateName = "Test Estate 1";

        public static String EstateReference = "C6634DE3";

        /// <summary>
        /// The security user identifier
        /// </summary>
        public static Guid EstateSecurityUserId = Guid.Parse("CBEE25E6-1B08-4023-B20C-CFE0AD746808");

        /// <summary>
        /// The estate security user added event
        /// </summary>
        public static SecurityUserAddedToEstateEvent EstateSecurityUserAddedEvent =
            new SecurityUserAddedToEstateEvent(TestData.EstateId, TestData.EstateSecurityUserId, TestData.EmailAddress);

        public static String EstateUserEmailAddress = "testestateuser@estate1.co.uk";

        public static String EstateUserFamilyName = "Estate";

        public static String EstateUserGivenName = "Test";

        public static String EstateUserMiddleName = "Middle";

        public static String EstateUserPassword = "123456";

        public static Guid EventId1 = Guid.Parse("C8CC622C-07D9-48E9-B544-F53BD29DE1E6");

        public static DateTime FeeCalculatedDateTime = new DateTime(2021, 3, 23);

        public static Int32 FeeCalculationType = 0;

        public static Int32 FeeType = 0;

        public static Decimal FeeValue = 0.0005m;

        public static Guid FileId = Guid.Parse("5F7F45D6-0604-46C7-AA88-EAA885A6B208");

        public static Guid FileImportLogId = Guid.Parse("5F1149F8-0313-45E4-BE3A-3D7B07EEB414");

        public static String FileLine = "D,124567,100";

        public static String FileLocation = "home/txnproc/bulkfiles/safaricom/ExampleFile.csv";

        public static String FilePath = "home/txnproc/bulkfiles";

        public static Guid FileProfileId = Guid.Parse("D0D3A4E5-870E-42F6-AD0E-5E24252BC95E");

        public static DateTime FileReceivedDate = new DateTime(2021, 12, 16);

        public static DateTime FileReceivedDateTime = new DateTime(2021, 12, 16, 1, 2, 3);

        public static DateTime FileUploadedDateTime = new DateTime(2021, 5, 7);

        public static DateTime FloatCreatedDateTime = new DateTime(2024, 3, 19);

        public static Decimal FloatCreditAmount = 100m;

        public static Decimal FloatCreditCostPrice = 90m;

        public static DateTime FloatCreditPurchsedDateTime = new DateTime(2024, 3, 19);

        public static Guid FloatId = Guid.Parse("470EC52A-7522-4D5C-A942-FC8C65C1E0B2");

        public static DateTime HistoryEndDate = new DateTime(2021, 8, 30);

        public static DateTime HistoryStartDate = new DateTime(2021, 8, 29);

        public static DateTime ImportLogDateTime = new DateTime(2021, 5, 7);

        public static Boolean IsAuthorised = true;

        public static Boolean IsAuthorisedFalse = false;

        public static Boolean IsAuthorisedTrue = true;

        public static Boolean IsSettled = true;

        public static Int32 LineNumber = 1;

        public static String MerchantAddressLine1 = "Address Line 1";

        public static String MerchantAddressLine1Update = "Address Line 1 Update";

        public static String MerchantAddressLine2 = "Address Line 2";

        public static String MerchantAddressLine2Update = "Address Line 2 Update";

        public static String MerchantAddressLine3 = "Address Line 3";

        public static String MerchantAddressLine3Update = "Address Line 3 Update";

        public static String MerchantAddressLine4 = "Address Line 4";

        public static String MerchantAddressLine4Update = "Address Line 4 Update";

        public static String MerchantContactEmailAddress = "testcontact@merchant1.co.uk";

        public static String MerchantContactName = "Mr Test Contact";

        public static String MerchantContactPhoneNumber = "1234567890";

        public static MerchantContact MerchantContactEntity = new MerchantContact{
                                                                                     ContactId = Guid.NewGuid(),
                                                                                     Name = TestData.MerchantContactName,
                                                                                     EmailAddress = TestData.MerchantContactEmailAddress,
                                                                                     PhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                     CreatedDateTime = TestData.DateMerchantCreated
                                                                                 };

        public static String MerchantCountry = "United Kingdom";

        public static String MerchantCountryUpdate = "United Kingdom Update";

        public static MerchantDepositSource MerchantDepositSourceAutomatic = MerchantDepositSource.Automatic;

        public static MerchantDepositSource MerchantDepositSourceManual = MerchantDepositSource.Manual;

        public static Guid MerchantId = Guid.Parse("ac476195-f993-4712-8ea1-cb41c0b44328");

        public static String MerchantName = "Test Merchant 1";

        public static String MerchantNameUpdated = "Test Merchant 1 Updated";

        public static String MerchantNumber = "12345678";

        public static String MerchantPostalCode = "TE571NG";

        public static String MerchantPostalCodeUpdate = "TE571NGUpdate";

        public static String MerchantReference = "82DD8D48";

        public static String MerchantRegion = "Test Region";

        public static String MerchantRegionUpdate = "Test Region Update";

        public static Guid MerchantSecurityUserId = Guid.Parse("DFCE7A95-CB6D-442A-928A-F1B41D2AA4A9");

        public static Guid MerchantStatementId = Guid.Parse("C8CC622C-07D9-48E9-B544-F53BD29DE1E6");

        public static String MerchantTown = "Test Town";

        public static String MerchantTownUpdate = "Test Town Update";

        public static String MerchantUserEmailAddress = "testmerchantuser@merchant1.co.uk";

        public static String MerchantUserFamilyName = "Merchant";

        public static String MerchantUserGivenName = "Test";

        public static String MerchantUserMiddleName = "Middle";

        public static String MerchantUserPassword = "123456";

        public static Guid MessageId = Guid.Parse("353FB307-FDD5-41AE-A2AF-C927D57EADBB");

        public static String NewDeviceIdentifier = "EMULATOR78910";

        public static DateTime NextSettlementDate = new DateTime(2021, 8, 30);

        public static Int32 NumberOfFeesSettled = 1;

        public static String OperatorAuthorisationCode = "OP1234";

        public static Guid OperatorId = Guid.Parse("6A63DA8B-4621-4731-90B1-A9D09B130D4B");

        public static Guid OperatorId2 = Guid.Parse("8E5741AA-66EC-42D9-BE0F-AA106B41AED1");

        public static String OperatorIdentifier = "Voucher";

        public static String OperatorMerchantNumber = "00000001";

        public static String OperatorName = "Test Operator 1";

        public static String OperatorName2 = "Test Operator Name 2";

        public static String OperatorResponseCode = "200";

        public static String OperatorResponseMessage = "Topup Successful";

        public static String OperatorTerminalNumber = "00000001";

        public static String OperatorTransactionId = "SF12345";

        public static String OriginalFileName = "ExampleFile.csv";

        /// <summary>
        /// The post code
        /// </summary>
        public static String PostCode = "PostCode";

        public static DateTime ProcessingCompletedDateTime = new DateTime(2021, 5, 7);

        public static DateTime ProcessingStartedDateTime = new DateTime(2024, 3, 19);

        public static Guid ProductId = Guid.Parse("C6309D4C-3182-4D96-AEEA-E9DBBB9DED8F");

        public static Guid ProductId2 = Guid.Parse("642522E4-05F1-4218-9739-18211930F489");

        public static String ProductName2 = "Product 2";

        public static ProductType ProductTypeBillPayment = ProductType.BillPayment;

        public static ProductType ProductTypeMobileTopup = ProductType.MobileTopup;

        public static ProductType ProductTypeVoucher = ProductType.Voucher;

        public static String RecipientEmail = "testemail@recipient.co.uk";

        public static String RecipientMobile = "123455679";

        /// <summary>
        /// The reconcilation transaction count
        /// </summary>
        public static Int32 ReconcilationTransactionCount = 1;

        /// <summary>
        /// The reconcilation transaction value
        /// </summary>
        public static Decimal ReconcilationTransactionValue = 100.00m;

        /// <summary>
        /// The region
        /// </summary>
        public static String Region = "Region";

        public static Boolean RequireCustomMerchantNumber = true;

        public static Boolean RequireCustomMerchantNumberFalse = false;

        public static Boolean RequireCustomMerchantNumberTrue = true;

        public static Boolean RequireCustomTerminalNumber = true;

        public static Boolean RequireCustomTerminalNumberFalse = false;

        public static Boolean RequireCustomTerminalNumberTrue = true;

        public static String ResponseCode = "0000";

        public static String ResponseMessage = "SUCCESS";

        public static Guid SecurityUserId = Guid.Parse("45B74A2E-BF92-44E9-A300-08E5CDEACFE3");

        public static DateTime SettledDate = new DateTime(2021, 10, 6, 1, 2, 3);

        public static Decimal SettledFeeAmount1 = 1.00m;

        public static Decimal SettledFeeAmount2 = 0.85m;

        public static DateTime SettledFeeDateTime1 = new DateTime(2021, 12, 17, 00, 00, 00);

        public static DateTime SettledFeeDateTime2 = new DateTime(2021, 12, 17, 01, 00, 00);

        public static Guid SettledFeeId1 = Guid.Parse("B4D429AE-756D-4F04-8941-4D41B1A75060");

        public static Guid SettledFeeId2 = Guid.Parse("85C64CF1-6522-408D-93E3-D156B4D5C45B");

        public static DateTime SettlementDate = new DateTime(2021, 10, 6);

        public static DateTime SettlementDueDate = new DateTime(2021, 10, 6);

        public static Guid SettlementId = Guid.Parse("7CF02BE4-4BF0-4BB2-93C1-D6E5EC769E56");

        public static Boolean SettlementIsCompleted = true;

        public static SettlementSchedule SettlementSchedule = SettlementSchedule.Immediate;

        public static DataTransferObjects.Responses.Merchant.SettlementSchedule SettlementScheduleDTO = DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly;

        public static String StartDate = "20210104";

        public static DateTime StatementCreateDate = new DateTime(2021, 12, 10);

        public static DateTime StatementEmailedDate = new DateTime(2021, 12, 12);

        public static DateTime StatementGeneratedDate = new DateTime(2021, 12, 11);

        public static Guid StatementId = Guid.Parse("17D432A3-E2A8-47FD-B067-CBF4C132447C");

        public static String TerminalNumber = "12345679";

        /// <summary>
        /// The town
        /// </summary>
        public static String Town = "Town";

        public static Decimal? TransactionAmount = 100.00m;

        public static Decimal? TransactionAmount1 = 100.00m;

        public static Decimal? TransactionAmount2 = 85.00m;

        public static readonly DateTime TransactionCompletedDateTime = new DateTime(2021, 12, 16);

        public static DateTime TransactionDateTime = DateTime.Now;

        public static DateTime TransactionDateTime1 = new DateTime(2021, 12, 10, 11, 00, 00);

        public static DateTime TransactionDateTime2 = new DateTime(2021, 12, 10, 11, 30, 00);

        public static String TransactionFeeDescription = "Commission for Merchant";

        public static Guid TransactionFeeId = Guid.Parse("B83FCCCE-0D45-4FC2-8952-ED277A124BDB");

        public static Guid TransactionFeeId1 = Guid.Parse("2680A005-797C-4501-B1BB-2ACE124B352A");

        public static Decimal TransactionFeeValue = 0.5m;

        public static Guid TransactionId = Guid.Parse("5E3755D1-17DE-4101-9728-75162BAA0A22");

        public static Guid TransactionId1 = Guid.Parse("82E1ACE2-EA34-4501-832D-1DB97B8B4294");

        public static Guid TransactionId2 = Guid.Parse("620A2DD3-75D6-4D19-A239-D1D539B85CE3");

        public static String TransactionNumber = "1";

        public static String TransactionReference = "123456";

        public static Int32 TransactionSource = 1;

        public static String TransactionType = "Logon";

        public static Guid UserId = Guid.Parse("BE52C5AC-72E5-4976-BAA0-98699E36C1EB");

        public static Int32 ValueOfFeesSettled = 1;

        public static String VoucherCode = "1234GHT";

        public static DateTime VoucherExpiryDate = new DateTime(2021, 12, 16);

        public static DateTime VoucherGeneratedDate = new DateTime(2021, 12, 16);

        public static Guid VoucherId = Guid.Parse("1736C058-5AC3-4AAC-8167-10DBAC2B7968");

        public static DateTime VoucherIssuedDate = new DateTime(2021, 12, 16);

        public static String VoucherMessage = String.Empty;

        public static readonly DateTime VoucherRedeemedDate = new DateTime(2021, 12, 16);

        public static Decimal VoucherValue = 10.00m;

        public static PositiveMoney WithdrawalAmount = PositiveMoney.Create(Money.Create(1000.00m));

        public static PositiveMoney WithdrawalAmount2 = PositiveMoney.Create(Money.Create(1200.00m));

        public static DateTime WithdrawalDateTime = new DateTime(2019, 11, 16);

        public static DateTime WithdrawalDateTime2 = new DateTime(2019, 11, 16);

        public static Guid WithdrawalId = Guid.Parse("D5E3614F-78A4-46E6-9F86-3F9AAEC62ABA");

        public static String WithdrawalReference = "Withdraw1";

        #endregion

        #region Properties

        public static Dictionary<String, String> AdditionalRequestData =>
            new Dictionary<String, String>{
                                              { "Amount", "100.00" },
                                              { "CustomerAccountNumber", "123456789" }
                                          };

        public static AdditionalRequestDataRecordedEvent AdditionalRequestDataRecordedEvent => new AdditionalRequestDataRecordedEvent(TestData.TransactionId, TestData.EstateId, TestData.MerchantId, TestData.OperatorName, TestData.AdditionalRequestData);

        public static Dictionary<String, String> AdditionalResponseData =>
            new Dictionary<String, String>{
                                              { "Amount", "100.00" },
                                              { "CustomerAccountNumber", "123456789" }
                                          };

        public static AdditionalResponseDataRecordedEvent AdditionalResponseDataRecordedEvent =>
            new AdditionalResponseDataRecordedEvent(TestData.TransactionId,
                                                    TestData.EstateId,
                                                    TestData.MerchantId,
                                                    TestData.OperatorName,
                                                    TestData.AdditionalResponseData);

        public static MerchantCommands.AddMerchantAddressCommand AddMerchantAddressCommand =>
            new MerchantCommands.AddMerchantAddressCommand(TestData.EstateId,
                                                           TestData.MerchantId,
                                                           TestData.Address);

        public static MerchantCommands.AddMerchantContactCommand AddMerchantContactCommand =>
            new MerchantCommands.AddMerchantContactCommand(TestData.EstateId,
                                                           TestData.MerchantId,
                                                           TestData.Contact);

        public static MerchantCommands.AddMerchantContractCommand AddMerchantContractCommand => new(TestData.EstateId, TestData.MerchantId, TestData.AddMerchantContractRequest);

        public static AddMerchantContractRequest AddMerchantContractRequest =>
            new AddMerchantContractRequest{
                                              ContractId = TestData.ContractId
                                          };

        public static MerchantCommands.AddMerchantDeviceCommand AddMerchantDeviceCommand => new(TestData.EstateId, TestData.MerchantId, TestData.AddMerchantDeviceRequest);

        public static AddMerchantDeviceRequest AddMerchantDeviceRequest =>
            new AddMerchantDeviceRequest{
                                            DeviceIdentifier = TestData.DeviceIdentifier
                                        };
        
        public static AddProductToContractRequest AddProductToContractRequest =>
            AddProductToContractRequest.Create(TestData.ContractId,
                                               TestData.EstateId,
                                               TestData.ProductId,
                                               TestData.ProductName,
                                               TestData.ProductDisplayText,
                                               TestData.ProductFixedValue,
                                               TestData.ProductTypeMobileTopup);

        public static Address Address =>
            new Address{
                           AddressLine1 = TestData.AddressLine1,
                           AddressLine2 = TestData.AddressLine2,
                           AddressLine3 = TestData.AddressLine3,
                           AddressLine4 = TestData.AddressLine4,
                           Country = TestData.Country,
                           PostalCode = TestData.PostCode,
                           Region = TestData.Region,
                           Town = TestData.Town
                       };

        /// <summary>
        /// The address added event
        /// </summary>
        public static AddressAddedEvent AddressAddedEvent =>
            new AddressAddedEvent(TestData.MerchantId,
                                  TestData.EstateId,
                                  TestData.AddressId,
                                  TestData.AddressLine1,
                                  TestData.AddressLine2,
                                  TestData.AddressLine3,
                                  TestData.AddressLine4,
                                  TestData.Town,
                                  TestData.Region,
                                  TestData.PostCode,
                                  TestData.Country);

        public static AddSettledFeeToMerchantStatementRequest AddSettledFeeToMerchantStatementRequest =>
            AddSettledFeeToMerchantStatementRequest.Create(TestData.EstateId,
                                                           TestData.MerchantId,
                                                           TestData.SettledFeeDateTime1,
                                                           TestData.SettledFeeAmount1,
                                                           TestData.TransactionId1,
                                                           TestData.SettledFeeId1);

        public static AddTransactionFeeForProductToContractRequest AddTransactionFeeForProductToContractRequest =>
            AddTransactionFeeForProductToContractRequest.Create(TestData.ContractId,
                                                                TestData.EstateId,
                                                                TestData.ProductId,
                                                                TestData.TransactionFeeId,
                                                                TestData.TransactionFeeDescription,
                                                                CalculationType.Fixed,
                                                                Models.Contract.FeeType.Merchant,
                                                                TestData.TransactionFeeValue);

        public static AddTransactionToMerchantStatementRequest AddTransactionToMerchantStatementRequest =>
            AddTransactionToMerchantStatementRequest.Create(TestData.EstateId,
                                                            TestData.MerchantId,
                                                            TestData.TransactionDateTime1,
                                                            TestData.TransactionAmount1,
                                                            TestData.IsAuthorisedTrue,
                                                            TestData.TransactionId);

        public static DataTransferObjects.Requests.Merchant.AssignOperatorRequest AssignOperatorRequestToMerchant =>
            new AssignOperatorRequest{
                                         MerchantNumber = TestData.OperatorMerchantNumber,
                                         OperatorId = TestData.OperatorId,
                                         TerminalNumber = TestData.OperatorTerminalNumber
                                     };

        public static MerchantCommands.AssignOperatorToMerchantCommand AssignOperatorToMerchantCommand =>
            new MerchantCommands.AssignOperatorToMerchantCommand(TestData.EstateId,
                                                                 TestData.MerchantId,
                                                                 TestData.AssignOperatorRequestToMerchant);

        public static CallbackReceivedEnrichedEvent CallbackReceivedEnrichedEvent =>
            new CallbackReceivedEnrichedEvent(TestData.CallbackId){
                                                                      Reference = TestData.CallbackReference,
                                                                      CallbackMessage = JsonConvert.SerializeObject(TestData.Deposit),
                                                                      EstateId = TestData.EstateId,
                                                                      MessageFormat = TestData.CallbackMessageFormat,
                                                                      TypeString = TestData.CallbackTypeString
                                                                  };

        public static Contact Contact =>
            new Contact{
                           ContactName = TestData.ContactName,
                           EmailAddress = TestData.ContactEmail,
                           PhoneNumber = TestData.ContactPhone
                       };

        /// <summary>
        /// The contact added event
        /// </summary>
        public static ContactAddedEvent ContactAddedEvent =>
            new ContactAddedEvent(TestData.MerchantId,
                                  TestData.EstateId,
                                  TestData.ContactId,
                                  TestData.ContactName,
                                  TestData.ContactPhone,
                                  TestData.ContactEmail);

        public static ContractAddedToMerchantEvent ContractAddedToMerchantEvent =>
            new ContractAddedToMerchantEvent(TestData.MerchantId,
                                             TestData.EstateId,
                                             TestData.ContractId);

        public static ContractCreatedEvent ContractCreatedEvent => new ContractCreatedEvent(TestData.ContractId, TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

        public static OperatorCreatedEvent OperatorCreatedEvent = new OperatorCreatedEvent(TestData.OperatorId,
                                                                                           TestData.EstateId,
                                                                                           TestData.OperatorName,
                                                                                           TestData.RequireCustomMerchantNumber,
                                                                                           TestData.RequireCustomTerminalNumber);

        public static Contract ContractEntity =>
            new Contract{
                            OperatorId = TestData.OperatorId,
                            Description = TestData.ContractDescription,
                            ContractId = TestData.ContractId
                        };

        public static Contract ContractEntity2 =>
            new Contract{
                            OperatorId = TestData.OperatorId2,
                            Description = TestData.ContractDescription,
                            ContractId = TestData.ContractId2
                        };

        public static Models.Contract.Contract ContractModel =>
            new Models.Contract.Contract{
                                            OperatorId = TestData.OperatorId,
                                            ContractId = TestData.ContractId,
                                            Description = TestData.ContractDescription,
                                            IsCreated = true,
                                            Products = null
                                        };

        public static Models.Contract.Contract ContractModelWithProducts =>
            new Models.Contract.Contract{
                                            OperatorId = TestData.OperatorId,
                                            ContractId = TestData.ContractId,
                                            Description = TestData.ContractDescription,
                                            IsCreated = true,
                                            Products = new List<Product>{
                                                                            new Product{
                                                                                           Value = TestData.ProductFixedValue,
                                                                                           ProductId = TestData.ProductId,
                                                                                           DisplayText = TestData.ProductDisplayText,
                                                                                           Name = TestData.ProductName,
                                                                                           TransactionFees = null
                                                                                       }
                                                                        }
                                        };

        public static Models.Contract.Contract ContractModelWithProductsAndTransactionFees =>
            new Models.Contract.Contract{
                                            OperatorId = TestData.OperatorId,
                                            ContractId = TestData.ContractId,
                                            Description = TestData.ContractDescription,
                                            IsCreated = true,
                                            Products = new List<Product>{
                                                                            new Product{
                                                                                           Value = TestData.ProductFixedValue,
                                                                                           ProductId = TestData.ProductId,
                                                                                           DisplayText = TestData.ProductDisplayText,
                                                                                           Name = TestData.ProductName,
                                                                                           TransactionFees = new List<TransactionFeeModel>{
                                                                                                                                              new TransactionFeeModel{
                                                                                                                                                                         TransactionFeeId = TestData.TransactionFeeId,
                                                                                                                                                                         Description = TestData.TransactionFeeDescription,
                                                                                                                                                                         Value = TestData.TransactionFeeValue,
                                                                                                                                                                         CalculationType = CalculationType.Fixed
                                                                                                                                                                     }
                                                                                                                                          }
                                                                                       }
                                                                        }
                                        };

        public static ContractProduct ContractProductEntity =>
            new ContractProduct{
                                   Value = TestData.ProductFixedValue,
                                   ProductId = TestData.ProductId,
                                   ProductName = TestData.ProductName,
                                   DisplayText = TestData.ProductDisplayText
                               };

        public static ContractProduct ContractProductEntity2 =>
            new ContractProduct{
                                   Value = TestData.ProductFixedValue,
                                   ProductId = TestData.ProductId2,
                                   ProductName = TestData.ProductName,
                                   DisplayText = TestData.ProductDisplayText
                               };

        public static ContractProductTransactionFee ContractProductTransactionFeeEntity =>
            new ContractProductTransactionFee{
                                                 Value = TestData.ProductFixedValue,
                                                 TransactionFeeId = TestData.TransactionFeeId,
                                                 Description = TestData.TransactionFeeDescription,
                                                 CalculationType = 0
                                             };

        public static CreateContractRequest CreateContractRequest => CreateContractRequest.Create(TestData.ContractId, TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

        public static EstateCommands.CreateEstateCommand CreateEstateCommand => new EstateCommands.CreateEstateCommand(TestData.CreateEstateRequest);

        public static CreateEstateRequest CreateEstateRequest =>
            new CreateEstateRequest{
                                       EstateId = TestData.EstateId,
                                       EstateName = TestData.EstateName,
                                   };

        public static EstateCommands.CreateEstateUserCommand CreateEstateUserCommand => new EstateCommands.CreateEstateUserCommand(TestData.EstateId, TestData.CreateEstateUserRequest);

        public static CreateOperatorRequest CreateOperatorRequest =>
            new CreateOperatorRequest(){
                                           OperatorId = TestData.OperatorId,
                                           RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                                           RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                                           Name = TestData.OperatorName
                                       };
        public static OperatorCommands.CreateOperatorCommand CreateOperatorCommand => new(TestData.EstateId,  TestData.CreateOperatorRequest);

        public static CreateEstateUserRequest CreateEstateUserRequest =>
            new CreateEstateUserRequest{
                                           EmailAddress = TestData.EstateUserEmailAddress,
                                           FamilyName = TestData.EstateUserFamilyName,
                                           GivenName = TestData.EstateUserGivenName,
                                           MiddleName = TestData.EstateUserMiddleName,
                                           Password = TestData.EstateUserPassword
                                       };

        public static MerchantCommands.CreateMerchantCommand CreateMerchantCommand => new(TestData.EstateId, TestData.CreateMerchantRequest);

        public static CreateMerchantRequest CreateMerchantRequest =>
            new CreateMerchantRequest{
                                         Address = new Address{
                                                                  AddressLine1 = TestData.MerchantAddressLine1,
                                                                  AddressLine2 = TestData.MerchantAddressLine2,
                                                                  AddressLine3 = TestData.MerchantAddressLine3,
                                                                  AddressLine4 = TestData.MerchantAddressLine4,
                                                                  Country = TestData.MerchantCountry,
                                                                  PostalCode = TestData.MerchantPostalCode,
                                                                  Region = TestData.MerchantRegion,
                                                                  Town = TestData.MerchantTown
                                                              },
                                         Contact = new Contact{
                                                                  ContactName = TestData.MerchantContactName,
                                                                  EmailAddress = TestData.MerchantContactEmailAddress,
                                                                  PhoneNumber = TestData.MerchantContactPhoneNumber
                                                              },
                                         CreatedDateTime = TestData.DateMerchantCreated,
                                         MerchantId = TestData.MerchantId,
                                         Name = TestData.MerchantName,
                                         SettlementSchedule = TestData.SettlementScheduleDTO
                                     };

        public static MerchantCommands.CreateMerchantUserCommand CreateMerchantUserCommand => new(TestData.EstateId, TestData.MerchantId, TestData.CreateMerchantUserRequest);

        public static CreateMerchantUserRequest CreateMerchantUserRequest =>
            new CreateMerchantUserRequest{
                                             EmailAddress = TestData.EmailAddress,
                                             FamilyName = TestData.MerchantUserFamilyName,
                                             GivenName = TestData.MerchantUserGivenName,
                                             MiddleName = TestData.MerchantUserMiddleName,
                                             Password = TestData.MerchantUserPassword
                                         };

        public static IReadOnlyDictionary<String, String> DefaultAppSettings =>
            new Dictionary<String, String>{
                                              ["AppSettings:ClientId"] = "clientId",
                                              ["AppSettings:ClientSecret"] = "clientSecret",
                                              ["AppSettings:BlinkBinariesPath"] = "clientSecret"
                                          };

        public static Deposit Deposit =>
            new Deposit{
                           Reference = TestData.CallbackReference,
                           Amount = TestData.DepositAmount.Value,
                           DateTime = TestData.DepositDateTime,
                           DepositId = TestData.DepositId,
                           AccountNumber = TestData.DepositAccountNumber,
                           HostIdentifier = TestData.DepositHostIdentifier,
                           SortCode = TestData.DepositSortCode
                       };

        /// <summary>
        /// The device added to merchant event
        /// </summary>
        public static DeviceAddedToMerchantEvent DeviceAddedToMerchantEvent => new DeviceAddedToMerchantEvent(TestData.MerchantId, TestData.EstateId, TestData.DeviceId, TestData.DeviceIdentifier);

        public static DeviceSwappedForMerchantEvent DeviceSwappedForMerchantEvent =>
            new DeviceSwappedForMerchantEvent(TestData.MerchantId,
                                              TestData.EstateId,
                                              TestData.DeviceId,
                                              TestData.DeviceIdentifier,
                                              TestData.NewDeviceIdentifier);

        public static DisableTransactionFeeForProductRequest DisableTransactionFeeForProductRequest =>
            DisableTransactionFeeForProductRequest.Create(TestData.ContractId,
                                                          TestData.EstateId,
                                                          TestData.ProductId,
                                                          TestData.TransactionFeeId);

        public static EmailMerchantStatementRequest EmailMerchantStatementRequest => EmailMerchantStatementRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.MerchantStatementId);

        /// <summary>
        /// The estate created event
        /// </summary>
        public static EstateCreatedEvent EstateCreatedEvent => new EstateCreatedEvent(TestData.EstateId, TestData.EstateName);

        public static Estate EstateEntity =>
            new Estate{
                          Name = TestData.EstateName,
                          EstateId = TestData.EstateId,
                          Reference = TestData.MerchantReference
                      };

        public static Models.Estate.Estate EstateModel =>
            new Models.Estate.Estate{
                                        EstateId = TestData.EstateId,
                                        Name = TestData.EstateName,
                                        Operators = null,
                                        SecurityUsers = null
                                    };

        public static Models.Estate.Estate EstateModelWithOperators =>
            new Models.Estate.Estate{
                                        EstateId = TestData.EstateId,
                                        Name = TestData.EstateName,
                                        Operators = new List<Operator>{
                                                                          new Operator{
                                                                                          OperatorId = TestData.OperatorId
                                                                                      }
                                                                      },
                                        SecurityUsers = null
                                    };

        public static Models.Estate.Estate EstateModelWithOperatorsAndSecurityUsers =>
            new Models.Estate.Estate{
                                        EstateId = TestData.EstateId,
                                        Name = TestData.EstateName,
                                        Operators = new List<Operator>{
                                                                          new Operator{
                                                                                          OperatorId = TestData.OperatorId
                                                                                      }
                                                                      },
                                        SecurityUsers = new List<SecurityUser>{
                                                                                  new SecurityUser{
                                                                                                      EmailAddress = TestData.EstateUserEmailAddress,
                                                                                                      SecurityUserId = TestData.SecurityUserId
                                                                                                  }
                                                                              }
                                    };

        public static Models.Estate.Estate EstateModelWithSecurityUsers =>
            new Models.Estate.Estate{
                                        EstateId = TestData.EstateId,
                                        Name = TestData.EstateName,
                                        Operators = null,
                                        SecurityUsers = new List<SecurityUser>{
                                                                                  new SecurityUser{
                                                                                                      EmailAddress = TestData.EstateUserEmailAddress,
                                                                                                      SecurityUserId = TestData.SecurityUserId
                                                                                                  }
                                                                              }
                                    };

        public static EstateOperator EstateOperatorEntity =>
            new EstateOperator{
                                  EstateReportingId = 1,
                                  OperatorReportingId = 1
                              };

        public static OperatorEntity OperatorEntity =>
            new OperatorEntity{
                                  Name = TestData.OperatorName,
                                  OperatorId = TestData.OperatorId,
                                  RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                                  RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                                  EstateReportingId = 1,
                                  OperatorReportingId = 1
                              };
        
        public static EstateOperator EstateOperatorEntity2 =>
            new EstateOperator{
                EstateReportingId = 1,
                OperatorReportingId = 2
            };

        public static EstateReferenceAllocatedEvent EstateReferenceAllocatedEvent => new EstateReferenceAllocatedEvent(TestData.EstateId, TestData.EstateReference);

        public static EstateSecurityUser EstateSecurityUserEntity =>
            new EstateSecurityUser{
                                      EmailAddress = TestData.EstateUserEmailAddress,
                                      SecurityUserId = TestData.SecurityUserId
                                  };

        public static FileAddedToImportLogEvent FileAddedToImportLogEvent =>
            new FileAddedToImportLogEvent(TestData.FileImportLogId,
                                          TestData.FileId,
                                          TestData.EstateId,
                                          TestData.MerchantId,
                                          TestData.UserId,
                                          TestData.FileProfileId,
                                          TestData.OriginalFileName,
                                          TestData.FilePath,
                                          TestData.FileUploadedDateTime);

        public static FileCreatedEvent FileCreatedEvent =>
            new FileCreatedEvent(TestData.FileId,
                                 TestData.FileImportLogId,
                                 TestData.EstateId,
                                 TestData.MerchantId,
                                 TestData.UserId,
                                 TestData.FileProfileId,
                                 TestData.FileLocation,
                                 TestData.FileUploadedDateTime);

        public static FileLineAddedEvent FileLineAddedEvent => new FileLineAddedEvent(TestData.FileId, TestData.EstateId, TestData.MerchantId, TestData.LineNumber, TestData.FileLine);

        public static FileLineDetails FileLineDetails1 =>
            new FileLineDetails{
                                   Transaction = new Transaction{
                                                                    AuthCode = "ABCD1234",
                                                                    IsAuthorised = true,
                                                                    IsCompleted = true,
                                                                    ResponseCode = "0000",
                                                                    ResponseMessage = "SUCCESS",
                                                                    TransactionId = Guid.Parse("B7CB5ACE-84FC-4F29-B297-3D71FC229687"),
                                                                    TransactionNumber = "0001"
                                                                },
                                   FileLineData = "FileDataLine1",
                                   Status = "S",
                                   FileLineNumber = 1
                               };

        public static FileLineDetails FileLineDetails2 =>
            new FileLineDetails{
                                   Transaction = new Transaction{
                                                                    AuthCode = "ABCD1235",
                                                                    IsAuthorised = true,
                                                                    IsCompleted = true,
                                                                    ResponseCode = "0000",
                                                                    ResponseMessage = "SUCCESS",
                                                                    TransactionId = Guid.Parse("E14C986F-34D7-4B3C-BC1E-EEEAC8399897"),
                                                                    TransactionNumber = "0002"
                                                                },
                                   FileLineData = "FileDataLine2",
                                   Status = "S",
                                   FileLineNumber = 2
                               };

        public static List<FileLineDetails> FileLineDetailsList =>
            new List<FileLineDetails>{
                                         TestData.FileLineDetails1,
                                         TestData.FileLineDetails2
                                     };

        public static FileLineProcessingFailedEvent FileLineProcessingFailedEvent =>
            new FileLineProcessingFailedEvent(TestData.FileId,
                                              TestData.EstateId,
                                              TestData.MerchantId,
                                              TestData.LineNumber,
                                              TestData.TransactionId,
                                              TestData.ResponseCode,
                                              TestData.ResponseMessage);

        public static FileLineProcessingIgnoredEvent FileLineProcessingIgnoredEvent => new FileLineProcessingIgnoredEvent(TestData.FileId, TestData.EstateId, TestData.MerchantId, TestData.LineNumber);

        public static FileLineProcessingSuccessfulEvent FileLineProcessingSuccessfulEvent => new FileLineProcessingSuccessfulEvent(TestData.FileId, TestData.EstateId, TestData.MerchantId, TestData.LineNumber, TestData.TransactionId);

        public static File FileModel =>
            new File{
                        FileId = TestData.FileId,
                        FileLineDetails = TestData.FileLineDetailsList,
                        FileReceivedDate = TestData.FileReceivedDate,
                        FileReceivedDateTime = TestData.FileReceivedDateTime
                    };

        public static FileProcessingCompletedEvent FileProcessingCompletedEvent => new FileProcessingCompletedEvent(TestData.FileId, TestData.EstateId, TestData.MerchantId, TestData.ProcessingCompletedDateTime);

        public static FixedValueProductAddedToContractEvent FixedValueProductAddedToContractEvent =>
            new FixedValueProductAddedToContractEvent(TestData.ContractId,
                                                      TestData.EstateId,
                                                      TestData.ProductId,
                                                      TestData.ProductName,
                                                      TestData.ProductDisplayText,
                                                      TestData.ProductFixedValue,
                                                      (Int32)TestData.ProductTypeMobileTopup);

        public static FloatCreatedForContractProductEvent FloatCreatedForContractProductEvent => new FloatCreatedForContractProductEvent(TestData.FloatId, TestData.EstateId, TestData.ContractId, TestData.ProductId, TestData.FloatCreatedDateTime);

        public static FloatCreditPurchasedEvent FloatCreditPurchasedEvent => new FloatCreditPurchasedEvent(TestData.FloatId, TestData.EstateId, TestData.FloatCreditPurchsedDateTime, TestData.FloatCreditAmount, TestData.FloatCreditCostPrice);

        public static FloatDecreasedByTransactionEvent FloatDecreasedByTransactionEvent => new FloatDecreasedByTransactionEvent(TestData.FloatId, TestData.EstateId, TestData.TransactionId, TestData.TransactionAmount.GetValueOrDefault());

        public static MerchantCommands.GenerateMerchantStatementCommand GenerateMerchantStatementCommand => new(TestData.EstateId, TestData.MerchantId, TestData.GenerateMerchantStatementRequest);

        public static GenerateMerchantStatementRequest GenerateMerchantStatementRequest =>
            new GenerateMerchantStatementRequest{
                                                    MerchantStatementDate = TestData.StatementCreateDate
                                                };
        public static MerchantQueries.GetMerchantContractsQuery GetMerchantContractsQuery => new MerchantQueries.GetMerchantContractsQuery(TestData.EstateId, TestData.MerchantId);
        public static MerchantQueries.GetMerchantQuery GetMerchantQuery => new MerchantQueries.GetMerchantQuery(TestData.EstateId, TestData.MerchantId);

        public static MerchantQueries.GetMerchantsQuery GetMerchantsQuery => new MerchantQueries.GetMerchantsQuery(TestData.EstateId);

        public static EstateQueries.GetEstateQuery GetEstateQuery => new (TestData.EstateId);

        public static OperatorQueries.GetOperatorQuery GetOperatorQuery => new(TestData.EstateId, TestData.OperatorId);
        public static OperatorQueries.GetOperatorsQuery GetOperatorsQuery => new(TestData.EstateId);
        public static EstateQueries.GetEstatesQuery GetEstatesQuery => new(TestData.EstateId);

        public static MerchantQueries.GetTransactionFeesForProductQuery GetTransactionFeesForProductQuery =>
            new MerchantQueries.GetTransactionFeesForProductQuery(TestData.EstateId,
                                                                  TestData.MerchantId,
                                                                  TestData.ContractId,
                                                                  TestData.ProductId);

        public static ImportLogCreatedEvent ImportLogCreatedEvent => new ImportLogCreatedEvent(TestData.FileImportLogId, TestData.EstateId, TestData.ImportLogDateTime);

        public static MerchantCommands.MakeMerchantDepositCommand MakeMerchantDepositCommand =>
            new(TestData.EstateId,
                TestData.MerchantId,
                TestData.MerchantDepositSourceManual,
                TestData.MakeMerchantDepositRequest);

        public static MakeMerchantDepositRequest MakeMerchantDepositRequest =>
            new MakeMerchantDepositRequest{
                                              DepositDateTime = TestData.DepositDateTime,
                                              Amount = TestData.DepositAmount.Value,
                                              Reference = TestData.DepositReference
                                          };

        public static MerchantCommands.MakeMerchantWithdrawalCommand MakeMerchantWithdrawalCommand =>
            new(TestData.EstateId,
                TestData.MerchantId,
                TestData.MakeMerchantWithdrawalRequest);

        public static MakeMerchantWithdrawalRequest MakeMerchantWithdrawalRequest =>
            new MakeMerchantWithdrawalRequest{
                                                 WithdrawalDateTime = TestData.WithdrawalDateTime,
                                                 Amount = TestData.WithdrawalAmount.Value,
                                                 Reference = TestData.WithdrawalReference
                                             };

        public static MerchantAddress MerchantAddressEntity =>
            new MerchantAddress{
                                   AddressLine1 = TestData.MerchantAddressLine1,
                                   CreatedDateTime = TestData.DateMerchantCreated,
                                   AddressLine2 = TestData.MerchantAddressLine2,
                                   AddressLine3 = TestData.MerchantAddressLine3,
                                   AddressLine4 = TestData.MerchantAddressLine4,
                                   Country = TestData.MerchantCountry,
                                   PostalCode = TestData.MerchantPostalCode,
                                   Region = TestData.MerchantRegion,
                                   Town = TestData.MerchantTown,
                                   AddressId = Guid.NewGuid()
                               };

        public static MerchantAddressLine1UpdatedEvent MerchantAddressLine1UpdatedEvent => new MerchantAddressLine1UpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.AddressLine1);
        public static MerchantAddressLine2UpdatedEvent MerchantAddressLine2UpdatedEvent => new MerchantAddressLine2UpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.AddressLine2);
        public static MerchantAddressLine3UpdatedEvent MerchantAddressLine3UpdatedEvent => new MerchantAddressLine3UpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.AddressLine3);
        public static MerchantAddressLine4UpdatedEvent MerchantAddressLine4UpdatedEvent => new MerchantAddressLine4UpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.AddressLine4);

        public static MerchantBalanceResponse MerchantBalance =>
            new MerchantBalanceResponse{
                                           MerchantId = TestData.MerchantId,
                                           AvailableBalance = TestData.AvailableBalance,
                                           Balance = TestData.Balance,
                                           EstateId = TestData.EstateId,
                                       };

        public static MerchantBalanceResponse MerchantBalanceNoAvailableBalance =>
            new MerchantBalanceResponse{
                                           MerchantId = TestData.MerchantId,
                                           AvailableBalance = 0,
                                           Balance = 0,
                                           EstateId = TestData.EstateId,
                                       };
        public static MerchantContactEmailAddressUpdatedEvent MerchantContactEmailAddressUpdatedEvent => new MerchantContactEmailAddressUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.ContactId, TestData.ContactEmailUpdate);
        public static MerchantContactNameUpdatedEvent MerchantContactNameUpdatedEvent => new MerchantContactNameUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.ContactId, TestData.ContactNameUpdate);
        public static MerchantContactPhoneNumberUpdatedEvent MerchantContactPhoneNumberUpdatedEvent => new MerchantContactPhoneNumberUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.ContactId, TestData.ContactPhoneUpdate);

        public static List<Models.Contract.Contract> MerchantContracts =>
            new List<Models.Contract.Contract>{
                                                  new Models.Contract.Contract{
                                                                                  ContractId = TestData.ContractId,
                                                                                  Description = TestData.ContractDescription,
                                                                                  OperatorId = TestData.OperatorId,
                                                                                  OperatorName = TestData.OperatorName,
                                                                                  Products = new List<Product>{
                                                                                                                  new Product{
                                                                                                                                 ProductId = TestData.ProductId,
                                                                                                                                 Value = TestData.ProductFixedValue,
                                                                                                                                 DisplayText = TestData.ProductDisplayText,
                                                                                                                                 Name = TestData.ProductName
                                                                                                                             }
                                                                                                              }
                                                                              }
                                              };
        public static MerchantCountyUpdatedEvent MerchantCountyUpdatedEvent => new MerchantCountyUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.Country);

        /// <summary>
        /// The merchant created event
        /// </summary>
        public static MerchantCreatedEvent MerchantCreatedEvent => new MerchantCreatedEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantName, DateTime.Now);

        public static MerchantDevice MerchantDeviceEntity =>
            new MerchantDevice{
                                  DeviceId = TestData.DeviceId,
                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                  CreatedDateTime = TestData.DateMerchantCreated
                              };

        public static Merchant MerchantEntity =>
            new Merchant{
                            MerchantId = TestData.MerchantId,
                            CreatedDateTime = TestData.DateMerchantCreated,
                            Name = TestData.MerchantName,
                            Reference = TestData.MerchantReference
                        };

        public static MerchantFeeAddedPendingSettlementEvent MerchantFeeAddedPendingSettlementEvent =>
            new MerchantFeeAddedPendingSettlementEvent(TestData.SettlementId,
                                                       TestData.EstateId,
                                                       TestData.MerchantId,
                                                       TestData.TransactionId,
                                                       TestData.CalculatedValue,
                                                       TestData.FeeCalculationType,
                                                       TestData.TransactionFeeId,
                                                       TestData.FeeValue,
                                                       TestData.FeeCalculatedDateTime);

        public static SettledMerchantFeeAddedToTransactionEvent MerchantFeeAddedToTransactionEvent =>
            new SettledMerchantFeeAddedToTransactionEvent(TestData.TransactionId,
                                                          TestData.EstateId,
                                                          TestData.MerchantId,
                                                          TestData.CalculatedValue,
                                                          TestData.FeeCalculationType,
                                                          TestData.TransactionFeeId,
                                                          TestData.FeeValue,
                                                          TestData.FeeCalculatedDateTime,
                                                          TestData.SettledDate,
                                                          TestData.SettlementId);

        public static MerchantFeeSettledEvent MerchantFeeSettledEvent =>
            new MerchantFeeSettledEvent(TestData.SettlementId,
                                        TestData.EstateId,
                                        TestData.MerchantId,
                                        TestData.TransactionId,
                                        TestData.CalculatedValue,
                                        TestData.FeeCalculationType,
                                        TestData.TransactionFeeId,
                                        TestData.FeeValue,
                                        TestData.FeeCalculatedDateTime,
                                        TestData.SettledDate);

        public static Models.Merchant.Merchant MerchantModelWithNullAddresses =>
            new Models.Merchant.Merchant{
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = SettlementSchedule.Immediate,
                                            Addresses = null,
                                            Contacts = new List<Models.Merchant.Contact>{
                                                                                            new Models.Merchant.Contact{
                                                                                                                           ContactId = Guid.NewGuid(),
                                                                                                                           ContactName = TestData.MerchantContactName,
                                                                                                                           ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                                           ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                                                       }
                                                                                        },
                                            Devices = new List<Device>{
                                                                          new Device{
                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                        IsEnabled = true,
                                                                                        DeviceId = TestData.DeviceId,
                                                                                    }
                                                                      },
                                            Operators = new List<Models.Merchant.Operator>{
                                                                                              new Models.Merchant.Operator{
                                                                                                                              Name = TestData.OperatorName,
                                                                                                                              TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                                              MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                                              OperatorId = TestData.OperatorId
                                                                                                                          }
                                                                                          }
                                        };

        public static Models.Merchant.Merchant MerchantModelWithNullAddressesAndContacts =>
            new Models.Merchant.Merchant{
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = SettlementSchedule.Immediate,
                                            Addresses = null,
                                            Contacts = null,
                                            Devices = new List<Device>{
                                                                          new Device{
                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                        IsEnabled = true,
                                                                                        DeviceId = TestData.DeviceId,
                                                                                    }
                                                                      },
                                            Operators = new List<Models.Merchant.Operator>{
                                                                                              new Models.Merchant.Operator{
                                                                                                                              Name = TestData.OperatorName,
                                                                                                                              TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                                              MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                                              OperatorId = TestData.OperatorId
                                                                                                                          }
                                                                                          }
                                        };

        public static Models.Merchant.Merchant MerchantModelWithNullContacts =>
            new Models.Merchant.Merchant{
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = SettlementSchedule.Immediate,
                                            Addresses = new List<Models.Merchant.Address>{
                                                                                             new Models.Merchant.Address{
                                                                                                                            Town = TestData.MerchantTown,
                                                                                                                            AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                                            AddressId = Guid.NewGuid(),
                                                                                                                            Region = TestData.MerchantRegion,
                                                                                                                            Country = TestData.MerchantCountry,
                                                                                                                            AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                                            AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                                            AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                                            PostalCode = TestData.MerchantPostalCode
                                                                                                                        }
                                                                                         },
                                            Contacts = null,
                                            Devices = new List<Device>{
                                                                          new Device{
                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                        IsEnabled = true,
                                                                                        DeviceId = TestData.DeviceId,
                                                                                    }
                                                                      },
                                            Operators = new List<Models.Merchant.Operator>{
                                                                                              new Models.Merchant.Operator{
                                                                                                                              Name = TestData.OperatorName,
                                                                                                                              TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                                              MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                                              OperatorId = TestData.OperatorId
                                                                                                                          }
                                                                                          }
                                        };

        public static Models.Merchant.Merchant MerchantModelWithNullDevices =>
            new Models.Merchant.Merchant{
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = SettlementSchedule.Immediate,
                                            Addresses = new List<Models.Merchant.Address>{
                                                                                             new Models.Merchant.Address{
                                                                                                                            Town = TestData.MerchantTown,
                                                                                                                            AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                                            AddressId = Guid.NewGuid(),
                                                                                                                            Region = TestData.MerchantRegion,
                                                                                                                            Country = TestData.MerchantCountry,
                                                                                                                            AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                                            AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                                            AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                                            PostalCode = TestData.MerchantPostalCode
                                                                                                                        }
                                                                                         },
                                            Contacts = new List<Models.Merchant.Contact>{
                                                                                            new Models.Merchant.Contact{
                                                                                                                           ContactId = Guid.NewGuid(),
                                                                                                                           ContactName = TestData.MerchantContactName,
                                                                                                                           ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                                           ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                                                       }
                                                                                        },
                                            Devices = null,
                                            Operators = new List<Models.Merchant.Operator>{
                                                                                              new Models.Merchant.Operator{
                                                                                                                              Name = TestData.OperatorName,
                                                                                                                              TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                                              MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                                              OperatorId = TestData.OperatorId
                                                                                                                          }
                                                                                          }
                                        };

        public static Models.Merchant.Merchant MerchantModelWithNullOperators =>
            new Models.Merchant.Merchant{
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = SettlementSchedule.Immediate,
                                            Addresses = new List<Models.Merchant.Address>{
                                                                                             new Models.Merchant.Address{
                                                                                                                            Town = TestData.MerchantTown,
                                                                                                                            AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                                            AddressId = Guid.NewGuid(),
                                                                                                                            Region = TestData.MerchantRegion,
                                                                                                                            Country = TestData.MerchantCountry,
                                                                                                                            AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                                            AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                                            AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                                            PostalCode = TestData.MerchantPostalCode
                                                                                                                        }
                                                                                         },
                                            Contacts = new List<Models.Merchant.Contact>{
                                                                                            new Models.Merchant.Contact{
                                                                                                                           ContactId = Guid.NewGuid(),
                                                                                                                           ContactName = TestData.MerchantContactName,
                                                                                                                           ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                                           ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                                                       }
                                                                                        },
                                            Devices = new List<Device>{
                                                                          new Device{
                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                        IsEnabled = true,
                                                                                        DeviceId = TestData.DeviceId,
                                                                                    }
                                                                      },
                                            Operators = null
                                        };

        public static MerchantNameUpdatedEvent MerchantNameUpdatedEvent => new MerchantNameUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantNameUpdated);

        public static MerchantOperator MerchantOperatorEntity =>
            new MerchantOperator{
                                    Name = TestData.OperatorName,
                                    OperatorId = TestData.OperatorId,
                                    TerminalNumber = TestData.OperatorTerminalNumber,
                                    MerchantNumber = TestData.OperatorMerchantNumber
                                };
        public static MerchantPostalCodeUpdatedEvent MerchantPostalCodeUpdatedEvent => new MerchantPostalCodeUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.PostCode);

        public static MerchantReferenceAllocatedEvent MerchantReferenceAllocatedEvent => new MerchantReferenceAllocatedEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantReference);
        public static MerchantRegionUpdatedEvent MerchantRegionUpdatedEvent => new MerchantRegionUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.Region);

        /// <summary>
        /// The merchant security user added event
        /// </summary>
        public static SecurityUserAddedToMerchantEvent MerchantSecurityUserAddedEvent => new SecurityUserAddedToMerchantEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantSecurityUserId, TestData.EmailAddress);

        public static MerchantSecurityUser MerchantSecurityUserEntity =>
            new MerchantSecurityUser{
                                        SecurityUserId = TestData.SecurityUserId,
                                        EmailAddress = TestData.MerchantUserEmailAddress,
                                        CreatedDateTime = TestData.DateMerchantCreated
                                    };
        public static MerchantTownUpdatedEvent MerchantTownUpdatedEvent => new MerchantTownUpdatedEvent(TestData.MerchantId, TestData.EstateId, TestData.AddressId, TestData.Town);

        public static OperatorAddedToEstateEvent OperatorAddedToEstateEvent =>
            new OperatorAddedToEstateEvent(TestData.EstateId,
                                           TestData.OperatorId);

        public static OperatorAssignedToMerchantEvent OperatorAssignedToMerchantEvent =>
            new OperatorAssignedToMerchantEvent(TestData.MerchantId,
                                                TestData.EstateId,
                                                TestData.OperatorId,
                                                TestData.OperatorName,
                                                TestData.MerchantNumber,
                                                TestData.TerminalNumber);

        public static OperatorRemovedFromMerchantEvent OperatorRemovedFromMerchantEvent => new OperatorRemovedFromMerchantEvent(TestData.MerchantId, TestData.EstateId, TestData.OperatorId);

        public static OverallTotalsRecordedEvent OverallTotalsRecordedEvent =>
            new OverallTotalsRecordedEvent(TestData.TransactionId,
                                           TestData.EstateId,
                                           TestData.MerchantId,
                                           TestData.ReconcilationTransactionCount,
                                           TestData.ReconcilationTransactionValue);

        public static ProductDetailsAddedToTransactionEvent ProductDetailsAddedToTransactionEvent => new ProductDetailsAddedToTransactionEvent(TestData.TransactionId, TestData.EstateId, TestData.MerchantId, TestData.ContractId, TestData.ProductId);

        public static List<TransactionFeeModel> ProductTransactionFees =>
            new List<TransactionFeeModel>{
                                             new TransactionFeeModel{
                                                                        TransactionFeeId = TestData.TransactionFeeId,
                                                                        Description = TestData.TransactionFeeDescription,
                                                                        Value = TestData.TransactionFeeValue,
                                                                        CalculationType = CalculationType.Fixed
                                                                    }
                                         };

        public static ReconciliationHasBeenLocallyAuthorisedEvent ReconciliationHasBeenLocallyAuthorisedEvent =>
            new ReconciliationHasBeenLocallyAuthorisedEvent(TestData.TransactionId,
                                                            TestData.EstateId,
                                                            TestData.MerchantId,
                                                            TestData.ResponseCode,
                                                            TestData.ResponseMessage);

        public static ReconciliationHasBeenLocallyDeclinedEvent ReconciliationHasBeenLocallyDeclinedEvent =>
            new ReconciliationHasBeenLocallyDeclinedEvent(TestData.TransactionId,
                                                          TestData.EstateId,
                                                          TestData.MerchantId,
                                                          TestData.ResponseCode,
                                                          TestData.ResponseMessage);

        public static ReconciliationHasCompletedEvent ReconciliationHasCompletedEvent => new ReconciliationHasCompletedEvent(TestData.TransactionId, TestData.EstateId, TestData.MerchantId);

        public static ReconciliationHasStartedEvent ReconciliationHasStartedEvent => new ReconciliationHasStartedEvent(TestData.TransactionId, TestData.EstateId, TestData.MerchantId, TestData.TransactionDateTime);

        public static MerchantCommands.RemoveMerchantContractCommand RemoveMerchantContractCommand =>
            new MerchantCommands.RemoveMerchantContractCommand(TestData.EstateId,
                                                               TestData.MerchantId,
                                                               TestData.ContractId);

        public static EstateCommands.RemoveOperatorFromEstateCommand RemoveOperatorFromEstateCommand => new (TestData.EstateId, TestData.OperatorId);
        public static MerchantCommands.RemoveOperatorFromMerchantCommand RemoveOperatorFromMerchantCommand => new MerchantCommands.RemoveOperatorFromMerchantCommand(TestData.EstateId, TestData.MerchantId, TestData.OperatorId);

        public static SettledFee SettledFee1 =>
            new SettledFee{
                              Amount = TestData.SettledFeeAmount1,
                              DateTime = TestData.SettledFeeDateTime1,
                              SettledFeeId = TestData.SettledFeeId1,
                              TransactionId = TestData.TransactionId1
                          };

        public static SettledFee SettledFee2 =>
            new SettledFee{
                              Amount = TestData.SettledFeeAmount2,
                              DateTime = TestData.SettledFeeDateTime2,
                              SettledFeeId = TestData.SettledFeeId2,
                              TransactionId = TestData.TransactionId2
                          };

        public static SettledFeeAddedToStatementEvent SettledFeeAddedToStatementEvent =>
            new SettledFeeAddedToStatementEvent(TestData.MerchantStatementId,
                                                Guid.NewGuid(),
                                                TestData.EstateId,
                                                TestData.MerchantId,
                                                TestData.TransactionFeeId,
                                                TestData.TransactionId,
                                                TestData.SettledFeeDateTime1,
                                                TestData.SettledFeeAmount1);

        public static SettlementCompletedEvent SettlementCompletedEvent => new SettlementCompletedEvent(TestData.SettlementId, TestData.EstateId, TestData.MerchantId);

        public static SettlementCreatedForDateEvent SettlementCreatedForDateEvent => new SettlementCreatedForDateEvent(TestData.SettlementId, TestData.EstateId, TestData.MerchantId, TestData.SettlementDate);

        public static SettlementFeeModel SettlementFeeModel =>
            new SettlementFeeModel{
                                      SettlementDate = TestData.SettlementDate,
                                      SettlementId = TestData.SettlementId,
                                      CalculatedValue = TestData.CalculatedValue,
                                      MerchantId = TestData.MerchantId,
                                      MerchantName = TestData.MerchantName,
                                      FeeDescription = TestData.TransactionFeeDescription,
                                      IsSettled = TestData.IsSettled,
                                      TransactionId = TestData.TransactionId
                                  };

        public static List<SettlementFeeModel> SettlementFeeModels =>
            new List<SettlementFeeModel>{
                                            new SettlementFeeModel{
                                                                      SettlementDate = TestData.SettlementDate,
                                                                      SettlementId = TestData.SettlementId,
                                                                      CalculatedValue = TestData.CalculatedValue,
                                                                      MerchantId = TestData.MerchantId,
                                                                      MerchantName = TestData.MerchantName,
                                                                      FeeDescription = TestData.TransactionFeeDescription,
                                                                      IsSettled = TestData.IsSettled,
                                                                      TransactionId = TestData.TransactionId
                                                                  }
                                        };

        public static SettlementModel SettlementModel =>
            new SettlementModel{
                                   IsCompleted = TestData.SettlementIsCompleted,
                                   SettlementDate = TestData.SettlementDate,
                                   NumberOfFeesSettled = TestData.NumberOfFeesSettled,
                                   SettlementId = TestData.SettlementId,
                                   ValueOfFeesSettled = TestData.ValueOfFeesSettled,
                                   SettlementFees = TestData.SettlementFeeModels
                               };

        public static List<SettlementModel> SettlementModels =>
            new List<SettlementModel>{
                                         new SettlementModel{
                                                                IsCompleted = TestData.SettlementIsCompleted,
                                                                SettlementDate = TestData.SettlementDate,
                                                                NumberOfFeesSettled = TestData.NumberOfFeesSettled,
                                                                SettlementId = TestData.SettlementId,
                                                                ValueOfFeesSettled = TestData.ValueOfFeesSettled
                                                            }
                                     };

        public static SettlementProcessingStartedEvent SettlementProcessingStartedEvent =>
            new SettlementProcessingStartedEvent(TestData.SettlementId,
                                                 TestData.EstateId,
                                                 TestData.MerchantId,
                                                 TestData.ProcessingStartedDateTime);

        public static SettlementScheduleChangedEvent SettlementScheduleChangedEvent => new SettlementScheduleChangedEvent(TestData.MerchantId, TestData.EstateId, (Int32)TestData.SettlementSchedule, TestData.NextSettlementDate);

        public static StatementCreatedEvent StatementCreatedEvent => new StatementCreatedEvent(TestData.MerchantStatementId, TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);

        public static StatementGeneratedEvent StatementGeneratedEvent => new StatementGeneratedEvent(TestData.MerchantStatementId, TestData.EstateId, TestData.MerchantId, TestData.StatementGeneratedDate);

        public static MerchantCommands.SwapMerchantDeviceCommand SwapMerchantDeviceCommand => new MerchantCommands.SwapMerchantDeviceCommand(TestData.EstateId, TestData.MerchantId, TestData.DeviceIdentifier, TestData.SwapMerchantDeviceRequest);

        public static SwapMerchantDeviceRequest SwapMerchantDeviceRequest =>
            new SwapMerchantDeviceRequest{
                                             NewDeviceIdentifier = TestData.NewDeviceIdentifier
                                         };

        public static Models.MerchantStatement.Transaction Transaction1 =>
            new Models.MerchantStatement.Transaction{
                                                        Amount = TestData.TransactionAmount1.Value,
                                                        DateTime = TestData.TransactionDateTime1,
                                                        TransactionId = TestData.TransactionId1
                                                    };

        public static Models.MerchantStatement.Transaction Transaction2 =>
            new Models.MerchantStatement.Transaction{
                                                        Amount = TestData.TransactionAmount2.Value,
                                                        DateTime = TestData.TransactionDateTime2,
                                                        TransactionId = TestData.TransactionId2
                                                    };

        public static TransactionAddedToStatementEvent TransactionAddedToStatementEvent =>
            new TransactionAddedToStatementEvent(TestData.MerchantStatementId,
                                                 Guid.NewGuid(),
                                                 TestData.EstateId,
                                                 TestData.MerchantId,
                                                 TestData.TransactionId,
                                                 TestData.TransactionDateTime,
                                                 TestData.TransactionAmount.GetValueOrDefault());

        public static TransactionAuthorisedByOperatorEvent TransactionAuthorisedByOperatorEvent =>
            new TransactionAuthorisedByOperatorEvent(TestData.TransactionId,
                                                     TestData.EstateId,
                                                     TestData.MerchantId,
                                                     TestData.OperatorName,
                                                     TestData.AuthorisationCode,
                                                     TestData.OperatorResponseCode,
                                                     TestData.OperatorResponseMessage,
                                                     TestData.OperatorTransactionId,
                                                     TestData.ResponseCode,
                                                     TestData.ResponseMessage);

        public static TransactionDeclinedByOperatorEvent TransactionDeclinedByOperatorEvent =>
            new TransactionDeclinedByOperatorEvent(TestData.TransactionId,
                                                   TestData.EstateId,
                                                   TestData.MerchantId,
                                                   TestData.OperatorName,
                                                   TestData.DeclinedOperatorResponseCode,
                                                   TestData.DeclinedOperatorResponseMessage,
                                                   TestData.DeclinedResponseCode,
                                                   TestData.DeclinedResponseMessage);

        public static TransactionFeeForProductAddedToContractEvent TransactionFeeForProductAddedToContractEvent =>
            new TransactionFeeForProductAddedToContractEvent(TestData.ContractId,
                                                             TestData.EstateId,
                                                             TestData.ProductId,
                                                             TestData.TransactionFeeId,
                                                             TestData.TransactionFeeDescription,
                                                             TestData.FeeCalculationType,
                                                             TestData.FeeType,
                                                             TestData.FeeValue);

        public static TransactionFeeForProductDisabledEvent TransactionFeeForProductDisabledEvent => new TransactionFeeForProductDisabledEvent(TestData.ContractId, TestData.EstateId, TestData.ProductId, TestData.TransactionFeeId);

        public static TransactionHasBeenCompletedEvent TransactionHasBeenCompletedEvent =>
            new TransactionHasBeenCompletedEvent(TestData.TransactionId1,
                                                 TestData.EstateId,
                                                 TestData.MerchantId,
                                                 TestData.ResponseCode,
                                                 TestData.ResponseMessage,
                                                 TestData.IsAuthorisedTrue,
                                                 TestData.TransactionDateTime1,
                                                 TestData.TransactionAmount1);

        public static TransactionHasBeenLocallyAuthorisedEvent TransactionHasBeenLocallyAuthorisedEvent =>
            new TransactionHasBeenLocallyAuthorisedEvent(TestData.TransactionId,
                                                         TestData.EstateId,
                                                         TestData.MerchantId,
                                                         TestData.AuthorisationCode,
                                                         TestData.ResponseCode,
                                                         TestData.ResponseMessage);

        public static TransactionHasBeenLocallyDeclinedEvent TransactionHasBeenLocallyDeclinedEvent =>
            new TransactionHasBeenLocallyDeclinedEvent(TestData.TransactionId,
                                                       TestData.EstateId,
                                                       TestData.MerchantId,
                                                       TestData.DeclinedResponseCode,
                                                       TestData.DeclinedResponseMessage);

        public static TransactionHasStartedEvent TransactionHasStartedEvent =>
            new TransactionHasStartedEvent(TestData.TransactionId,
                                           TestData.EstateId,
                                           TestData.MerchantId,
                                           TestData.TransactionDateTime,
                                           TestData.TransactionNumber,
                                           TestData.TransactionType,
                                           TestData.TransactionReference,
                                           TestData.DeviceIdentifier,
                                           TestData.TransactionAmount);

        public static TransactionSourceAddedToTransactionEvent TransactionSourceAddedToTransactionEvent => new TransactionSourceAddedToTransactionEvent(TestData.TransactionId, TestData.EstateId, TestData.MerchantId, TestData.TransactionSource);

        public static MerchantCommands.UpdateMerchantAddressCommand UpdateMerchantAddressCommand =>
            new MerchantCommands.UpdateMerchantAddressCommand(TestData.EstateId,
                                                              TestData.MerchantId,
                                                              Guid.NewGuid(),
                                                              TestData.Address);

        public static MerchantCommands.UpdateMerchantCommand UpdateMerchantCommand => new(TestData.EstateId, TestData.MerchantId, TestData.UpdateMerchantRequest);

        public static MerchantCommands.UpdateMerchantContactCommand UpdateMerchantContactCommand =>
            new MerchantCommands.UpdateMerchantContactCommand(TestData.EstateId,
                                                              TestData.MerchantId,
                                                              Guid.NewGuid(),
                                                              TestData.Contact);
        public static UpdateMerchantRequest UpdateMerchantRequest =>
            new UpdateMerchantRequest{
                                         Name = TestData.MerchantNameUpdated,
                                         SettlementSchedule = DataTransferObjects.Responses.Merchant.SettlementSchedule.NotSet
                                     };

        public static VariableValueProductAddedToContractEvent VariableValueProductAddedToContractEvent => new VariableValueProductAddedToContractEvent(TestData.ContractId, TestData.EstateId, TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, (Int32)TestData.ProductTypeMobileTopup);

        public static VoucherFullyRedeemedEvent VoucherFullyRedeemedEvent =>
            new VoucherFullyRedeemedEvent(TestData.VoucherId,
                                          TestData.EstateId,
                                          TestData.VoucherRedeemedDate);

        public static VoucherGeneratedEvent VoucherGeneratedEvent =>
            new VoucherGeneratedEvent(TestData.VoucherId,
                                      TestData.EstateId,
                                      TestData.TransactionId,
                                      TestData.VoucherGeneratedDate,
                                      TestData.OperatorIdentifier,
                                      TestData.VoucherValue,
                                      TestData.VoucherCode,
                                      TestData.VoucherExpiryDate,
                                      TestData.VoucherMessage);

        public static VoucherIssuedEvent VoucherIssuedEvent =>
            new VoucherIssuedEvent(TestData.VoucherId,
                                   TestData.EstateId,
                                   TestData.VoucherIssuedDate,
                                   TestData.RecipientEmail,
                                   TestData.RecipientMobile);

        #endregion

        #region Methods

        public static ContractAggregate CreatedContractAggregate(){
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            return contractAggregate;
        }

        public static ContractAggregate CreatedContractAggregateWithAProduct(){
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            contractAggregate.AddFixedValueProduct(TestData.ProductId,
                                                   TestData.ProductName,
                                                   TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue,
                                                   TestData.ProductTypeMobileTopup);

            return contractAggregate;
        }

        public static ContractAggregate CreatedContractAggregateWithAProductAndTransactionFee(CalculationType calculationType, FeeType feeType){
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            contractAggregate.AddFixedValueProduct(TestData.ProductId,
                                                   TestData.ProductName,
                                                   TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue,
                                                   TestData.ProductTypeMobileTopup);

            Product product = contractAggregate.GetProducts().Single(p => p.ProductId == TestData.ProductId);
            contractAggregate.AddTransactionFee(product,
                                                TestData.TransactionFeeId,
                                                TestData.TransactionFeeDescription,
                                                calculationType,
                                                feeType,
                                                TestData.TransactionFeeValue);

            return contractAggregate;
        }

        public static OperatorAggregate CreatedOperatorAggregate(){
            OperatorAggregate operatorAggregate = OperatorAggregate.Create(TestData.OperatorId);
            operatorAggregate.Create(TestData.EstateId, TestData.OperatorName, TestData.RequireCustomMerchantNumber, TestData.RequireCustomTerminalNumber);
            return operatorAggregate;
        }

        public static EstateAggregate CreatedEstateAggregate(){
            EstateAggregate estateAggregate = EstateAggregate.Create(TestData.EstateId);

            estateAggregate.Create(TestData.EstateName);

            return estateAggregate;
        }

        public static MerchantAggregate CreatedMerchantAggregate(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            return merchantAggregate;
        }

        public static MerchantDepositListAggregate CreatedMerchantDepositListAggregate(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            MerchantDepositListAggregate merchantDepositListAggregate = MerchantDepositListAggregate.Create(TestData.MerchantId);
            merchantDepositListAggregate.Create(merchantAggregate, TestData.DateMerchantCreated);

            return merchantDepositListAggregate;
        }

        public static ContractAggregate EmptyContractAggregate(){
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            return contractAggregate;
        }

        public static MerchantAggregate EmptyMerchantAggregate(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            return merchantAggregate;
        }

        public static EstateAggregate EstateAggregateWithOperator(){
            EstateAggregate estateAggregate = EstateAggregate.Create(TestData.EstateId);

            estateAggregate.Create(TestData.EstateName);
            estateAggregate.AddOperator(TestData.OperatorId);

            return estateAggregate;
        }

        public static MerchantStatementAggregate GeneratedMerchantStatementAggregate(){
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregate.AddTransactionToStatement(TestData.MerchantStatementId,
                                                                 TestData.EventId1,
                                                                 TestData.StatementCreateDate,
                                                                 TestData.EstateId,
                                                                 TestData.MerchantId,
                                                                 new Models.MerchantStatement.Transaction{
                                                                                                             Amount = TestData.TransactionAmount1.Value,
                                                                                                             DateTime = TestData.TransactionDateTime1,
                                                                                                             TransactionId = TestData.TransactionId1
                                                                                                         });
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.MerchantStatementId,
                                                                TestData.EventId1,
                                                                TestData.StatementCreateDate,
                                                                TestData.EstateId,
                                                                TestData.MerchantId,
                                                                new SettledFee{
                                                                                  Amount = TestData.SettledFeeAmount1,
                                                                                  DateTime = TestData.SettledFeeDateTime1,
                                                                                  SettledFeeId = TestData.SettledFeeId1,
                                                                                  TransactionId = TestData.TransactionId1
                                                                              });
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            return merchantStatementAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithAddress(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddAddress(TestData.MerchantAddressLine1,
                                         TestData.MerchantAddressLine2,
                                         TestData.MerchantAddressLine3,
                                         TestData.MerchantAddressLine4,
                                         TestData.MerchantTown,
                                         TestData.MerchantRegion,
                                         TestData.MerchantPostalCode,
                                         TestData.MerchantCountry);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithContact(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddContact(TestData.MerchantContactName,
                                         TestData.MerchantContactPhoneNumber,
                                         TestData.MerchantContactEmailAddress);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithDevice(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithEverything(SettlementSchedule settlementSchedule){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddContact(TestData.MerchantContactName,
                                         TestData.MerchantContactPhoneNumber,
                                         TestData.MerchantContactEmailAddress);
            merchantAggregate.AddAddress(TestData.MerchantAddressLine1,
                                         TestData.MerchantAddressLine2,
                                         TestData.MerchantAddressLine3,
                                         TestData.MerchantAddressLine4,
                                         TestData.MerchantTown,
                                         TestData.MerchantRegion,
                                         TestData.MerchantPostalCode,
                                         TestData.MerchantCountry);
            merchantAggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);
            merchantAggregate.SetSettlementSchedule(settlementSchedule);
            merchantAggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);
            merchantAggregate.AddContract(TestData.CreatedContractAggregate());
            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithOperator(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);

            return merchantAggregate;
        }

        public static Models.Merchant.Merchant MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts(SettlementSchedule settlementSchedule = SettlementSchedule.Immediate) =>
            new Models.Merchant.Merchant{
                                            EstateId = TestData.EstateId,
                                            MerchantId = TestData.MerchantId,
                                            MerchantName = TestData.MerchantName,
                                            SettlementSchedule = settlementSchedule,
                                            Addresses = new List<Models.Merchant.Address>{
                                                                                             new Models.Merchant.Address{
                                                                                                                            Town = TestData.MerchantTown,
                                                                                                                            AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                                            AddressId = Guid.NewGuid(),
                                                                                                                            Region = TestData.MerchantRegion,
                                                                                                                            Country = TestData.MerchantCountry,
                                                                                                                            AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                                            AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                                            AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                                            PostalCode = TestData.MerchantPostalCode
                                                                                                                        }
                                                                                         },
                                            Contacts = new List<Models.Merchant.Contact>{
                                                                                            new Models.Merchant.Contact{
                                                                                                                           ContactId = Guid.NewGuid(),
                                                                                                                           ContactName = TestData.MerchantContactName,
                                                                                                                           ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                                           ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                                                       }
                                                                                        },
                                            Devices = new List<Device>{
                                                                          new Device{
                                                                                        DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                        IsEnabled = true,
                                                                                        DeviceId = TestData.DeviceId,
                                                                                    }
                                                                      },
                                            Operators = new List<Models.Merchant.Operator>{
                                                                                              new Models.Merchant.Operator{
                                                                                                                              Name = TestData.OperatorName,
                                                                                                                              TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                                              MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                                              OperatorId = TestData.OperatorId
                                                                                                                          }
                                                                                          },
                                            Contracts = new List<Models.Merchant.Contract>{
                                                                                              new Models.Merchant.Contract{
                                                                                                                              ContractId = TestData.ContractId,
                                                                                                                              ContractProducts = new List<Guid>{
                                                                                                                                                                   Guid.Parse("8EF716B9-422D-4FC6-B5A7-22FC4BABDD97")
                                                                                                                                                               }
                                                                                                                          }
                                                                                          }
                                        };

        public static MerchantStatementAggregate MerchantStatementAggregateWithTransactionLineAdded(){
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregate.AddTransactionToStatement(TestData.MerchantStatementId,
                                                                 TestData.EventId1,
                                                                 TestData.StatementCreateDate,
                                                                 TestData.EstateId,
                                                                 TestData.MerchantId,
                                                                 new Models.MerchantStatement.Transaction{
                                                                                                             Amount = TestData.TransactionAmount1.Value,
                                                                                                             DateTime = TestData.TransactionDateTime1,
                                                                                                             TransactionId = TestData.TransactionId1
                                                                                                         });

            return merchantStatementAggregate;
        }

        public static MerchantStatementAggregate MerchantStatementAggregateWithTransactionLineAndSettledFeeAdded(){
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.AddTransactionToStatement(TestData.MerchantStatementId,
                                                                 TestData.EventId1,
                                                                 TestData.StatementCreateDate,
                                                                 TestData.EstateId,
                                                                 TestData.MerchantId,
                                                                 new Models.MerchantStatement.Transaction{
                                                                                                             Amount = TestData.TransactionAmount1.Value,
                                                                                                             DateTime = TestData.TransactionDateTime1,
                                                                                                             TransactionId = TestData.TransactionId1
                                                                                                         });
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.MerchantStatementId,
                                                                TestData.EventId1,
                                                                TestData.StatementCreateDate,
                                                                TestData.EstateId,
                                                                TestData.MerchantId,
                                                                new SettledFee{
                                                                                  Amount = TestData.SettledFeeAmount1,
                                                                                  DateTime = TestData.SettledFeeDateTime1,
                                                                                  SettledFeeId = TestData.SettledFeeId1,
                                                                                  TransactionId = TestData.TransactionId1
                                                                              });

            return merchantStatementAggregate;
        }

        public static TokenResponse TokenResponse(){
            return SecurityService.DataTransferObjects.Responses.TokenResponse.Create("AccessToken", String.Empty, 100);
        }

        public static EstateCommands.AddOperatorToEstateCommand AddOperatorToEstateCommand => new EstateCommands.AddOperatorToEstateCommand(TestData.EstateId, TestData.AssignOperatorRequestToEstate);
        public static DataTransferObjects.Requests.Estate.AssignOperatorRequest AssignOperatorRequestToEstate =>
            new(){
                     OperatorId = TestData.OperatorId
                 };

        #endregion

        public static Models.Operator.Operator OperatorModel =>
            new Models.Operator.Operator(){
                                              OperatorId = TestData.OperatorId,
                                              RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                                              RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                                              Name = TestData.OperatorName
                                          };

        public static OperatorAggregate EmptyOperatorAggregate(){
            OperatorAggregate operatorAggregate = OperatorAggregate.Create(TestData.OperatorId);

            return operatorAggregate;
        }
    }
}
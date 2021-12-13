namespace EstateManagement.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BusinessLogic.Events;
    using BusinessLogic.Requests;
    using CallbackHandler.DataTransferObjects;
    using ContractAggregate;
    using EstateAggregate;
    using EstateReporting.Database.Entities;
    using EstateReporting.Database.ViewEntities;
    using MerchantAggregate;
    using Models;
    using Models.Contract;
    using Models.Merchant;
    using Models.MerchantStatement;
    using Newtonsoft.Json;
    using Address = Models.Merchant.Address;
    using Contact = Models.Merchant.Contact;
    using Contract = Models.Contract.Contract;
    using CreateEstateRequest = BusinessLogic.Requests.CreateEstateRequest;
    using CreateEstateRequestDTO = DataTransferObjects.Requests.CreateEstateRequest;
    using CreateMerchantRequest = BusinessLogic.Requests.CreateMerchantRequest;
    using Estate = Models.Estate;
    using Merchant = Models.Merchant.Merchant;
    using MerchantBalanceHistory = Models.Merchant.MerchantBalanceHistory;
    using Operator = Models.Estate.Operator;
    using SecurityUser = Models.SecurityUser;
    using TransactionFeeModel = Models.Contract.TransactionFee;
    using Transaction = Models.MerchantStatement.Transaction;

    public class TestData
    {
        #region Fields

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static String EstateName = "Test Estate 1";

        public static String EstateReference = "C6634DE3";

        public static CreateEstateRequest CreateEstateRequest = CreateEstateRequest.Create(TestData.EstateId, TestData.EstateName);

        public static CreateEstateRequestDTO CreateEstateRequestDTO = new CreateEstateRequestDTO
        {
                                                                    EstateId = Guid.NewGuid(),
                                                                    EstateName = TestData.EstateName
                                                                };

        public static String MerchantAddressLine1 = "Address Line 1";

        public static String MerchantAddressLine2 = "Address Line 2";

        public static String MerchantAddressLine3 = "Address Line 3";

        public static String MerchantAddressLine4 = "Address Line 4";

        public static String MerchantContactEmailAddress = "testcontact@merchant1.co.uk";

        public static String MerchantContactName = "Mr Test Contact";

        public static String MerchantContactPhoneNumber = "1234567890";

        public static String MerchantCountry = "United Kingdom";

        public static Guid MerchantId = Guid.Parse("ac476195-f993-4712-8ea1-cb41c0b44328");

        public static String MerchantReference = "82DD8D48";

        public static Decimal AvailableBalance = 1000.00m;
        
        public static Decimal Balance = 1000.00m;

        public static String MerchantName = "Test Merchant 1";

        public static String MerchantPostalCode = "TE571NG";

        public static String MerchantRegion = "Test Region";

        public static String MerchantTown = "Test Town";

        public static CreateMerchantRequest CreateMerchantRequest = CreateMerchantRequest.Create(TestData.EstateId,
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
                                                                                                 TestData.SettlementSchedule);

        public static EstateAggregate EmptyEstateAggregate = EstateAggregate.Create(TestData.EstateId);

        public static Estate.Estate EstateModel = new Estate.Estate
                                           {
                                               EstateId = TestData.EstateId,
                                               Name = TestData.EstateName,
                                               Operators = null,
                                               SecurityUsers = null
        };
        
        public static Estate.Estate EstateModelWithOperators = new Estate.Estate
                                           {
                                               EstateId = TestData.EstateId,
                                               Name = TestData.EstateName,
                                               Operators = new List<Estate.Operator>
                                                           {
                                                               new Estate.Operator
                                                               {
                                                                   RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumberTrue,
                                                                   Name = TestData.OperatorName,
                                                                   OperatorId = TestData.OperatorId,
                                                                   RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumberTrue
                                                               }
                                                           },
                                               SecurityUsers = null
        };

        public static Estate.Estate EstateModelWithSecurityUsers = new Estate.Estate
                                                                   {
                                                                       EstateId = TestData.EstateId,
                                                                       Name = TestData.EstateName,
                                                                       Operators = null,
                                                                       SecurityUsers = new List<SecurityUser>
                                                                                       {
                                                                                           new SecurityUser
                                                                                           {
                                                                                               EmailAddress = TestData.EstateUserEmailAddress,
                                                                                               SecurityUserId = TestData.SecurityUserId
                                                                                           }
                                                                                       }
                                                                   };

        public static Estate.Estate EstateModelWithOperatorsAndSecurityUsers = new Estate.Estate
                                                                               {
                                                                                   EstateId = TestData.EstateId,
                                                                                   Name = TestData.EstateName,
                                                                                   Operators = new List<Estate.Operator>
                                                                                               {
                                                                                                   new Estate.Operator
                                                                                                   {
                                                                                                       RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumberTrue,
                                                                                                       Name = TestData.OperatorName,
                                                                                                       OperatorId = TestData.OperatorId,
                                                                                                       RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumberTrue
                                                                                                   }
                                                                                               },
                                                                                   SecurityUsers = new List<SecurityUser>
                                                                                                   {
                                                                                                       new SecurityUser
                                                                                                       {
                                                                                                           EmailAddress = TestData.EstateUserEmailAddress,
                                                                                                           SecurityUserId = TestData.SecurityUserId
                                                                                                       }
                                                                                                   }
                                                                               };

        public static DateTime DateMerchantCreated = new DateTime(2019,11,16);

        public static Guid MerchantAddressId = Guid.Parse("F463D464-CD2F-4293-98F1-A31529B12426");

        public static Guid MerchantContactId = Guid.Parse("37B08E8A-8B1E-482A-AE9C-C87DC3B36026");

        #endregion

        #region Methods

        public static ContractAggregate EmptyContractAggregate()
        {
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            return contractAggregate;
        }

        public static ContractAggregate CreatedContractAggregate()
        {
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            return contractAggregate;
        }

        public static EstateAggregate CreatedEstateAggregate()
        {
            EstateAggregate estateAggregate = EstateAggregate.Create(TestData.EstateId);

            estateAggregate.Create(TestData.EstateName);

            return estateAggregate;
        }

        public static EstateAggregate EstateAggregateWithOperator(Boolean requireCustomMerchantNumber = false, Boolean requireCustomTerminalNumber = false)
        {
            EstateAggregate estateAggregate = EstateAggregate.Create(TestData.EstateId);

            estateAggregate.Create(TestData.EstateName);
            estateAggregate.AddOperator(TestData.OperatorId,TestData.OperatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);

            return estateAggregate;
        }

        #endregion

        public static MerchantAggregate CreatedMerchantAggregate()
        {
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithDevice()
        {
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithAddress()
        {
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddAddress(TestData.MerchantAddressId, TestData.MerchantAddressLine1, TestData.MerchantAddressLine2, 
                                         TestData.MerchantAddressLine3, TestData.MerchantAddressLine4, TestData.MerchantTown, 
                                         TestData.MerchantRegion, TestData.MerchantPostalCode, TestData.MerchantCountry);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithContact()
        {
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AddContact(TestData.MerchantContactId, TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                         TestData.MerchantContactEmailAddress);

            return merchantAggregate;
        }

        public static MerchantAggregate MerchantAggregateWithOperator()
        {
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            merchantAggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);

            return merchantAggregate;
        }

        public static Merchant MerchantModelWithAddressesContactsDevicesAndOperators(SettlementSchedule settlementSchedule = SettlementSchedule.Immediate) => new Merchant
                                                                       {
                                                                           MerchantId = TestData.MerchantId,
                                                                           MerchantName = TestData.MerchantName,
                                                                           EstateId = TestData.EstateId,
                                                                           SettlementSchedule = settlementSchedule,
                                                                           Addresses = new List<Address>
                                                                                       {
                                                                                           new Address
                                                                                           {
                                                                                               Town = TestData.MerchantTown,
                                                                                               AddressLine4 = TestData.MerchantAddressLine4,
                                                                                               AddressId = TestData.MerchantAddressId,
                                                                                               Region = TestData.MerchantRegion,
                                                                                               Country = TestData.MerchantCountry,
                                                                                               AddressLine1 = TestData.MerchantAddressLine1,
                                                                                               AddressLine2 = TestData.MerchantAddressLine2,
                                                                                               AddressLine3 = TestData.MerchantAddressLine3,
                                                                                               PostalCode = TestData.MerchantPostalCode
                                                                                           }
                                                                                       },
                                                                           Contacts = new List<Contact>
                                                                                      {
                                                                                          new Contact
                                                                                          {
                                                                                              ContactId = TestData.MerchantContactId,
                                                                                              ContactName = TestData.MerchantContactName,
                                                                                              ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                              ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                          }
                                                                                      },
                                                                           Devices = new Dictionary<Guid, String>
                                                                                     {
                                                                                         {TestData.DeviceId, TestData.DeviceIdentifier}
                                                                                     },
                                                                           Operators = new List<Models.Merchant.Operator>
                                                                                       {
                                                                                           new Models.Merchant.Operator
                                                                                           {
                                                                                               Name = TestData.OperatorName,
                                                                                               TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                               MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                               OperatorId = TestData.OperatorId
                                                                                           }
                                                                                       }
                                                                       };

        public static Merchant MerchantModelWithNullAddresses = new Merchant
                                                                               {
                                                                                   MerchantId = TestData.MerchantId,
                                                                                   MerchantName = TestData.MerchantName,
                                                                                   EstateId = TestData.EstateId,
                                                                                   SettlementSchedule = SettlementSchedule.Immediate,
            Addresses = null,
                                                                                   Contacts = new List<Contact>
                                                                                              {
                                                                                                  new Contact
                                                                                                  {
                                                                                                      ContactId = TestData.MerchantContactId,
                                                                                                      ContactName = TestData.MerchantContactName,
                                                                                                      ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                      ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                                  }
                                                                                              },
                                                                                   Devices = new Dictionary<Guid, String>
                                                                                             {
                                                                                                 {TestData.DeviceId, TestData.DeviceIdentifier}
                                                                                             },
                                                                                   Operators = new List<Models.Merchant.Operator>
                                                                                               {
                                                                                                   new Models.Merchant.Operator
                                                                                                   {
                                                                                                       Name = TestData.OperatorName,
                                                                                                       TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                       MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                       OperatorId = TestData.OperatorId
                                                                                                   }
                                                                                               }
                                                                               };

        public static Merchant MerchantModelWithNullContacts = new Merchant
                                                                           {
                                                                               MerchantId = TestData.MerchantId,
                                                                               MerchantName = TestData.MerchantName,
                                                                               EstateId = TestData.EstateId,
                                                                               SettlementSchedule = SettlementSchedule.Immediate,
            Addresses = new List<Address>
                                                                                           {
                                                                                               new Address
                                                                                               {
                                                                                                   Town = TestData.MerchantTown,
                                                                                                   AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                   AddressId = TestData.MerchantAddressId,
                                                                                                   Region = TestData.MerchantRegion,
                                                                                                   Country = TestData.MerchantCountry,
                                                                                                   AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                   AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                   AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                   PostalCode = TestData.MerchantPostalCode
                                                                                               }
                                                                                           },
                                                                               Contacts = null,
                                                                               Devices = new Dictionary<Guid, String>
                                                                                         {
                                                                                             {TestData.DeviceId, TestData.DeviceIdentifier}
                                                                                         },
                                                                               Operators = new List<Models.Merchant.Operator>
                                                                                           {
                                                                                               new Models.Merchant.Operator
                                                                                               {
                                                                                                   Name = TestData.OperatorName,
                                                                                                   TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                   MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                                   OperatorId = TestData.OperatorId
                                                                                               }
                                                                                           }
        };

        public static Merchant MerchantModelWithNullDevices = new Merchant
                                                              {
                                                                  MerchantId = TestData.MerchantId,
                                                                  MerchantName = TestData.MerchantName,
                                                                  EstateId = TestData.EstateId,
                                                                  SettlementSchedule = SettlementSchedule.Immediate,
            Addresses = new List<Address>
                                                                              {
                                                                                  new Address
                                                                                  {
                                                                                      Town = TestData.MerchantTown,
                                                                                      AddressLine4 = TestData.MerchantAddressLine4,
                                                                                      AddressId = TestData.MerchantAddressId,
                                                                                      Region = TestData.MerchantRegion,
                                                                                      Country = TestData.MerchantCountry,
                                                                                      AddressLine1 = TestData.MerchantAddressLine1,
                                                                                      AddressLine2 = TestData.MerchantAddressLine2,
                                                                                      AddressLine3 = TestData.MerchantAddressLine3,
                                                                                      PostalCode = TestData.MerchantPostalCode
                                                                                  }
                                                                              },
                                                                  Contacts = new List<Contact>
                                                                             {
                                                                                 new Contact
                                                                                 {
                                                                                     ContactId = TestData.MerchantContactId,
                                                                                     ContactName = TestData.MerchantContactName,
                                                                                     ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                     ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                 }
                                                                             },
                                                                  Devices = null,
                                                                  Operators = new List<Models.Merchant.Operator>
                                                                              {
                                                                                  new Models.Merchant.Operator
                                                                                  {
                                                                                      Name = TestData.OperatorName,
                                                                                      TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                      MerchantNumber = TestData.OperatorMerchantNumber,
                                                                                      OperatorId = TestData.OperatorId
                                                                                  }
                                                                              }
                                                              };

        public static Merchant MerchantModelWithNullOperators = new Merchant
                                                                {
                                                                    MerchantId = TestData.MerchantId,
                                                                    MerchantName = TestData.MerchantName,
                                                                    EstateId = TestData.EstateId,
                                                                    SettlementSchedule = SettlementSchedule.Immediate,
            Addresses = new List<Address>
                                                                                {
                                                                                    new Address
                                                                                    {
                                                                                        Town = TestData.MerchantTown,
                                                                                        AddressLine4 = TestData.MerchantAddressLine4,
                                                                                        AddressId = TestData.MerchantAddressId,
                                                                                        Region = TestData.MerchantRegion,
                                                                                        Country = TestData.MerchantCountry,
                                                                                        AddressLine1 = TestData.MerchantAddressLine1,
                                                                                        AddressLine2 = TestData.MerchantAddressLine2,
                                                                                        AddressLine3 = TestData.MerchantAddressLine3,
                                                                                        PostalCode = TestData.MerchantPostalCode
                                                                                    }
                                                                                },
                                                                    Contacts = new List<Contact>
                                                                               {
                                                                                   new Contact
                                                                                   {
                                                                                       ContactId = TestData.MerchantContactId,
                                                                                       ContactName = TestData.MerchantContactName,
                                                                                       ContactPhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                       ContactEmailAddress = TestData.MerchantContactEmailAddress
                                                                                   }
                                                                               },
                                                                    Devices = new Dictionary<Guid, String>
                                                                              {
                                                                                  {TestData.DeviceId, TestData.DeviceIdentifier}
                                                                              },
                                                                    Operators = null
                                                                };

        public static Guid OperatorId = Guid.Parse("6A63DA8B-4621-4731-90B1-A9D09B130D4B");

        public static Guid OperatorId2 = Guid.Parse("8E5741AA-66EC-42D9-BE0F-AA106B41AED1");

        public static String OperatorName = "Test Operator 1";

        public static String OperatorName2 = "Test Operator Name 2";

        public static Boolean RequireCustomMerchantNumberTrue = true;
        
        public static Boolean RequireCustomMerchantNumberFalse = false;

        public static Boolean RequireCustomTerminalNumberTrue = true;

        public static Boolean RequireCustomTerminalNumberFalse = false;

        public static AddOperatorToEstateRequest AddOperatorToEstateRequest = AddOperatorToEstateRequest.Create(TestData.EstateId, TestData.OperatorId,
                                                                                                 TestData.OperatorName,TestData.RequireCustomMerchantNumberFalse,
                                                                                                 TestData.RequireCustomTerminalNumberFalse);

        public static String OperatorMerchantNumber = "00000001";

        public static String OperatorTerminalNumber = "00000001";

        public static AssignOperatorToMerchantRequest AssignOperatorToMerchantRequest = AssignOperatorToMerchantRequest.Create(TestData.EstateId,
                                                                                                                               TestData.MerchantId,
                                                                                                                               TestData.MerchantId,
                                                                                                                               TestData.OperatorMerchantNumber,
                                                                                                                               TestData.OperatorTerminalNumber);

        public static String EstateUserEmailAddress = "testestateuser@estate1.co.uk";

        public static String MerchantUserEmailAddress = "testmerchantuser@merchant1.co.uk";

        public static String EstateUserPassword="123456";

        public static String MerchantUserPassword = "123456";

        public static String EstateUserGivenName = "Test";

        public static String MerchantUserGivenName = "Test";

        public static String EstateUserMiddleName = "Middle";

        public static String MerchantUserMiddleName = "Middle";

        public static String EstateUserFamilyName = "Estate";
        public static String MerchantUserFamilyName = "Merchant";

        public static CreateEstateUserRequest CreateEstateUserRequest = CreateEstateUserRequest.Create(TestData.EstateId,
                                                                                                       TestData.EstateUserEmailAddress,
                                                                                                       TestData.EstateUserPassword,
                                                                                                       TestData.EstateUserGivenName,
                                                                                                       TestData.EstateUserMiddleName,
                                                                                                       TestData.EstateUserFamilyName);

        public static Guid SecurityUserId = Guid.Parse("45B74A2E-BF92-44E9-A300-08E5CDEACFE3");

        public static CreateMerchantUserRequest CreateMerchantUserRequest = CreateMerchantUserRequest.Create(TestData.EstateId,
                                                                                                             TestData.MerchantId,
                                                                                                             TestData.MerchantUserEmailAddress,
                                                                                                             TestData.MerchantUserPassword,
                                                                                                             TestData.MerchantUserGivenName,
                                                                                                             TestData.MerchantUserMiddleName,
                                                                                                             TestData.MerchantUserFamilyName);

        public static Guid DeviceId = Guid.Parse("B434EA1A-1684-442F-8BEB-21D84C4F53B3");
        public static String DeviceIdentifier = "EMULATOR123456";
        public static String NewDeviceIdentifier = "EMULATOR78910";

        public static AddMerchantDeviceRequest AddMerchantDeviceRequest = AddMerchantDeviceRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.DeviceId, TestData.DeviceIdentifier);

        public static EstateReporting.Database.Entities.Estate EstateEntity = new EstateReporting.Database.Entities.Estate
                                                                              {
                                                                                  Name = TestData.EstateName,
                                                                                  EstateId = TestData.EstateId
                                                                              };
        
        public static EstateOperator EstateOperatorEntity = new EstateOperator
                                                            {
                                                                EstateId = TestData.EstateId,
                                                                OperatorId = TestData.OperatorId,
                                                                RequireCustomTerminalNumber = TestData.RequireCustomMerchantNumberTrue,
                                                                Name = TestData.OperatorName,
                                                                RequireCustomMerchantNumber = TestData.RequireCustomTerminalNumberTrue
                                                            };

        public static EstateOperator EstateOperatorEntity2 = new EstateOperator
                                                            {
                                                                EstateId = TestData.EstateId,
                                                                OperatorId = TestData.OperatorId2,
                                                                RequireCustomTerminalNumber = TestData.RequireCustomMerchantNumberTrue,
                                                                Name = TestData.OperatorName2,
                                                                RequireCustomMerchantNumber = TestData.RequireCustomTerminalNumberTrue
                                                            };

        public static EstateSecurityUser EstateSecurityUserEntity = new EstateSecurityUser
                                                                    {
                                                                        EstateId = TestData.EstateId,
                                                                        EmailAddress = TestData.EstateUserEmailAddress,
                                                                        SecurityUserId = TestData.SecurityUserId
                                                                    };

        public static EstateReporting.Database.Entities.Merchant MerchantEntity = new EstateReporting.Database.Entities.Merchant
                                                                                  {
            MerchantId = TestData.MerchantId,
            EstateId = TestData.EstateId,
            CreatedDateTime= TestData.DateMerchantCreated,
            Name = TestData.MerchantName
            
                                                                                  };

        public static EstateReporting.Database.Entities.MerchantContact MerchantContactEntity = new MerchantContact
                                                                                                {
                                                                                                    ContactId = TestData.MerchantContactId,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    Name = TestData.MerchantContactName,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    EmailAddress = TestData.MerchantContactEmailAddress,
                                                                                                    PhoneNumber = TestData.MerchantContactPhoneNumber,
                                                                                                    CreatedDateTime = TestData.DateMerchantCreated
                                                                                                };

        public static EstateReporting.Database.Entities.MerchantAddress MerchantAddressEntity = new MerchantAddress
                                                                                                {
                                                                                                    AddressLine1 = TestData.MerchantAddressLine1,
                                                                                                    MerchantId = TestData.MerchantId,
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    CreatedDateTime = TestData.DateMerchantCreated,
                                                                                                    AddressLine2 = TestData.MerchantAddressLine2,
                                                                                                    AddressLine3 = TestData.MerchantAddressLine3,
                                                                                                    AddressLine4 = TestData.MerchantAddressLine4,
                                                                                                    Country = TestData.MerchantCountry,
                                                                                                    PostalCode = TestData.MerchantPostalCode,
                                                                                                    Region = TestData.MerchantRegion,
                                                                                                    Town = TestData.MerchantTown,
                                                                                                    AddressId = TestData.MerchantAddressId
                                                                                                };

        public static EstateReporting.Database.Entities.MerchantOperator MerchantOperatorEntity = new MerchantOperator
                                                                                                  {
                                                                                                      EstateId = TestData.EstateId,
                                                                                                      MerchantId = TestData.MerchantId,
                                                                                                      Name = TestData.OperatorName,
                                                                                                      OperatorId = TestData.OperatorId,
                                                                                                      TerminalNumber = TestData.OperatorTerminalNumber,
                                                                                                      MerchantNumber = TestData.OperatorMerchantNumber
                                                                                                  };

        public static EstateReporting.Database.Entities.MerchantDevice MerchantDeviceEntity = new MerchantDevice
                                                                                              {
                                                                                                  MerchantId = TestData.MerchantId,
                                                                                                  EstateId = TestData.EstateId,
                                                                                                  DeviceId = TestData.DeviceId,
                                                                                                  DeviceIdentifier = TestData.DeviceIdentifier,
                                                                                                  CreatedDateTime = TestData.DateMerchantCreated
                                                                                              };

        public static EstateReporting.Database.Entities.MerchantSecurityUser MerchantSecurityUserEntity = new MerchantSecurityUser
                                                                                                          {
                                                                                                              MerchantId = TestData.MerchantId,
                                                                                                              EstateId = TestData.EstateId,
                                                                                                              SecurityUserId = TestData.SecurityUserId,
                                                                                                              EmailAddress = TestData.MerchantUserEmailAddress,
                                                                                                              CreatedDateTime = TestData.DateMerchantCreated
                                                                                                          };

        public static Guid DepositId = Guid.Parse("A15460B1-9665-4C3E-861D-3B65D0EBEF19");

        public static String DepositReference = "Test Deposit 1";
        public static String DepositReference2 = "Test Deposit 2";

        public static DateTime DepositDateTime = new DateTime(2019, 11, 16);
        public static DateTime DepositDateTime2 = new DateTime(2019, 11, 16);

        public static Decimal DepositAmount = 1000.00m;
        public static Decimal DepositAmount2 = 1200.00m;

        public static MerchantDepositSource MerchantDepositSourceManual = MerchantDepositSource.Manual;

        public static MerchantDepositSource MerchantDepositSourceAutomatic = MerchantDepositSource.Automatic;

        public static MakeMerchantDepositRequest MakeMerchantDepositRequest = MakeMerchantDepositRequest.Create(TestData.EstateId,
                                                                                                                TestData.MerchantId,
                                                                                                                TestData.MerchantDepositSourceManual,
                                                                                                                TestData.DepositReference,
                                                                                                                TestData.DepositDateTime,
                                                                                                                TestData.DepositAmount);

        public static MerchantBalance MerchantBalanceModel = new MerchantBalance
                                                             {
                                                                 AvailableBalance = TestData.AvailableBalance,
                                                                 MerchantId = TestData.MerchantId,
                                                                 Balance = TestData.Balance,
                                                                 EstateId = TestData.EstateId
                                                             };

        public static Guid ContractId = Guid.Parse("3C50EDAB-0718-4666-8BEB-1BD5BF08E1D7");

        public static Guid ContractId2 = Guid.Parse("086C2FC0-DB29-4A75-8983-3A3A78628A2A");

        public static String ContractDescription = "Test Contract";

        public static String ContractDescription2 = "Test Contract 2";

        public static String ProductName = "Product 1";

        public static String ProductName2 = "Product 2";

        public static Guid ProductId = Guid.Parse("C6309D4C-3182-4D96-AEEA-E9DBBB9DED8F");

        public static Guid ProductId2 = Guid.Parse("642522E4-05F1-4218-9739-18211930F489");

        public static Guid ProductId3 = Guid.Parse("08657CB3-3737-4113-8BB1-C8118B3EEA06");

        public static CreateContractRequest CreateContractRequest = CreateContractRequest.Create(TestData.ContractId, TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

        public static String ProductDisplayText = "100 KES";

        public static Decimal ProductFixedValue = 100.00m;

        public static Guid TransactionFeeId = Guid.Parse("B83FCCCE-0D45-4FC2-8952-ED277A124BDB");

        public static Guid TransactionFeeId1 = Guid.Parse("2680A005-797C-4501-B1BB-2ACE124B352A");

        public static String TransactionFeeDescription = "Commission for Merchant";

        public static Decimal TransactionFeeValue = 0.5m;

        public static AddProductToContractRequest AddProductToContractRequest = AddProductToContractRequest.Create(TestData.ContractId,TestData.EstateId,TestData.ProductId,
                                                                                                                   TestData.ProductName,
                                                                                                                   TestData.ProductDisplayText,
                                                                                                                   TestData.ProductFixedValue);

        public static AddTransactionFeeForProductToContractRequest AddTransactionFeeForProductToContractRequest =
            AddTransactionFeeForProductToContractRequest.Create(TestData.ContractId,
                                                                TestData.EstateId,
                                                                TestData.ProductId,
                                                                TestData.TransactionFeeId,
                                                                TestData.TransactionFeeDescription,
                                                                CalculationType.Fixed,
                                                                FeeType.Merchant,
                                                                TestData.TransactionFeeValue);

        public static ContractAggregate CreatedContractAggregateWithAProduct()
        {
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            contractAggregate.AddFixedValueProduct(TestData.ProductId,TestData.ProductName,TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue);

            return contractAggregate;
        }

        public static ContractAggregate CreatedContractAggregateWithAProductAndTransactionFee(CalculationType calculationType, FeeType feeType)
        {
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            contractAggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue);

            Product product = contractAggregate.GetProducts().Single(p => p.ProductId == TestData.ProductId);
            contractAggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription,
                                                calculationType, feeType, TestData.TransactionFeeValue);

            return contractAggregate;
        }

        public static Contract ContractModel = new Contract
        {
            EstateId = TestData.EstateId,
            OperatorId = TestData.OperatorId,
            ContractId = TestData.ContractId,
            Description = TestData.ContractDescription,
            IsCreated = true,
            Products = null
        };
        
        public static Contract ContractModelWithProducts = new Contract
                                               {
                                                   EstateId = TestData.EstateId,
                                                   OperatorId = TestData.OperatorId,
                                                   ContractId = TestData.ContractId,
                                                   Description = TestData.ContractDescription,
                                                   IsCreated = true,
                                                   Products = new List<Product>
                                                              {
                                                                  new Product
                                                                  {
                                                                      Value = TestData.ProductFixedValue,
                                                                      ProductId = TestData.ProductId,
                                                                      DisplayText = TestData.ProductDisplayText,
                                                                      Name = TestData.ProductName,
                                                                      TransactionFees = null
                                                                  }
                                                              }
                                               };

        public static Contract ContractModelWithProductsAndTransactionFees = new Contract
        {
            EstateId = TestData.EstateId,
            OperatorId = TestData.OperatorId,
            ContractId = TestData.ContractId,
            Description = TestData.ContractDescription,
            IsCreated = true,
            Products = new List<Product>
                                                              {
                                                                  new Product
                                                                  {
                                                                      Value = TestData.ProductFixedValue,
                                                                      ProductId = TestData.ProductId,
                                                                      DisplayText = TestData.ProductDisplayText,
                                                                      Name = TestData.ProductName,
                                                                      TransactionFees = new List<TransactionFeeModel>
                                                                                        {
                                                                                            new TransactionFeeModel
                                                                                            {
                                                                                                TransactionFeeId = TestData.TransactionFeeId,
                                                                                                Description = TestData.TransactionFeeDescription,
                                                                                                Value = TestData.TransactionFeeValue,
                                                                                                CalculationType = CalculationType.Fixed
                                                                                            }
                                                                                        }
                                                                  }
                                                              }
        };

        public static EstateReporting.Database.Entities.Contract ContractEntity = new EstateReporting.Database.Entities.Contract
                                                                                  {
                                                                                      EstateId = TestData.EstateId,
                                                                                      OperatorId = TestData.OperatorId,
                                                                                      Description = TestData.ContractDescription,
                                                                                      ContractId = TestData.ContractId
                                                                                  };

        public static EstateReporting.Database.Entities.Contract ContractEntity2 = new EstateReporting.Database.Entities.Contract
                                                                                  {
                                                                                      EstateId = TestData.EstateId,
                                                                                      OperatorId = TestData.OperatorId2,
                                                                                      Description = TestData.ContractDescription,
                                                                                      ContractId = TestData.ContractId2
                                                                                  };

        public static EstateReporting.Database.Entities.ContractProduct ContractProductEntity = new EstateReporting.Database.Entities.ContractProduct
                                                                                                {
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    ContractId = TestData.ContractId,
                                                                                                    Value = TestData.ProductFixedValue,
                                                                                                    ProductId = TestData.ProductId,
                                                                                                    ProductName = TestData.ProductName,
                                                                                                    DisplayText = TestData.ProductDisplayText
                                                                                                };

        public static EstateReporting.Database.Entities.ContractProduct ContractProductEntity2 = new EstateReporting.Database.Entities.ContractProduct
                                                                                                {
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    ContractId = TestData.ContractId2,
                                                                                                    Value = TestData.ProductFixedValue,
                                                                                                    ProductId = TestData.ProductId2,
                                                                                                    ProductName = TestData.ProductName,
                                                                                                    DisplayText = TestData.ProductDisplayText
                                                                                                };

        public static EstateReporting.Database.Entities.ContractProduct ContractProductEntity3 = new EstateReporting.Database.Entities.ContractProduct
                                                                                                 {
                                                                                                     EstateId = TestData.EstateId,
                                                                                                     ContractId = TestData.ContractId2,
                                                                                                     Value = TestData.ProductFixedValue,
                                                                                                     ProductId = TestData.ProductId3,
                                                                                                     ProductName = TestData.ProductName,
                                                                                                     DisplayText = TestData.ProductDisplayText
                                                                                                 };

        public static EstateReporting.Database.Entities.ContractProductTransactionFee ContractProductTransactionFeeEntity = new EstateReporting.Database.Entities.ContractProductTransactionFee
        {
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    ContractId = TestData.ContractId,
                                                                                                    Value = TestData.ProductFixedValue,
                                                                                                    ProductId = TestData.ProductId,
                                                                                                    TransactionFeeId = TestData.TransactionFeeId,
                                                                                                    Description = TestData.TransactionFeeDescription,
                                                                                                    CalculationType = 0
                                                                                                };

        public static List<TransactionFeeModel> ProductTransactionFees = new List<TransactionFeeModel>
                                                                    {
                                                                        new TransactionFeeModel
                                                                        {
                                                                            TransactionFeeId = TestData.TransactionFeeId,
                                                                            Description = TestData.TransactionFeeDescription,
                                                                            Value = TestData.TransactionFeeValue,
                                                                            CalculationType = CalculationType.Fixed
                                                                        }
                                                                    };

        public static MerchantBalanceHistory MerchantBalanceHistoryOut =>
            new MerchantBalanceHistory
            {
                MerchantId = TestData.MerchantId,
                EntryDateTime = TestData.EntryDateTime,
                Balance = TestData.Balance,
                ChangeAmount = TestData.ChangeAmount1,
                EntryType = TestData.EntryType1,
                EstateId = TestData.EstateId,
                EventId = TestData.BalanceEventId1,
                In = null,
                Out = TestData.ChangeAmount1,
                Reference = TestData.BalanceReference1,
                TransactionId = TestData.TransactionId
            };

        public static MerchantBalanceView MerchantBalanceHistoryOutEntity =>
            new MerchantBalanceView
            {
                MerchantId = TestData.MerchantId,
                EntryDateTime = TestData.EntryDateTime,
                Balance = TestData.Balance,
                ChangeAmount = TestData.ChangeAmount1,
                EntryType = TestData.EntryType1,
                EstateId = TestData.EstateId,
                EventId = TestData.BalanceEventId1,
                In = null,
                Out = TestData.ChangeAmount1,
                Reference = TestData.BalanceReference1,
                TransactionId = TestData.TransactionId
            };

        public static MerchantBalanceHistory MerchantBalanceHistoryIn =>
            new MerchantBalanceHistory
            {
                MerchantId = TestData.MerchantId,
                EntryDateTime = TestData.EntryDateTime,
                Balance = TestData.Balance,
                ChangeAmount = TestData.ChangeAmount2,
                EntryType = TestData.EntryType2,
                EstateId = TestData.EstateId,
                EventId = TestData.BalanceEventId2,
                In = null,
                Out = TestData.ChangeAmount2,
                Reference = TestData.BalanceReference2,
                TransactionId = TestData.TransactionId
            };

        public static MerchantBalanceView MerchantBalanceHistoryInEntity =>
            new MerchantBalanceView
            {
                MerchantId = TestData.MerchantId,
                EntryDateTime = TestData.EntryDateTime,
                Balance = TestData.Balance,
                ChangeAmount = TestData.ChangeAmount2,
                EntryType = TestData.EntryType2,
                EstateId = TestData.EstateId,
                EventId = TestData.BalanceEventId2,
                In = null,
                Out = TestData.ChangeAmount2,
                Reference = TestData.BalanceReference2,
                TransactionId = TestData.TransactionId
            };

        public static List<Contract> MerchantContracts = new List<Contract>
                                                         {
                                                             new Contract
                                                             {
                                                                 ContractId = TestData.ContractId,
                                                                 EstateId = TestData.EstateId,
                                                                 Description = TestData.ContractDescription,
                                                                 OperatorId = TestData.OperatorId,
                                                                 OperatorName = TestData.OperatorName,
                                                                 Products = new List<Product>
                                                                            {
                                                                                new Product
                                                                                {
                                                                                    ProductId = TestData.ProductId,
                                                                                    Value = TestData.ProductFixedValue,
                                                                                    DisplayText = TestData.ProductDisplayText,
                                                                                    Name = TestData.ProductName
                                                                                }
                                                                            }
                                                             }
                                                         };

        public static DisableTransactionFeeForProductRequest DisableTransactionFeeForProductRequest = DisableTransactionFeeForProductRequest.Create(TestData.ContractId,
                                                                                                                                                    TestData.EstateId,
                                                                                                                                                    TestData.ProductId,
                                                                                                                                                    TestData.TransactionFeeId);

        private static DateTime EntryDateTime = new DateTime(2021,2,18);

        private static Decimal ChangeAmount1 = -10m;

        private static String EntryType1 ="D";

        private static Guid BalanceEventId1 = Guid.Parse("269331F9-B20E-4E20-9D75-FDE4CE014EF2");

        private static String BalanceReference1 = "Transaction Completed";

        private static Decimal ChangeAmount2 = -0.05m;

        private static String EntryType2 = "C";

        private static Guid BalanceEventId2 = Guid.Parse("BC604870-3451-4BBE-B798-D1B21E3E9996");

        private static String BalanceReference2 = "Transaction Fee";

        private static Guid TransactionId = Guid.Parse("5E3755D1-17DE-4101-9728-75162BAA0A22");

        public static DateTime HistoryStartDate = new DateTime(2021, 8, 29);
        public static DateTime HistoryEndDate = new DateTime(2021,8,30);

        public static SettlementSchedule SettlementSchedule = SettlementSchedule.Immediate;

        public static DateTime NextSettlementDate = new DateTime(2021,8,30);

        public static SetMerchantSettlementScheduleRequest SetMerchantSettlementScheduleRequest => SetMerchantSettlementScheduleRequest.Create(TestData.EstateId,
                                                                                                                                               TestData.MerchantId, TestData.SettlementSchedule);

        public static SwapMerchantDeviceRequest SwapMerchantDeviceRequest =>
            SwapMerchantDeviceRequest.Create(TestData.EstateId, TestData.MerchantId, TestData.DeviceId, TestData.DeviceIdentifier, TestData.NewDeviceIdentifier);

        public static List<MerchantBalanceView> MerchantBalanceHistoryEntities =>
            new List<MerchantBalanceView>
            {
                TestData.MerchantBalanceHistoryInEntity,
                TestData.MerchantBalanceHistoryOutEntity
            };
        public static Guid CallbackId = Guid.Parse("ABC603D3-360E-4F58-8BB9-827EE7A1CB03");

        public static String CallbackReference = "Estate1-Merchant1";

        public static String CallbackMessage = "Message1";

        public static Int32 CallbackMessageFormat = 1;

        public static String CallbackTypeString = "CallbackHandler.DataTransferObjects.Deposit";

        public static CallbackHandler.DataTransferObjects.Deposit Deposit =>
            new CallbackHandler.DataTransferObjects.Deposit
            {
                Reference = TestData.CallbackReference,
                Amount = TestData.DepositAmount,
                DateTime = TestData.DepositDateTime,
                DepositId = TestData.DepositId,
                AccountNumber = TestData.DepositAccountNumber,
                HostIdentifier = TestData.DepositHostIdentifier,
                SortCode = TestData.DepositSortCode
            };

        public static String DepositAccountNumber ="12345678";

        public static Guid DepositHostIdentifier = Guid.Parse("1D1BD9F0-D953-4B2A-9969-98D3C0CDFA2A");

        public static String DepositSortCode = "112233";

        public static Guid MerchantStatementId = Guid.Parse("C8CC622C-07D9-48E9-B544-F53BD29DE1E6");

        public static DateTime StatementCreateDate = new DateTime(2021,12,10);

        public static DateTime StatementGeneratedDate = new DateTime(2021, 12, 11);

        public static CallbackReceivedEnrichedEvent CallbackReceivedEnrichedEvent =>
            new CallbackReceivedEnrichedEvent(TestData.CallbackId)
            {
                Reference = TestData.CallbackReference,
                CallbackMessage = JsonConvert.SerializeObject(Deposit),
                EstateId = TestData.EstateId,
                MessageFormat = TestData.CallbackMessageFormat,
                TypeString = TestData.CallbackTypeString
            };

        public static Transaction Transaction1 => new Transaction
        {
            Amount = TestData.TransactionAmount1,
            DateTime = TestData.TransactionDateTime1,
            OperatorId = TestData.OperatorId,
            TransactionId = TestData.TransactionId1
        };

        public static Transaction Transaction2 => new Transaction
                                                  {
                                                      Amount = TestData.TransactionAmount2,
                                                      DateTime = TestData.TransactionDateTime2,
                                                      OperatorId = TestData.OperatorId,
                                                      TransactionId = TestData.TransactionId2
                                                  };

        private static Decimal TransactionAmount1 = 100.00m;
        private static Decimal TransactionAmount2 = 85.00m;

        private static DateTime TransactionDateTime1 = new DateTime(2021, 12, 10,11,00,00);

        private static DateTime TransactionDateTime2 = new DateTime(2021, 12, 10, 11, 30, 00);

        private static Guid TransactionId1 = Guid.Parse("82E1ACE2-EA34-4501-832D-1DB97B8B4294");
        private static Guid TransactionId2 = Guid.Parse("620A2DD3-75D6-4D19-A239-D1D539B85CE3");

        private static Decimal SettledFeeAmount1 = 1.00m;
        private static Decimal SettledFeeAmount2 = 0.85m;

        private static DateTime SettledFeeDateTime1 = new DateTime(2021, 12, 17, 00, 00, 00);

        private static DateTime SettledFeeDateTime2 = new DateTime(2021, 12, 17, 01, 00, 00);

        private static Guid SettledFeeId1 = Guid.Parse("B4D429AE-756D-4F04-8941-4D41B1A75060");
        private static Guid SettledFeeId2 = Guid.Parse("85C64CF1-6522-408D-93E3-D156B4D5C45B");

        public static SettledFee SettledFee1 =>
            new SettledFee
            {
                Amount = TestData.SettledFeeAmount1,
                DateTime = TestData.SettledFeeDateTime1,
                SettledFeeId = TestData.SettledFeeId1,
                TransactionId = TestData.TransactionId1
            };

        public static SettledFee SettledFee2 =>
            new SettledFee
            {
                Amount = TestData.SettledFeeAmount2,
                DateTime = TestData.SettledFeeDateTime2,
                SettledFeeId = TestData.SettledFeeId2,
                TransactionId = TestData.TransactionId2
            };
    }


}
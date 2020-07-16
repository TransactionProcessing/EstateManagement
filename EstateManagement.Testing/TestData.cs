namespace EstateManagement.Testing
{
    using System;
    using System.Collections.Generic;
    using BusinessLogic.Requests;
    using ContractAggregate;
    using EstateAggregate;
    using EstateReporting.Database.Entities;
    using MerchantAggregate;
    using Models;
    using Models.Contract;
    using Models.Merchant;
    using Address = Models.Merchant.Address;
    using Contact = Models.Merchant.Contact;
    using Contract = Models.Contract.Contract;
    using CreateEstateRequest = BusinessLogic.Requests.CreateEstateRequest;
    using CreateEstateRequestDTO = DataTransferObjects.Requests.CreateEstateRequest;
    using CreateMerchantRequest = BusinessLogic.Requests.CreateMerchantRequest;
    using Estate = Models.Estate;
    using Merchant = Models.Merchant.Merchant;
    using Operator = Models.Estate.Operator;
    using SecurityUser = Models.SecurityUser;

    public class TestData
    {
        #region Fields

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static String EstateName = "Test Estate 1";

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
                                                                                                 TestData.MerchantContactEmailAddress);

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

        public static Merchant MerchantModelWithAddressesContactsDevicesAndOperators = new Merchant
                                                                       {
                                                                           MerchantId = TestData.MerchantId,
                                                                           MerchantName = TestData.MerchantName,
                                                                           EstateId = TestData.EstateId,
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

        public static DateTime DepositDateTime = new DateTime(2019, 11, 16);

        public static Decimal DepositAmount = 1000.00m;

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

        public static String ContractDescription = "Test Contract";

        public static String ProductName = "Product 1";

        public static Guid ProductId = Guid.Parse("C6309D4C-3182-4D96-AEEA-E9DBBB9DED8F");

        public static CreateContractRequest CreateContractRequest = CreateContractRequest.Create(TestData.ContractId, TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

        public static String ProductDisplayText = "100 KES";

        public static Decimal ProductFixedValue = 100.00m;

        public static Guid TransactionFeeId = Guid.Parse("B83FCCCE-0D45-4FC2-8952-ED277A124BDB");

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
                                                                TestData.TransactionFeeValue);

        public static ContractAggregate CreatedContractAggregateWithAProduct()
        {
            ContractAggregate contractAggregate = ContractAggregate.Create(TestData.ContractId);

            contractAggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            contractAggregate.AddFixedValueProduct(TestData.ProductId,TestData.ProductName,TestData.ProductDisplayText,
                                                   TestData.ProductFixedValue);

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
                                                                      TransactionFees = new List<TransactionFee>
                                                                                        {
                                                                                            new TransactionFee
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

        public static EstateReporting.Database.Entities.ContractProduct ContractProductEntity = new EstateReporting.Database.Entities.ContractProduct
                                                                                                {
                                                                                                    EstateId = TestData.EstateId,
                                                                                                    ContractId = TestData.ContractId,
                                                                                                    Value = TestData.ProductFixedValue,
                                                                                                    ProductId = TestData.ProductId,
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


    }
}
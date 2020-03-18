namespace EstateManagement.Testing
{
    using System;
    using System.Collections.Generic;
    using BusinessLogic.Requests;
    using EstateAggregate;
    using EstateReporting.Database.Entities;
    using MerchantAggregate;
    using Address = Models.Merchant.Address;
    using Contact = Models.Merchant.Contact;
    using CreateEstateRequest = BusinessLogic.Requests.CreateEstateRequest;
    using CreateEstateRequestDTO = DataTransferObjects.Requests.CreateEstateRequest;
    using CreateMerchantRequest = BusinessLogic.Requests.CreateMerchantRequest;
    using Estate = Models.Estate;
    using Merchant = Models.Merchant.Merchant;
    using Operator = Models.Operator;
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

        public static Guid MerchantId = Guid.Parse("AC476195-F993-4712-8EA1-CB41C0B44328");

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

        public static Estate EstateModel = new Estate
                                           {
                                               EstateId = TestData.EstateId,
                                               Name = TestData.EstateName,
                                               Operators = null,
                                               SecurityUsers = null
        };
        
        public static Estate EstateModelWithOperators = new Estate
                                           {
                                               EstateId = TestData.EstateId,
                                               Name = TestData.EstateName,
                                               Operators = new List<Operator>
                                                           {
                                                               new Operator
                                                               {
                                                                   RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumberTrue,
                                                                   Name = TestData.OperatorName,
                                                                   OperatorId = TestData.OperatorId,
                                                                   RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumberTrue
                                                               }
                                                           },
                                               SecurityUsers = null
        };

        public static Estate EstateModelWithSecurityUsers = new Estate
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

        public static Estate EstateModelWithOperatorsAndSecurityUsers = new Estate
                                                        {
                                                            EstateId = TestData.EstateId,
                                                            Name = TestData.EstateName,
                                                            Operators = new List<Operator>
                                                                        {
                                                                            new Operator
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

        public static Merchant MerchantModelWithAddressesAndContacts = new Merchant
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
                                                                                      }
                                                                       };

        public static Merchant MerchantModelWithNullAddressesAndWithContacts = new Merchant
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
                                                                                      }
        };

        public static Merchant MerchantModelWithAddressesAndNullContacts = new Merchant
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
                                                                               Contacts = null
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
    }
}
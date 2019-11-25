namespace EstateManagement.Testing
{
    using System;
    using System.Collections.Generic;
    using BusinessLogic.Commands;
    using DataTransferObjects.Requests;
    using EstateAggregate;
    using MerchantAggregate;
    using Models;
    using Models.Merchant;
    using Address = Models.Merchant.Address;
    using Contact = Models.Merchant.Contact;

    public class TestData
    {
        #region Fields

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static String EstateName = "Test Estate 1";

        public static CreateEstateCommand CreateEstateCommand = CreateEstateCommand.Create(TestData.EstateId, TestData.EstateName);

        public static CreateEstateRequest CreateEstateRequest = new CreateEstateRequest
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

        public static CreateMerchantCommand CreateMerchantCommand = CreateMerchantCommand.Create(TestData.EstateId,
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
                                               Name = TestData.EstateName
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

        public static AddOperatorToEstateCommand CreateOperatorCommand = AddOperatorToEstateCommand.Create(TestData.EstateId, TestData.OperatorId,
                                                                                                 TestData.OperatorName,TestData.RequireCustomMerchantNumberFalse,
                                                                                                 TestData.RequireCustomTerminalNumberFalse);
    }
}
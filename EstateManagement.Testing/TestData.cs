namespace EstateManagement.Testing
{
    using System;
    using BusinessLogic.Commands;
    using DataTransferObjects.Requests;
    using EstateAggregate;
    using Models;

    public class TestData
    {
        #region Fields

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static String EstateName = "Test Estate 1";

        public static CreateEstateCommand CreateEstateCommand = CreateEstateCommand.Create(TestData.EstateId, TestData.EstateName);

        public static CreateEstateRequest CreateEstateRequest = new CreateEstateRequest
                                                                {
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
    }
}
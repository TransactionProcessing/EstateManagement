using System;

namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;

    [ExcludeFromCodeCoverage]

    internal static class ExampleData
    {
        internal static String BalanceHistoryReference = "Example Reference";
        internal static Decimal BalanceHistoryChangeAmount = 100.00m;
        internal static DateTime BalanceHistoryEntryDateTime = new DateTime(2021, 4, 13, 17, 5, 0);
        internal static Guid EventId = Guid.Parse("4A696C27-6517-4EB1-B3BF-20B3AF632159");
        internal static Guid BalanceHistoryTransactionId = Guid.Parse("29A89152-EF68-4709-9457-00580AA06680");
        internal static String BalanceHistoryEntryTypeDebit = "D";
        internal static String BalanceHistoryEntryTypeCredit = "C";
        internal static Decimal AvailableBalance = 100.00m;
        internal static Decimal Balance = 100.00m;
        internal static Guid DepositId = Guid.Parse("C732C32C-D17C-45D7-AE82-4DE86737FEED");
        internal static Guid UserId = Guid.Parse("9C8747E9-DA91-4673-B6ED-02D552E81190");
        internal static Guid ContactId = Guid.Parse("E5B48AA8-6628-4C43-8355-8E3B935271E0");
        internal static Guid TransactionFeeId = Guid.Parse("F5C4B17F-6372-4C0C-8392-13C22CAAF7F0");
        internal static Guid AddressId = Guid.Parse("BF5D01F9-C671-49D8-8D29-E18292A92A48");
        internal static Guid ProductId = Guid.Parse("E3BE87DD-2A9B-4BD5-982C-EB863FE3D656");
        internal static Guid ContractId = Guid.Parse("7988738C-18CA-45B8-BE18-FFEA98F8F5BC");
        internal static Guid MerchantId = Guid.Parse("117999D7-84F8-44E3-94DD-17CED21DAD36");

        internal static Guid DeviceId = Guid.Parse("206218AE-D2CE-49E2-81C4-6E8AF7E533B8");
        internal static MerchantDepositSource MerchantDepositSourceManual = MerchantDepositSource.Manual;
        internal static MerchantDepositSource MerchantDepositSourceAutomatic = MerchantDepositSource.Automatic;
        internal static String DepositReference = "Example Deposit Reference";
        internal static Decimal DepositAmount = 1000.00m;

        internal static DateTime DepositDateTime = new DateTime(2021, 4, 13, 17, 5, 0);
        internal static String OperatorName = "Example Operator";

        internal static String EstateUserPassword = "Password";
        internal static String EstateUserEmailAddress = "exampleuser@exampleestate.com";
        internal static String MerchantUserEmailAddress = "exampleuser@examplemerchant.com";

        internal static String EstateUserFamilyName = "ExampleFamily";

        internal static String EstateUserGivenName = "ExampleGiven";

        internal static String EstateUserMiddleName = "ExampleMiddle";

        internal static Guid EstateId = Guid.Parse("335FAD4F-8E07-4CF3-84D2-4D63BC8CE33D");

        internal static String EstateName = "Example Estate";

        internal static String ContractDescription = "Example Contract Description";

        internal static String ContactName = "Example Contact";

        internal static String ContactEmailAddress = "exampleaddress@examplecontact.co.uk";

        internal static String ContactPhoneNumber = "0123456789";
        internal static Guid OperatorId = Guid.Parse("9E1940B6-A857-4293-8A06-31E027E9AB02");

        internal static String OperatorMerchantNumber = "123456789";

        internal static String OperatorTerminalNumber = "00000001";
        internal static String ServiceProviderFixedFeeDescription = "Example Service Provider Fixed Fee";

        internal static String ServiceProviderPercentageFeeDescription = "Example Service Provider Percentage Fee";

        internal static String MerchantFixedFeeDescription = "Example Merchant Fixed Fee";

        internal static String MerchantPercentageFeeDescription = "Example Merchant Percentage Fee";

        internal static String AddressLine1 = "Example Address Line 1";

        internal static String AddressLine2 = "Example Address Line 2";

        internal static String AddressLine3 = "Example Address Line 3";

        internal static String AddressLine4 = "Example Address Line 4";

        internal static String Country = "United Kingdom";

        internal static String PostalCode = "TE57 1NG";

        internal static String Region = "Example Region";

        internal static String Town = "Example Town";

        internal static String DeviceIdentifier = "exampledevice1";

        internal static String ProductDisplayText = "Example1";

        internal static String ProductName = "Example Product";

        internal static Decimal? ProductNullValue = null;

        internal static Decimal? ProductValue = 10.00m;

        internal static String MerchantName = "Example Merchant";

        internal static DateTime MerchantStatementDateTime = new DateTime(2021, 4, 13, 17, 5, 0);

        internal static Guid MerchantStatementId = Guid.Parse("5B56D47F-11BA-4B81-98C6-EB376FAA4B3E");
    }
}

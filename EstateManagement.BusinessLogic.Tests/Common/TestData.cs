namespace EstateManagement.BusinessLogic.Tests.Common
{
    using System;
    using Commands;

    public class TestData
    {
        public static String EstateName = "Test Estate 1";

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static CreateEstateCommand CreateEstateCommand = CreateEstateCommand.Create(TestData.EstateId, TestData.EstateName);
    }
}

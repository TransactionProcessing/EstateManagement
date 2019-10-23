namespace EstateManagement.Tests.Common
{
    using System;
    using DataTransferObjects.Requests;

    public class TestData
    {
        public static String EstateName = "Test Estate 1";

        public static CreateEstateRequest CreateEstateRequest = new CreateEstateRequest
                                                                {
                                                                    EstateName = TestData.EstateName
                                                                };

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");
    }
}

using EstateManagement.BusinessLogic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.Testing
{
    using DataTransferObjects.Requests;
    using EstateAggregate;
    using Models;

    public class TestData
    {
        public static String EstateName = "Test Estate 1";

        public static Guid EstateId = Guid.Parse("488AAFDE-D1DF-4CE0-A0F7-819E42C4885C");

        public static CreateEstateCommand CreateEstateCommand = CreateEstateCommand.Create(TestData.EstateId, TestData.EstateName);

        public static EstateAggregate EmptyEstateAggregate  = EstateAggregate.Create(TestData.EstateId);

        public static EstateAggregate CreatedEstateAggregate()
        {
            EstateAggregate estateAggregate = EstateAggregate.Create(TestData.EstateId);

            estateAggregate.Register(TestData.EstateName);

            return estateAggregate;
        }

        public static CreateEstateRequest CreateEstateRequest = new CreateEstateRequest
                                                                {
                                                                    EstateName = TestData.EstateName
                                                                };

        public static Estate EstateModel = new Estate
                                           {
                                               EstateId = TestData.EstateId,
                                               Name = TestData.EstateName
                                           };
    }
}

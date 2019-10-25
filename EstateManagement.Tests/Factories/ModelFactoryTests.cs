using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.Tests.Factories
{
    using DataTransferObjects.Responses;
    using EstateAggregate;
    using EstateManagement.Factories;
    using Models;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_EstateAggregate_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_NullInput_IsConverted()
        {
            Estate estateModel = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldBeNull();
        }
    }
}

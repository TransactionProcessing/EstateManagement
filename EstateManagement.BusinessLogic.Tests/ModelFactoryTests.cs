using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests
{
    using EstateAggregate;
    using Models;
    using Models.Factories;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_EstateAggregate_IsConverted()
        {
            EstateAggregate estateAggregate = TestData.CreatedEstateAggregate();

            ModelFactory modelFactory = new ModelFactory();

            Estate model = modelFactory.ConvertFrom(estateAggregate);

            model.ShouldNotBeNull();
            model.EstateId.ShouldBe(estateAggregate.AggregateId);
            model.Name.ShouldBe(estateAggregate.EstateName);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_NullInput_IsConverted()
        {
            EstateAggregate estateAggregate = null;

            ModelFactory modelFactory = new ModelFactory();

            Estate model = modelFactory.ConvertFrom(estateAggregate);

            model.ShouldBeNull();
        }
    }
}

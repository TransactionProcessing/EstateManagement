namespace EstateManagement.EstateAggregate.Tests
{
    using System;
    using System.Linq;
    using EstateManagement.Testing;
    using Models;
    using Shouldly;
    using Xunit;

    public class EstateAggregateTests
    {
        [Fact]
        public void EstateAggregate_CanBeCreated_IsCreated()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
        }

        [Fact]
        public void EstateAggregate_Create_IsCreated()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
            aggregate.EstateName.ShouldBe(TestData.EstateName);
            aggregate.IsCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EstateAggregate_Create_InvalidEstateName_ErrorThrown(String estateName)
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Create(estateName);
                                                });
            
            exception.Message.ShouldContain("Estate name must be provided when registering a new estate");
        }

        [Fact]
        public void EstateAggregate_Create_EstateAlreadyCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.Create(TestData.EstateName);
                                                                                  });

            exception.Message.ShouldContain($"Estate with name {TestData.EstateName} has already been created");
        }

        [Fact]
        public void EstateAggregate_GetEstate_NoOperators_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Operators.ShouldBeNull();
        }

        [Fact]
        public void EstateAggregate_GetEstate_WithAnOperator_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Operators.ShouldHaveSingleItem();
            
            Operator @operator =model.Operators.Single();
            @operator.OperatorId.ShouldBe(TestData.OperatorId);
            @operator.Name.ShouldBe(TestData.OperatorName);
            @operator.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberFalse);
            @operator.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberFalse);
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorIsAdded()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_EstateNotCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain("Estate has not been created");
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorWithIdAlreadyAdded_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain($"Duplicate operator details are not allowed, an operator already exists on this estate with Id [{TestData.OperatorId}]");
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorWithNameAlreadyAdded_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId2, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain($"Duplicate operator details are not allowed, an operator already exists on this estate with Name [{TestData.OperatorName}]");
        }
    }
}

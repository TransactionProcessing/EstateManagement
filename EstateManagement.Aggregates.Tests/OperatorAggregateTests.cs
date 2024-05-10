using EstateManagement.Testing;

namespace EstateManagement.ContractAggregate.Tests;

using Models.Operator;
using OperatorAggregate;
using Shouldly;

public class OperatorAggregateTests{
    [Fact]
    public void OperatorAggregate_Create_OperatorIsCreated(){

    }

    [Fact]
    public void OperatorAggregate_CanBeCreated_IsCreated()
    {
        OperatorAggregate aggregate = OperatorAggregate.Create(TestData.OperatorId);

        aggregate.AggregateId.ShouldBe(TestData.OperatorId);
    }

    [Fact]
    public void OperatorAggregate_Create_IsCreated()
    {
        OperatorAggregate aggregate = OperatorAggregate.Create(TestData.OperatorId);
        aggregate.Create(TestData.EstateId, TestData.OperatorName, TestData.RequireCustomMerchantNumber, TestData.RequireCustomTerminalNumber);
        
        aggregate.AggregateId.ShouldBe(TestData.OperatorId);
        aggregate.Name.ShouldBe(TestData.OperatorName);
        aggregate.IsCreated.ShouldBeTrue();
        aggregate.EstateId.ShouldBe(TestData.EstateId);
        aggregate.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumber);
        aggregate.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumber);
    }

    [Fact]
    public void OperatorAggregate_GetOperator_OperatorIsReturned()
    {
        OperatorAggregate aggregate = OperatorAggregate.Create(TestData.OperatorId);
        aggregate.Create(TestData.EstateId, TestData.OperatorName, TestData.RequireCustomMerchantNumber, TestData.RequireCustomTerminalNumber);
        Operator @operator = aggregate.GetOperator();
        @operator.OperatorId.ShouldBe(TestData.OperatorId);
        @operator.Name.ShouldBe(TestData.OperatorName);
        @operator.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumber);
        @operator.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumber);
    }
}
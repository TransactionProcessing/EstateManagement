using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.ContractAggregate.Tests
{
    using Contract.DomainEvents;
    using Models.Contract;
    using Shouldly;
    using Testing;
    using Xunit;

    public class DomainEventTests
    {
        [Fact]
        public void ContractCreatedEvent_CanBeCreated_IsCreated()
        {
            ContractCreatedEvent contractCreatedEvent = new ContractCreatedEvent(TestData.ContractId, TestData.EstateId,
                                                                                    TestData.OperatorId, TestData.ContractDescription);

            contractCreatedEvent.ShouldNotBeNull();
            contractCreatedEvent.AggregateId.ShouldBe(TestData.ContractId);
            contractCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            contractCreatedEvent.ContractId.ShouldBe(TestData.ContractId);
            contractCreatedEvent.Description.ShouldBe(TestData.ContractDescription);
            contractCreatedEvent.OperatorId.ShouldBe(TestData.OperatorId);
            contractCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
        }

        [Fact]
        public void ProductAddedToContractEvent_CanBeCreated_IsCreated()
        {
            FixedValueProductAddedToContractEvent fixedValueProductAddedToContractEvent = new FixedValueProductAddedToContractEvent(TestData.ContractId, TestData.EstateId,
                                                                                                                             TestData.ProductId, TestData.ProductName,
                                                                                                                             TestData.ProductDisplayText,
                                                                                                                             TestData.ProductFixedValue);

            fixedValueProductAddedToContractEvent.ShouldNotBeNull();
            fixedValueProductAddedToContractEvent.AggregateId.ShouldBe(TestData.ContractId);
            fixedValueProductAddedToContractEvent.EventId.ShouldNotBe(Guid.Empty);
            fixedValueProductAddedToContractEvent.ContractId.ShouldBe(TestData.ContractId);
            fixedValueProductAddedToContractEvent.EstateId.ShouldBe(TestData.EstateId);
            fixedValueProductAddedToContractEvent.ProductId.ShouldBe(TestData.ProductId);
            fixedValueProductAddedToContractEvent.ProductName.ShouldBe(TestData.ProductName);
            fixedValueProductAddedToContractEvent.DisplayText.ShouldBe(TestData.ProductDisplayText);
            fixedValueProductAddedToContractEvent.Value.ShouldBe(TestData.ProductFixedValue);
        }

        [Fact]
        public void VariableValueProductAddedToContractEvent_CanBeCreated_IsCreated()
        {
            VariableValueProductAddedToContractEvent variableValueProductAddedToContractEvent = new VariableValueProductAddedToContractEvent(TestData.ContractId, TestData.EstateId,
                                                                                                                                             TestData.ProductId, TestData.ProductName,
                                                                                                                                             TestData.ProductDisplayText);

            variableValueProductAddedToContractEvent.ShouldNotBeNull();
            variableValueProductAddedToContractEvent.AggregateId.ShouldBe(TestData.ContractId);
            variableValueProductAddedToContractEvent.EventId.ShouldNotBe(Guid.Empty);
            variableValueProductAddedToContractEvent.ContractId.ShouldBe(TestData.ContractId);
            variableValueProductAddedToContractEvent.EstateId.ShouldBe(TestData.EstateId);
            variableValueProductAddedToContractEvent.ProductId.ShouldBe(TestData.ProductId);
            variableValueProductAddedToContractEvent.ProductName.ShouldBe(TestData.ProductName);
            variableValueProductAddedToContractEvent.DisplayText.ShouldBe(TestData.ProductDisplayText);
        }

        [Theory]
        [InlineData(CalculationType.Fixed, FeeType.Merchant)]
        [InlineData(CalculationType.Fixed, FeeType.ServiceProvider)]
        [InlineData(CalculationType.Percentage, FeeType.Merchant)]
        [InlineData(CalculationType.Percentage, FeeType.ServiceProvider)]
        public void TransactionFeeForProductAddedToContractEvent_CanBeCreated_IsCreated(CalculationType calculationType,FeeType feeType)
        {
            TransactionFeeForProductAddedToContractEvent transactionFeeForProductAddedToContractEvent = new TransactionFeeForProductAddedToContractEvent(TestData.ContractId,
                                                                                                                                                            TestData.EstateId,
                                                                                                                                                            TestData.ProductId,
                                                                                                                                                            TestData.TransactionFeeId,
                                                                                                                                                            TestData.TransactionFeeDescription,
                                                                                                                                                            (Int32)calculationType,
                                                                                                                                                            (Int32)feeType,
                                                                                                                                                            TestData.TransactionFeeValue);

            transactionFeeForProductAddedToContractEvent.ShouldNotBeNull();
            transactionFeeForProductAddedToContractEvent.AggregateId.ShouldBe(TestData.ContractId);
            transactionFeeForProductAddedToContractEvent.EventId.ShouldNotBe(Guid.Empty);
            transactionFeeForProductAddedToContractEvent.ContractId.ShouldBe(TestData.ContractId);
            transactionFeeForProductAddedToContractEvent.EstateId.ShouldBe(TestData.EstateId);
            transactionFeeForProductAddedToContractEvent.ProductId.ShouldBe(TestData.ProductId);
            transactionFeeForProductAddedToContractEvent.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
            transactionFeeForProductAddedToContractEvent.Description.ShouldBe(TestData.TransactionFeeDescription);
            transactionFeeForProductAddedToContractEvent.CalculationType.ShouldBe((Int32)calculationType);
            transactionFeeForProductAddedToContractEvent.FeeType.ShouldBe((Int32)feeType);
            transactionFeeForProductAddedToContractEvent.Value.ShouldBe(TestData.TransactionFeeValue);
        }

        [Fact]
        public void TransactionFeeForProductDisabledEvent_CanBeCreated_IsCreated()
        {
            TransactionFeeForProductDisabledEvent transactionFeeForProductDisabledEvent =
                new TransactionFeeForProductDisabledEvent(TestData.ContractId, TestData.EstateId, TestData.ProductId, TestData.TransactionFeeId);

            transactionFeeForProductDisabledEvent.ShouldNotBeNull();
            transactionFeeForProductDisabledEvent.AggregateId.ShouldBe(TestData.ContractId);
            transactionFeeForProductDisabledEvent.EventId.ShouldNotBe(Guid.Empty);
            transactionFeeForProductDisabledEvent.ContractId.ShouldBe(TestData.ContractId);
            transactionFeeForProductDisabledEvent.EstateId.ShouldBe(TestData.EstateId);
            transactionFeeForProductDisabledEvent.ProductId.ShouldBe(TestData.ProductId);
            transactionFeeForProductDisabledEvent.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
        }
    }
}

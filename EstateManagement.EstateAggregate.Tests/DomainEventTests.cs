using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.EstateAggregate.Tests
{
    using Estate.DomainEvents;
    using Shouldly;
    using Testing;
    using Xunit;

    public class DomainEventTests
    {
        [Fact]
        public void EstateCreatedEvent_CanBeCreated_IsCreated()
        {
            EstateCreatedEvent estateCreatedEvent = EstateCreatedEvent.Create(TestData.EstateId, TestData.EstateName);

            estateCreatedEvent.ShouldNotBeNull();
            estateCreatedEvent.AggregateId.ShouldBe(TestData.EstateId);
            estateCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            estateCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            estateCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
            estateCreatedEvent.EstateName.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public void OperatorAddedToEstateEvent_CanBeCreated_IsCreated()
        {
            OperatorAddedToEstateEvent operatorAddedToEstateEvent = OperatorAddedToEstateEvent.Create(TestData.EstateId,
                                                                                                      TestData.OperatorId,
                                                                                                      TestData.OperatorName,
                                                                                                      TestData.RequireCustomMerchantNumberFalse,
                                                                                                      TestData.RequireCustomTerminalNumberFalse);

            operatorAddedToEstateEvent.ShouldNotBeNull();
            operatorAddedToEstateEvent.AggregateId.ShouldBe(TestData.EstateId);
            operatorAddedToEstateEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            operatorAddedToEstateEvent.EventId.ShouldNotBe(Guid.Empty);
            operatorAddedToEstateEvent.EstateId.ShouldBe(TestData.EstateId);
            operatorAddedToEstateEvent.OperatorId.ShouldBe(TestData.OperatorId);
            operatorAddedToEstateEvent.Name.ShouldBe(TestData.OperatorName);
            operatorAddedToEstateEvent.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberFalse);
            operatorAddedToEstateEvent.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberFalse);
        }
    }
}

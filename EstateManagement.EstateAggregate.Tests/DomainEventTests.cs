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
            EstateCreatedEvent estateCreatedEvent = new EstateCreatedEvent(TestData.EstateId, TestData.EstateName);

            estateCreatedEvent.ShouldNotBeNull();
            estateCreatedEvent.AggregateId.ShouldBe(TestData.EstateId);
            estateCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            estateCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
            estateCreatedEvent.EstateName.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public void EstateReferenceAllocatedEvent_CanBeCreated_IsCreated()
        {
            EstateReferenceAllocatedEvent estateReferenceAllocatedEvent = new EstateReferenceAllocatedEvent(TestData.EstateId, TestData.EstateReference);

            estateReferenceAllocatedEvent.ShouldNotBeNull();
            estateReferenceAllocatedEvent.AggregateId.ShouldBe(TestData.EstateId);
            estateReferenceAllocatedEvent.EventId.ShouldNotBe(Guid.Empty);
            estateReferenceAllocatedEvent.EstateId.ShouldBe(TestData.EstateId);
            estateReferenceAllocatedEvent.EstateReference.ShouldBe(TestData.EstateReference);
        }

        [Fact]
        public void OperatorAddedToEstateEvent_CanBeCreated_IsCreated()
        {
            OperatorAddedToEstateEvent operatorAddedToEstateEvent = new OperatorAddedToEstateEvent(TestData.EstateId,
                                                                                                                   TestData.OperatorId,
                                                                                                                   TestData.OperatorName,
                                                                                                                   TestData.RequireCustomMerchantNumberFalse,
                                                                                                                   TestData.RequireCustomTerminalNumberFalse);

            operatorAddedToEstateEvent.ShouldNotBeNull();
            operatorAddedToEstateEvent.AggregateId.ShouldBe(TestData.EstateId);
            operatorAddedToEstateEvent.EventId.ShouldNotBe(Guid.Empty);
            operatorAddedToEstateEvent.EstateId.ShouldBe(TestData.EstateId);
            operatorAddedToEstateEvent.OperatorId.ShouldBe(TestData.OperatorId);
            operatorAddedToEstateEvent.Name.ShouldBe(TestData.OperatorName);
            operatorAddedToEstateEvent.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberFalse);
            operatorAddedToEstateEvent.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberFalse);
        }

        [Fact]
        public void SecurityUserAddedEvent_CanBeCreated_IsCreated()
        {
            SecurityUserAddedToEstateEvent securityUserAddedEvent = new SecurityUserAddedToEstateEvent(TestData.EstateId,
                                                                                                      TestData.SecurityUserId,
                                                                                                      TestData.EstateUserEmailAddress);

            securityUserAddedEvent.ShouldNotBeNull();
            securityUserAddedEvent.AggregateId.ShouldBe(TestData.EstateId);
            securityUserAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            securityUserAddedEvent.EstateId.ShouldBe(TestData.EstateId);
            securityUserAddedEvent.SecurityUserId.ShouldBe(TestData.SecurityUserId);
            securityUserAddedEvent.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
        }
    }
}

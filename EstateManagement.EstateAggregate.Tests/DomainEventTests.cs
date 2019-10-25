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
    }
}

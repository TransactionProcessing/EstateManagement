namespace EstateManagement.MerchantDepositListAggregate.Tests
{
    using System;
    using Merchant.DomainEvents;
    using Shouldly;
    using Testing;
    using Xunit;

    public class DomainEventTests
    {
        [Fact]
        public void MerchantDepositListCreatedEvent_CanBeCreated_IsCreated()
        {
            MerchantDepositListCreatedEvent merchantCreatedEvent =
                new MerchantDepositListCreatedEvent(TestData.MerchantId, TestData.EstateId, TestData.DateMerchantCreated);

            merchantCreatedEvent.ShouldNotBeNull();
            merchantCreatedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            merchantCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            merchantCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
            merchantCreatedEvent.DateCreated.ShouldBe(TestData.DateMerchantCreated);
            merchantCreatedEvent.MerchantId.ShouldBe(TestData.MerchantId);
        }
        
        [Fact]
        public void ManualDepositMadeEvent_CanBeCreated_IsCreated()
        {
            ManualDepositMadeEvent manualDepositMadeEvent = new ManualDepositMadeEvent(TestData.MerchantId, 
                                                                                          TestData.EstateId,
                                                                                          TestData.DepositId,
                                                                                          TestData.DepositReference,
                                                                                          TestData.DepositDateTime,
                                                                                          TestData.DepositAmount.Value);

            manualDepositMadeEvent.ShouldNotBeNull();
            manualDepositMadeEvent.AggregateId.ShouldBe(TestData.MerchantId);
            manualDepositMadeEvent.EventId.ShouldNotBe(Guid.Empty);
            manualDepositMadeEvent.EstateId.ShouldBe(TestData.EstateId);
            manualDepositMadeEvent.MerchantId.ShouldBe(TestData.MerchantId);
            manualDepositMadeEvent.DepositId.ShouldBe(TestData.DepositId);
            manualDepositMadeEvent.Reference.ShouldBe(TestData.DepositReference);
            manualDepositMadeEvent.DepositDateTime.ShouldBe(TestData.DepositDateTime);
            manualDepositMadeEvent.Amount.ShouldBe(TestData.DepositAmount.Value);
        }

        [Fact]
        public void AutomaticDepositMadeEvent_CanBeCreated_IsCreated()
        {
            AutomaticDepositMadeEvent automaticDepositMadeEvent = new AutomaticDepositMadeEvent(TestData.MerchantId,
                                                                                                TestData.EstateId,
                                                                                                TestData.DepositId,
                                                                                                TestData.DepositReference,
                                                                                                TestData.DepositDateTime,
                                                                                                TestData.DepositAmount.Value);

            automaticDepositMadeEvent.ShouldNotBeNull();
            automaticDepositMadeEvent.AggregateId.ShouldBe(TestData.MerchantId);
            automaticDepositMadeEvent.EventId.ShouldNotBe(Guid.Empty);
            automaticDepositMadeEvent.EstateId.ShouldBe(TestData.EstateId);
            automaticDepositMadeEvent.MerchantId.ShouldBe(TestData.MerchantId);
            automaticDepositMadeEvent.DepositId.ShouldBe(TestData.DepositId);
            automaticDepositMadeEvent.Reference.ShouldBe(TestData.DepositReference);
            automaticDepositMadeEvent.DepositDateTime.ShouldBe(TestData.DepositDateTime);
            automaticDepositMadeEvent.Amount.ShouldBe(TestData.DepositAmount.Value);
        }
    }
}

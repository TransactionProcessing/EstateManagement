using System;
using Xunit;

namespace EstateManagement.MerchantAggregate.Tests
{
    using EstateManagement.Testing;
    using Merchant.DomainEvents;
    using Shouldly;

    public class DomainEventTests
    {
        [Fact]
        public void MerchantCreatedEvent_CanBeCreated_IsCreated()
        {
            MerchantCreatedEvent merchantCreatedEvent =
                MerchantCreatedEvent.Create(TestData.MerchantId, TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            merchantCreatedEvent.ShouldNotBeNull();
            merchantCreatedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            merchantCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            merchantCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            merchantCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
            merchantCreatedEvent.DateCreated.ShouldBe(TestData.DateMerchantCreated);
            merchantCreatedEvent.MerchantName.ShouldBe(TestData.MerchantName);
            merchantCreatedEvent.MerchantId.ShouldBe(TestData.MerchantId);
        }

        [Fact]
        public void AddressAddedEvent_CanBeCreated_IsCreated()
        {
            AddressAddedEvent addressAddedEvent =
                AddressAddedEvent.Create(TestData.MerchantId, TestData.EstateId,TestData.MerchantAddressId,
                                         TestData.MerchantAddressLine1, TestData.MerchantAddressLine2,
                                         TestData.MerchantAddressLine3, TestData.MerchantAddressLine4,
                                         TestData.MerchantTown, TestData.MerchantRegion,
                                         TestData.MerchantPostalCode, TestData.MerchantCountry);

            addressAddedEvent.ShouldNotBeNull();
            addressAddedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            addressAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            addressAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            addressAddedEvent.EstateId.ShouldBe(TestData.EstateId);
            addressAddedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            addressAddedEvent.AddressId.ShouldBe(TestData.MerchantAddressId);
            addressAddedEvent.AddressLine1.ShouldBe(TestData.MerchantAddressLine1);
            addressAddedEvent.AddressLine2.ShouldBe(TestData.MerchantAddressLine2);
            addressAddedEvent.AddressLine3.ShouldBe(TestData.MerchantAddressLine3);
            addressAddedEvent.AddressLine4.ShouldBe(TestData.MerchantAddressLine4);
            addressAddedEvent.Town.ShouldBe(TestData.MerchantTown);
            addressAddedEvent.Region.ShouldBe(TestData.MerchantRegion);
            addressAddedEvent.PostalCode.ShouldBe(TestData.MerchantPostalCode);
            addressAddedEvent.Country.ShouldBe(TestData.MerchantCountry);
        }

        [Fact]
        public void ContactAddedEvent_CanBeCreated_IsCreated()
        {
            ContactAddedEvent contactAddedEvent =
                ContactAddedEvent.Create(TestData.MerchantId, TestData.EstateId, TestData.MerchantContactId,
                                         TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                         TestData.MerchantContactEmailAddress);

            contactAddedEvent.ShouldNotBeNull();
            contactAddedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            contactAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            contactAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            contactAddedEvent.EstateId.ShouldBe(TestData.EstateId);
            contactAddedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            contactAddedEvent.ContactId.ShouldBe(TestData.MerchantContactId);
            contactAddedEvent.ContactName.ShouldBe(TestData.MerchantContactName);
            contactAddedEvent.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
            contactAddedEvent.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
        }
    }
}

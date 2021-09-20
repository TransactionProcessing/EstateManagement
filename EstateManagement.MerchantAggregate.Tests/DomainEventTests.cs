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
                new MerchantCreatedEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            merchantCreatedEvent.ShouldNotBeNull();
            merchantCreatedEvent.AggregateId.ShouldBe(TestData.MerchantId);
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
                new AddressAddedEvent(TestData.MerchantId, TestData.EstateId,TestData.MerchantAddressId,
                                         TestData.MerchantAddressLine1, TestData.MerchantAddressLine2,
                                         TestData.MerchantAddressLine3, TestData.MerchantAddressLine4,
                                         TestData.MerchantTown, TestData.MerchantRegion,
                                         TestData.MerchantPostalCode, TestData.MerchantCountry);

            addressAddedEvent.ShouldNotBeNull();
            addressAddedEvent.AggregateId.ShouldBe(TestData.MerchantId);
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
                new ContactAddedEvent(TestData.MerchantId, TestData.EstateId, TestData.MerchantContactId,
                                         TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                         TestData.MerchantContactEmailAddress);

            contactAddedEvent.ShouldNotBeNull();
            contactAddedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            contactAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            contactAddedEvent.EstateId.ShouldBe(TestData.EstateId);
            contactAddedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            contactAddedEvent.ContactId.ShouldBe(TestData.MerchantContactId);
            contactAddedEvent.ContactName.ShouldBe(TestData.MerchantContactName);
            contactAddedEvent.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
            contactAddedEvent.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
        }

        [Fact]
        public void OperatorAssignedToMerchantEvent_CanBeCreated_IsCreated()
        {
            OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent =
                new OperatorAssignedToMerchantEvent(TestData.MerchantId,
                                                       TestData.EstateId,
                                                       TestData.OperatorId,
                                                       TestData.OperatorName,
                                                       TestData.OperatorMerchantNumber,
                                                       TestData.OperatorTerminalNumber);

            operatorAssignedToMerchantEvent.ShouldNotBeNull();
            operatorAssignedToMerchantEvent.AggregateId.ShouldBe(TestData.MerchantId);
            operatorAssignedToMerchantEvent.EventId.ShouldNotBe(Guid.Empty);
            operatorAssignedToMerchantEvent.EstateId.ShouldBe(TestData.EstateId);
            operatorAssignedToMerchantEvent.MerchantId.ShouldBe(TestData.MerchantId);
            operatorAssignedToMerchantEvent.OperatorId.ShouldBe(TestData.OperatorId);
            operatorAssignedToMerchantEvent.Name.ShouldBe(TestData.OperatorName);
            operatorAssignedToMerchantEvent.MerchantNumber.ShouldBe(TestData.OperatorMerchantNumber);
            operatorAssignedToMerchantEvent.TerminalNumber.ShouldBe(TestData.OperatorTerminalNumber);
        }

        [Fact]
        public void SecurityUserAddedEvent_CanBeCreated_IsCreated()
        {
            SecurityUserAddedToMerchantEvent securityUserAddedEvent = new SecurityUserAddedToMerchantEvent(TestData.MerchantId,
                TestData.EstateId,
                                                                                          TestData.SecurityUserId,
                                                                                          TestData.EstateUserEmailAddress);

            securityUserAddedEvent.ShouldNotBeNull();
            securityUserAddedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            securityUserAddedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            securityUserAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            securityUserAddedEvent.EstateId.ShouldBe(TestData.EstateId);
            securityUserAddedEvent.SecurityUserId.ShouldBe(TestData.SecurityUserId);
            securityUserAddedEvent.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
        }

        [Fact]
        public void DeviceAddedToMerchantEvent_CanBeCreated_IsCreated()
        {
            DeviceAddedToMerchantEvent deviceAddedToMerchantEvent =
                new DeviceAddedToMerchantEvent(TestData.MerchantId, TestData.EstateId, TestData.DeviceId, TestData.DeviceIdentifier);

            deviceAddedToMerchantEvent.ShouldNotBeNull();
            deviceAddedToMerchantEvent.AggregateId.ShouldBe(TestData.MerchantId);
            deviceAddedToMerchantEvent.EventId.ShouldNotBe(Guid.Empty);
            deviceAddedToMerchantEvent.EstateId.ShouldBe(TestData.EstateId);
            deviceAddedToMerchantEvent.MerchantId.ShouldBe(TestData.MerchantId);
            deviceAddedToMerchantEvent.DeviceId.ShouldBe(TestData.DeviceId);
            deviceAddedToMerchantEvent.DeviceIdentifier.ShouldBe(TestData.DeviceIdentifier);
        }

        [Fact]
        public void ManualDepositMadeEvent_CanBeCreated_IsCreated()
        {
            ManualDepositMadeEvent manualDepositMadeEvent = new ManualDepositMadeEvent(TestData.MerchantId, 
                                                                                          TestData.EstateId,
                                                                                          TestData.DepositId,
                                                                                          TestData.DepositReference,
                                                                                          TestData.DepositDateTime,
                                                                                          TestData.DepositAmount);

            manualDepositMadeEvent.ShouldNotBeNull();
            manualDepositMadeEvent.AggregateId.ShouldBe(TestData.MerchantId);
            manualDepositMadeEvent.EventId.ShouldNotBe(Guid.Empty);
            manualDepositMadeEvent.EstateId.ShouldBe(TestData.EstateId);
            manualDepositMadeEvent.MerchantId.ShouldBe(TestData.MerchantId);
            manualDepositMadeEvent.DepositId.ShouldBe(TestData.DepositId);
            manualDepositMadeEvent.Reference.ShouldBe(TestData.DepositReference);
            manualDepositMadeEvent.DepositDateTime.ShouldBe(TestData.DepositDateTime);
            manualDepositMadeEvent.Amount.ShouldBe(TestData.DepositAmount);
        }

        [Fact]
        public void SettlementScheduleChangedEvent_CanBeCreated_IsCreated()
        {
            SettlementScheduleChangedEvent settlementScheduleChangedEvent =
                new SettlementScheduleChangedEvent(TestData.MerchantId, TestData.EstateId, (Int32)TestData.SettlementSchedule, TestData.NextSettlementDate);

            settlementScheduleChangedEvent.ShouldNotBeNull();
            settlementScheduleChangedEvent.AggregateId.ShouldBe(TestData.MerchantId);
            settlementScheduleChangedEvent.EventId.ShouldNotBe(Guid.Empty);
            settlementScheduleChangedEvent.EstateId.ShouldBe(TestData.EstateId);
            settlementScheduleChangedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            settlementScheduleChangedEvent.SettlementSchedule.ShouldBe((Int32)TestData.SettlementSchedule);
            settlementScheduleChangedEvent.NextSettlementDate.ShouldBe(TestData.NextSettlementDate);
        }
    }
}

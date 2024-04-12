using System;
using System.Collections.Generic;

namespace EstateManagement.MerchantAggregate.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ContractAggregate;
    using EstateManagement.Models;
    using Models.Merchant;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantAggregateTests
    {
        [Fact]
        public void MerchantAggregate_CanBeCreated_IsCreated()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            aggregate.AggregateId.ShouldBe(TestData.MerchantId);
            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.ShouldBeNull();
        }

        [Fact]
        public void MerchantAggregate_Create_IsCreated()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.GenerateReference();

            aggregate.AggregateId.ShouldBe(TestData.MerchantId);
            aggregate.EstateId.ShouldBe(TestData.EstateId);
            aggregate.Name.ShouldBe(TestData.MerchantName);
            aggregate.DateCreated.ShouldBe(TestData.DateMerchantCreated);
            aggregate.IsCreated.ShouldBeTrue();
            aggregate.MerchantReference.ShouldBe(TestData.MerchantReference);
        }
        
        [Fact]
        public async Task MerchantAggregate_Create_MerchantAlreadyCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            Should.NotThrow(() => { aggregate.Create(TestData.MerchantId, TestData.MerchantName, TestData.DateMerchantCreated); });
        }

        [Fact]
        public void MerchantAggregate_GenerateReference_CalledTwice_NoErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.GenerateReference();

            Should.NotThrow(() => { aggregate.GenerateReference(); });
        }

        [Fact]
        public void MerchantAggregate_AddAddress_AddressIsAdded()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddAddress(TestData.MerchantAddressId, TestData.MerchantAddressLine1, TestData.MerchantAddressLine2,
                                 TestData.MerchantAddressLine3, TestData.MerchantAddressLine4, TestData.MerchantTown,
                                 TestData.MerchantRegion, TestData.MerchantPostalCode,TestData.MerchantCountry);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Addresses.ShouldHaveSingleItem();
            Address addressModel = merchantModel.Addresses.Single();
            addressModel.AddressId.ShouldBe(TestData.MerchantAddressId);
            addressModel.AddressLine1.ShouldBe(TestData.MerchantAddressLine1);
            addressModel.AddressLine2.ShouldBe(TestData.MerchantAddressLine2);
            addressModel.AddressLine3.ShouldBe(TestData.MerchantAddressLine3);
            addressModel.AddressLine4.ShouldBe(TestData.MerchantAddressLine4);
            addressModel.Town.ShouldBe(TestData.MerchantTown);
            addressModel.Region.ShouldBe(TestData.MerchantRegion);
            addressModel.PostalCode.ShouldBe(TestData.MerchantPostalCode);
            addressModel.Country.ShouldBe(TestData.MerchantCountry);
        }

        [Fact]
        public void MerchantAggregate_AddAddress_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            
            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                          {
                                                                                              aggregate.AddAddress(TestData.MerchantAddressId,
                                                                                                                   TestData.MerchantAddressLine1,
                                                                                                                   TestData.MerchantAddressLine2,
                                                                                                                   TestData.MerchantAddressLine3,
                                                                                                                   TestData.MerchantAddressLine4,
                                                                                                                   TestData.MerchantTown,
                                                                                                                   TestData.MerchantRegion,
                                                                                                                   TestData.MerchantPostalCode,
                                                                                                                   TestData.MerchantCountry);
                                                                                          });

            exception.Message.ShouldContain($"Merchant has not been created");
        }

        [Fact]
        public void MerchantAggregate_AddContact_ContactIsAdded()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddContact(TestData.MerchantContactId, TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                 TestData.MerchantContactEmailAddress);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Contacts.ShouldHaveSingleItem();
            Contact contactModel = merchantModel.Contacts.Single();
            contactModel.ContactId.ShouldBe(TestData.MerchantContactId);
            contactModel.ContactName.ShouldBe(TestData.MerchantContactName);
            contactModel.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
            contactModel.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
        }

        [Fact]
        public void MerchantAggregate_AddContact_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.AddContact(TestData.MerchantContactId, TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                     TestData.MerchantContactEmailAddress);
            });

            exception.Message.ShouldContain($"Merchant has not been created");
        }

        [Fact]
        public void MerchantAggregate_AssignOperator_OperatorIsAssigned()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AssignOperator(TestData.OperatorId,TestData.OperatorName,TestData.OperatorMerchantNumber,TestData.OperatorTerminalNumber);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Operators.ShouldHaveSingleItem();
            Models.Merchant.Operator operatorModel = merchantModel.Operators.Single();
            operatorModel.OperatorId.ShouldBe(TestData.OperatorId);
            operatorModel.Name.ShouldBe(TestData.OperatorName);
            operatorModel.MerchantNumber.ShouldBe(TestData.OperatorMerchantNumber);
            operatorModel.TerminalNumber.ShouldBe(TestData.OperatorTerminalNumber);
        }

        [Fact]
        public void MerchantAggregate_AssignOperator_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_AssignOperator_OperatorAlreadyAssigned_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.AssignOperator(TestData.OperatorId, TestData.OperatorName, TestData.OperatorMerchantNumber, TestData.OperatorTerminalNumber);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_AddSecurityUserToMerchant_SecurityUserIsAdded()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddSecurityUser(TestData.SecurityUserId, TestData.MerchantUserEmailAddress);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.SecurityUsers.ShouldHaveSingleItem();
            SecurityUser securityUser = merchantModel.SecurityUsers.Single();
            securityUser.EmailAddress.ShouldBe(TestData.MerchantUserEmailAddress);
            securityUser.SecurityUserId.ShouldBe(TestData.SecurityUserId);
        }

        [Fact]
        public void MerchantAggregate_AddSecurityUserToMerchant_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                          {
                                                                                              aggregate.AddSecurityUser(TestData.SecurityUserId, TestData.EstateUserEmailAddress);
                                                                                          });

            exception.Message.ShouldContain("Merchant has not been created");
        }

        [Fact]
        public void MerchantAggregate_AddDevice_DeviceAdded()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Devices.ShouldHaveSingleItem();
            merchantModel.Devices.Single().Key.ShouldBe(TestData.DeviceId);
            merchantModel.Devices.Single().Value.ShouldBe(TestData.DeviceIdentifier);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MerchantAggregate_AddDevice_DeviceIdentifierInvalid_ErrorThrown(String deviceIdentifier)
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddDevice(TestData.DeviceId, deviceIdentifier);
                                                });
        }

        [Fact]
        public void MerchantAggregate_AddDevice_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_AddDevice_MerchantNoSpaceForDevice_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);
                                                    });
        }

        [Fact(Skip="Not valid until can request additional device")]
        public void MerchantAggregate_AddDevice_DuplicateDevice_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);
                                                    });
        }

        

        [Fact]
        public void MerchantAggregate_SetSetttlmentSchedule_ScheduleIsSet()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.SetSettlementSchedule(SettlementSchedule.Immediate);
            aggregate.SettlementSchedule.ShouldBe(SettlementSchedule.Immediate);
            aggregate.NextSettlementDueDate.ShouldBe(DateTime.MinValue);

            aggregate.SetSettlementSchedule(SettlementSchedule.Weekly);
            aggregate.SettlementSchedule.ShouldBe(SettlementSchedule.Weekly);
            aggregate.NextSettlementDueDate.ShouldBe(DateTime.Now.Date.AddDays(7));

            aggregate.SetSettlementSchedule(SettlementSchedule.Immediate);
            aggregate.SettlementSchedule.ShouldBe(SettlementSchedule.Immediate);
            aggregate.NextSettlementDueDate.ShouldBe(DateTime.MinValue);

            aggregate.SetSettlementSchedule(SettlementSchedule.Monthly);
            aggregate.SettlementSchedule.ShouldBe(SettlementSchedule.Monthly);
            aggregate.NextSettlementDueDate.ShouldBe(DateTime.Now.Date.AddMonths(1));
        }

        [Theory]
        [InlineData(SettlementSchedule.Immediate, SettlementSchedule.Immediate)]
        [InlineData(SettlementSchedule.Weekly, SettlementSchedule.Weekly)]
        [InlineData(SettlementSchedule.Monthly, SettlementSchedule.Monthly)]
        public void MerchantAggregate_SetSetttlmentSchedule_SameValue_NoEventRaised(SettlementSchedule originalSettlementSchedule, SettlementSchedule newSettlementSchedule)
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.SetSettlementSchedule(originalSettlementSchedule);
            aggregate.SetSettlementSchedule(newSettlementSchedule);
            
            Type type = aggregate.GetType();
            PropertyInfo property = type.GetProperty("PendingEvents", BindingFlags.Instance | BindingFlags.NonPublic);
            Object value = property.GetValue(aggregate);
            value.ShouldNotBeNull();
            List<IDomainEvent> eventHistory = (List<IDomainEvent>)value;
            eventHistory.Count.ShouldBe(2);

            Merchant merchant = aggregate.GetMerchant();
            merchant.SettlementSchedule.ShouldBe(originalSettlementSchedule);
        }

        [Fact]
        public void MerchantAggregate_SwapDevice_DeviceIsSwapped()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddDevice(TestData.DeviceId, TestData.DeviceIdentifier);

            aggregate.SwapDevice(TestData.DeviceId,TestData.DeviceIdentifier, TestData.NewDeviceIdentifier);

            Merchant merchant = aggregate.GetMerchant();
            merchant.Devices.Count.ShouldBe(1);
            merchant.Devices.ContainsValue(TestData.DeviceIdentifier).ShouldBeFalse();
            merchant.Devices.ContainsValue(TestData.NewDeviceIdentifier).ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MerchantAggregate_SwapDevice_InvalidOriginalDeviceIdentifier_ErrorThrown(String originalDeviceIdentifier)
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.SwapDevice(TestData.DeviceId, originalDeviceIdentifier, TestData.NewDeviceIdentifier);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void MerchantAggregate_SwapDevice_InvalidNewDeviceIdentifier_ErrorThrown(String newDeviceIdentifier)
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.SwapDevice(TestData.DeviceId, TestData.DeviceIdentifier, newDeviceIdentifier);
            });
        }

        [Fact]
        public void MerchantAggregate_SwapDevice_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.SwapDevice(TestData.DeviceId, TestData.DeviceIdentifier, TestData.NewDeviceIdentifier);
            });
        }

        [Fact]
        public void MerchantAggregate_SwapDevice_MerchantDoesNotHaveOriginalDevice_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.SwapDevice(TestData.DeviceId, TestData.DeviceIdentifier, TestData.NewDeviceIdentifier);
            });
        }

        [Fact]
        public void MerchantAggregate_SwapDevice_MerchantAlreadyHasNewDevice_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.AddDevice(TestData.DeviceId, TestData.NewDeviceIdentifier);
            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.SwapDevice(TestData.DeviceId, TestData.NewDeviceIdentifier, TestData.NewDeviceIdentifier);
            });
        }

        [Fact]
        public void MerchantAggregate_AddContract_ContractAndProductsAddedToMerchant(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);
            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            ContractAggregate contractAggregate = TestData.CreatedContractAggregateWithAProduct();

            merchantAggregate.AddContract(contractAggregate);

            Merchant merchant = merchantAggregate.GetMerchant();
            merchant.Contracts.Count.ShouldBe(1);
            Contract contract = merchant.Contracts.SingleOrDefault();
            contract.ShouldNotBeNull();
            contract.ContractProducts.Count.ShouldBe(contractAggregate.GetProducts().Count);

        }

        [Fact]
        public void MerchantAggregate_AddContract_MerchantNotCreated_ErrorThrown(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);

            ContractAggregate contractAggregate = TestData.CreatedContractAggregateWithAProduct();
            Should.Throw<InvalidOperationException>(() => { merchantAggregate.AddContract(contractAggregate); });
        }

        [Fact]
        public void MerchantAggregate_AddContract_ContractAlreadyAdded_ErrorThrown(){
            MerchantAggregate merchantAggregate = MerchantAggregate.Create(TestData.MerchantId);
            merchantAggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            ContractAggregate contractAggregate = TestData.CreatedContractAggregateWithAProduct();
            merchantAggregate.AddContract(contractAggregate);

            Should.Throw<InvalidOperationException>(() => { merchantAggregate.AddContract(contractAggregate); });
        }
    }
}

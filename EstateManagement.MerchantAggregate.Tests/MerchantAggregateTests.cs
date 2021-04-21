using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.MerchantAggregate.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using EstateManagement.Models;
    using Models.Merchant;
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

            aggregate.AggregateId.ShouldBe(TestData.MerchantId);
            aggregate.EstateId.ShouldBe(TestData.EstateId);
            aggregate.Name.ShouldBe(TestData.MerchantName);
            aggregate.DateCreated.ShouldBe(TestData.DateMerchantCreated);
            aggregate.IsCreated.ShouldBeTrue();
        }
        
        [Fact]
        public async Task MerchantAggregate_Create_MerchantAlreadyCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() => { aggregate.Create(TestData.MerchantId, TestData.MerchantName, TestData.DateMerchantCreated); });

            exception.Message.ShouldContain($"Merchant {TestData.MerchantName} is already created");
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
        public void MerchantAggregate_MakeDeposit_DepositMade()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Deposits.ShouldHaveSingleItem();
            merchantModel.Deposits.Single().Source.ShouldBe(TestData.MerchantDepositSourceManual);
            //merchantModel.Deposits.Single().DepositId.ShouldNotBe(TestData.DepositId);
            merchantModel.Deposits.Single().DepositDateTime.ShouldBe(TestData.DepositDateTime);
            merchantModel.Deposits.Single().Reference.ShouldBe(TestData.DepositReference);
            merchantModel.Deposits.Single().Amount.ShouldBe(TestData.DepositAmount);
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_TwoDeposits_BothDepositsMade()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference2, TestData.DepositDateTime2, TestData.DepositAmount2);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Deposits.Count.ShouldBe(2);
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_TwoDepositsSameDetailsApartFromAmounts_BothDepositsMade()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, "Test Data Gen Deposit", new DateTime(2021,1,1,0,0,0), 650.00m);
            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, "Test Data Gen Deposit", new DateTime(2021, 1, 1, 0, 0, 0), 934.00m);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Deposits.Count.ShouldBe(2);
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_TwoDepositsOneMonthApartSameDetails_BothDepositsMade()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, "Test Data Gen Deposit", new DateTime(2021, 1, 1, 0, 0, 0), 650.00m);
            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, "Test Data Gen Deposit", new DateTime(2021, 2, 1, 0, 0, 0), 650.00m);

            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Deposits.Count.ShouldBe(2);
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_MerchantNotCreated_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_DuplicateDeposit_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
            Merchant merchantModel = aggregate.GetMerchant();
            merchantModel.Deposits.ShouldHaveSingleItem();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.MakeDeposit(TestData.MerchantDepositSourceManual, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_DepositSourceNotSet_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);
            
            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.MakeDeposit(MerchantDepositSource.NotSet, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
                                                    });
        }

        [Fact]
        public void MerchantAggregate_MakeDeposit_AutomaticDepositSource_ErrorThrown()
        {
            MerchantAggregate aggregate = MerchantAggregate.Create(TestData.MerchantId);
            aggregate.Create(TestData.EstateId, TestData.MerchantName, TestData.DateMerchantCreated);

            Should.Throw<NotSupportedException>(() =>
                                                    {
                                                        aggregate.MakeDeposit(TestData.MerchantDepositSourceAutomatic, TestData.DepositReference, TestData.DepositDateTime, TestData.DepositAmount);
                                                    });
        }

    }
}

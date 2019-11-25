using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Commands
{
    using BusinessLogic.Commands;
    using Shouldly;
    using Testing;
    using Xunit;

    public class CommandsTests
    {
        [Fact]
        public void CreateEstateCommand_CanBeCreated_IsCreated()
        {
            CreateEstateCommand createEstateCommand = CreateEstateCommand.Create(TestData.EstateId, TestData.EstateName);

            createEstateCommand.ShouldNotBeNull();
            createEstateCommand.CommandId.ShouldNotBe(Guid.Empty);
            createEstateCommand.EstateId.ShouldBe(TestData.EstateId);
            createEstateCommand.Name.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public void CreateMerchantCommand_CanBeCreated_IsCreated()
        {
            CreateMerchantCommand createMerchantCommand = CreateMerchantCommand.Create(TestData.EstateId,TestData.MerchantId,
                                                                                       TestData.MerchantName, TestData.MerchantAddressLine1,
                                                                                       TestData.MerchantAddressLine2, TestData.MerchantAddressLine3,
                                                                                       TestData.MerchantAddressLine4, TestData.MerchantTown,
                                                                                       TestData.MerchantRegion, TestData.MerchantPostalCode, TestData.MerchantCountry,
                                                                                       TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                                                                       TestData.MerchantContactEmailAddress);

            createMerchantCommand.ShouldNotBeNull();
            createMerchantCommand.CommandId.ShouldNotBe(Guid.Empty);
            createMerchantCommand.EstateId.ShouldBe(TestData.EstateId);
            createMerchantCommand.MerchantId.ShouldBe(TestData.MerchantId);
            createMerchantCommand.Name.ShouldBe(TestData.MerchantName);
            createMerchantCommand.AddressLine1.ShouldBe(TestData.MerchantAddressLine1);
            createMerchantCommand.AddressLine2.ShouldBe(TestData.MerchantAddressLine2);
            createMerchantCommand.AddressLine3.ShouldBe(TestData.MerchantAddressLine3);
            createMerchantCommand.AddressLine4.ShouldBe(TestData.MerchantAddressLine4);
            createMerchantCommand.Town.ShouldBe(TestData.MerchantTown);
            createMerchantCommand.Region.ShouldBe(TestData.MerchantRegion);
            createMerchantCommand.PostalCode.ShouldBe(TestData.MerchantPostalCode);
            createMerchantCommand.Country.ShouldBe(TestData.MerchantCountry);
            createMerchantCommand.ContactName.ShouldBe(TestData.MerchantContactName);
            createMerchantCommand.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
            createMerchantCommand.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
        }

        [Fact]
        public void CreateOperatorCommand_CanBeCreated_IsCreated()
        {
            AddOperatorToEstateCommand createOperatorCommand = AddOperatorToEstateCommand.Create(TestData.EstateId,
                                                                                       TestData.OperatorId,
                                                                                       TestData.OperatorName,
                                                                                       TestData.RequireCustomMerchantNumberTrue,
                                                                                       TestData.RequireCustomTerminalNumberTrue);

            createOperatorCommand.ShouldNotBeNull();
            createOperatorCommand.CommandId.ShouldNotBe(Guid.Empty);
            createOperatorCommand.EstateId.ShouldBe(TestData.EstateId);
            createOperatorCommand.OperatorId.ShouldBe(TestData.OperatorId);
            createOperatorCommand.Name.ShouldBe(TestData.OperatorName);
            createOperatorCommand.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberTrue);
            createOperatorCommand.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberTrue);

        }
    }
}

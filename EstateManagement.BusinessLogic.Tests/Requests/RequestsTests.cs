using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Commands
{
    using Requests;
    using Shouldly;
    using Testing;
    using Xunit;

    public class RequestsTests
    {
        [Fact]
        public void CreateEstateCommand_CanBeCreated_IsCreated()
        {
            CreateEstateRequest createEstateRequest = CreateEstateRequest.Create(TestData.EstateId, TestData.EstateName);

            createEstateRequest.ShouldNotBeNull();
            createEstateRequest.EstateId.ShouldBe(TestData.EstateId);
            createEstateRequest.Name.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public void CreateMerchantRequest_CanBeCreated_IsCreated()
        {
            CreateMerchantRequest createMerchantRequest = CreateMerchantRequest.Create(TestData.EstateId,TestData.MerchantId,
                                                                                       TestData.MerchantName, TestData.MerchantAddressLine1,
                                                                                       TestData.MerchantAddressLine2, TestData.MerchantAddressLine3,
                                                                                       TestData.MerchantAddressLine4, TestData.MerchantTown,
                                                                                       TestData.MerchantRegion, TestData.MerchantPostalCode, TestData.MerchantCountry,
                                                                                       TestData.MerchantContactName, TestData.MerchantContactPhoneNumber,
                                                                                       TestData.MerchantContactEmailAddress);

            createMerchantRequest.ShouldNotBeNull();
            createMerchantRequest.EstateId.ShouldBe(TestData.EstateId);
            createMerchantRequest.MerchantId.ShouldBe(TestData.MerchantId);
            createMerchantRequest.Name.ShouldBe(TestData.MerchantName);
            createMerchantRequest.AddressLine1.ShouldBe(TestData.MerchantAddressLine1);
            createMerchantRequest.AddressLine2.ShouldBe(TestData.MerchantAddressLine2);
            createMerchantRequest.AddressLine3.ShouldBe(TestData.MerchantAddressLine3);
            createMerchantRequest.AddressLine4.ShouldBe(TestData.MerchantAddressLine4);
            createMerchantRequest.Town.ShouldBe(TestData.MerchantTown);
            createMerchantRequest.Region.ShouldBe(TestData.MerchantRegion);
            createMerchantRequest.PostalCode.ShouldBe(TestData.MerchantPostalCode);
            createMerchantRequest.Country.ShouldBe(TestData.MerchantCountry);
            createMerchantRequest.ContactName.ShouldBe(TestData.MerchantContactName);
            createMerchantRequest.ContactPhoneNumber.ShouldBe(TestData.MerchantContactPhoneNumber);
            createMerchantRequest.ContactEmailAddress.ShouldBe(TestData.MerchantContactEmailAddress);
        }

        [Fact]
        public void AddOperatorToEstateRequest_CanBeCreated_IsCreated()
        {
            AddOperatorToEstateRequest addOperatorToEstateRequest = AddOperatorToEstateRequest.Create(TestData.EstateId,
                                                                                       TestData.OperatorId,
                                                                                       TestData.OperatorName,
                                                                                       TestData.RequireCustomMerchantNumberTrue,
                                                                                       TestData.RequireCustomTerminalNumberTrue);

            addOperatorToEstateRequest.ShouldNotBeNull();
            addOperatorToEstateRequest.EstateId.ShouldBe(TestData.EstateId);
            addOperatorToEstateRequest.OperatorId.ShouldBe(TestData.OperatorId);
            addOperatorToEstateRequest.Name.ShouldBe(TestData.OperatorName);
            addOperatorToEstateRequest.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberTrue);
            addOperatorToEstateRequest.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberTrue);

        }

        [Fact]
        public void AssignOperatorToMerchantRequest_CanBeCreated_IsCreated()
        {
            AssignOperatorToMerchantRequest assignOperatorToMerchantRequest = AssignOperatorToMerchantRequest.Create(TestData.EstateId,
                                                                                                                     TestData.MerchantId,
                                                                                                                     TestData.OperatorId,
                                                                                                                     TestData.OperatorMerchantNumber,
                                                                                                                     TestData.OperatorTerminalNumber);

            assignOperatorToMerchantRequest.ShouldNotBeNull();
            assignOperatorToMerchantRequest.EstateId.ShouldBe(TestData.EstateId);
            assignOperatorToMerchantRequest.MerchantId.ShouldBe(TestData.MerchantId);
            assignOperatorToMerchantRequest.OperatorId.ShouldBe(TestData.OperatorId);
            assignOperatorToMerchantRequest.MerchantNumber.ShouldBe(TestData.OperatorMerchantNumber);
            assignOperatorToMerchantRequest.TerminalNumber.ShouldBe(TestData.OperatorTerminalNumber);
        }

        [Fact]
        public void CreateEstateUserRequest_CanBeCreated_IsCreated()
        {
            CreateEstateUserRequest createEstateUserRequest = CreateEstateUserRequest.Create(TestData.EstateId,
                                                                                             TestData.EstateUserEmailAddress,
                                                                                             TestData.EstateUserPassword,
                                                                                             TestData.EstateUserGivenName,
                                                                                             TestData.EstateUserMiddleName,
                                                                                             TestData.EstateUserFamilyName);

            createEstateUserRequest.ShouldNotBeNull();
            createEstateUserRequest.EstateId.ShouldBe(TestData.EstateId);
            createEstateUserRequest.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
            createEstateUserRequest.Password.ShouldBe(TestData.EstateUserPassword);
            createEstateUserRequest.GivenName.ShouldBe(TestData.EstateUserGivenName);
            createEstateUserRequest.MiddleName.ShouldBe(TestData.EstateUserMiddleName);
            createEstateUserRequest.FamilyName.ShouldBe(TestData.EstateUserFamilyName);

        }
    }
}

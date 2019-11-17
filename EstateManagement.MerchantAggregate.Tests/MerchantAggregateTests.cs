using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.MerchantAggregate.Tests
{
    using System.Threading.Tasks;
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
    }
}

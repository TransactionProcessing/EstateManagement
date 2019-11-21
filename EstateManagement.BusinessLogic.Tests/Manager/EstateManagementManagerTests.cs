using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Manager
{
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using Manger;
    using MerchantAggregate;
    using Models;
    using Models.Factories;
    using Models.Merchant;
    using Moq;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateManagementManagerTests
    {
        private readonly Mock<IAggregateRepository<EstateAggregate>> EstateAggregateRepository;
        private readonly Mock<IAggregateRepository<MerchantAggregate>> MerchantAggregateRepository;

        private readonly Mock<IModelFactory> ModelFactory;

        private readonly EstateManagementManager EstateManagementManager;

        public EstateManagementManagerTests()
        {
            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            this.ModelFactory = new Mock<IModelFactory>();

            this.EstateManagementManager = new EstateManagementManager(this.EstateAggregateRepository.Object, this.MerchantAggregateRepository.Object,
                                                                       this.ModelFactory.Object);
        }

        [Fact]
        public async Task EstateManagementManager_GetEstate_EstateIsReturned()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            this.ModelFactory.Setup(m => m.ConvertFrom(It.IsAny<EstateAggregate>())).Returns(TestData.EstateModel);

            Estate estateModel =  await this.EstateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            EstateManagementManager estateManagementManager = new EstateManagementManager(aggregateRepositoryManager.Object, modelFactory.Object);

            var estateModel =  await estateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(TestData.EstateModel.EstateId);
            estateModel.Name.ShouldBe(TestData.EstateModel.Name);
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_MerchantIsReturnedWithEmptyAddressesAndContacts()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldBeEmpty();
            merchantModel.Contacts.ShouldBeEmpty();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_WithAddress_MerchantIsReturnedWithEmptyContacts()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithAddress);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldHaveSingleItem();
            merchantModel.Contacts.ShouldBeEmpty();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_WithContact_MerchantIsReturnedWithEmptyAddresses()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithContact);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldBeEmpty();
            merchantModel.Contacts.ShouldHaveSingleItem();
        }
    }
}

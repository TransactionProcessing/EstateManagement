using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Manager
{
    using System.Threading;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.Generators;
    using EstateAggregate;
    using Manger;
    using MerchantAggregate;
    using Models;
    using Models.Estate;
    using Models.Factories;
    using Models.Merchant;
    using Moq;
    using Repository;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateManagementManagerTests
    {
        private readonly Mock<IAggregateRepositoryManager> AggregateRepositoryManager;
        private readonly Mock<IAggregateRepository<EstateAggregate>> EstateAggregateRepository;
        private readonly Mock<IAggregateRepository<MerchantAggregate>> MerchantAggregateRepository;
        private readonly Mock<IEstateManagementRepository> EstateManagementRepository;
        private readonly Mock<IEventStoreContextManager> EventStoreContextManager;
        private readonly Mock<IModelFactory> ModelFactory;

        private readonly EstateManagementManager EstateManagementManager;

        public EstateManagementManagerTests()
        {
            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            this.EstateManagementRepository = new Mock<IEstateManagementRepository>();
            this.EventStoreContextManager = new Mock<IEventStoreContextManager>();

            this.ModelFactory = new Mock<IModelFactory>();

            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(this.EstateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(this.MerchantAggregateRepository.Object);

            this.EstateManagementManager =
                new EstateManagementManager(aggregateRepositoryManager.Object, this.EstateManagementRepository.Object, this.EventStoreContextManager.Object, this.ModelFactory.Object);
        }

        [Fact]
        public async Task EstateManagementManager_GetEstate_EstateIsReturned()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            this.EstateManagementRepository.Setup(e => e.GetEstate(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateModel);

            Estate estateModel =  await this.EstateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);
            
            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(TestData.EstateModel.EstateId);
            estateModel.Name.ShouldBe(TestData.EstateModel.Name);
        }

        [Fact]
        public async Task EstateManagementManager_GetEstate_InvalidEstateId_ErrorIsThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);
            this.EstateManagementRepository.Setup(e => e.GetEstate(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateModel);

            Should.Throw<NotFoundException>(async () => { await this.EstateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None); });
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_MerchantIsReturnedWithNullAddressesAndContacts()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldBeNull();
            merchantModel.Contacts.ShouldBeNull();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_WithAddress_MerchantIsReturnedWithNullContacts()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithAddress);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldHaveSingleItem();
            merchantModel.Contacts.ShouldBeNull();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_WithContact_MerchantIsReturnedWithNullAddresses()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithContact);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.EstateId.ShouldBe(TestData.EstateId);
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldBeNull();
            merchantModel.Contacts.ShouldHaveSingleItem();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchantBalance_MerchantBalanceIsReturned()
        {
            Mock<IEventStoreContext> eventStoreContext = new Mock<IEventStoreContext>();

            String projectionState = "{\r\n  \"merchants\": {\r\n    \"" + $"{TestData.MerchantId}" + "\": {\r\n      \"MerchantId\": \"b3054488-ccfc-4bfe-9b0c-ad7ac10b16e8\",\r\n      \"MerchantName\": \"Test Merchant 2\",\r\n      \"AvailableBalance\": 1000.00,\r\n      \"Balance\": 1000.00,\r\n      \"LastDepositDateTime\": null,\r\n      \"LastSaleDateTime\": null,\r\n      \"PendingBalanceUpdates\": []\r\n    }\r\n  },\r\n  \"debug\": []\r\n}";

            eventStoreContext.Setup(e => e.GetPartitionStateFromProjection(It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(projectionState);
            this.EventStoreContextManager.Setup(m => m.GetEventStoreContext(It.IsAny<String>())).Returns(eventStoreContext.Object);

            var merchantBalanceModel = await this.EstateManagementManager.GetMerchantBalance(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantBalanceModel.EstateId.ShouldBe(TestData.EstateId);
            merchantBalanceModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantBalanceModel.AvailableBalance.ShouldBe(TestData.AvailableBalance);
            merchantBalanceModel.Balance.ShouldBe(TestData.Balance);
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchants_MerchantListIsReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetMerchants(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Merchant>
                                                                                                                                     {
                                                                                                                                         TestData
                                                                                                                                             .MerchantModelWithAddressesContactsDevicesAndOperators
                                                                                                                                     });

            List<Merchant> merchantList = await this.EstateManagementManager.GetMerchants(TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.ShouldHaveSingleItem();
        }
    }
}

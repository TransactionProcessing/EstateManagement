using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Manager
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.Generators;
    using EstateAggregate;
    using Manger;
    using MerchantAggregate;
    using Models;
    using Models.Contract;
    using Models.Estate;
    using Models.Factories;
    using Models.Merchant;
    using Moq;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateManagementManagerTests
    {
        private readonly Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;
        private readonly Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> MerchantAggregateRepository;
        private readonly Mock<IEstateManagementRepository> EstateManagementRepository;
        private readonly Mock<IEventStoreContext> EventStoreContext;
        private readonly Mock<IModelFactory> ModelFactory;

        private readonly EstateManagementManager EstateManagementManager;

        public EstateManagementManagerTests()
        {
            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();
            this.EstateManagementRepository = new Mock<IEstateManagementRepository>();
            
            this.ModelFactory = new Mock<IModelFactory>();
            
            this.EstateManagementManager =
                new EstateManagementManager(this.EstateAggregateRepository.Object,
                                            this.MerchantAggregateRepository.Object,
                                            this.EstateManagementRepository.Object, this.ModelFactory.Object);
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
        public async Task EstateManagementManager_GetMerchants_MerchantListIsReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetMerchants(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Merchant>
                                                                                                                                     {
                                                                                                                                         TestData
                                                                                                                                             .MerchantModelWithAddressesContactsDevicesAndOperators()
                                                                                                                                     });

            List<Merchant> merchantList = await this.EstateManagementManager.GetMerchants(TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.ShouldHaveSingleItem();
        }

        [Fact]
        public async Task EstateManagementManager_GetContract_ContractIsReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetContract(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Boolean>(), It.IsAny<Boolean>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractModelWithProductsAndTransactionFees);

            Contract contractModel = await this.EstateManagementManager.GetContract(TestData.EstateId, TestData.ContractId, false, false, CancellationToken.None);

            contractModel.ShouldNotBeNull();
            contractModel.EstateId.ShouldBe(TestData.EstateId);
            contractModel.ContractId.ShouldBe(TestData.ContractId);
            contractModel.Description.ShouldBe(TestData.ContractDescription);
            contractModel.OperatorId.ShouldBe(TestData.OperatorId);
            contractModel.Products.ShouldNotBeNull();
            contractModel.Products.First().ProductId.ShouldBe(TestData.ProductId);
            contractModel.Products.First().TransactionFees.ShouldNotBeNull();
            contractModel.Products.First().TransactionFees.First().TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
        }

        [Fact]
        public async Task EstateManagementManager_GetContracts_ContractAreReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetContracts(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Contract>() {TestData.ContractModelWithProductsAndTransactionFees});

            List<Contract> contractModelList = await this.EstateManagementManager.GetContracts(TestData.EstateId, CancellationToken.None);

            contractModelList.ShouldNotBeNull();
            contractModelList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task EstateManagementManager_GetTransactionFeesForProduct_TransactionFeesAreReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetTransactionFeesForProduct(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ProductTransactionFees);

            List<TransactionFee> transactionFees = await this.EstateManagementManager.GetTransactionFeesForProduct(TestData.EstateId, TestData.MerchantId, TestData.ContractId,TestData.ProductId, CancellationToken.None);

            transactionFees.ShouldNotBeNull();
            transactionFees.ShouldHaveSingleItem();
            transactionFees.First().TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchantContracts_MerchantContractsReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetMerchantContracts(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantContracts);

            List<Contract> merchantContracts = await this.EstateManagementManager.GetMerchantContracts(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantContracts.ShouldNotBeNull();
            merchantContracts.ShouldHaveSingleItem();
            merchantContracts.Single().ContractId.ShouldBe(TestData.ContractId);
        }
    }
}

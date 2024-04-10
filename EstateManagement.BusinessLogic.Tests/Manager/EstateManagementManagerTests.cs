using System;
using System.Collections.Generic;

namespace EstateManagement.BusinessLogic.Tests.Manager
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ContractAggregate;
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
    using Shared.Exceptions;
    using Shouldly;
    using Testing;
    using Xunit;
    using Contract = Models.Contract.Contract;

    public class EstateManagementManagerTests
    {
        private readonly Mock<IEstateManagementRepository> EstateManagementRepository;
        private readonly Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;
        private readonly Mock<IAggregateRepository<ContractAggregate, DomainEvent>> ContractAggregateRepository;
        private readonly Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> MerchantAggregateRepository;

        private readonly Mock<IModelFactory> ModelFactory;

        private readonly EstateManagementManager EstateManagementManager;

        public EstateManagementManagerTests()
        {
            this.EstateManagementRepository = new Mock<IEstateManagementRepository>();

            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            this.ContractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate, DomainEvent>>();
            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();

            this.ModelFactory = new Mock<IModelFactory>();

            
            this.EstateManagementManager =
                new EstateManagementManager(this.EstateManagementRepository.Object, 
                                            this.EstateAggregateRepository.Object,
                                            this.ContractAggregateRepository.Object,
                                            this.MerchantAggregateRepository.Object,
                                            this.ModelFactory.Object);
        }

        [Fact]
        public async Task EstateManagementManager_GetEstate_EstateIsReturned(){
            this.EstateAggregateRepository.Setup(a => a.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            this.EstateManagementRepository.Setup(e => e.GetEstate(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateModel);

            Estate estateModel =  await this.EstateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);
            
            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(TestData.EstateModel.EstateId);
            estateModel.Name.ShouldBe(TestData.EstateModel.Name);
        }

        [Fact]
        public async Task EstateManagementManager_GetEstate_InvalidEstateId_ErrorisThrown()
        {
            this.EstateAggregateRepository.Setup(a => a.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);
            this.EstateManagementRepository.Setup(e => e.GetEstate(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateModel);

            Should.Throw<NotFoundException>(async () => {
                                                await this.EstateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);
                                            });
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_MerchantIsReturned(){
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithEverything(SettlementSchedule.Monthly));
            
            Merchant expectedModel = TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts(SettlementSchedule.Monthly);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantReportingId.ShouldBe(expectedModel.MerchantReportingId);
            merchantModel.EstateId.ShouldBe(expectedModel.EstateId);
            merchantModel.EstateReportingId.ShouldBe(expectedModel.EstateReportingId);
            merchantModel.NextStatementDate.ShouldBe(expectedModel.NextStatementDate);
            merchantModel.MerchantId.ShouldBe(expectedModel.MerchantId);
            merchantModel.MerchantName.ShouldBe(expectedModel.MerchantName);
            merchantModel.SettlementSchedule.ShouldBe(expectedModel.SettlementSchedule);

            merchantModel.Addresses.ShouldHaveSingleItem();
            merchantModel.Addresses.Single().AddressId.ShouldBe(expectedModel.Addresses.Single().AddressId);
            merchantModel.Addresses.Single().AddressLine1.ShouldBe(expectedModel.Addresses.Single().AddressLine1);
            merchantModel.Addresses.Single().AddressLine2.ShouldBe(expectedModel.Addresses.Single().AddressLine2);
            merchantModel.Addresses.Single().AddressLine3.ShouldBe(expectedModel.Addresses.Single().AddressLine3);
            merchantModel.Addresses.Single().AddressLine4.ShouldBe(expectedModel.Addresses.Single().AddressLine4);
            merchantModel.Addresses.Single().Country.ShouldBe(expectedModel.Addresses.Single().Country);
            merchantModel.Addresses.Single().PostalCode.ShouldBe(expectedModel.Addresses.Single().PostalCode);
            merchantModel.Addresses.Single().Region.ShouldBe(expectedModel.Addresses.Single().Region);
            merchantModel.Addresses.Single().Town.ShouldBe(expectedModel.Addresses.Single().Town);
            
            merchantModel.Contacts.ShouldHaveSingleItem();
            merchantModel.Contacts.Single().ContactEmailAddress.ShouldBe(expectedModel.Contacts.Single().ContactEmailAddress);
            merchantModel.Contacts.Single().ContactId.ShouldBe(expectedModel.Contacts.Single().ContactId);
            merchantModel.Contacts.Single().ContactName.ShouldBe(expectedModel.Contacts.Single().ContactName);
            merchantModel.Contacts.Single().ContactPhoneNumber.ShouldBe(expectedModel.Contacts.Single().ContactPhoneNumber);

            merchantModel.Devices.ShouldHaveSingleItem();
            merchantModel.Devices.Single().Key.ShouldBe(expectedModel.Devices.Single().Key);
            merchantModel.Devices.Single().Value.ShouldBe(expectedModel.Devices.Single().Value);

            merchantModel.Operators.ShouldHaveSingleItem();
            merchantModel.Operators.Single().MerchantNumber.ShouldBe(expectedModel.Operators.Single().MerchantNumber);
            merchantModel.Operators.Single().Name.ShouldBe(expectedModel.Operators.Single().Name);
            merchantModel.Operators.Single().OperatorId.ShouldBe(expectedModel.Operators.Single().OperatorId);
            merchantModel.Operators.Single().TerminalNumber.ShouldBe(expectedModel.Operators.Single().TerminalNumber);

        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_MerchantIsReturnedWithNullAddressesAndContacts()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithOperator);
            this.EstateManagementRepository.Setup(m => m.GetMerchant(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantModelWithNullAddressesAndContacts);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
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
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldHaveSingleItem();
            merchantModel.Contacts.ShouldBeNull();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_WithContact_MerchantIsReturnedWithNullAddresses()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithContact);

            //this.EstateManagementRepository.Setup(m => m.GetMerchant(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantModelWithNullAddresses);

            Merchant merchantModel = await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(TestData.MerchantId);
            merchantModel.MerchantName.ShouldBe(TestData.MerchantName);
            merchantModel.Addresses.ShouldBeNull();
            merchantModel.Contacts.ShouldHaveSingleItem();
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchant_MerchantNotCreated_ErrorThrown()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyMerchantAggregate);

            Should.Throw<NotFoundException>(async () => {
                                                await this.EstateManagementManager.GetMerchant(TestData.EstateId, TestData.MerchantId, CancellationToken.None);
                                            });
        }

        [Fact]
        public async Task EstateManagementManager_GetMerchants_MerchantListIsReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetMerchants(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Merchant>
                                                                                                                                     {
                                                                                                                                         TestData
                                                                                                                                             .MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts()
                                                                                                                                     });

            List<Merchant> merchantList = await this.EstateManagementManager.GetMerchants(TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.ShouldHaveSingleItem();
        }

        [Fact]
        public async Task EstateManagementManager_GetContract_ContractIsReturned(){
            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregateWithAProductAndTransactionFee(CalculationType.Fixed,FeeType.Merchant));
            
            Contract contractModel = await this.EstateManagementManager.GetContract(TestData.EstateId, TestData.ContractId, CancellationToken.None);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(TestData.ContractId);
            contractModel.Description.ShouldBe(TestData.ContractDescription);
            contractModel.OperatorId.ShouldBe(TestData.OperatorId);
            contractModel.Products.ShouldNotBeNull();
            contractModel.Products.First().ProductId.ShouldBe(TestData.ProductId);
            contractModel.Products.First().TransactionFees.ShouldNotBeNull();
            contractModel.Products.First().TransactionFees.First().TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
        }

        [Fact]
        public async Task EstateManagementManager_GetContract_ContractNotCreated_ErrorIsThrown()
        {
            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            Should.Throw<NotFoundException>(async () => {
                                                        Contract contractModel = await this.EstateManagementManager.GetContract(TestData.EstateId, TestData.ContractId, CancellationToken.None);
                                                    });
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
            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregateWithAProductAndTransactionFee(CalculationType.Fixed, FeeType.Merchant));

            List<TransactionFee> transactionFees = await this.EstateManagementManager.GetTransactionFeesForProduct(TestData.EstateId, TestData.MerchantId, TestData.ContractId,TestData.ProductId, CancellationToken.None);

            transactionFees.ShouldNotBeNull();
            transactionFees.ShouldHaveSingleItem();
            transactionFees.First().TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
        }

        [Fact]
        public async Task EstateManagementManager_GetTransactionFeesForProduct_ContractNotFound_ErrorThrown()
        {
            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            Should.Throw<NotFoundException>(async () => {
                                                await this.EstateManagementManager.GetTransactionFeesForProduct(TestData.EstateId, TestData.MerchantId, TestData.ContractId, TestData.ProductId, CancellationToken.None);
                                            });
        }

        [Fact]
        public async Task EstateManagementManager_GetTransactionFeesForProduct_ProductNotFound_ErrorThrown()
        {
            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregate);

            Should.Throw<NotFoundException>(async () => {
                                                await this.EstateManagementManager.GetTransactionFeesForProduct(TestData.EstateId, TestData.MerchantId, TestData.ContractId, TestData.ProductId, CancellationToken.None);
                                            });
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

        [Fact]
        public async Task EstateManagementManager_GetFileDetails_FileDetailsAreReturned()
        {
            this.EstateManagementRepository.Setup(e => e.GetFileDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.FileModel);

            var fileDetails = await this.EstateManagementManager.GetFileDetails(TestData.EstateId, TestData.FileId, CancellationToken.None);

            fileDetails.ShouldNotBeNull();
        }
    }
}

namespace EstateManagement.Repository.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Database;
    using EstateReporting.Database.Entities;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Models.Contract;
    using Models.Factories;
    using Moq;
    using Shared.EntityFramework;
    using Shared.Exceptions;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using Xunit;
    using Contract = EstateReporting.Database.Entities.Contract;
    using Merchant = Models.Merchant.Merchant;
    using ContractProductTransactionFeeModel = Models.Contract.TransactionFee;
    using MerchantBalanceHistory = Models.Merchant.MerchantBalanceHistory;

    public class EstateManagementRepositoryTests
    {
        public EstateManagementRepositoryTests()
        {
            Logger.Initialise(NullLogger.Instance);
        }

        private Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>> GetMockDbContextFactory()
        {
            return new Mock<Shared.EntityFramework.IDbContextFactory<EstateReportingGenericContext>>();
        }

        [Fact]
        public void EstateManagementRepository_CanBeCreated_IsCreated()
        {
            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            estateManagementRepository.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetEstate_EstateRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Estates.Add(TestData.EstateEntity);
            context.EstateOperators.Add(TestData.EstateOperatorEntity);
            context.EstateSecurityUsers.Add(TestData.EstateSecurityUserEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Estate>(), It.IsAny<List<EstateOperator>>(), It.IsAny<List<EstateSecurityUser>>()))
                        .Returns(new Models.Estate.Estate());
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Models.Estate.Estate estateModel = await estateManagementRepository.GetEstate(TestData.EstateId, CancellationToken.None);

            estateModel.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetEstate_EstateNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Estate>(), It.IsAny<List<EstateOperator>>(), It.IsAny<List<EstateSecurityUser>>()))
                        .Returns(new Models.Estate.Estate());
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Should.Throw<NotFoundException>(async () => { await estateManagementRepository.GetEstate(TestData.EstateId, CancellationToken.None); });
        }


        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetMerchants_MerchantRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Merchants.Add(TestData.MerchantEntity);
            context.MerchantContacts.Add(TestData.MerchantContactEntity);
            context.MerchantAddresses.Add(TestData.MerchantAddressEntity);
            context.MerchantDevices.Add(TestData.MerchantDeviceEntity);
            context.MerchantOperators.Add(TestData.MerchantOperatorEntity);
            context.MerchantSecurityUsers.Add(TestData.MerchantSecurityUserEntity);

            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<EstateReporting.Database.Entities.Merchant>(),
                                                  It.IsAny<List<MerchantAddress>>(),
                                                  It.IsAny<List<MerchantContact>>(),
                                                  It.IsAny<List<MerchantOperator>>(),
                                                  It.IsAny<List<MerchantDevice>>(),
                                                  It.IsAny<List<MerchantSecurityUser>>()));
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            List<Merchant> merchantListModel = await estateManagementRepository.GetMerchants(TestData.EstateId, CancellationToken.None);

            merchantListModel.ShouldNotBeNull();
            merchantListModel.ShouldNotBeEmpty();
            merchantListModel.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetMerchants_NoMerchantsFound_NullMerchantsReturned(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<EstateReporting.Database.Entities.Merchant>(),
                                                  It.IsAny<List<MerchantAddress>>(),
                                                  It.IsAny<List<MerchantContact>>(),
                                                  It.IsAny<List<MerchantOperator>>(),
                                                  It.IsAny<List<MerchantDevice>>(),
                                                  It.IsAny<List<MerchantSecurityUser>>()));
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            List<Merchant> merchantListModel = await estateManagementRepository.GetMerchants(TestData.EstateId, CancellationToken.None);

            merchantListModel.ShouldBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetContract_ContractRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Contracts.Add(TestData.ContractEntity);
            context.ContractProducts.Add(TestData.ContractProductEntity);
            context.ContractProductTransactionFees.Add(TestData.ContractProductTransactionFeeEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Contract>(), It.IsAny<List<ContractProduct>>(), It.IsAny<List<ContractProductTransactionFee>>()))
                        .Returns(TestData.ContractModel);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Models.Contract.Contract contractModel =
                await estateManagementRepository.GetContract(TestData.EstateId, TestData.ContractId, false, false, CancellationToken.None);

            contractModel.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetContract_IncludeProducts_ContractRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Contracts.Add(TestData.ContractEntity);
            context.ContractProducts.Add(TestData.ContractProductEntity);
            context.ContractProductTransactionFees.Add(TestData.ContractProductTransactionFeeEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Contract>(), It.IsAny<List<ContractProduct>>(), It.IsAny<List<ContractProductTransactionFee>>()))
                        .Returns(TestData.ContractModel);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Models.Contract.Contract contractModel =
                await estateManagementRepository.GetContract(TestData.EstateId, TestData.ContractId, true, false, CancellationToken.None);

            contractModel.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetContract_IncludeProductsWithFees_ContractRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Contracts.Add(TestData.ContractEntity);
            context.ContractProducts.Add(TestData.ContractProductEntity);
            context.ContractProductTransactionFees.Add(TestData.ContractProductTransactionFeeEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Contract>(), It.IsAny<List<ContractProduct>>(), It.IsAny<List<ContractProductTransactionFee>>()))
                        .Returns(TestData.ContractModel);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Models.Contract.Contract contractModel =
                await estateManagementRepository.GetContract(TestData.EstateId, TestData.ContractId, false, true, CancellationToken.None);

            contractModel.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetContract_ContractNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Contract>(), It.IsAny<List<ContractProduct>>(), It.IsAny<List<ContractProductTransactionFee>>()))
                        .Returns(TestData.ContractModel);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await estateManagementRepository.GetContract(TestData.EstateId,
                                                                                             TestData.ContractId,
                                                                                             false,
                                                                                             false,
                                                                                             CancellationToken.None);
                                            });
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetTransactionFeesForProduct_TransactionFeesForProductRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.ContractProductTransactionFees.Add(TestData.ContractProductTransactionFeeEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<List<ContractProductTransactionFee>>())).Returns(TestData.ProductTransactionFees);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            List<ContractProductTransactionFeeModel> transactionFeesModel =
                await estateManagementRepository.GetTransactionFeesForProduct(TestData.EstateId,
                                                                              TestData.MerchantId,
                                                                              TestData.ContractId,
                                                                              TestData.ProductId,
                                                                              CancellationToken.None);

            transactionFeesModel.ShouldNotBeNull();
            transactionFeesModel.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetMerchantContracts_MerchantContractsRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            await context.Merchants.AddAsync(TestData.MerchantEntity, CancellationToken.None);
            await context.EstateOperators.AddAsync(TestData.EstateOperatorEntity, CancellationToken.None);
            await context.EstateOperators.AddAsync(TestData.EstateOperatorEntity2, CancellationToken.None);
            await context.Contracts.AddAsync(TestData.ContractEntity, CancellationToken.None);
            await context.Contracts.AddAsync(TestData.ContractEntity2, CancellationToken.None);
            await context.ContractProducts.AddAsync(TestData.ContractProductEntity, CancellationToken.None);
            await context.ContractProducts.AddAsync(TestData.ContractProductEntity2, CancellationToken.None);
            await context.ContractProducts.AddAsync(TestData.ContractProductEntity3, CancellationToken.None);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<List<ContractProductTransactionFee>>())).Returns(TestData.ProductTransactionFees);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            List<Models.Contract.Contract> merchantContractsModelList =
                await estateManagementRepository.GetMerchantContracts(TestData.EstateId, TestData.MerchantId, CancellationToken.None);

            merchantContractsModelList.ShouldNotBeNull();
            merchantContractsModelList.Count.ShouldBe(2);
            merchantContractsModelList[0].Products.ShouldNotBeNull();
            merchantContractsModelList[0].Products.ShouldHaveSingleItem();
            merchantContractsModelList[1].Products.ShouldNotBeNull();
            merchantContractsModelList[1].Products.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        public async Task EstateManagementRepository_GetContracts_ContractsRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingGenericContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Contracts.Add(TestData.ContractEntity);
            context.ContractProducts.Add(TestData.ContractProductEntity);
            context.EstateOperators.Add(TestData.EstateOperatorEntity);
            await context.SaveChangesAsync();

            var dbContextFactory = this.GetMockDbContextFactory();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Contract>(), It.IsAny<List<ContractProduct>>(), It.IsAny<List<ContractProductTransactionFee>>()))
                        .Returns(TestData.ContractModel);
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            List<Models.Contract.Contract> contractModelList = await estateManagementRepository.GetContracts(TestData.EstateId, CancellationToken.None);

            contractModelList.ShouldNotBeNull();
            contractModelList.ShouldNotBeEmpty();
        }

        private static async Task<EstateReportingGenericContext> GetContext(String databaseName,
                                                                            TestDatabaseType databaseType = TestDatabaseType.InMemory)
        {
            EstateReportingGenericContext context = null;
            if (databaseType == TestDatabaseType.InMemory)
            {
                DbContextOptionsBuilder<EstateReportingGenericContext> builder = new DbContextOptionsBuilder<EstateReportingGenericContext>().UseInMemoryDatabase(databaseName)
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                context = new EstateReportingSqlServerContext(builder.Options);
            }
            else
            {
                throw new NotSupportedException($"Database type [{databaseType}] not supported");
            }

            return context;
        }
    }

    public enum TestDatabaseType
    {
        InMemory = 0,

        SqliteInMemory = 1
    }
}

namespace EstateManagement.Repository.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Database;
    using EstateReporting.Database.Entities;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Models.Factories;
    using Moq;
    using Shared.EntityFramework;
    using Shared.Exceptions;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using Xunit;
    using Merchant = Models.Merchant.Merchant;

    public class EstateManagementRepositoryTests
    {
        public EstateManagementRepositoryTests()
        {
            Logger.Initialise(NullLogger.Instance);
        }

        [Fact]
        public void EstateManagementRepository_CanBeCreated_IsCreated()
        {
            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            estateManagementRepository.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        [InlineData(TestDatabaseType.SqliteInMemory)]
        public async Task EstateManagementRepository_GetEstate_EstateRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Estates.Add(TestData.EstateEntity);
            context.EstateOperators.Add(TestData.EstateOperatorEntity);
            context.EstateSecurityUsers.Add(TestData.EstateSecurityUserEntity);
            await context.SaveChangesAsync();

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Estate>(), It.IsAny<List<EstateOperator>>(), It.IsAny<List<EstateSecurityUser>>())).Returns(new Models.Estate.Estate());
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Models.Estate.Estate estateModel = await estateManagementRepository.GetEstate(TestData.EstateId, CancellationToken.None);
            
            estateModel.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        [InlineData(TestDatabaseType.SqliteInMemory)]
        public async Task EstateManagementRepository_GetEstate_EstateNotFound_ErrorThrown(TestDatabaseType testDatabaseType)
        {
            EstateReportingContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();

            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<Estate>(), It.IsAny<List<EstateOperator>>(), It.IsAny<List<EstateSecurityUser>>())).Returns(new Models.Estate.Estate());
            EstateManagementRepository estateManagementRepository = new EstateManagementRepository(dbContextFactory.Object, modelFactory.Object);

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await estateManagementRepository.GetEstate(TestData.EstateId, CancellationToken.None);
                                            });
        }


        [Theory]
        [InlineData(TestDatabaseType.InMemory)]
        [InlineData(TestDatabaseType.SqliteInMemory)]
        public async Task EstateManagementRepository_GetMerchants_MerchantRetrieved(TestDatabaseType testDatabaseType)
        {
            EstateReportingContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            context.Merchants.Add(TestData.MerchantEntity);
            context.MerchantContacts.Add(TestData.MerchantContactEntity);
            context.MerchantAddresses.Add(TestData.MerchantAddressEntity);
            context.MerchantDevices.Add(TestData.MerchantDeviceEntity);
            context.MerchantOperators.Add(TestData.MerchantOperatorEntity);

            await context.SaveChangesAsync();

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
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
        [InlineData(TestDatabaseType.SqliteInMemory)]
        public async Task EstateManagementRepository_GetMerchants_NoMerchantsFound_NullMerchantsReturned(TestDatabaseType testDatabaseType)
        {
            EstateReportingContext context = await EstateManagementRepositoryTests.GetContext(Guid.NewGuid().ToString("N"), testDatabaseType);
            //await context.SaveChangesAsync();

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
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

        private static async Task<EstateReportingContext> GetContext(String databaseName, TestDatabaseType databaseType = TestDatabaseType.InMemory)
        {
            EstateReportingContext context = null;
            if (databaseType == TestDatabaseType.InMemory)
            {
                DbContextOptionsBuilder<EstateReportingContext> builder = new DbContextOptionsBuilder<EstateReportingContext>()
                                                                          .UseInMemoryDatabase(databaseName)
                                                                          .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                context = new EstateReportingContext(builder.Options);
            }
            else if (databaseType == TestDatabaseType.SqliteInMemory)
            {
                SqliteConnection inMemorySqlite = new SqliteConnection("Data Source=:memory:");
                inMemorySqlite.Open();

                DbContextOptionsBuilder<EstateReportingContext> builder = new DbContextOptionsBuilder<EstateReportingContext>().UseSqlite(inMemorySqlite);
                context = new EstateReportingContext(builder.Options);
                await context.Database.MigrateAsync();

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
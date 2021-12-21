using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using BusinessLogic.Services;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Moq;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantStatementDomainServiceTests
    {
        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_TransactionAdded()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.CreatedMerchantStatementAggregate();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
                            {
                                await merchantStatementDomainService.AddTransactionToStatement(TestData.EstateId,
                                                                                               TestData.MerchantId,
                                                                                               TestData.TransactionDateTime1,
                                                                                               TestData.TransactionAmount1,
                                                                                               TestData.IsAuthorisedTrue,
                                                                                               TestData.TransactionId1,
                                                                                               CancellationToken.None);
                            });

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_TransactionNotAuthorised_TransactionNotAddedToStatement()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.CreatedMerchantStatementAggregate();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await merchantStatementDomainService.AddTransactionToStatement(TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               TestData.TransactionDateTime1,
                                                                               TestData.TransactionAmount1,
                                                                               TestData.IsAuthorisedFalse,
                                                                               TestData.TransactionId1,
                                                                               CancellationToken.None);
            });

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldBeEmpty();
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_LogonTransaction_TransactionNotAddedToStatement()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.CreatedMerchantStatementAggregate();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await merchantStatementDomainService.AddTransactionToStatement(TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               TestData.TransactionDateTime1,
                                                                               null,
                                                                               TestData.IsAuthorisedTrue,
                                                                               TestData.TransactionId1,
                                                                               CancellationToken.None);
            });

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldBeEmpty();
        }
        
        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_StatementNotAlreadyCreated_TransactionAdded()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = new MerchantStatementAggregate();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await merchantStatementDomainService.AddTransactionToStatement(TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               TestData.TransactionDateTime1,
                                                                               TestData.TransactionAmount1,
                                                                               TestData.IsAuthorisedTrue,
                                                                               TestData.TransactionId1,
                                                                               CancellationToken.None);
            });

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddSettledFeeToStatement_SettledFeeAdded()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.MerchantStatementAggregateWithTransactionLineAdded();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await merchantStatementDomainService.AddSettledFeeToStatement(TestData.EstateId,
                                                                               TestData.MerchantId,
                                                                               TestData.SettledFeeDateTime1,
                                                                               TestData.SettledFeeAmount1,
                                                                               TestData.TransactionId1,
                                                                               TestData.SettledFeeId1,
                                                                               CancellationToken.None);
            });

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(2);
        }


        [Fact]
        public async Task MerchantStatementDomainService_GenerateStatement_StatementGenerated()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository =
                new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            
            MerchantStatementAggregate merchantStatementAggregate = TestData.MerchantStatementAggregateWithTransactionLineAndSettledFeeAdded();

            Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository =
                new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(merchantStatementAggregate);
            MerchantStatementDomainService merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await merchantStatementDomainService.GenerateStatement(TestData.EstateId,
                                                                              TestData.MerchantId,
                                                                              TestData.StatementCreateDate,
                                                                              CancellationToken.None);
            });

            var merchantStatement = merchantStatementAggregate.GetStatement(false);
            merchantStatement.IsGenerated.ShouldBeTrue();
        }
    }
}

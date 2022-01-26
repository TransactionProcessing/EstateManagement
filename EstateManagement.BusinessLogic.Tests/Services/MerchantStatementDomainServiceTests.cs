using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.IO.Abstractions.TestingHelpers;
    using System.Threading;
    using BusinessLogic.Services;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using MessagingService.Client;
    using MessagingService.DataTransferObjects;
    using Microsoft.Extensions.Configuration;
    using Models.MerchantStatement;
    using Moq;
    using Repository;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantStatementDomainServiceTests
    {
        private Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
        private Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
        private Mock<IEstateManagementRepository> estateManagementRepository = new Mock<IEstateManagementRepository>();
        private Mock<IStatementBuilder> statementBuilder = new Mock<IStatementBuilder>();
        private Mock<IMessagingServiceClient> messagingServiceClient = new Mock<IMessagingServiceClient>();
        private Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

        private MockFileSystem fileSystem = new MockFileSystem();

        private Mock<IPDFGenerator> pdfGenerator = new Mock<IPDFGenerator>();

        private MerchantStatementDomainService merchantStatementDomainService;
        public MerchantStatementDomainServiceTests()
        {
            merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object,
                                                   estateManagementRepository.Object, statementBuilder.Object,
                                                   messagingServiceClient.Object, securityServiceClient.Object,
                                                   fileSystem,
                                                   this.pdfGenerator.Object);
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_TransactionAdded()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            
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
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);

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
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
         
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
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = new MerchantStatementAggregate();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);

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
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.MerchantStatementAggregateWithTransactionLineAdded();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            
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
        public async Task MerchantStatementDomainService_AddSettledFeeToStatement_StatementNotAlreadyCreated_SettledFeeAdded()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = new MerchantStatementAggregate();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(merchantStatementAggregate);
            
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
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public async Task MerchantStatementDomainService_GenerateStatement_StatementGenerated()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.MerchantStatementAggregateWithTransactionLineAndSettledFeeAdded();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(merchantStatementAggregate);
            
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

        [Fact]
        public async Task MerchantStatementDomainService_EmailStatement_StatementGenerated()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            MerchantStatementAggregate merchantStatementAggregate = TestData.GeneratedMerchantStatementAggregate();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(merchantStatementAggregate);
            this.statementBuilder.Setup(s => s.GetStatementHtml(It.IsAny<StatementHeader>(), It.IsAny<CancellationToken>())).ReturnsAsync("<html></html>");

            this.estateManagementRepository.Setup(e => e.GetStatement(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StatementHeader());

            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            this.securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse());

            this.messagingServiceClient.Setup(m => m.SendEmail(It.IsAny<String>(), It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendEmailResponse
                              {
                                  MessageId = Guid.NewGuid()
                              });

            Should.NotThrow(async () =>
                            {
                                await merchantStatementDomainService.EmailStatement(TestData.EstateId,
                                                                                       TestData.MerchantId,
                                                                                       TestData.MerchantStatementId,
                                                                                       CancellationToken.None);
                            });
            
            var merchantStatement = merchantStatementAggregate.GetStatement(false);
            merchantStatement.HasBeenEmailed.ShouldBeTrue();
        }
    }
}

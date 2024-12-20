﻿using System;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Requests;
using SimpleResults;

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
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using Xunit;
    
    public class MerchantStatementDomainServiceTests
    {
        private Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();
        private Mock<IAggregateRepository<MerchantStatementAggregate, DomainEvent>> merchantStatementAggregateRepository = new Mock<IAggregateRepository<MerchantStatementAggregate, DomainEvent>>();
        private Mock<IEstateManagementRepository> estateManagementRepository = new Mock<IEstateManagementRepository>();
        private Mock<IStatementBuilder> statementBuilder = new Mock<IStatementBuilder>();
        private Mock<IMessagingServiceClient> messagingServiceClient = new Mock<IMessagingServiceClient>();
        private Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

        private MockFileSystem fileSystem = new MockFileSystem();

        private Mock<IPDFGenerator> pdfGenerator = new Mock<IPDFGenerator>();

        private MerchantStatementDomainService merchantStatementDomainService;
        public MerchantStatementDomainServiceTests()
        {
            this.merchantStatementAggregateRepository
                .Setup(m => m.SaveChanges(It.IsAny<MerchantStatementAggregate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SimpleResults.Result.Success());

            merchantStatementDomainService =
                new MerchantStatementDomainService(merchantAggregateRepository.Object, merchantStatementAggregateRepository.Object,
                                                   estateManagementRepository.Object, statementBuilder.Object,
                                                   messagingServiceClient.Object, securityServiceClient.Object,
                                                   fileSystem,
                                                   this.pdfGenerator.Object);

        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_TransactionAdded() {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));
            
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(Result.Success(merchantStatementAggregate));
            
            MerchantStatementCommands.AddTransactionToMerchantStatementCommand command = new(TestData.EstateId,
                TestData.MerchantId,
                TestData.TransactionDateTime1,
                TestData.TransactionAmount1,
                TestData.IsAuthorisedTrue,
                TestData.TransactionId1);

            var result = await merchantStatementDomainService.AddTransactionToStatement(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_TransactionNotAuthorised_TransactionNotAddedToStatement()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                                .ReturnsAsync(Result.Success(merchantStatementAggregate));

            MerchantStatementCommands.AddTransactionToMerchantStatementCommand command = new(TestData.EstateId,
                TestData.MerchantId,
                TestData.TransactionDateTime1,
                TestData.TransactionAmount1,
                TestData.IsAuthorisedFalse,
                TestData.TransactionId1);

            var result = await merchantStatementDomainService.AddTransactionToStatement(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldBeEmpty();
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_LogonTransaction_TransactionNotAddedToStatement()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersionFromLastEvent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(merchantStatementAggregate));

            MerchantStatementCommands.AddTransactionToMerchantStatementCommand command = new(TestData.EstateId, TestData.MerchantId,
                    TestData.TransactionDateTime1, null, TestData.IsAuthorisedTrue, TestData.TransactionId1);

            var result = await merchantStatementDomainService.AddTransactionToStatement(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldBeEmpty();
        }
        
        [Fact]
        public async Task MerchantStatementDomainService_AddTransactionToStatement_StatementNotAlreadyCreated_TransactionAdded()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = new MerchantStatementAggregate();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(merchantStatementAggregate));

            MerchantStatementCommands.AddTransactionToMerchantStatementCommand command = new(TestData.EstateId,
                TestData.MerchantId,
                TestData.TransactionDateTime1,
                TestData.TransactionAmount1,
                TestData.IsAuthorisedTrue,
                TestData.TransactionId1);

            var result = await merchantStatementDomainService.AddTransactionToStatement(command, CancellationToken.None);
                result.IsSuccess.ShouldBeTrue();

                var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public async Task MerchantStatementDomainService_AddSettledFeeToStatement_SettledFeeAdded()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = TestData.Aggregates.MerchantStatementAggregateWithTransactionLineAdded();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(merchantStatementAggregate));

            MerchantStatementCommands.AddSettledFeeToMerchantStatementCommand command = new(TestData.EstateId,
                TestData.MerchantId,
                TestData.SettledFeeDateTime1,
                TestData.SettledFeeAmount1,
                TestData.TransactionId1,
                TestData.SettledFeeId1);

            var result = await merchantStatementDomainService.AddSettledFeeToStatement(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            var merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(2);
        }
        
        [Fact]
        public async Task MerchantStatementDomainService_GenerateStatement_StatementGenerated()
        {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = TestData.Aggregates.MerchantStatementAggregateWithTransactionLineAndSettledFeeAdded();

            merchantStatementAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(merchantStatementAggregate));

            var result = await merchantStatementDomainService.GenerateStatement(TestData.GenerateMerchantStatementCommand, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement(false);
            merchantStatement.IsGenerated.ShouldBeTrue();
        }

        [Fact]
        public async Task MerchantStatementDomainService_EmailStatement_StatementGenerated() {
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(TestData.Aggregates.CreatedMerchantAggregate()));

            MerchantStatementAggregate merchantStatementAggregate = TestData.Aggregates.GeneratedMerchantStatementAggregate();

            merchantStatementAggregateRepository
                .Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(merchantStatementAggregate));
            this.statementBuilder
                .Setup(s => s.GetStatementHtml(It.IsAny<StatementHeader>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("<html></html>");

            this.estateManagementRepository
                .Setup(e => e.GetStatement(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StatementHeader());

            IConfigurationRoot configurationRoot =
                new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            this.securityServiceClient
                .Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.TokenResponse());

            this.messagingServiceClient
                .Setup(m => m.SendEmail(It.IsAny<String>(), It.IsAny<SendEmailRequest>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            MerchantStatementCommands.EmailMerchantStatementCommand command = new(TestData.EstateId, TestData.MerchantId,
                TestData.MerchantStatementId);

            var result = await merchantStatementDomainService.EmailStatement(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

            var merchantStatement = merchantStatementAggregate.GetStatement(false);
            merchantStatement.HasBeenEmailed.ShouldBeTrue();
        }
    }

    // TODO: Save changes failure tests
}

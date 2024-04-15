using System;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services;
    using ContractAggregate;
    using DataTransferObjects.Requests.Merchant;
    using EstateAggregate;
    using MerchantAggregate;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Models.Contract;
    using Moq;
    using Requests;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using TransactionProcessor.Client;
    using Xunit;

    public class MerchantDomainServiceTests
    {
        private readonly Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> MerchantAggregateRepository;

        private readonly Mock<IAggregateRepository<MerchantDepositListAggregate, DomainEvent>> MerchantDepositListAggregateRepository;

        private readonly Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;

        private readonly Mock<ISecurityServiceClient> SecurityServiceClient;

        private readonly Mock<ITransactionProcessorClient> TransactionProcessorClient;

        private readonly Mock<IAggregateRepository<ContractAggregate, DomainEvent>> ContractAggregateRepository;

        private readonly MerchantDomainService DomainService;
        
        public MerchantDomainServiceTests()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(new NullLogger());

            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();
            this.MerchantDepositListAggregateRepository = new Mock<IAggregateRepository<MerchantDepositListAggregate, DomainEvent>>();
            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            this.SecurityServiceClient = new Mock<ISecurityServiceClient>();
            this.TransactionProcessorClient = new Mock<ITransactionProcessorClient>();
            this.ContractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate, DomainEvent>>();
            this.DomainService = new MerchantDomainService(this.EstateAggregateRepository.Object,
                                                           this.MerchantAggregateRepository.Object,
                                                           this.MerchantDepositListAggregateRepository.Object,
                                                           this.ContractAggregateRepository.Object,
                                                           this.SecurityServiceClient.Object,
                                                           this.TransactionProcessorClient.Object);
        }

        [Theory]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.NotSet)]
        public async Task MerchantDomainService_CreateMerchant_MerchantIsCreated(DataTransferObjects.Responses.Merchant.SettlementSchedule settlementSchedule)
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            MerchantCommands.CreateMerchantCommand command = new MerchantCommands.CreateMerchantCommand(TestData.EstateId, TestData.CreateMerchantRequest);

            command.RequestDto.SettlementSchedule = settlementSchedule;
            
            Should.NotThrow( async () =>
                            {
                                await this.DomainService.CreateMerchant(command, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchant_MerchantIdNotSet_MerchantIsCreated()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            MerchantCommands.CreateMerchantCommand command = new MerchantCommands.CreateMerchantCommand(TestData.EstateId, TestData.CreateMerchantRequest);
            command.RequestDto.MerchantId = null;

            Should.NotThrow(async () =>
                            {
                                await this.DomainService.CreateMerchant(command, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchant_AlreadyCreated_MerchantIsCreated()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            
            await this.DomainService.CreateMerchant(TestData.CreateMerchantCommand, CancellationToken.None); ;

            Should.NotThrow(async () =>
                            {
                                await this.DomainService.CreateMerchant(TestData.CreateMerchantCommand, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantDomainService_CreateMerchant_EstateNotFound_ErrorThrown()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.CreateMerchant(TestData.CreateMerchantCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AssignOperatorToMerchant_OperatorAssigned()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));
            
            await this.DomainService.AssignOperatorToMerchant(TestData.AssignOperatorToMerchantCommand, CancellationToken.None);
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_MerchantNotCreated_ErrorThrown()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator());

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await this.DomainService.AssignOperatorToMerchant(TestData.AssignOperatorToMerchantCommand, CancellationToken.None);
                                                    });
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_EstateNotCreated_ErrorThrown()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.AssignOperatorToMerchant(TestData.AssignOperatorToMerchantCommand, CancellationToken.None);
            });
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorNotFoundForEstate_ErrorThrown()
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            
            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.AssignOperatorToMerchant(TestData.AssignOperatorToMerchantCommand, CancellationToken.None);
            });
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorRequiresMerchantNumber_MerchantNumberNotSet_ErrorThrown(String merchantNumber)
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberFalse));

            MerchantCommands.AssignOperatorToMerchantCommand command = new MerchantCommands.AssignOperatorToMerchantCommand(TestData.EstateId,
                                                                                                                            TestData.MerchantId,
                                                                                                                            new AssignOperatorRequest()
                                                                                                                            {
                                                                                                                                MerchantNumber = merchantNumber,
                                                                                                                                OperatorId = TestData.OperatorId,
                                                                                                                                TerminalNumber = TestData.TerminalNumber
                                                                                                                            });

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.AssignOperatorToMerchant(command, CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorRequiresTerminalNumber_TerminalNumberNotSet_ErrorThrown(String terminalNumber)
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));

            MerchantCommands.AssignOperatorToMerchantCommand command = new MerchantCommands.AssignOperatorToMerchantCommand(TestData.EstateId,
                                                                                                                            TestData.MerchantId,
                                                                                                                            new AssignOperatorRequest(){
                                                                                                                                                           MerchantNumber = TestData.MerchantNumber,
                                                                                                                                                           OperatorId = TestData.OperatorId,
                                                                                                                                                           TerminalNumber = terminalNumber
                                                                                                                                                       });

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await this.DomainService.AssignOperatorToMerchant(command, CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_MerchantUserIsCreated()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            Should.NotThrow(async () =>
            {
                await this.DomainService.CreateMerchantUser(TestData.CreateMerchantUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_EstateNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.CreateMerchantUser(TestData.CreateMerchantUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_MerchantNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.CreateMerchantUser(TestData.CreateMerchantUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_ErrorCreatingUser_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            Should.Throw<Exception>(async () =>
            {
                await this.DomainService.CreateMerchantUser(TestData.CreateMerchantUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_MerchantDeviceIsAdded()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.NotThrow(async () =>
            {
                await this.DomainService.AddDeviceToMerchant(TestData.AddMerchantDeviceCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_EstateNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.AddDeviceToMerchant(TestData.AddMerchantDeviceCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_MerchantNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.AddDeviceToMerchant(TestData.AddMerchantDeviceCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantDeposit_DepositIsMade()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.CreatedMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.NotThrow(async () =>
            {
                await this.DomainService.MakeMerchantDeposit(TestData.MakeMerchantDepositCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantDeposit_EstateNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.MakeMerchantDeposit(TestData.MakeMerchantDepositCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantDeposit_MerchantNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await this.DomainService.MakeMerchantDeposit(TestData.MakeMerchantDepositCommand, CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantDeposit_NoDepositsYet_DepositIsMade()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.EmptyMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.NotThrow(async () =>
                            {
                                await this.DomainService.MakeMerchantDeposit(TestData.MakeMerchantDepositCommand, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_SwapMerchantDevice_MerchantDeviceSwapped()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithDevice);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.NotThrow(async () =>
                            {
                                await this.DomainService.SwapMerchantDevice(TestData.SwapMerchantDeviceCommand, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_SwapMerchantDevice_MerchantNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.Throw<InvalidOperationException>(async () =>
                            {
                                await this.DomainService.SwapMerchantDevice(TestData.SwapMerchantDeviceCommand, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_SwapMerchantDevice_EstateNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EstateAggregate());

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantAggregateWithDevice);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.SwapMerchantDevice(TestData.SwapMerchantDeviceCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantWithdrawal_WithdrawalIsMade()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.CreatedMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            this.TransactionProcessorClient.Setup(t => t.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
                .ReturnsAsync(TestData.MerchantBalance);
            
            Should.NotThrow(async () =>
                            {
                                await this.DomainService.MakeMerchantWithdrawal(TestData.MakeMerchantWithdrawalCommand,
                                                                             CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantWithdrawal_EstateNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.CreatedMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            this.TransactionProcessorClient.Setup(t => t.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>(),true))
                .ReturnsAsync(TestData.MerchantBalance);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await this.DomainService.MakeMerchantWithdrawal(TestData.MakeMerchantWithdrawalCommand,
                                                                                                        CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantWithdrawal_MerchantNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.CreatedMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            this.TransactionProcessorClient.Setup(t => t.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
                .ReturnsAsync(TestData.MerchantBalance);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.MakeMerchantWithdrawal(TestData.MakeMerchantWithdrawalCommand,
                                                                CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantWithdrawal_MerchantDepositListNotCreated_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MerchantDepositListAggregate());
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            this.TransactionProcessorClient.Setup(t => t.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
                .ReturnsAsync(TestData.MerchantBalance);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await this.DomainService.MakeMerchantWithdrawal(TestData.MakeMerchantWithdrawalCommand,
                                                                                                        CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_MakeMerchantWithdrawal_NotEnoughFundsToWithdraw_ErrorThrown()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.MerchantDepositListAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.CreatedMerchantDepositListAggregate);
            this.MerchantDepositListAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantDepositListAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            this.TransactionProcessorClient.Setup(t => t.GetMerchantBalance(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>(),true))
                .ReturnsAsync(TestData.MerchantBalanceNoAvailableBalance);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await this.DomainService.MakeMerchantWithdrawal(TestData.MakeMerchantWithdrawalCommand,
                                                                CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddContractToMerchant_ContractAdded(){

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregateWithAProductAndTransactionFee(CalculationType.Fixed, FeeType.Merchant));

            await this.DomainService.AddContractToMerchant(TestData.AddMerchantContractCommand, CancellationToken.None);
        }

        [Fact]
        public async Task MerchantDomainService_AddContractToMerchant_ContractNotCreated_ErrorThrown(){
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            Should.Throw<InvalidOperationException>(async () => {
                await this.DomainService.AddContractToMerchant(TestData.AddMerchantContractCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddContractToMerchant_MerchantNotCreated_ErrorThrown(){

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyMerchantAggregate);

            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregateWithAProductAndTransactionFee(CalculationType.Fixed, FeeType.Merchant));

            Should.Throw<InvalidOperationException>(async () => {
                await this.DomainService.AddContractToMerchant(TestData.AddMerchantContractCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddContractToMerchant_EstateNotCreated_ErrorThrown(){

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);

            this.ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregateWithAProductAndTransactionFee(CalculationType.Fixed, FeeType.Merchant));
            Should.Throw<InvalidOperationException>(async () => {
                await this.DomainService.AddContractToMerchant(TestData.AddMerchantContractCommand, CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly)]
        [InlineData(DataTransferObjects.Responses.Merchant.SettlementSchedule.NotSet)]
        public async Task MerchantDomainService_UpdateMerchant_MerchantIsUpdated(DataTransferObjects.Responses.Merchant.SettlementSchedule settlementSchedule)
        {
            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            MerchantCommands.UpdateMerchantCommand command = new(TestData.EstateId, TestData.MerchantId, TestData.UpdateMerchantRequest);

            command.RequestDto.SettlementSchedule = settlementSchedule;

            Should.NotThrow(async () =>
                            {
                                await this.DomainService.UpdateMerchant(command, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task MerchantDomainService_AddMerchantAddress_AddressAdded()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.NotThrow(async () =>
                                                    {
                                                        await this.DomainService.AddMerchantAddress(TestData.AddMerchantAddressCommand, CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_UpdateMerchantAddress_AddressAdded()
        {
            this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            this.MerchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            this.MerchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Should.NotThrow(async () =>
                            {
                                await this.DomainService.UpdateMerchantAddress(TestData.UpdateMerchantAddressCommand, CancellationToken.None);
                            });
        }
    }
}

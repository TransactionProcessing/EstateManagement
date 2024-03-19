namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.EventHandling;
    using EstateManagement.MerchantStatement.DomainEvents;
    using EstateManagement.Repository;
    using Moq;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using Xunit;

    public class StatementDomainEventHandlerTests
    {
        private Mock<IEstateReportingRepository> EstateReportingRepository;

        private StatementDomainEventHandler DomainEventHandler;
        public StatementDomainEventHandlerTests() {
            Logger.Initialise(NullLogger.Instance);
            this.EstateReportingRepository = new Mock<IEstateReportingRepository>();
            this.DomainEventHandler = new StatementDomainEventHandler(this.EstateReportingRepository.Object);
        }

        [Fact]
        public async Task StatementDomainEventHandler_Handle_StatementGeneratedEvent_EventIsHandled()
        {
            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(TestData.StatementGeneratedEvent, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task StatementDomainEventHandler_Handle_StatementCreatedEvent_EventIsHandled()
        {
            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(TestData.StatementCreatedEvent, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task StatementDomainEventHandler_Handle_TransactionAddedToStatementEvent_EventIsHandled()
        {
            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(TestData.TransactionAddedToStatementEvent, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task StatementDomainEventHandler_Handle_SettledFeeAddedToStatementEvent_EventIsHandled()
        {
            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(TestData.SettledFeeAddedToStatementEvent, CancellationToken.None);
                            });
        }
        
    }
}
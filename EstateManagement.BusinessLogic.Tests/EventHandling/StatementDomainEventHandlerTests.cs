namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.EventHandling;
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
    }
}
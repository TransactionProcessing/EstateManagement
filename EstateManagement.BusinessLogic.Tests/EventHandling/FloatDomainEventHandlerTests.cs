using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using BusinessLogic.EventHandling;
    using EstateManagement.Repository;
    using EstateManagement.Testing;
    using Moq;
    using Shared.EventStore.EventHandling;
    using Shared.Logger;
    using Shouldly;
    using TransactionProcessor.Float.DomainEvents;
    using Xunit;

    public class FloatDomainEventHandlerTests{
        private Mock<IEstateReportingRepository> EstateReportingRepository;
        private IDomainEventHandler DomainEventHandler;
        public FloatDomainEventHandlerTests(){
            Logger.Initialise(NullLogger.Instance);
            this.EstateReportingRepository = new Mock<IEstateReportingRepository>();
            this.DomainEventHandler = new FloatDomainEventHandler(this.EstateReportingRepository.Object);
        }

        //FloatCreatedForContractProductEvent
        //FloatCreditPurchasedEvent
        //FloatDecreasedByTransactionEvent
        [Fact]
        public async Task FloatDomainEventHandler_FloatCreatedForContractProductEvent_EventIsHandled()
        {
            FloatCreatedForContractProductEvent floatCreatedForContractProductEvent = TestData.FloatCreatedForContractProductEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(floatCreatedForContractProductEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task FloatDomainEventHandler_FloatCreditPurchasedEvent_EventIsHandled()
        {
            FloatCreditPurchasedEvent floatCreditPurchasedEvent = TestData.FloatCreditPurchasedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(floatCreditPurchasedEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task FloatDomainEventHandler_FloatDecreasedByTransactionEvent_EventIsHandled()
        {
            FloatDecreasedByTransactionEvent floatDecreasedByTransactionEvent = TestData.FloatDecreasedByTransactionEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(floatDecreasedByTransactionEvent, CancellationToken.None); });
        }
    }
}

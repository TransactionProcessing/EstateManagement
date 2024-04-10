using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using BusinessLogic.EventHandling;
    using MediatR;
    using Moq;
    using Repository;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using TransactionProcessor.Reconciliation.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;
    using Xunit;

    public class TransactionDomainEventHandlerTests
    {
        private Mock<IMediator> Mediator;

        private Mock<IEstateReportingRepository> EstateReportingRepository;

        private TransactionDomainEventHandler DomainEventHandler;

        public TransactionDomainEventHandlerTests() {
            Logger.Initialise(NullLogger.Instance);
            this.Mediator = new Mock<IMediator>();
            this.EstateReportingRepository = new Mock<IEstateReportingRepository>();
            this.DomainEventHandler = new TransactionDomainEventHandler(this.Mediator.Object, this.EstateReportingRepository.Object);
        }
        [Fact]
        public async Task TransactionDomainEventHandler_Handle_TransactionHasBeenCompletedEvent_EventIsHandled()
        {
            this.Mediator.Setup(m => m.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Unit());
            
            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(TestData.TransactionHasBeenCompletedEvent, CancellationToken.None);
                            });
        }

        [Fact]
        public void TransactionDomainEventHandler_AdditionalRequestDataRecordedEvent_EventIsHandled()
        {
            AdditionalRequestDataRecordedEvent additionalRequestDataRecordedEvent = TestData.AdditionalRequestDataRecordedEvent;

            Logger.Initialise(NullLogger.Instance);

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(additionalRequestDataRecordedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_AdditionalResponseDataRecordedEvent_EventIsHandled()
        {
            AdditionalResponseDataRecordedEvent additionalResponseDataRecordedEvent = TestData.AdditionalResponseDataRecordedEvent;
            
            Logger.Initialise(NullLogger.Instance);

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(additionalResponseDataRecordedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_OverallTotalsRecordedEvent_EventIsHandled()
        {
            OverallTotalsRecordedEvent overallTotalsRecordedEvent = TestData.OverallTotalsRecordedEvent;
            
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(overallTotalsRecordedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_ProductDetailsAddedToTransactionEvent_EventIsHandled()
        {
            ProductDetailsAddedToTransactionEvent productDetailsAddedToTransactionEvent = TestData.ProductDetailsAddedToTransactionEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(productDetailsAddedToTransactionEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_ReconciliationHasBeenLocallyAuthorisedEvent_EventIsHandled()
        {
            ReconciliationHasBeenLocallyAuthorisedEvent reconciliationHasBeenLocallyAuthorisedEvent = TestData.ReconciliationHasBeenLocallyAuthorisedEvent;
            
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(reconciliationHasBeenLocallyAuthorisedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_ReconciliationHasBeenLocallyDeclinedEvent_EventIsHandled()
        {
            ReconciliationHasBeenLocallyDeclinedEvent reconciliationHasBeenLocallyDeclinedEvent = TestData.ReconciliationHasBeenLocallyDeclinedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(reconciliationHasBeenLocallyDeclinedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_ReconciliationHasCompletedEvent_EventIsHandled()
        {
            ReconciliationHasCompletedEvent reconciliationHasCompletedEvent = TestData.ReconciliationHasCompletedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(reconciliationHasCompletedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_ReconciliationHasStartedEvent_EventIsHandled()
        {
            ReconciliationHasStartedEvent reconciliationHasStartedEvent = TestData.ReconciliationHasStartedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(reconciliationHasStartedEvent, CancellationToken.None); });
        }
        
        [Fact]
        public void TransactionDomainEventHandler_TransactionAuthorisedByOperatorEvent_EventIsHandled()
        {
            TransactionAuthorisedByOperatorEvent transactionAuthorisedByOperatorEvent = TestData.TransactionAuthorisedByOperatorEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionAuthorisedByOperatorEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionDeclinedByOperatorEvent_EventIsHandled()
        {
            TransactionDeclinedByOperatorEvent transactionDeclinedByOperatorEvent = TestData.TransactionDeclinedByOperatorEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionDeclinedByOperatorEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionHasBeenCompletedEvent_EventIsHandled()
        {
            TransactionHasBeenCompletedEvent transactionHasBeenCompletedEvent = TestData.TransactionHasBeenCompletedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionHasBeenCompletedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionHasBeenLocallyAuthorisedEvent_EventIsHandled()
        {
            TransactionHasBeenLocallyAuthorisedEvent transactionHasBeenLocallyAuthorisedEvent = TestData.TransactionHasBeenLocallyAuthorisedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionHasBeenLocallyAuthorisedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionHasBeenLocallyDeclinedEvent_EventIsHandled()
        {
            TransactionHasBeenLocallyDeclinedEvent transactionHasBeenLocallyDeclinedEvent = TestData.TransactionHasBeenLocallyDeclinedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionHasBeenLocallyDeclinedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionHasStartedEvent_EventIsHandled()
        {
            TransactionHasStartedEvent transactionHasStartedEvent = TestData.TransactionHasStartedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionHasStartedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_VoucherFullyRedeemedEvent_EventIsHandled()
        {
            VoucherFullyRedeemedEvent voucherFullyRedeemedEvent = TestData.VoucherFullyRedeemedEvent;
                
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(voucherFullyRedeemedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_VoucherGeneratedEvent_EventIsHandled()
        {
            VoucherGeneratedEvent voucherGeneratedEvent = TestData.VoucherGeneratedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(voucherGeneratedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_VoucherIssuedEvent_EventIsHandled()
        {
            VoucherIssuedEvent voucherIssuedEvent = TestData.VoucherIssuedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(voucherIssuedEvent, CancellationToken.None); });
        }

        [Fact]
        public void TransactionDomainEventHandler_TransactionSourceAddedToTransactionEvent_EventIsHandled()
        {
            TransactionSourceAddedToTransactionEvent transactionSourceAddedToTransactionEvent = TestData.TransactionSourceAddedToTransactionEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionSourceAddedToTransactionEvent, CancellationToken.None); });
        }
    }
}

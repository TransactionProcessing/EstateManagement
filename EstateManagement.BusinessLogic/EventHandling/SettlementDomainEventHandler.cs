namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Repository;
    using MediatR;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.EventHandling.IDomainEventHandler" />
    public class SettlementDomainEventHandler : IDomainEventHandler
    {
        #region Fields
        
        private readonly IEstateReportingRepository EstateReportingRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettlementDomainEventHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SettlementDomainEventHandler(IEstateReportingRepository estateReportingRepository) {
            this.EstateReportingRepository = estateReportingRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(MerchantFeeSettledEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.MarkMerchantFeeAsSettled(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(SettlementProcessingStartedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.MarkSettlementAsProcessingStarted(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(SettlementCreatedForDateEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateSettlement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(MerchantFeeAddedToTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            // Generate the settlement id from the date
            Guid settlementId = SettlementDomainEventHandler.GetSettlementId(domainEvent.SettlementDueDate);

            await this.EstateReportingRepository.AddSettledMerchantFeeToSettlement(settlementId, domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddPendingMerchantFeeToSettlement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(SettlementCompletedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.MarkSettlementAsCompleted(domainEvent, cancellationToken);
        }
        
        public static Guid GetSettlementId(DateTime dt)
        {
            Byte[] bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using EstateManagement.Repository;
    using MediatR;
    using Requests;
    using Services;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using Shared.Logger;
    using TransactionProcessor.Reconciliation.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.EventHandling.IDomainEventHandler" />
    public class TransactionDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        private readonly IEstateReportingRepository EstateReportingRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDomainEventHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public TransactionDomainEventHandler(IMediator mediator, IEstateReportingRepository estateReportingRepository) {
            this.Mediator = mediator;
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
        private async Task HandleSpecificDomainEvent(TransactionHasBeenCompletedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            AddTransactionToMerchantStatementRequest addTransactionToMerchantStatementRequest = AddTransactionToMerchantStatementRequest.Create(domainEvent.EstateId,
                domainEvent.MerchantId,
                domainEvent.CompletedDateTime,
                domainEvent.TransactionAmount,
                domainEvent.IsAuthorised,
                domainEvent.TransactionId);

            await this.Mediator.Send(addTransactionToMerchantStatementRequest, cancellationToken);

            await this.EstateReportingRepository.CompleteTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Hexadecimals the string from bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        internal static String HexStringFromBytes(Byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (Byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionHasStartedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.StartTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(AdditionalRequestDataRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.RecordTransactionAdditionalRequestData(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(AdditionalResponseDataRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.RecordTransactionAdditionalResponseData(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionAuthorisedByOperatorEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionDeclinedByOperatorEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }
        
        private async Task HandleSpecificDomainEvent(TransactionSourceAddedToTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddSourceDetailsToTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ProductDetailsAddedToTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddProductDetailsToTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(MerchantFeeAddedToTransactionEnrichedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddFeeDetailsToTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ServiceProviderFeeAddedToTransactionEnrichedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddFeeDetailsToTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ReconciliationHasStartedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.StartReconciliation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(OverallTotalsRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateReconciliationOverallTotals(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateReconciliationStatus(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateReconciliationStatus(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ReconciliationHasCompletedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CompleteReconciliation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(VoucherGeneratedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddGeneratedVoucher(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(VoucherIssuedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateVoucherIssueDetails(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(VoucherFullyRedeemedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateVoucherRedemptionDetails(domainEvent, cancellationToken);
        }

        #endregion

    }
}
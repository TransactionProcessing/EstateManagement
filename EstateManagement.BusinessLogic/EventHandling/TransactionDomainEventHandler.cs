using SimpleResults;

namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Repository;
    using MediatR;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
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
        public async Task<Result> Handle(IDomainEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            return await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionHasBeenCompletedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.CompleteTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Hexadecimals the string from bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        internal static String HexStringFromBytes(Byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in bytes)
            {
                String hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionHasStartedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.StartTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(AdditionalRequestDataRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            var result = await this.EstateReportingRepository.RecordTransactionAdditionalRequestData(domainEvent, cancellationToken);
            if (result.IsFailed)
                return result;
            return await this.EstateReportingRepository.SetTransactionAmount(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(AdditionalResponseDataRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.RecordTransactionAdditionalResponseData(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionAuthorisedByOperatorEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(TransactionDeclinedByOperatorEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateTransactionAuthorisation(domainEvent, cancellationToken);
        }
        
        private async Task<Result> HandleSpecificDomainEvent(TransactionSourceAddedToTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.AddSourceDetailsToTransaction(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(ProductDetailsAddedToTransactionEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.AddProductDetailsToTransaction(domainEvent, cancellationToken);
        }
        
        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(ReconciliationHasStartedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.StartReconciliation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(OverallTotalsRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateReconciliationOverallTotals(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateReconciliationStatus(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateReconciliationStatus(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(ReconciliationHasCompletedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.CompleteReconciliation(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(VoucherGeneratedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.AddGeneratedVoucher(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(VoucherIssuedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateVoucherIssueDetails(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task<Result> HandleSpecificDomainEvent(VoucherFullyRedeemedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.UpdateVoucherRedemptionDetails(domainEvent, cancellationToken);
        }

        #endregion
    }
}
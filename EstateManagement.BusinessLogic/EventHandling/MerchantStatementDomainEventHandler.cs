namespace EstateManagement.BusinessLogic.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MerchantStatement.DomainEvents;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Transaction.DomainEvents;

    public class MerchantStatementDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementDomainEventHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public MerchantStatementDomainEventHandler(IMediator mediator)
        {
            this.Mediator = mediator;
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
        private async Task HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            EmailMerchantStatementRequest emailMerchantStatementRequest = EmailMerchantStatementRequest.Create(domainEvent.EstateId,
                                                                                                               domainEvent.MerchantId,
                                                                                                               domainEvent.MerchantStatementId);
            
            await this.Mediator.Send(emailMerchantStatementRequest, cancellationToken);
        }

        private async Task HandlesSpecificDomainEvent(TransactionHasBeenCompletedEvent domainEvent,
                                                      CancellationToken cancellationToken) {
            AddTransactionToMerchantStatementRequest addTransactionToMerchantStatementRequest = AddTransactionToMerchantStatementRequest.Create(domainEvent.EstateId,
                domainEvent.MerchantId,
                domainEvent.CompletedDateTime,
                domainEvent.TransactionAmount,
                domainEvent.IsAuthorised,
                domainEvent.TransactionId);

            await this.Mediator.Send(addTransactionToMerchantStatementRequest, cancellationToken);
        }

        #endregion
    }
}
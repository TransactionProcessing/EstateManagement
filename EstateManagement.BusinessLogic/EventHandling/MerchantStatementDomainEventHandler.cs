using SimpleResults;

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
        private async Task<Result> HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                             CancellationToken cancellationToken) {
            MerchantStatementCommands.EmailMerchantStatementCommand command = new(domainEvent.EstateId,
                domainEvent.MerchantId, domainEvent.MerchantStatementId);
            
            return await this.Mediator.Send(command, cancellationToken);
        }

        private async Task<Result> HandleSpecificDomainEvent(TransactionHasBeenCompletedEvent domainEvent,
                                                             CancellationToken cancellationToken) {
            MerchantStatementCommands.AddTransactionToMerchantStatementCommand command = new(domainEvent.EstateId,
                domainEvent.MerchantId,
                domainEvent.CompletedDateTime,
                domainEvent.TransactionAmount,
                domainEvent.IsAuthorised,
                domainEvent.TransactionId);

            return await this.Mediator.Send(command, cancellationToken);
        }

        #endregion
    }
}
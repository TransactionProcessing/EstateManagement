namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MediatR;
    using Requests;
    using Services;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using Shared.Logger;
    using TransactionProcessor.Transaction.DomainEvents;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDomainEventHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public TransactionDomainEventHandler(IMediator mediator)
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
        }
        
        #endregion
    }
}
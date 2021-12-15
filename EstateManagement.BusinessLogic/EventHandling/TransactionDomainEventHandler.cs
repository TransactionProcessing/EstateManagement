using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.EventHandling
{
    using System.Threading;
    using System.Transactions;
    using Common;
    using MediatR;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Models.MerchantStatement;
    using Requests;
    using Services;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Transaction.DomainEvents;
    using Transaction = Models.MerchantStatement.Transaction;

    public class TransactionDomainEventHandler : IDomainEventHandler
    {
        private readonly IMediator Mediator;
        
        public TransactionDomainEventHandler(IMediator mediator)
        {
            this.Mediator = mediator;
        }
     
        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

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
    }
}

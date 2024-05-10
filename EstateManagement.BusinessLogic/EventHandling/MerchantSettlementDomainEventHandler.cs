namespace EstateManagement.BusinessLogic.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Events;
using EstateManagement.Merchant.DomainEvents;
using EstateManagement.MerchantStatement.DomainEvents;
using MediatR;
using Requests;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;
using TransactionProcessor.Settlement.DomainEvents;
using TransactionProcessor.Transaction.DomainEvents;

public class MerchantSettlementDomainEventHandler : IDomainEventHandler
{
    private readonly IMediator Mediator;

    public MerchantSettlementDomainEventHandler(IMediator mediator) {
        this.Mediator = mediator;
    }

    public async Task Handle(IDomainEvent domainEvent,
                             CancellationToken cancellationToken)
    {
        Task t = domainEvent switch
        {
            MerchantFeeSettledEvent mfse => this.HandleSpecificDomainEvent(mfse, cancellationToken),
            _ => null
        };
        if (t != null)
            await t;
    }

    private async Task HandleSpecificDomainEvent(MerchantFeeSettledEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        AddSettledFeeToMerchantStatementRequest addSettledFeeToMerchantStatementRequest = AddSettledFeeToMerchantStatementRequest.Create(domainEvent.EstateId,
            domainEvent.MerchantId,
            domainEvent.FeeCalculatedDateTime,
            domainEvent.CalculatedValue,
            domainEvent.TransactionId,
            domainEvent.FeeId);

        await this.Mediator.Send(addSettledFeeToMerchantStatementRequest, cancellationToken);
    }
}
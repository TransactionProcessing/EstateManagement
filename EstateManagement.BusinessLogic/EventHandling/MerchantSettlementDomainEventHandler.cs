using SimpleResults;

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

    public async Task<Result>  Handle(IDomainEvent domainEvent,
                             CancellationToken cancellationToken)
    {
        Task<Result> t = domainEvent switch
        {
            MerchantFeeSettledEvent mfse => this.HandleSpecificDomainEvent(mfse, cancellationToken),
            _ => null
        };
        if (t != null)
            return await t;

        return Result.Success();
    }

    private async Task<Result> HandleSpecificDomainEvent(MerchantFeeSettledEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        MerchantStatementCommands.AddSettledFeeToMerchantStatementCommand  command = new(domainEvent.EstateId,
            domainEvent.MerchantId,
            domainEvent.FeeCalculatedDateTime,
            domainEvent.CalculatedValue,
            domainEvent.TransactionId,
            domainEvent.FeeId);

        return await this.Mediator.Send(command, cancellationToken);
    }
}
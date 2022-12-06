namespace EstateManagement.Merchant.DomainEvents;

using System;
using Shared.DomainDrivenDesign.EventSourcing;

public record WithdrawalMadeEvent : DomainEvent
{
    #region Constructors

    public WithdrawalMadeEvent(Guid aggregateId,
                               Guid estateId,
                               Guid withdrawalId,
                               DateTime withdrawalDateTime,
                               Decimal amount) : base(aggregateId, Guid.NewGuid())
    {
        this.EstateId = estateId;
        this.WithdrawalId = withdrawalId;
        this.MerchantId = aggregateId;
        this.WithdrawalDateTime = withdrawalDateTime;
        this.Amount = amount;
    }

    #endregion

    #region Properties

    public Decimal Amount { get; init; }
        
    public DateTime WithdrawalDateTime { get; init; }

    public Guid WithdrawalId { get; init; }

    public Guid EstateId { get; init; }

    public Guid MerchantId { get; init; }
        
    #endregion
}
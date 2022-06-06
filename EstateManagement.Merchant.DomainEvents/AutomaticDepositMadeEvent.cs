namespace EstateManagement.Merchant.DomainEvents;

using System;
using Shared.DomainDrivenDesign.EventSourcing;

public record AutomaticDepositMadeEvent : DomainEvent
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ManualDepositMadeEvent" /> class.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="estateId">The estate identifier.</param>
    /// <param name="depositId">The deposit identifier.</param>
    /// <param name="reference">The reference.</param>
    /// <param name="depositDateTime">The deposit date time.</param>
    /// <param name="amount">The amount.</param>
    public AutomaticDepositMadeEvent(Guid aggregateId,
                                     Guid estateId,
                                     Guid depositId,
                                     String reference,
                                     DateTime depositDateTime,
                                     Decimal amount) : base(aggregateId, Guid.NewGuid())
    {
        this.EstateId = estateId;
        this.MerchantId = aggregateId;
        this.DepositId = depositId;
        this.Reference = reference;
        this.DepositDateTime = depositDateTime;
        this.Amount = amount;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the amount.
    /// </summary>
    /// <value>
    /// The amount.
    /// </value>
    public Decimal Amount { get; init; }

    /// <summary>
    /// Gets the deposit date time.
    /// </summary>
    /// <value>
    /// The deposit date time.
    /// </value>
    public DateTime DepositDateTime { get; init; }

    /// <summary>
    /// Gets the deposit identifier.
    /// </summary>
    /// <value>
    /// The deposit identifier.
    /// </value>
    public Guid DepositId { get; init; }

    /// <summary>
    /// Gets the estate identifier.
    /// </summary>
    /// <value>
    /// The estate identifier.
    /// </value>
    public Guid EstateId { get; init; }

    /// <summary>
    /// Gets the merchant identifier.
    /// </summary>
    /// <value>
    /// The merchant identifier.
    /// </value>
    public Guid MerchantId { get; init; }

    /// <summary>
    /// Gets the reference.
    /// </summary>
    /// <value>
    /// The reference.
    /// </value>
    /// init; }
    public String Reference { get; init; }

    #endregion
}
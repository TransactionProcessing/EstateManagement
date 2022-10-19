﻿namespace EstateManagement.MerchantStatement.DomainEvents;

using System;
using Shared.DomainDrivenDesign.EventSourcing;

public record StatementEmailedEvent : DomainEvent
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StatementGeneratedEvent" /> class.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="estateId">The estate identifier.</param>
    /// <param name="merchantId">The merchant identifier.</param>
    /// <param name="dateEmailed">The date emailed.</param>
    /// <param name="messageId">The message identifier.</param>
    public StatementEmailedEvent(Guid aggregateId,
                                 Guid estateId,
                                 Guid merchantId,
                                 DateTime dateEmailed,
                                 Guid messageId) : base(aggregateId, Guid.NewGuid())
    {
        this.MerchantStatementId = aggregateId;
        this.EstateId = estateId;
        this.MerchantId = merchantId;
        this.DateEmailed = dateEmailed;
        this.MessageId = messageId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the date emailed.
    /// </summary>
    /// <value>
    /// The date emailed.
    /// </value>
    public DateTime DateEmailed { get; init; }

    /// <summary>
    /// Gets or sets the message identifier.
    /// </summary>
    /// <value>
    /// The message identifier.
    /// </value>
    public Guid MessageId { get; init; }

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
    /// Gets or sets the merchant statement identifier.
    /// </summary>
    /// <value>
    /// The merchant statement identifier.
    /// </value>
    public Guid MerchantStatementId { get; init; }

    #endregion
}
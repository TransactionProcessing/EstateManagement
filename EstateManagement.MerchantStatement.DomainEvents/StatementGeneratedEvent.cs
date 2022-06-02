namespace EstateManagement.MerchantStatement.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent" />
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.IDomainEvent" />
    /// <seealso cref="System.IEquatable&lt;Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent&gt;" />
    /// <seealso cref="System.IEquatable&lt;EstateManagement.MerchantStatement.DomainEvents.StatementGeneratedEvent&gt;" />
    public record StatementGeneratedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementGeneratedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="dateGenerated">The date generated.</param>
        public StatementGeneratedEvent(Guid aggregateId,
                                       Guid estateId,
                                       Guid merchantId,
                                       DateTime dateGenerated) : base(aggregateId, Guid.NewGuid())
        {
            this.MerchantStatementId = aggregateId;
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.DateGenerated = dateGenerated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the date generated.
        /// </summary>
        /// <value>
        /// The date generated.
        /// </value>
        public DateTime DateGenerated { get; init; }

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
}
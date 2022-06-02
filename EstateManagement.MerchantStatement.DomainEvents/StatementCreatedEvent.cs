namespace EstateManagement.MerchantStatement.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent&gt;" />
    /// <seealso cref="System.IEquatable&lt;EstateManagement.MerchantStatement.DomainEvents.StatementCreatedEvent&gt;" />
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent" />
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.IDomainEvent" />
    /// <seealso cref="System.IEquatable&lt;Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent&gt;" />
    /// <seealso cref="System.IEquatable&lt;EstateManagement.MerchantStatement.DomainEvents.StatementCreatedEvent&gt;" />
    public record StatementCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="dateCreated">The date created.</param>
        public StatementCreatedEvent(Guid aggregateId,
                                     Guid estateId,
                                     Guid merchantId,
                                     DateTime dateCreated) : base(aggregateId, Guid.NewGuid())
        {
            this.MerchantStatementId = aggregateId;
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.DateCreated = dateCreated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        public DateTime DateCreated { get; init; }

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
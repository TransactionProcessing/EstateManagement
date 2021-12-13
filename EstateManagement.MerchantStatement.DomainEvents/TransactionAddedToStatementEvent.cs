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
    /// <seealso cref="System.IEquatable&lt;EstateManagement.MerchantStatement.DomainEvents.TransactionAddedToStatementEvent&gt;" />
    public record TransactionAddedToStatementEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionAddedToStatementEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="transactionDateTime">The transaction date time.</param>
        /// <param name="transactionValue">The transaction value.</param>
        /// <param name="operatorId">The operator identifier.</param>
        public TransactionAddedToStatementEvent(Guid aggregateId,
                                                Guid estateId,
                                                Guid merchantId,
                                                Guid transactionId,
                                                DateTime transactionDateTime,
                                                Decimal transactionValue,
                                                Guid operatorId) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.TransactionId = transactionId;
            this.TransactionDateTime = transactionDateTime;
            this.TransactionValue = transactionValue;
            this.OperatorId = operatorId;
            this.MerchantStatementId = aggregateId;
        }

        #endregion

        #region Properties

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

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date time.
        /// </summary>
        /// <value>
        /// The transaction date time.
        /// </value>
        public DateTime TransactionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; init; }

        /// <summary>
        /// Gets or sets the transaction value.
        /// </summary>
        /// <value>
        /// The transaction value.
        /// </value>
        public Decimal TransactionValue { get; set; }

        #endregion
    }
}
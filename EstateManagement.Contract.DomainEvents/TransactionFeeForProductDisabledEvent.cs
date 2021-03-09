namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record TransactionFeeForProductDisabledEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductDisabledEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        public TransactionFeeForProductDisabledEvent(Guid aggregateId,
                                                      Guid estateId,
                                                      Guid productId,
                                                      Guid transactionFeeId) : base(aggregateId, Guid.NewGuid())
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; init; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public Guid ProductId { get; init; }

        /// <summary>
        /// Gets the transaction fee identifier.
        /// </summary>
        /// <value>
        /// The transaction fee identifier.
        /// </value>
        public Guid TransactionFeeId { get; init; }

        #endregion
    }
}
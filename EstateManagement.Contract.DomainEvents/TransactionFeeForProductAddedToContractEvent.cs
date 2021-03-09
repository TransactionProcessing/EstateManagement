namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent" />
    public record TransactionFeeForProductAddedToContractEvent : DomainEventRecord.DomainEvent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductAddedToContractEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="feeType">Type of the fee.</param>
        /// <param name="value">The value.</param>
        public TransactionFeeForProductAddedToContractEvent(Guid aggregateId,
                                                             Guid estateId,
                                                             Guid productId,
                                                             Guid transactionFeeId,
                                                             String description,
                                                             Int32 calculationType,
                                                             Int32 feeType,
                                                             Decimal value) : base(aggregateId, Guid.NewGuid())
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
            this.Description = description;
            this.CalculationType = calculationType;
            this.FeeType = feeType;
            this.Value = value;
        }


        #region Properties

        /// <summary>
        /// Gets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        public Int32 CalculationType { get; init; }
        public Int32 FeeType { get; init; }

        
        public Guid ContractId { get; init; }
        public String Description { get; init; }
        
        public Guid EstateId { get; init; }
        public Guid ProductId { get; init; }
        
        public Guid TransactionFeeId { get; init; }

        
        public Decimal Value { get; init; }

        #endregion

}
}
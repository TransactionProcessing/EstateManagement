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
    public record FixedValueProductAddedToContractEvent : DomainEvent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedValueProductAddedToContractEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        public FixedValueProductAddedToContractEvent(Guid aggregateId,
                                                      Guid estateId,
                                                      Guid productId,
                                                      String productName,
                                                      String displayText,
                                                      Decimal value) : base(aggregateId, Guid.NewGuid())
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.ProductName = productName;
            this.DisplayText = displayText;
            this.Value = value;
        }


        #region Properties


        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; init; }
        public String DisplayText { get; init; }

        public Guid EstateId { get; init; }
        public Guid ProductId { get; init; }

        public String ProductName { get; init; }

        public Decimal Value { get; init; }

        #endregion

        
    }
}
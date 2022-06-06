namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record VariableValueProductAddedToContractEvent : DomainEvent
    {
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="VariableValueProductAddedToContractEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        public VariableValueProductAddedToContractEvent(Guid aggregateId,
                                                         Guid estateId,
                                                         Guid productId,
                                                         String productName,
                                                         String displayText) : base(aggregateId, Guid.NewGuid())
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.ProductName = productName;
            this.DisplayText = displayText;
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
        /// Gets the display text.
        /// </summary>
        /// <value>
        /// The display text.
        /// </value>
        public String DisplayText { get; init; }

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
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public String ProductName { get; init; }

        #endregion
        
    }
}
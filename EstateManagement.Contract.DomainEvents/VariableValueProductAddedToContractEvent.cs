namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    [JsonObject]
    public class VariableValueProductAddedToContractEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableValueProductAddedToContractEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public VariableValueProductAddedToContractEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableValueProductAddedToContractEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        private VariableValueProductAddedToContractEvent(Guid aggregateId,
                                                         Guid eventId,
                                                         Guid estateId,
                                                         Guid productId,
                                                         String productName,
                                                         String displayText) : base(aggregateId, eventId)
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
        [JsonProperty]
        public Guid ContractId { get; private set; }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        /// <value>
        /// The display text.
        /// </value>
        [JsonProperty]
        public String DisplayText { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty]
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        [JsonProperty]
        public String ProductName { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <returns></returns>
        public static VariableValueProductAddedToContractEvent Create(Guid aggregateId,
                                                                   Guid estateId,
                                                                   Guid productId,
                                                                   String productName,
                                                                   String displayText)
        {
            return new VariableValueProductAddedToContractEvent(aggregateId, Guid.NewGuid(), estateId, productId, productName, displayText);
        }

        #endregion
    }
}
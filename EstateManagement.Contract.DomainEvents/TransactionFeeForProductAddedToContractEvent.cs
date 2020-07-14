namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    public class TransactionFeeForProductAddedToContractEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductAddedToContractEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TransactionFeeForProductAddedToContractEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductAddedToContractEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="value">The value.</param>
        private TransactionFeeForProductAddedToContractEvent(Guid aggregateId,
                                                             Guid eventId,
                                                             Guid estateId,
                                                             Guid productId,
                                                             Guid transactionFeeId,
                                                             String description,
                                                             Int32 calculationType,
                                                             Decimal value) : base(aggregateId, eventId)
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
            this.Description = description;
            this.CalculationType = calculationType;
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        [JsonProperty]
        public Int32 CalculationType { get; private set; }

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        [JsonProperty]
        public Guid ContractId { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty]
        public String Description { get; private set; }

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
        /// Gets the transaction fee identifier.
        /// </summary>
        /// <value>
        /// The transaction fee identifier.
        /// </value>
        [JsonProperty]
        public Guid TransactionFeeId { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty]
        public Decimal Value { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <returns></returns>
        public static TransactionFeeForProductAddedToContractEvent Create(Guid aggregateId,
                                                                          Guid estateId,
                                                                          Guid productId,
                                                                          Guid transactionFeeId,
                                                                          String description,
                                                                          Int32 calculationType,
                                                                          Decimal value)
        {
            return new TransactionFeeForProductAddedToContractEvent(aggregateId, Guid.NewGuid(), estateId, productId, transactionFeeId, description, calculationType, value);
        }

        #endregion
    }
}
namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    [JsonObject]
    public class OperatorAssignedToMerchantEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorAssignedToMerchantEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public OperatorAssignedToMerchantEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorAssignedToMerchantEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        private OperatorAssignedToMerchantEvent(Guid aggregateId,
                                                Guid eventId,
                                                Guid estateId,
                                                Guid operatorId,
                                                String name,
                                                String merchantNumber,
                                                String terminalNumber) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.MerchantNumber = merchantNumber;
            this.TerminalNumber = terminalNumber;
            this.MerchantId = aggregateId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        [JsonProperty]
        public Guid MerchantId { get; private set; }

        /// <summary>
        /// Gets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        [JsonProperty]
        public String MerchantNumber { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty]
        public String Name { get; private set; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [JsonProperty]
        public Guid OperatorId { get; private set; }

        /// <summary>
        /// Gets the terminal number.
        /// </summary>
        /// <value>
        /// The terminal number.
        /// </value>
        [JsonProperty]
        public String TerminalNumber { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        /// <returns></returns>
        public static OperatorAssignedToMerchantEvent Create(Guid aggregateId,
                                                             Guid estateId,
                                                             Guid operatorId,
                                                             String name,
                                                             String merchantNumber,
                                                             String terminalNumber)
        {
            return new OperatorAssignedToMerchantEvent(aggregateId, Guid.NewGuid(), estateId, operatorId, name, merchantNumber, terminalNumber);
        }

        #endregion
    }
}
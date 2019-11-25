namespace EstateManagement.Estate.DomainEvents
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
    public class OperatorAddedToEstateEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public OperatorAddedToEstateEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        private OperatorAddedToEstateEvent(Guid aggregateId,
                                           Guid eventId,
                                           Guid operatorId,
                                           String name,
                                           Boolean requireCustomMerchantNumber,
                                           Boolean requireCustomTerminalNumber) : base(aggregateId, eventId)
        {
            this.EstateId = aggregateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.RequireCustomMerchantNumber = requireCustomMerchantNumber;
            this.RequireCustomTerminalNumber = requireCustomTerminalNumber;
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
        /// Gets a value indicating whether [require custom merchant number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom merchant number]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public Boolean RequireCustomMerchantNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [require custom terminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom terminal number]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public Boolean RequireCustomTerminalNumber { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <returns></returns>
        public static OperatorAddedToEstateEvent Create(Guid aggregateId,
                                                        Guid operatorId,
                                                        String name,
                                                        Boolean requireCustomMerchantNumber,
                                                        Boolean requireCustomTerminalNumber)
        {
            return new OperatorAddedToEstateEvent(aggregateId, Guid.NewGuid(), operatorId, name, requireCustomMerchantNumber, requireCustomTerminalNumber);
        }

        #endregion
    }
}
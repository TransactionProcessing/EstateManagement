namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    [JsonObject]
    public class ContractCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ContractCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        private ContractCreatedEvent(Guid aggregateId,
                                     Guid eventId,
                                     Guid estateId,
                                     Guid operatorId,
                                     String description) : base(aggregateId, eventId)
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Description = description;
        }

        #endregion

        #region Properties

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
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [JsonProperty]
        public Guid OperatorId { get; private set; }

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        [JsonProperty]
        public Guid ContractId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static ContractCreatedEvent Create(Guid aggregateId,
                                                  Guid estateId,
                                                  Guid operatorId,
                                                  String description)
        {
            return new ContractCreatedEvent(aggregateId, Guid.NewGuid(), estateId, operatorId, description);
        }

        #endregion
    }
}
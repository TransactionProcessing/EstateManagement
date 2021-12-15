namespace EstateManagement.BusinessLogic.Events
{
    using System;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent" />
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.IDomainEvent" />
    /// <seealso cref="System.IEquatable&lt;Shared.DomainDrivenDesign.EventSourcing.DomainEventRecord.DomainEvent&gt;" />
    /// <seealso cref="System.IEquatable&lt;EstateManagement.BusinessLogic.Events.CallbackReceivedEnrichedEvent&gt;" />
    public record CallbackReceivedEnrichedEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackReceivedEnrichedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public CallbackReceivedEnrichedEvent(Guid aggregateId) : base(aggregateId, Guid.NewGuid())
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the callback message.
        /// </summary>
        /// <value>
        /// The callback message.
        /// </value>
        [JsonProperty("callbackMessage")]
        public String CallbackMessage { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty("estateid")]
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the message format.
        /// </summary>
        /// <value>
        /// The message format.
        /// </value>
        [JsonProperty("messageFormat")]
        public Int32 MessageFormat { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        [JsonProperty("reference")]
        public String Reference { get; set; }

        /// <summary>
        /// Gets or sets the type string.
        /// </summary>
        /// <value>
        /// The type string.
        /// </value>
        [JsonProperty("typeString")]
        public String TypeString { get; set; }

        #endregion
    }
}
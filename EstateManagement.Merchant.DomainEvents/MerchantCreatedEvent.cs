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
    public class MerchantCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MerchantCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="dateCreated">The date created.</param>
        private MerchantCreatedEvent(Guid aggregateId,
                                     Guid eventId,
                                     Guid estateId,
                                     String merchantName,
                                     DateTime dateCreated) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.MerchantName = merchantName;
            this.DateCreated = dateCreated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [JsonProperty]
        public DateTime DateCreated { get; private set; }

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
        /// Gets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        [JsonProperty]
        public String MerchantName { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <returns></returns>
        public static MerchantCreatedEvent Create(Guid aggregateId,
                                                  Guid estateId,
                                                  String merchantName,
                                                  DateTime dateCreated)
        {
            return new MerchantCreatedEvent(aggregateId, Guid.NewGuid(), estateId, merchantName, dateCreated);
        }

        #endregion
    }
}
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
    public class SecurityUserAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserAddedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SecurityUserAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        private SecurityUserAddedEvent(Guid aggregateId,
                                       Guid eventId,
                                       Guid estateId,
                                       Guid securityUserId,
                                       String emailAddress) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.SecurityUserId = securityUserId;
            this.EmailAddress = emailAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [JsonProperty]
        public String EmailAddress { get; private set; }

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
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        [JsonProperty]
        public Guid SecurityUserId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public static SecurityUserAddedEvent Create(Guid aggregateId,
                                                    Guid estateId,
                                                    Guid securityUserId,
                                                    String emailAddress)
        {
            return new SecurityUserAddedEvent(aggregateId, Guid.NewGuid(), estateId, securityUserId, emailAddress);
        }

        #endregion
    }
}
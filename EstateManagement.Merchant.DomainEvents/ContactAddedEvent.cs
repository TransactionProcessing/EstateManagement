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
    public class ContactAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ContactAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        private ContactAddedEvent(Guid aggregateId,
                                  Guid eventId,
                                  Guid estateId,
                                  Guid contactId,
                                  String contactName,
                                  String contactPhoneNumber,
                                  String contactEmailAddress) : base(aggregateId, eventId)
        {
            this.MerchantId = aggregateId;
            this.EstateId = estateId;
            this.ContactId = contactId;
            this.ContactName = contactName;
            this.ContactPhoneNumber = contactPhoneNumber;
            this.ContactEmailAddress = contactEmailAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contact email address.
        /// </summary>
        /// <value>
        /// The contact email address.
        /// </value>
        [JsonProperty]
        public String ContactEmailAddress { get; private set; }

        /// <summary>
        /// Gets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        [JsonProperty]
        public Guid ContactId { get; private set; }

        /// <summary>
        /// Gets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        [JsonProperty]
        public String ContactName { get; private set; }

        /// <summary>
        /// Gets the contact phone number.
        /// </summary>
        /// <value>
        /// The contact phone number.
        /// </value>
        [JsonProperty]
        public String ContactPhoneNumber { get; private set; }

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

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        /// <returns></returns>
        public static ContactAddedEvent Create(Guid aggregateId,
                                               Guid estateId,
                                               Guid contactId,
                                               String contactName,
                                               String contactPhoneNumber,
                                               String contactEmailAddress)
        {
            return new ContactAddedEvent(aggregateId, Guid.NewGuid(), estateId, contactId, contactName, contactPhoneNumber, contactEmailAddress);
        }

        #endregion
    }
}
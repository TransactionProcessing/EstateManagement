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
    public record ContactAddedEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        public ContactAddedEvent(Guid aggregateId,
                                  Guid estateId,
                                  Guid contactId,
                                  String contactName,
                                  String contactPhoneNumber,
                                  String contactEmailAddress) : base(aggregateId, Guid.NewGuid())
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
        public String ContactEmailAddress { get; init; }

        /// <summary>
        /// Gets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public Guid ContactId { get; init; }

        /// <summary>
        /// Gets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        public String ContactName { get; init; }

        /// <summary>
        /// Gets the contact phone number.
        /// </summary>
        /// <value>
        /// The contact phone number.
        /// </value>
        public String ContactPhoneNumber { get; init; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; init; }

        #endregion
    }
}
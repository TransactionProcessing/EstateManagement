namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Merchant.DomainEvents;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventStore.Aggregate" />
    public class MerchantAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The addresses
        /// </summary>
        private readonly List<Address> Addresses;

        /// <summary>
        /// The contacts
        /// </summary>
        private readonly List<Contact> Contacts;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MerchantAggregate()
        {
            // Nothing here
            this.Addresses = new List<Address>();
            this.Contacts = new List<Contact>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private MerchantAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Addresses = new List<Address>();
            this.Contacts = new List<Contact>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCreated { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="addressLine3">The address line3.</param>
        /// <param name="addressLine4">The address line4.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="country">The country.</param>
        public void AddAddress(Guid addressId,
                               String addressLine1,
                               String addressLine2,
                               String addressLine3,
                               String addressLine4,
                               String town,
                               String region,
                               String postalCode,
                               String country)
        {
            this.EnsureMerchantHasBeenCreated();

            AddressAddedEvent addressAddedEvent = AddressAddedEvent.Create(this.AggregateId,
                                                                           this.EstateId,
                                                                           addressId,
                                                                           addressLine1,
                                                                           addressLine2,
                                                                           addressLine3,
                                                                           addressLine4,
                                                                           town,
                                                                           region,
                                                                           postalCode,
                                                                           country);

            this.ApplyAndPend(addressAddedEvent);
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        public void AddContact(Guid contactId,
                               String contactName,
                               String contactPhoneNumber,
                               String contactEmailAddress)
        {
            this.EnsureMerchantHasBeenCreated();

            ContactAddedEvent contactAddedEvent =
                ContactAddedEvent.Create(this.AggregateId, this.EstateId, contactId, contactName, contactPhoneNumber, contactEmailAddress);

            this.ApplyAndPend(contactAddedEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static MerchantAggregate Create(Guid aggregateId)
        {
            return new MerchantAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="dateCreated">The date created.</param>
        public void Create(Guid estateId,
                           String merchantName,
                           DateTime dateCreated)
        {
            // Ensure this merchant has not already been created
            this.EnsureMerchantNotAlreadyCreated();

            MerchantCreatedEvent merchantCreatedEvent = MerchantCreatedEvent.Create(this.AggregateId, estateId, merchantName, dateCreated);

            this.ApplyAndPend(merchantCreatedEvent);
        }

        /// <summary>
        /// Ensures the merchant not already created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Merchant {this.Name} is already created</exception>
        private void EnsureMerchantNotAlreadyCreated()
        {
            if (this.IsCreated)
            {
                throw new InvalidOperationException($"Merchant {this.Name} is already created");
            }
        }

        /// <summary>
        /// Ensures the merchant has been created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Merchant {this.Name} has not been created</exception>
        private void EnsureMerchantHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant has not been created");
            }
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       this.EstateId,
                       MerchantId = this.AggregateId
                   };
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="merchantCreatedEvent">The merchant created event.</param>
        private void PlayEvent(MerchantCreatedEvent merchantCreatedEvent)
        {
            this.IsCreated = true;
            this.EstateId = merchantCreatedEvent.EstateId;
            this.Name = merchantCreatedEvent.MerchantName;
            this.AggregateId = merchantCreatedEvent.AggregateId;
            this.DateCreated = merchantCreatedEvent.DateCreated;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="addressAddedEvent">The address added event.</param>
        private void PlayEvent(AddressAddedEvent addressAddedEvent)
        {
            Address address = Address.Create(addressAddedEvent.AddressId,
                                             addressAddedEvent.AddressLine1,
                                             addressAddedEvent.AddressLine2,
                                             addressAddedEvent.AddressLine3,
                                             addressAddedEvent.AddressLine4,
                                             addressAddedEvent.Town,
                                             addressAddedEvent.Region,
                                             addressAddedEvent.PostalCode,
                                             addressAddedEvent.Country);

            this.Addresses.Add(address);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="contactAddedEvent">The contact added event.</param>
        private void PlayEvent(ContactAddedEvent contactAddedEvent)
        {
            Contact contact = Contact.Create(contactAddedEvent.ContactId,
                                             contactAddedEvent.ContactName,
                                             contactAddedEvent.ContactPhoneNumber,
                                             contactAddedEvent.ContactEmailAddress);

            this.Contacts.Add(contact);
        }

        #endregion
    }
}
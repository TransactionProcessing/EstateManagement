namespace EstateManagement.MerchantAggregate
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class Contact
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        private Contact(Guid contactId,
                        String contactName,
                        String contactPhoneNumber,
                        String contactEmailAddress)
        {
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
        public String ContactEmailAddress { get; }

        /// <summary>
        /// Gets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public Guid ContactId { get; }

        /// <summary>
        /// Gets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        public String ContactName { get; }

        /// <summary>
        /// Gets the contact phone number.
        /// </summary>
        /// <value>
        /// The contact phone number.
        /// </value>
        public String ContactPhoneNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified contact identifier.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        /// <returns></returns>
        internal static Contact Create(Guid contactId,
                                       String contactName,
                                       String contactPhoneNumber,
                                       String contactEmailAddress)
        {
            return new Contact(contactId, contactName, contactPhoneNumber, contactEmailAddress);
        }

        #endregion
    }
}
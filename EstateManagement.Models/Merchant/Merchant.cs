namespace EstateManagement.Models.Merchant
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Merchant
    {
        #region Constructors

        public Merchant()
        {
            //this.Addresses = new List<Address>();

            //this.Contacts = new List<Contact>();

            //this.Operators = new List<Operator>();

            //this.SecurityUsers = new List<SecurityUser>();

            //this.Devices = new Dictionary<Guid, String>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        public List<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        public String MerchantName { get; set; }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        public List<Operator> Operators { get; set; }

        /// <summary>
        /// Gets or sets the security users.
        /// </summary>
        /// <value>
        /// The security users.
        /// </value>
        public List<SecurityUser> SecurityUsers { get; set; }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public Dictionary<Guid, String> Devices { get; set; }

        #endregion
    }
}
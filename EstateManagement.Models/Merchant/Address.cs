namespace EstateManagement.Models.Merchant
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Address
    {
        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>
        /// The address identifier.
        /// </value>
        public Guid AddressId { get; set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        public String AddressLine3 { get; set; }

        /// <summary>
        /// Gets the address line4.
        /// </summary>
        /// <value>
        /// The address line4.
        /// </value>
        public String AddressLine4 { get; set; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public String Country { get; set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; set; }
    }
}
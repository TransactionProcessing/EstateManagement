namespace EstateManagement.MerchantAggregate
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class Address
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
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
        private Address(Guid addressId,
                        String addressLine1,
                        String addressLine2,
                        String addressLine3,
                        String addressLine4,
                        String town,
                        String region,
                        String postalCode,
                        String country)
        {
            this.AddressId = addressId;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.AddressLine3 = addressLine3;
            this.AddressLine4 = addressLine4;
            this.Town = town;
            this.Region = region;
            this.PostalCode = postalCode;
            this.Country = country;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the address identifier.
        /// </summary>
        /// <value>
        /// The address identifier.
        /// </value>
        internal Guid AddressId { get; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        internal String AddressLine1 { get; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        internal String AddressLine2 { get; }

        /// <summary>
        /// Gets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        internal String AddressLine3 { get; }

        /// <summary>
        /// Gets the address line4.
        /// </summary>
        /// <value>
        /// The address line4.
        /// </value>
        internal String AddressLine4 { get; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        internal String Country { get; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        internal String PostalCode { get; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        internal String Region { get; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        internal String Town { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified address identifier.
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
        /// <returns></returns>
        internal static Address Create(Guid addressId,
                                       String addressLine1,
                                       String addressLine2,
                                       String addressLine3,
                                       String addressLine4,
                                       String town,
                                       String region,
                                       String postalCode,
                                       String country)
        {
            return new Address(addressId, addressLine1, addressLine2, addressLine3, addressLine4, town, region, postalCode, country);
        }

        #endregion
    }
}
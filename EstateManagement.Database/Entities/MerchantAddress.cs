namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantaddress")]
    public class MerchantAddress
    {
        #region Properties

        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>
        /// The address identifier.
        /// </value>
        public Guid AddressId { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String? AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        public String? AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the address line4.
        /// </summary>
        /// <value>
        /// The address line4.
        /// </value>
        public String? AddressLine4 { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public String? Country { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

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
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String? Region { get; set; }

        /// <summary>
        /// Gets or sets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String? Town { get; set; }

        #endregion
    }
}
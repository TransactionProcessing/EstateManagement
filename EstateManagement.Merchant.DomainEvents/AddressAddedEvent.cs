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
    public class AddressAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressAddedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public AddressAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="addressLine3">The address line3.</param>
        /// <param name="addressLine4">The address line4.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="country">The country.</param>
        private AddressAddedEvent(Guid aggregateId,
                                  Guid eventId,
                                  Guid estateId,
                                  Guid addressId,
                                  String addressLine1,
                                  String addressLine2,
                                  String addressLine3,
                                  String addressLine4,
                                  String town,
                                  String region,
                                  String postalCode,
                                  String country) : base(aggregateId, eventId)
        {
            this.MerchantId = aggregateId;
            this.EstateId = estateId;
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
        [JsonProperty]
        public Guid AddressId { get; private set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        [JsonProperty]
        public String AddressLine1 { get; private set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        [JsonProperty]
        public String AddressLine2 { get; private set; }

        /// <summary>
        /// Gets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        [JsonProperty]
        public String AddressLine3 { get; private set; }

        /// <summary>
        /// Gets the address line4.
        /// </summary>
        /// <value>
        /// The address line4.
        /// </value>
        [JsonProperty]
        public String AddressLine4 { get; private set; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [JsonProperty]
        public String Country { get; private set; }

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
        public Guid MerchantId { get; private set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        [JsonProperty]
        public String PostalCode { get; private set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [JsonProperty]
        public String Region { get; private set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        [JsonProperty]
        public String Town { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
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
        public static AddressAddedEvent Create(Guid aggregateId,
                                               Guid estateId,
                                               Guid addressId,
                                               String addressLine1,
                                               String addressLine2,
                                               String addressLine3,
                                               String addressLine4,
                                               String town,
                                               String region,
                                               String postalCode,
                                               String country)
        {
            return new AddressAddedEvent(aggregateId,
                                         Guid.NewGuid(),
                                         estateId,
                                         addressId,
                                         addressLine1,
                                         addressLine2,
                                         addressLine3,
                                         addressLine4,
                                         town,
                                         region,
                                         postalCode,
                                         country);
        }

        #endregion
    }
}
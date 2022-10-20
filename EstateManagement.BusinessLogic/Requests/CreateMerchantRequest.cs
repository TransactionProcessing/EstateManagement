namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models;

    public class CreateMerchantRequest : IRequest
    {
        #region Constructors

        private CreateMerchantRequest(Guid estateId,
                                      Guid merchantId,
                                      String name,
                                      Guid addressId,
                                      String addressLine1,
                                      String addressLine2,
                                      String addressLine3,
                                      String addressLine4,
                                      String town,
                                      String region,
                                      String postalCode,
                                      String country,
                                      Guid contactId,
                                      String contactName,
                                      String contactPhoneNumber,
                                      String contactEmailAddress,
                                      SettlementSchedule settlementSchedule,
                                      DateTime createDateTime) {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.Name = name;
            this.AddressId = addressId;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.AddressLine3 = addressLine3;
            this.AddressLine4 = addressLine4;
            this.Town = town;
            this.Region = region;
            this.PostalCode = postalCode;
            this.Country = country;
            this.ContactId = contactId;
            this.ContactName = contactName;
            this.ContactPhoneNumber = contactPhoneNumber;
            this.ContactEmailAddress = contactEmailAddress;
            this.SettlementSchedule = settlementSchedule;
            this.CreateDateTime = createDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the address identifier.
        /// </summary>
        /// <value>
        /// The address identifier.
        /// </value>
        public Guid AddressId { get; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; }

        /// <summary>
        /// Gets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        public String AddressLine3 { get; }

        /// <summary>
        /// Gets the address line4.
        /// </summary>
        /// <value>
        /// The address line4.
        /// </value>
        public String AddressLine4 { get; }

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

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public String Country { get; }

        public DateTime CreateDateTime { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; }

        public SettlementSchedule SettlementSchedule { get; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; }

        #endregion

        #region Methods

        public static CreateMerchantRequest Create(Guid estateId,
                                                   Guid merchantId,
                                                   String name,
                                                   String addressLine1,
                                                   String addressLine2,
                                                   String addressLine3,
                                                   String addressLine4,
                                                   String town,
                                                   String region,
                                                   String postalCode,
                                                   String country,
                                                   String contactName,
                                                   String contactPhoneNumber,
                                                   String contactEmailAddress,
                                                   SettlementSchedule settlementSchedule,
                                                   DateTime? createDateTime) {
            return new CreateMerchantRequest(estateId,
                                             merchantId,
                                             name,
                                             Guid.NewGuid(),
                                             addressLine1,
                                             addressLine2,
                                             addressLine3,
                                             addressLine4,
                                             town,
                                             region,
                                             postalCode,
                                             country,
                                             Guid.NewGuid(),
                                             contactName,
                                             contactPhoneNumber,
                                             contactEmailAddress,
                                             settlementSchedule,
                                             createDateTime.GetValueOrDefault(DateTime.Now));
        }

        #endregion
    }
}
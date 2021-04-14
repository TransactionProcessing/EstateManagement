namespace EstateManagement.DataTransferObjects.Responses
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MerchantResponse
    {
        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        [JsonProperty("addresses")]
        public List<AddressResponse> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        [JsonProperty("contacts")]
        public List<ContactResponse> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        [JsonProperty("devices")]
        public Dictionary<Guid, String> Devices { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty("estate_id")]
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        [JsonProperty("merchant_name")]
        public String MerchantName { get; set; }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        [JsonProperty("operators")]
        public List<MerchantOperatorResponse> Operators { get; set; }

        /// <summary>
        /// Gets or sets the available balance.
        /// </summary>
        /// <value>
        /// The available balance.
        /// </value>
        [JsonProperty("available_balance")]
        public Decimal AvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        [JsonProperty("balance")]
        public Decimal Balance { get; set; }

        #endregion
    }
}
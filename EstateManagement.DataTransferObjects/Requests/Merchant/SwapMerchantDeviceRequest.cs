namespace EstateManagement.DataTransferObjects.Requests.Merchant
{
    using System;
    using Newtonsoft.Json;

    public class SwapMerchantDeviceRequest
    {
        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [JsonProperty("new_device_identifier")]
        public String NewDeviceIdentifier { get; set; }
    }
}
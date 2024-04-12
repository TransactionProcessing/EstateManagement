namespace EstateManagement.DataTransferObjects.Requests.Merchant
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public class AddMerchantDeviceRequest
    {
        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [JsonProperty("device_identifier")]
        public String DeviceIdentifier { get; set; }
    }
}

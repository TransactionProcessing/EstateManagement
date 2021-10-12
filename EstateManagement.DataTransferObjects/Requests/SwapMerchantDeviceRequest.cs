using System;
using Newtonsoft.Json;

namespace EstateManagement.DataTransferObjects.Requests
{
    public class SwapMerchantDeviceRequest
    {
        [JsonProperty("original_device_identifier")]
        public String OriginalDeviceIdentifier { get; set; }

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
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.DataTransferObjects.Requests
{
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

﻿using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace EstateManagement.DataTransferObjects.Responses
{
    [ExcludeFromCodeCoverage]
    public class SwapMerchantDeviceResponse
    {
        #region Properties

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
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [JsonProperty("device_id")]
        public Guid DeviceId { get; set; }

        #endregion
    }
}
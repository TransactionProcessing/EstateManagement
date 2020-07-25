﻿namespace EstateManagement.DataTransferObjects.Requests
{
    using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class AddTransactionFeeForProductToContractRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        [JsonProperty("calculation_type")]
        public CalculationType CalculationType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public Decimal Value { get; set; }

        #endregion
    }
}
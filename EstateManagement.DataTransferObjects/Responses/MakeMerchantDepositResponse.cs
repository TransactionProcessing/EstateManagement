﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.DataTransferObjects.Responses
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public class MakeMerchantDepositResponse
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
        [JsonProperty("deposit_id")]
        public Guid DepositId { get; set; }

        #endregion
    }

    [ExcludeFromCodeCoverage]
    public class MakeMerchantWithdrawalResponse
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
        /// Gets or sets the withdrawal identifier.
        /// </summary>
        /// <value>
        /// The withdrawal identifier.
        /// </value>
        [JsonProperty("withdrawal_id")]
        public Guid WithdrawalId { get; set; }

        #endregion
    }
}

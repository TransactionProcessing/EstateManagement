namespace EstateManagement.DataTransferObjects.Requests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MakeMerchantDepositRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [JsonProperty("amount")]
        public Decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the deposit date time.
        /// </summary>
        /// <value>
        /// The deposit date time.
        /// </value>
        [JsonProperty("deposit_date_time")]
        public DateTime DepositDateTime { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        [JsonProperty("reference")]
        public String Reference { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [JsonProperty("source")]
        public MerchantDepositSource Source { get; set; }

        #endregion
    }
}
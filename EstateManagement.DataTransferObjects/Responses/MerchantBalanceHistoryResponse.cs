using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.DataTransferObjects.Responses
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class MerchantBalanceHistoryResponse
    {
        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        [JsonProperty("event_id")]
        public Guid EventId { get; set; }

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
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        [JsonProperty("balance")]
        public Decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the change amount.
        /// </summary>
        /// <value>
        /// The change amount.
        /// </value>
        [JsonProperty("change_amount")]
        public Decimal ChangeAmount { get; set; }

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>
        /// The type of the entry.
        /// </value>
        [JsonProperty("entry_type")]
        public string EntryType { get; set; }

        /// <summary>
        /// Gets or sets the in.
        /// </summary>
        /// <value>
        /// The in.
        /// </value>
        [JsonProperty("in")]
        public Decimal? In { get; set; }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>
        /// The out.
        /// </value>
        [JsonProperty("out")]
        public Decimal? Out { get; set; }

        /// <summary>
        /// Gets or sets the entry date time.
        /// </summary>
        /// <value>
        /// The entry date time.
        /// </value>
        [JsonProperty("entry_date_time")]
        public DateTime EntryDateTime { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [JsonProperty("transaction_id")]
        public Guid TransactionId { get; set; }
    }
}

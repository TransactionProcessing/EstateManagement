using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.DataTransferObjects.Responses
{
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
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public Decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the change amount.
        /// </summary>
        /// <value>
        /// The change amount.
        /// </value>
        public Decimal ChangeAmount { get; set; }

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>
        /// The type of the entry.
        /// </value>
        public string EntryType { get; set; }

        /// <summary>
        /// Gets or sets the in.
        /// </summary>
        /// <value>
        /// The in.
        /// </value>
        public Decimal? In { get; set; }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>
        /// The out.
        /// </value>
        public Decimal? Out { get; set; }

        /// <summary>
        /// Gets or sets the entry date time.
        /// </summary>
        /// <value>
        /// The entry date time.
        /// </value>
        public DateTime EntryDateTime { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; set; }
    }
}

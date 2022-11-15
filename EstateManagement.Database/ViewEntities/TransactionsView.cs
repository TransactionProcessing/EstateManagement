namespace EstateManagement.Database.ViewEntities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("uvwTransactionsView")]
    public class TransactionsView
    {
        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date time.
        /// </summary>
        /// <value>
        /// The transaction date time.
        /// </value>
        public DateTime TransactionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>
        /// The transaction date.
        /// </value>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the day of week.
        /// </summary>
        /// <value>
        /// The day of week.
        /// </value>
        public String DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the week number.
        /// </summary>
        /// <value>
        /// The week number.
        /// </value>
        public Int32 WeekNumber { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public String Month { get; set; }

        /// <summary>
        /// Gets or sets the month number.
        /// </summary>
        /// <value>
        /// The month number.
        /// </value>
        public Int32 MonthNumber { get; set; }

        /// <summary>
        /// Gets or sets the year number.
        /// </summary>
        /// <value>
        /// The year number.
        /// </value>
        public Int32 YearNumber { get; set; }

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
        /// Gets or sets a value indicating whether this instance is authorised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authorised; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsAuthorised { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCompleted { get; set; }
        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>
        /// The type of the transaction.
        /// </value>
        public String TransactionType { get; set; }
        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        public String ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public Decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public String OperatorIdentifier { get; set; }
    }
}

namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("transactionfee")]
    public class TransactionFee
    {
        #region Properties

        /// <summary>
        /// Gets or sets the calculated value.
        /// </summary>
        /// <value>
        /// The calculated value.
        /// </value>
        public Decimal CalculatedValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        public Int32 CalculationType { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the fee identifier.
        /// </summary>
        /// <value>
        /// The fee identifier.
        /// </value>
        public Guid FeeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the fee.
        /// </summary>
        /// <value>
        /// The type of the fee.
        /// </value>
        public Int32 FeeType { get; set; }

        /// <summary>
        /// Gets or sets the fee value.
        /// </summary>
        /// <value>
        /// The fee value.
        /// </value>
        public Decimal FeeValue { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; set; }

        #endregion
    }
}
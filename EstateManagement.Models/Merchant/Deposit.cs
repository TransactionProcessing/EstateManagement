namespace EstateManagement.Models.Merchant
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Deposit
    {
        #region Properties

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public Decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the deposit date time.
        /// </summary>
        /// <value>
        /// The deposit date time.
        /// </value>
        public DateTime DepositDateTime { get; set; }

        /// <summary>
        /// Gets or sets the deposit identifier.
        /// </summary>
        /// <value>
        /// The deposit identifier.
        /// </value>
        public Guid DepositId { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public String Reference { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public MerchantDepositSource Source { get; set; }

        #endregion
    }
}
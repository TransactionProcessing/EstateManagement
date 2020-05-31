namespace EstateManagement.MerchantAggregate
{
    using System;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    internal class Deposit
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Deposit" /> class.
        /// </summary>
        /// <param name="depositId">The deposit identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        private Deposit(Guid depositId,
                        MerchantDepositSource source,
                        String reference,
                        DateTime depositDateTime,
                        Decimal amount)
        {
            this.DepositId = depositId;
            this.Source = source;
            this.Reference = reference;
            this.DepositDateTime = depositDateTime;
            this.Amount = amount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        internal Decimal Amount { get; }

        /// <summary>
        /// Gets the deposit date time.
        /// </summary>
        /// <value>
        /// The deposit date time.
        /// </value>
        internal DateTime DepositDateTime { get; }

        /// <summary>
        /// Gets the deposit identifier.
        /// </summary>
        /// <value>
        /// The deposit identifier.
        /// </value>
        internal Guid DepositId { get; }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        internal MerchantDepositSource Source { get; }

        internal String Reference { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified deposit identifier.
        /// </summary>
        /// <param name="depositId">The deposit identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        internal static Deposit Create(Guid depositId,
                                       MerchantDepositSource source,
                                       String reference,
                                       DateTime depositDateTime,
                                       Decimal amount)
        {
            return new Deposit(depositId, source, reference, depositDateTime, amount);
        }

        #endregion
    }
}
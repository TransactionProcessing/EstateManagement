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

        private Deposit(Guid depositId,
                        MerchantDepositSource source,
                        String reference,
                        DateTime depositDateTime,
                        Decimal amount) {
            this.DepositId = depositId;
            this.Source = source;
            this.Reference = reference;
            this.DepositDateTime = depositDateTime;
            this.Amount = amount;
        }

        #endregion

        #region Properties

        internal Decimal Amount { get; }

        internal DateTime DepositDateTime { get; }

        internal Guid DepositId { get; }

        internal String Reference { get; }

        internal MerchantDepositSource Source { get; }

        #endregion

        #region Methods

        internal static Deposit Create(Guid depositId,
                                       MerchantDepositSource source,
                                       String reference,
                                       DateTime depositDateTime,
                                       Decimal amount) {
            return new Deposit(depositId, source, reference, depositDateTime, amount);
        }

        #endregion
    }
}
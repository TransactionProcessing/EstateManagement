namespace EstateManagement.MerchantDepositListAggregate
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class Withdrawal
    {
        #region Constructors

        private Withdrawal(Guid withdrawalId,
                           DateTime withdrawalDateTime,
                           Decimal amount) {
            this.WithdrawalId = withdrawalId;
            this.WithdrawalDateTime = withdrawalDateTime;
            this.Amount = amount;
        }

        #endregion

        #region Properties

        public DateTime WithdrawalDateTime { get; }

        internal Decimal Amount { get; }

        internal Guid WithdrawalId { get; }

        #endregion

        #region Methods

        internal static Withdrawal Create(Guid withdrawalId,
                                          DateTime withdrawalDateTime,
                                          Decimal amount) {
            return new Withdrawal(withdrawalId, withdrawalDateTime, amount);
        }

        #endregion
    }
}
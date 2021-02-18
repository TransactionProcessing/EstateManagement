namespace EstateManagement.Factories
{
    using System;
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Models.Contract;
    using Models.Estate;
    using Models.Merchant;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        List<ContractResponse> ConvertFrom(List<Contract> contract);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        ContractResponse ConvertFrom(Contract contract);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <returns></returns>
        EstateResponse ConvertFrom(Estate estate);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <param name="merchantBalance">The merchant balance.</param>
        /// <returns></returns>
        MerchantResponse ConvertFrom(Merchant merchant,
                                     MerchantBalance merchantBalance = null);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchantBalance">The merchant balance.</param>
        /// <returns></returns>
        MerchantBalanceResponse ConvertFrom(MerchantBalance merchantBalance);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchantBalanceHistories">The merchant balance histories.</param>
        /// <returns></returns>
        List<MerchantBalanceHistoryResponse> ConvertFrom(List<MerchantBalanceHistory> merchantBalanceHistories);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchants">The merchants.</param>
        /// <returns></returns>
        List<MerchantResponse> ConvertFrom(List<Merchant> merchants);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="transactionFees">The transaction fees.</param>
        /// <returns></returns>
        List<ContractProductTransactionFee> ConvertFrom(List<TransactionFee> transactionFees);

        #endregion
    }
}
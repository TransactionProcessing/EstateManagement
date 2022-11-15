namespace EstateManagement.Factories
{
    using System;
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Models;
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
        /// <returns></returns>
        MerchantResponse ConvertFrom(Merchant merchant);
        
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

        SettlementFeeResponse ConvertFrom(SettlementFeeModel model);

        List<SettlementFeeResponse> ConvertFrom(List<SettlementFeeModel> model);

        SettlementResponse ConvertFrom(SettlementModel model);

        List<SettlementResponse> ConvertFrom(List<SettlementModel> model);

        #endregion
    }
}
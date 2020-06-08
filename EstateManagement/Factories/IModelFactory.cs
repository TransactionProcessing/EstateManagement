namespace EstateManagement.Factories
{
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
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
        /// <param name="merchant">The merchant.</param>
        /// <returns></returns>
        MerchantBalanceResponse ConvertFrom(MerchantBalance merchantBalance);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchants">The merchants.</param>
        /// <returns></returns>
        List<MerchantResponse> ConvertFrom(List<Merchant> merchants);

        #endregion
    }
}
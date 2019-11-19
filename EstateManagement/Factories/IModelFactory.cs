namespace EstateManagement.Factories
{
    using DataTransferObjects.Responses;
    using Models;
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

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Models.Estate;
    using Models.Merchant;

    public interface IEstateManagementManager
    {
        #region Methods

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Estate> GetEstate(Guid estateId,
                               CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Merchant> GetMerchant(Guid estateId, Guid merchantId,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchant balance.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<MerchantBalance> GetMerchantBalance(Guid estateId, Guid merchantId,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Merchant>> GetMerchants(Guid estateId, CancellationToken cancellationToken);
        #endregion
    }
}
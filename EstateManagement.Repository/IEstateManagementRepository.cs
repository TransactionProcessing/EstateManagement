namespace EstateManagement.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Estate;
    using Models.Merchant;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    public interface IEstateManagementRepository
    {
        #region Methods

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="includeProducts">if set to <c>true</c> [include products].</param>
        /// <param name="includeProductsWithFees">if set to <c>true</c> [include products with fees].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Contract> GetContract(Guid estateId,
                                   Guid contractId,
                                   Boolean includeProducts,
                                   Boolean includeProductsWithFees,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Estate> GetEstate(Guid estateId,
                                    CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Merchant>> GetMerchants(Guid estateId,
                                               CancellationToken cancellationToken);

        #endregion
    }
}
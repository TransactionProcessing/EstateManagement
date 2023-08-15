﻿namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Models.Contract;
    using Models.Estate;
    using Models.File;
    using Models.Merchant;

    public interface IEstateManagementManager
    {
        #region Methods

        /// <summary>
        /// Gets the merchant contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Contract>> GetMerchantContracts(Guid estateId,
                                            Guid merchantId,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Contract>> GetContracts(Guid estateId,
                                          CancellationToken cancellationToken);

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
        /// Gets the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Merchant> GetMerchant(Guid estateId, Guid merchantId,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Merchant>> GetMerchants(Guid estateId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the transaction fees for product.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<TransactionFee>> GetTransactionFeesForProduct(Guid estateId,
                                                                Guid merchantId,
                                                                Guid contractId,
                                                                Guid productId,
                                                                CancellationToken cancellationToken);

        Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken);

        #endregion
    }
}
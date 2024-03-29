﻿namespace EstateManagement.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Contract;
    using Models.Estate;
    using Models.File;
    using Models.Merchant;
    using Models.MerchantStatement;
    using Contract = Models.Contract.Contract;

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
        /// Gets the contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Contract>> GetContracts(Guid estateId,
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
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<Merchant>> GetMerchants(Guid estateId,
                                          CancellationToken cancellationToken);

        Task<Merchant> GetMerchant(Guid estateId,
                                         Guid merchantId,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchant from reference.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Merchant> GetMerchantFromReference(Guid estateId, 
                                                String reference,
                                                CancellationToken cancellationToken);

        Task<StatementHeader> GetStatement(Guid estateId,
                                           Guid merchantStatementId,
                                           CancellationToken cancellationToken);

        Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken);

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Contract;
    using Models.Estate;
    using Models.File;
    using Models.Merchant;
    using Contract = Models.Contract.Contract;
    using Operator = Models.Operator.Operator;

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

        Task<Contract> GetContract(Guid estateId,
                                  Guid contractId,
                                  CancellationToken cancellationToken);

        Task<Estate> GetEstate(Guid estateId,
                               CancellationToken cancellationToken);

        Task<List<Estate>> GetEstates(Guid estateId,
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
        Task<List<ContractProductTransactionFee>> GetTransactionFeesForProduct(Guid estateId,
                                                                Guid merchantId,
                                                                Guid contractId,
                                                                Guid productId,
                                                                CancellationToken cancellationToken);

        Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken);

        Task<Operator> GetOperator(Guid estateId,Guid operatorId,
                               CancellationToken cancellationToken);

        Task<List<Operator>> GetOperators(Guid estateId, CancellationToken cancellationToken);

        #endregion
    }
}
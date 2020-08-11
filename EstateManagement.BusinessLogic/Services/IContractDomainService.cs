namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    public interface IContractDomainService
    {
        #region Methods

        /// <summary>
        /// Adds the product to contract.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddProductToContract(Guid productId,
                                  Guid contractId,
                                  String productName,
                                  String displayText,
                                  Decimal? value,
                                  CancellationToken cancellationToken);

        /// <summary>
        /// Adds the transaction fee for product to contract.
        /// </summary>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="feeType">Type of the fee.</param>
        /// <param name="value">The value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddTransactionFeeForProductToContract(Guid transactionFeeId,
                                                   Guid contractId,
                                                   Guid productId,
                                                   String description,
                                                   CalculationType calculationType,
                                                   FeeType feeType,
                                                   Decimal value,
                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Disables the transaction fee for product.
        /// </summary>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task DisableTransactionFeeForProduct(Guid transactionFeeId,
                                                   Guid contractId,
                                                   Guid productId,
                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Creates the contract.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateContract(Guid contractId,
                            Guid estateId,
                            Guid operatorId,
                            String description,
                            CancellationToken cancellationToken);

        #endregion
    }
}
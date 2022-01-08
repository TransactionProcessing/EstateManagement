namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.String}" />
    public class DisableTransactionFeeForProductRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisableTransactionFeeForProductRequest" /> class.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        private DisableTransactionFeeForProductRequest(Guid contractId,
                                                       Guid estateId,
                                                       Guid productId,
                                                       Guid transactionFeeId)
        {
            this.ContractId = contractId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public Guid ProductId { get; }

        /// <summary>
        /// Gets the transaction fee identifier.
        /// </summary>
        /// <value>
        /// The transaction fee identifier.
        /// </value>
        public Guid TransactionFeeId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified contract identifier.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <returns></returns>
        public static DisableTransactionFeeForProductRequest Create(Guid contractId,
                                                                    Guid estateId,
                                                                    Guid productId,
                                                                    Guid transactionFeeId)
        {
            return new DisableTransactionFeeForProductRequest(contractId, estateId, productId, transactionFeeId);
        }

        #endregion
    }
}
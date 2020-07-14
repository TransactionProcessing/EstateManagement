namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.String}" />
    public class AddTransactionFeeForProductToContractRequest : IRequest<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTransactionFeeForProductToContractRequest" /> class.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="value">The value.</param>
        private AddTransactionFeeForProductToContractRequest(Guid contractId,
                                                             Guid estateId,
                                                             Guid productId,
                                                             Guid transactionFeeId,
                                                             String description,
                                                             CalculationType calculationType,
                                                             Decimal value)
        {
            this.ContractId = contractId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
            this.Description = description;
            this.CalculationType = calculationType;
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the calculation.
        /// </summary>
        /// <value>
        /// The type of the calculation.
        /// </value>
        public CalculationType CalculationType { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal Value { get; }

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; }

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
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static AddTransactionFeeForProductToContractRequest Create(Guid contractId,
                                                                          Guid estateId,
                                                                          Guid productId,
                                                                          Guid transactionFeeId,
                                                                          String description,
                                                                          CalculationType calculationType,
                                                                          Decimal value)
        {
            return new AddTransactionFeeForProductToContractRequest(contractId, estateId, productId, transactionFeeId, description, calculationType, value);
        }

        #endregion
    }
}
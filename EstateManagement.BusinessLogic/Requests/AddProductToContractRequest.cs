namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.String}" />
    public class AddProductToContractRequest : IRequest<Unit>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductToContractRequest" /> class.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        private AddProductToContractRequest(Guid contractId,
                                            Guid estateId,
                                            Guid productId,
                                            String productName,
                                            String displayText,
                                            Decimal? value)
        {
            this.ContractId = contractId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.ProductName = productName;
            this.DisplayText = displayText;
            this.Value = value;
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
        /// Gets the display text.
        /// </summary>
        /// <value>
        /// The display text.
        /// </value>
        public String DisplayText { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public String ProductName { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal? Value { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified contract identifier.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static AddProductToContractRequest Create(Guid contractId,
                                                         Guid estateId,
                                                         Guid productId,
                                                         String productName,
                                                         String displayText,
                                                         Decimal? value)
        {
            return new AddProductToContractRequest(contractId, estateId, productId, productName, displayText, value);
        }

        #endregion
    }
}
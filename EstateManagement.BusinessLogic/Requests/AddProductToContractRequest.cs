namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models.Contract;

    public class AddProductToContractRequest : IRequest<Unit>
    {
        #region Constructors

        private AddProductToContractRequest(Guid contractId,
                                            Guid estateId,
                                            Guid productId,
                                            String productName,
                                            String displayText,
                                            Decimal? value,
                                            ProductType productType) {
            this.ContractId = contractId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.ProductName = productName;
            this.DisplayText = displayText;
            this.Value = value;
            this.ProductType = productType;
        }

        #endregion

        #region Properties

        public Guid ContractId { get; }

        public String DisplayText { get; }

        public Guid EstateId { get; }

        public Guid ProductId { get; }

        public String ProductName { get; }

        public ProductType ProductType { get; }

        public Decimal? Value { get; }

        #endregion

        #region Methods

        public static AddProductToContractRequest Create(Guid contractId,
                                                         Guid estateId,
                                                         Guid productId,
                                                         String productName,
                                                         String displayText,
                                                         Decimal? value,
                                                         ProductType productType) {
            return new AddProductToContractRequest(contractId, estateId, productId, productName, displayText, value, productType);
        }

        #endregion
    }
}
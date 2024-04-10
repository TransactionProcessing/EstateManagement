using System;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    public class AddMerchantContractRequest : IRequest
    {
        #region Constructors

        private AddMerchantContractRequest(Guid estateId,
                                           Guid merchantId,
                                           Guid contractId)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.ContractId = contractId;
        }

        #endregion

        #region Properties

        public Guid EstateId { get; }

        public Guid MerchantId { get; }
        
        public Guid ContractId{ get; }

        #endregion

        #region Methods

        public static AddMerchantContractRequest Create(Guid estateId,
                                                        Guid merchantId,
                                                        Guid contractId)
        {
            return new AddMerchantContractRequest(estateId, merchantId, contractId);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Models.Contract;

    [ExcludeFromCodeCoverage]
    public class MerchantQueries
    {
        public record GetMerchantQuery(Guid EstateId, Guid MerchantId) : IRequest<Models.Merchant.Merchant>;

        public record GetMerchantContractsQuery(Guid EstateId, Guid MerchantId) : IRequest<List<Models.Contract.Contract>>;

        public record GetMerchantsQuery(Guid EstateId) : IRequest<List<Models.Merchant.Merchant>>;

        public record GetTransactionFeesForProductQuery(Guid EstateId, Guid MerchantId, Guid ContractId, Guid ProductId) : IRequest<List<ContractProductTransactionFee>>;

    }
}


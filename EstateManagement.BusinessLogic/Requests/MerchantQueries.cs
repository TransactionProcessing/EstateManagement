using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Models.Contract;

    [ExcludeFromCodeCoverage]
    public class MerchantQueries
    {
        public record GetMerchantQuery(Guid EstateId, Guid MerchantId) : IRequest<Result<Models.Merchant.Merchant>>;

        public record GetMerchantContractsQuery(Guid EstateId, Guid MerchantId) : IRequest<Result<List<Models.Contract.Contract>>>;

        public record GetMerchantsQuery(Guid EstateId) : IRequest<Result<List<Models.Merchant.Merchant>>>;

        public record GetTransactionFeesForProductQuery(Guid EstateId, Guid MerchantId, Guid ContractId, Guid ProductId) : IRequest<Result<List<ContractProductTransactionFee>>>;

    }
}


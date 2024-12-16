using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.Models;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

[ExcludeFromCodeCoverage]
public record SettlementQueries {
    public record GetSettlementQuery(Guid EstateId, Guid MerchantId, Guid SettlementId)
        : IRequest<Result<SettlementModel>>;

    public record GetSettlementsQuery(Guid EstateId,Guid? MerchantId,String StartDate, String EndDate)
        : IRequest<Result<List<SettlementModel>>>;
}
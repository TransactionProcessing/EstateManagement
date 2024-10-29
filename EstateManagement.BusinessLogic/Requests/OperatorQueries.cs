using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.Models;
using MediatR;
using Models.Operator;

[ExcludeFromCodeCoverage]
public class OperatorQueries{
    public record GetOperatorQuery(Guid EstateId, Guid OperatorId) : IRequest<Result<Operator>>;

    public record GetOperatorsQuery(Guid EstateId) : IRequest<Result<List<Operator>>>;
}

public record SettlementQueries {
    public record GetSettlementQuery(Guid EstateId, Guid MerchantId, Guid SettlementId)
        : IRequest<Result<SettlementModel>>;

    public record GetSettlementsQuery(Guid EstateId,Guid? MerchantId,String StartDate, String EndDate)
        : IRequest<Result<List<SettlementModel>>>;
}
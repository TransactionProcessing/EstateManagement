using System;
using System.Collections.Generic;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

public record ContractQueries {
    public record GetContractQuery(Guid EstateId, Guid ContractId) : IRequest<Result<Models.Contract.Contract>>;
    public record GetContractsQuery(Guid EstateId) : IRequest<Result<List<Models.Contract.Contract>>>;
}
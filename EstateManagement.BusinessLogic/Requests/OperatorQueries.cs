﻿using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Models.Operator;

[ExcludeFromCodeCoverage]
public class OperatorQueries{
    public record GetOperatorQuery(Guid EstateId, Guid OperatorId) : IRequest<Result<Operator>>;

    public record GetOperatorsQuery(Guid EstateId) : IRequest<Result<List<Operator>>>;
}
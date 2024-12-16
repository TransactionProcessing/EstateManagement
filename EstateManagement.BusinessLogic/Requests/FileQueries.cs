using System;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.Models.File;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

[ExcludeFromCodeCoverage]
public record FileQueries {
    public record GetFileQuery(Guid EstateId, Guid FileId) : IRequest<Result<File>>;

}
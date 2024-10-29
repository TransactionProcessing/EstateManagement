using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagement.DataTransferObjects.Responses.File;
using EstateManagement.Models.File;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests
{
    using DataTransferObjects.Requests.Estate;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public class EstateCommands{
        public record CreateEstateCommand(CreateEstateRequest RequestDto) : IRequest<Result>;

        public record CreateEstateUserCommand(Guid EstateId, CreateEstateUserRequest RequestDto) : IRequest<Result>;

        public record AddOperatorToEstateCommand(Guid EstateId, AssignOperatorRequest RequestDto) : IRequest<Result>;

        public record RemoveOperatorFromEstateCommand(Guid EstateId, Guid OperatorId) : IRequest<Result>;
    }

    public record FileQueries {
        public record GetFileQuery(Guid EstateId, Guid FileId) : IRequest<Result<File>>;

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests
{
    using DataTransferObjects.Requests.Estate;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public class EstateCommands{
        public record CreateEstateCommand(CreateEstateRequest RequestDto) : IRequest;

        public record CreateEstateUserCommand(Guid EstateId, CreateEstateUserRequest RequestDto) : IRequest;

        public record AddOperatorToEstateCommand(Guid EstateId, AssignOperatorRequest RequestDto) : IRequest;
    }
}

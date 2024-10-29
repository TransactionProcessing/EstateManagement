using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests
{
    using DataTransferObjects.Requests.Operator;
    using MediatR;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class OperatorCommands{
        public record CreateOperatorCommand(Guid EstateId, CreateOperatorRequest RequestDto) : IRequest<Result>;

        public record UpdateOperatorCommand(Guid EstateId, Guid OperatorId, UpdateOperatorRequest RequestDto) : IRequest<Result>;
    }
}

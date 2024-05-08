using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests
{
    using DataTransferObjects.Requests.Operator;
    using MediatR;
    using Models.Operator;

    public class OperatorCommands{
        public record CreateOperatorCommand(CreateOperatorRequest RequestDto) : IRequest;
    }

    public class OperatorQueries{
        public record GetOperatorQuery(Guid EstateId, Guid OperatorId) : IRequest<Operator>;

        public record GetOperatorsQuery(Guid EstateId) : IRequest<List<Operator>>;
    }
}

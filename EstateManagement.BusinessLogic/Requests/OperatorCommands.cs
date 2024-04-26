using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests
{
    using DataTransferObjects.Requests.Operator;
    using MediatR;

    public class OperatorCommands{
        public record CreateOperatorCommand(CreateOperatorRequest RequestDto) : IRequest;
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using Requests;
    using Services;

    public class OperatorRequestHandler : IRequestHandler<OperatorCommands.CreateOperatorCommand>
    {
        private readonly IOperatorDomainService OperatorDomainService;

        public OperatorRequestHandler(IOperatorDomainService operatorDomainService){
            this.OperatorDomainService = operatorDomainService;
        }
        public async Task Handle(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken){
            await this.OperatorDomainService.CreateOperator(command, cancellationToken);
        }
    }
}

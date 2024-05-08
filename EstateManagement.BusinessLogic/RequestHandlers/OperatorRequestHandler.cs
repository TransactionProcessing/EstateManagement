using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using Manger;
    using Models.Operator;
    using Requests;
    using Services;

    public class OperatorRequestHandler : IRequestHandler<OperatorCommands.CreateOperatorCommand>,
                                          IRequestHandler<OperatorQueries.GetOperatorQuery, Operator>,
                                          IRequestHandler<OperatorQueries.GetOperatorsQuery, List<Operator>>
    {
        private readonly IOperatorDomainService OperatorDomainService;

        private readonly IEstateManagementManager EstateManagementManager;

        public OperatorRequestHandler(IOperatorDomainService operatorDomainService, IEstateManagementManager estateManagementManager){
            this.OperatorDomainService = operatorDomainService;
            this.EstateManagementManager = estateManagementManager;
        }
        public async Task Handle(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken){
            await this.OperatorDomainService.CreateOperator(command, cancellationToken);
        }

        public async Task<Operator> Handle(OperatorQueries.GetOperatorQuery query, CancellationToken cancellationToken){
            Operator @operator = await this.EstateManagementManager.GetOperator(query.EstateId, query.OperatorId, cancellationToken);
            return @operator;
        }

        public async Task<List<Operator>> Handle(OperatorQueries.GetOperatorsQuery query, CancellationToken cancellationToken)
        {
            List<Operator> operators = await this.EstateManagementManager.GetOperators(query.EstateId, cancellationToken);
            return operators;
        }
    }
}

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleResults;

namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using Manger;
    using Models.Operator;
    using Requests;
    using Services;

    public class OperatorRequestHandler : IRequestHandler<OperatorCommands.CreateOperatorCommand,Result>,
                                          IRequestHandler<OperatorQueries.GetOperatorQuery, Result<Operator>>,
                                          IRequestHandler<OperatorQueries.GetOperatorsQuery, Result<List<Operator>>>,
                                          IRequestHandler<OperatorCommands.UpdateOperatorCommand, Result>
    {
        private readonly IOperatorDomainService OperatorDomainService;

        private readonly IEstateManagementManager EstateManagementManager;

        public OperatorRequestHandler(IOperatorDomainService operatorDomainService, IEstateManagementManager estateManagementManager){
            this.OperatorDomainService = operatorDomainService;
            this.EstateManagementManager = estateManagementManager;
        }
        public async Task<Result> Handle(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken){
            return await this.OperatorDomainService.CreateOperator(command, cancellationToken);
        }

        public async Task<Result<Operator>> Handle(OperatorQueries.GetOperatorQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetOperator(query.EstateId, query.OperatorId, cancellationToken);
        }

        public async Task<Result<List<Operator>>> Handle(OperatorQueries.GetOperatorsQuery query, CancellationToken cancellationToken)
        {
            return await this.EstateManagementManager.GetOperators(query.EstateId, cancellationToken);
        }

        public async Task<Result> Handle(OperatorCommands.UpdateOperatorCommand command, CancellationToken cancellationToken){
            return await this.OperatorDomainService.UpdateOperator(command, cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Services
{
    using System.Threading;
    using EstateAggregate;
    using OperatorAggregate;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;

    public interface IOperatorDomainService{
        Task CreateOperator(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken);
    }

    public class OperatorDomainService : IOperatorDomainService{
        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        private readonly IAggregateRepository<OperatorAggregate, DomainEvent> OperatorAggregateRepository;

        public OperatorDomainService(IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                     IAggregateRepository<OperatorAggregate, DomainEvent> operatorAggregateRepository){
            this.EstateAggregateRepository = estateAggregateRepository;
            this.OperatorAggregateRepository = operatorAggregateRepository;
        }

        public async Task CreateOperator(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken){
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);

            if (estateAggregate.IsCreated == false){
                throw new InvalidOperationException($"Estate with Id {command.EstateId} not created");
            }

            OperatorAggregate operatorAggregate = await this.OperatorAggregateRepository.GetLatestVersion(command.RequestDto.OperatorId, cancellationToken);
            if (operatorAggregate.IsCreated){
                throw new InvalidOperationException($"Operator with Id {command.RequestDto.OperatorId} already created");
            }

            operatorAggregate.Create(command.EstateId, command.RequestDto.Name, command.RequestDto.RequireCustomMerchantNumber.GetValueOrDefault(), command.RequestDto.RequireCustomTerminalNumber.GetValueOrDefault());

            await this.OperatorAggregateRepository.SaveChanges(operatorAggregate, cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.EventHandling
{
    using System.Threading;
    using EstateManagement.Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Float.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;

    public class FloatEventHandler : IDomainEventHandler
    {
        private readonly IEstateReportingRepository EstateReportingRepository;

        public FloatEventHandler(IEstateReportingRepository estateReportingRepository){
            this.EstateReportingRepository = estateReportingRepository;
        }

        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatCreatedForContractProductEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateFloat(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatCreditPurchasedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateFloatActivity(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatDecreasedByTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateFloatActivity(domainEvent, cancellationToken);
        }
    }
}

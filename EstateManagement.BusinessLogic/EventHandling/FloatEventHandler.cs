namespace EstateManagement.BusinessLogic.EventHandling{
    using System.Threading;
    using System.Threading.Tasks;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Float.DomainEvents;

    public class FloatDomainEventHandler : IDomainEventHandler{
        #region Fields

        private readonly IEstateReportingRepository EstateReportingRepository;

        #endregion

        #region Constructors

        public FloatDomainEventHandler(IEstateReportingRepository estateReportingRepository){
            this.EstateReportingRepository = estateReportingRepository;
        }

        #endregion

        #region Methods

        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken){
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatCreatedForContractProductEvent domainEvent,
                                                     CancellationToken cancellationToken){
            await this.EstateReportingRepository.CreateFloat(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatCreditPurchasedEvent domainEvent,
                                                     CancellationToken cancellationToken){
            await this.EstateReportingRepository.CreateFloatActivity(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(FloatDecreasedByTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken){
            await this.EstateReportingRepository.CreateFloatActivity(domainEvent, cancellationToken);
        }

        #endregion
    }
}
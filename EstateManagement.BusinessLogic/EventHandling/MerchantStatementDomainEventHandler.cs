namespace EstateManagement.BusinessLogic.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using MerchantStatement.DomainEvents;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;

    public class MerchantStatementDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The estate reporting repository
        /// </summary>
        private readonly IEstateReportingRepository EstateReportingRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDomainEventHandler" /> class.
        /// </summary>
        /// <param name="estateReportingRepository">The estate reporting repository.</param>
        public MerchantStatementDomainEventHandler(IEstateReportingRepository estateReportingRepository)
        {
            this.EstateReportingRepository = estateReportingRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(StatementCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(TransactionAddedToStatementEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddTransactionToStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(SettledFeeAddedToStatementEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddSettledFeeToStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.MarkStatementAsGenerated(domainEvent, cancellationToken);
        }
        #endregion
    }
}

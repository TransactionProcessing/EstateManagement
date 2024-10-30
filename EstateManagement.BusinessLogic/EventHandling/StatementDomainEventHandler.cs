using SimpleResults;

namespace EstateManagement.BusinessLogic.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using MerchantStatement.DomainEvents;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventHandling;

    public class StatementDomainEventHandler : IDomainEventHandler
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
        public StatementDomainEventHandler(IEstateReportingRepository estateReportingRepository)
        {
            this.EstateReportingRepository = estateReportingRepository;
        }

        #endregion

        #region Methods

        public async Task<Result> Handle(IDomainEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            return await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task<Result> HandleSpecificDomainEvent(StatementCreatedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.CreateStatement(domainEvent, cancellationToken);
        }

        private async Task<Result> HandleSpecificDomainEvent(TransactionAddedToStatementEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.AddTransactionToStatement(domainEvent, cancellationToken);
        }

        private async Task<Result> HandleSpecificDomainEvent(SettledFeeAddedToStatementEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.AddSettledFeeToStatement(domainEvent, cancellationToken);
        }

        private async Task<Result> HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                             CancellationToken cancellationToken)
        {
            return await this.EstateReportingRepository.MarkStatementAsGenerated(domainEvent, cancellationToken);
        }
        #endregion
    }
}

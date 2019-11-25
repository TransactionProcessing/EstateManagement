namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IEstateDomainService" />
    public class EstateDomainService : IEstateDomainService
    {
        #region Fields

        /// <summary>
        /// The aggregate repository manager
        /// </summary>
        private readonly IAggregateRepositoryManager AggregateRepositoryManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateDomainService"/> class.
        /// </summary>
        /// <param name="aggregateRepositoryManager">The aggregate repository manager.</param>
        public EstateDomainService(IAggregateRepositoryManager aggregateRepositoryManager)
        {
            this.AggregateRepositoryManager = aggregateRepositoryManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CreateEstate(Guid estateId,
                                       String estateName,
                                       CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            estateAggregate.Create(estateName);

            await estateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        /// <summary>
        /// Creates the operator.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddOperatorToEstate(Guid estateId,
                                         Guid operatorId,
                                         String operatorName,
                                         Boolean requireCustomMerchantNumber,
                                         Boolean requireCustomTerminalNumber,
                                         CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            estateAggregate.AddOperator(operatorId, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);

            await estateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        #endregion
    }
}
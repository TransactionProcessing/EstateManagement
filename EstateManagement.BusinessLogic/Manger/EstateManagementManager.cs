namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models;
    using Models.Factories;
    using Models.Merchant;
    using Repository;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Manger.IEstateManagementManager" />
    public class EstateManagementManager : IEstateManagementManager
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepositoryManager AggregateRepositoryManager;

        private readonly IEstateManagementRepository EstateManagementRepository;

        private readonly IAggregateRepository<MerchantAggregate> MerchantAggregateRepository;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateManagementManager" /> class.
        /// </summary>
        /// <param name="aggregateRepositoryManager">The aggregate repository manager.</param>
        /// <param name="estateManagementRepository">The estate management repository.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementManager(IAggregateRepositoryManager aggregateRepositoryManager,
                                       IEstateManagementRepository estateManagementRepository,
                                       IModelFactory modelFactory)
        {
            this.AggregateRepositoryManager = aggregateRepositoryManager;
            this.EstateManagementRepository = estateManagementRepository;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Estate> GetEstate(Guid estateId,
                                            CancellationToken cancellationToken)
        {
            // Get the estate from the aggregate repository
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.Version < 0)
            {
                throw new NotFoundException($"No estate found with Id [{estateId}]");
            }
            Estate estateModel = await this.EstateManagementRepository.GetEstate(estateId, cancellationToken);

            return estateModel;
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Merchant> GetMerchant(Guid estateId,
                                      Guid merchantId,
                                      CancellationToken cancellationToken)
        {
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);
            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            Merchant merchantModel = merchantAggregate.GetMerchant();

            return merchantModel;
        }

        #endregion
    }
}
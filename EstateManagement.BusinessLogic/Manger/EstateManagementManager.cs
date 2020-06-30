namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models.Estate;
    using Models.Factories;
    using Models.Merchant;
    using Newtonsoft.Json.Linq;
    using Repository;
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
        private readonly IAggregateRepository<EstateAggregate> EstateAggregateRepository;

        /// <summary>
        /// The estate management repository
        /// </summary>
        private readonly IEstateManagementRepository EstateManagementRepository;

        /// <summary>
        /// The event store context
        /// </summary>
        private readonly IEventStoreContext EventStoreContext;

        /// <summary>
        /// The merchant aggregate repository
        /// </summary>
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
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        /// <param name="merchantAggregateRepository">The merchant aggregate repository.</param>
        /// <param name="estateManagementRepository">The estate management repository.</param>
        /// <param name="eventStoreContext">The event store context.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementManager(IAggregateRepository<EstateAggregate> estateAggregateRepository,
                                       IAggregateRepository<MerchantAggregate> merchantAggregateRepository,
                                       IEstateManagementRepository estateManagementRepository,
                                       IEventStoreContext eventStoreContext,
                                       IModelFactory modelFactory)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.EstateManagementRepository = estateManagementRepository;
            this.EventStoreContext = eventStoreContext;
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
        /// <exception cref="NotFoundException">No estate found with Id [{estateId}]</exception>
        public async Task<Estate> GetEstate(Guid estateId,
                                            CancellationToken cancellationToken)
        {
            // Get the estate from the aggregate repository
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
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
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            Merchant merchantModel = merchantAggregate.GetMerchant();

            return merchantModel;
        }

        /// <summary>
        /// Gets the merchant balance.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<MerchantBalance> GetMerchantBalance(Guid estateId,
                                                              Guid merchantId,
                                                              CancellationToken cancellationToken)
        {
            String projectionState =
                await this.EventStoreContext.GetPartitionStateFromProjection("MerchantBalanceCalculator", $"MerchantBalanceHistory-{merchantId:N}", cancellationToken);

            JObject parsedState = JObject.Parse(projectionState);
            JToken? merchantRecord = parsedState["merchants"][$"{merchantId}"];

            return new MerchantBalance
                   {
                       AvailableBalance = decimal.Parse(merchantRecord["AvailableBalance"].ToString()),
                       Balance = decimal.Parse(merchantRecord["Balance"].ToString()),
                       EstateId = estateId,
                       MerchantId = merchantId
                   };
        }

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<Merchant>> GetMerchants(Guid estateId,
                                                       CancellationToken cancellationToken)
        {
            return await this.EstateManagementRepository.GetMerchants(estateId, cancellationToken);
        }

        #endregion
    }
}
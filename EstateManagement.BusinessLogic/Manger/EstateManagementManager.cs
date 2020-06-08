namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models;
    using Models.Estate;
    using Models.Factories;
    using Models.Merchant;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Repository;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shared.Logger;

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

        private readonly IEventStoreContextManager EventStoreContextManager;

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
                                       IEventStoreContextManager eventStoreContextManager,
                                       IModelFactory modelFactory)
        {
            this.AggregateRepositoryManager = aggregateRepositoryManager;
            this.EstateManagementRepository = estateManagementRepository;
            this.EventStoreContextManager = eventStoreContextManager;
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
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);
            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

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
            IEventStoreContext context = this.EventStoreContextManager.GetEventStoreContext(estateId.ToString());
            
            //var contextType = typeof(EventStoreContext);
            //FieldInfo[] fields = contextType.GetFields(BindingFlags.NonPublic |
            //                                           BindingFlags.Instance);

            //var settingsField = fields.Where(f => f.FieldType == typeof(EventStoreConnectionSettings)).SingleOrDefault();

            //var settings = (EventStoreConnectionSettings)settingsField.GetValue(context);
            //var ipAddress = Dns.GetHostAddresses(settings.IpAddress);
            
            //foreach (IPAddress address in ipAddress)
            //{
            //    Logger.LogDebug(address.ToString());
            //}

            //IPEndPoint endpoint = new IPEndPoint(ipAddress[0], settings.HttpPort);

            String projectionState = await context.GetPartitionStateFromProjection("MerchantBalanceCalculator", $"MerchantBalanceHistory-{merchantId:N}");

            JObject parsedState = JObject.Parse(projectionState);
            JToken? merchantRecord = parsedState["merchants"][$"{merchantId}"];
            
            return new MerchantBalance
                   {
                       AvailableBalance = Decimal.Parse(merchantRecord["AvailableBalance"].ToString()),
                       Balance = Decimal.Parse(merchantRecord["Balance"].ToString()),
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
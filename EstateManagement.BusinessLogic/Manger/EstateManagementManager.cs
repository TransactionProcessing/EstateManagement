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
    using Shared.DomainDrivenDesign.EventStore;

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
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementManager(IAggregateRepository<EstateAggregate> estateAggregateRepository,
                                       IAggregateRepository<MerchantAggregate> merchantAggregateRepository,
                                       IModelFactory modelFactory)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
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
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            Estate estateModel = this.ModelFactory.ConvertFrom(estateAggregate);

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

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using Models;
    using Models.Factories;
    using Shared.DomainDrivenDesign.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Manger.IEstateManagmentManager" />
    public class EstateManagementManager : IEstateManagmentManager
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate> EstateAggregateRepository;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateManagementManager"/> class.
        /// </summary>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementManager(IAggregateRepository<EstateAggregate> estateAggregateRepository,
                                       IModelFactory modelFactory)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
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

        #endregion
    }
}
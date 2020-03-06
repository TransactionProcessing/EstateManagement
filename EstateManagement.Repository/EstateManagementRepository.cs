namespace EstateManagement.Repository
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Database;
    using EstateReporting.Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models.Factories;
    using Shared.EntityFramework;
    using Shared.Exceptions;
    using EstateModel = Models.Estate;

    /// <summary>
    /// 
    /// </summary>
    public interface IEstateManagementRepository
    {
        #region Methods

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<EstateModel> GetEstate(Guid estateId,
                                    CancellationToken cancellationToken);

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Repository.IEstateManagementRepository" />
    public class EstateManagementRepository : IEstateManagementRepository
    {
        #region Fields

        /// <summary>
        /// The context factory
        /// </summary>
        private readonly IDbContextFactory<EstateReportingContext> ContextFactory;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateManagementRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementRepository(IDbContextFactory<EstateReportingContext> contextFactory,
                                          IModelFactory modelFactory)
        {
            this.ContextFactory = contextFactory;
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
        public async Task<EstateModel> GetEstate(Guid estateId,
                                                 CancellationToken cancellationToken)
        {
            EstateReportingContext context = await this.ContextFactory.GetContext(estateId, cancellationToken);

            Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken);

            if (estate == null)
            {
                throw new NotFoundException($"No estate found with Id [{estateId}]");
            }

            return this.ModelFactory.ConvertFrom(estate);
        }

        #endregion
    }
}
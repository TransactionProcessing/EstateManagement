namespace EstateManagement.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Database;
    using EstateReporting.Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using Models.Factories;
    using Shared.EntityFramework;
    using Shared.Exceptions;
    using EstateModel = Models.Estate.Estate;
    using MerchantModel = Models.Merchant.Merchant;

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

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<MerchantModel>> GetMerchants(Guid estateId,
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
                throw new NotFoundException($"No estate found in read model with Id [{estateId}]");
            }

            List<EstateOperator> estateOperators = await context.EstateOperators.Where(eo => eo.EstateId == estateId).ToListAsync(cancellationToken);
            List<EstateSecurityUser> estateSecurityUsers = await context.EstateSecurityUsers.Where(esu => esu.EstateId == estateId).ToListAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);
        }

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<MerchantModel>> GetMerchants(Guid estateId,
                                                       CancellationToken cancellationToken)
        {
            EstateReportingContext context = await this.ContextFactory.GetContext(estateId, cancellationToken);

            List<Merchant> merchants = await (from m in context.Merchants where m.EstateId == estateId select m).ToListAsync(cancellationToken);

            List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where a.EstateId == estateId select a).ToListAsync(cancellationToken);
            List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where c.EstateId == estateId select c).ToListAsync(cancellationToken);
            List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where d.EstateId == estateId select d).ToListAsync(cancellationToken);
            List<MerchantSecurityUser> merchantSecurityUsers = await (from u in context.MerchantSecurityUsers where u.EstateId == estateId select u).ToListAsync(cancellationToken);
            List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where o.EstateId == estateId select o).ToListAsync(cancellationToken);
            
            if (merchants.Any() == false)
            {
                return null;
            }

            List<MerchantModel> models = new List<MerchantModel>();

            foreach (Merchant merchant in merchants)
            {
                List<MerchantAddress> addresses = merchantAddresses.Where(a => a.MerchantId == merchant.MerchantId).ToList();
                List<MerchantContact> contacts = merchantContacts.Where(c => c.MerchantId == merchant.MerchantId).ToList();
                List<MerchantDevice> devices = merchantDevices.Where(d => d.MerchantId == merchant.MerchantId).ToList();
                List<MerchantSecurityUser> securityUsers = merchantSecurityUsers.Where(s => s.MerchantId == merchant.MerchantId).ToList();
                List<MerchantOperator> operators = merchantOperators.Where(o => o.MerchantId == merchant.MerchantId).ToList();

                models.Add(this.ModelFactory.ConvertFrom(merchant, addresses, contacts, operators, devices, securityUsers));
            }

            return models;
        }

        #endregion
    }
}
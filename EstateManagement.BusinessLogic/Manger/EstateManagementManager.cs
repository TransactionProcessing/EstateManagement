namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models.Contract;
    using Models.Estate;
    using Models.Factories;
    using Models.Merchant;
    using Newtonsoft.Json.Linq;
    using NLog;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
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
        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        /// <summary>
        /// The estate management repository
        /// </summary>
        private readonly IEstateManagementRepository EstateManagementRepository;
        
        /// <summary>
        /// The merchant aggregate repository
        /// </summary>
        private readonly IAggregateRepository<MerchantAggregate, DomainEvent> MerchantAggregateRepository;

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
        public EstateManagementManager(IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                       IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                       IEstateManagementRepository estateManagementRepository,
                                       IModelFactory modelFactory)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.EstateManagementRepository = estateManagementRepository;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<Contract>> GetContracts(Guid estateId,
                                                       CancellationToken cancellationToken)
        {
            List<Contract> contracts = await this.EstateManagementRepository.GetContracts(estateId, cancellationToken);

            return contracts;
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="includeProducts">if set to <c>true</c> [include products].</param>
        /// <param name="includeProductsWithFees">if set to <c>true</c> [include products with fees].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Contract> GetContract(Guid estateId,
                                                Guid contractId,
                                                Boolean includeProducts,
                                                Boolean includeProductsWithFees,
                                                CancellationToken cancellationToken)
        {
            Contract contractModel = await this.EstateManagementRepository.GetContract(estateId, contractId, includeProducts, includeProductsWithFees, cancellationToken);

            return contractModel;
        }

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No estate found with Id [{estateId}]</exception>
        public async Task<Estate> GetEstate(Guid estateId,
                                            CancellationToken cancellationToken) {

            // Get the estate from the aggregate repository
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
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
        /// Gets the merchant contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<Contract>> GetMerchantContracts(Guid estateId,
                                                               Guid merchantId,
                                                               CancellationToken cancellationToken)
        {
            List<Contract> contractModels = await this.EstateManagementRepository.GetMerchantContracts(estateId, merchantId, cancellationToken);

            return contractModels;
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

        /// <summary>
        /// Gets the transaction fees for product.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<TransactionFee>> GetTransactionFeesForProduct(Guid estateId,
                                                                             Guid merchantId,
                                                                             Guid contractId,
                                                                             Guid productId,
                                                                             CancellationToken cancellationToken)
        {
            return await this.EstateManagementRepository.GetTransactionFeesForProduct(estateId, merchantId, contractId, productId, cancellationToken);
        }

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ContractAggregate;
    using EstateAggregate;
    using MerchantAggregate;
    using Models.Contract;
    using Models.Estate;
    using Models.Factories;
    using Models.File;
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
        
        private readonly IEstateManagementRepository EstateManagementRepository;

        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        private readonly IAggregateRepository<ContractAggregate, DomainEvent> ContractAggregateRepository;

        private readonly IAggregateRepository<MerchantAggregate, DomainEvent> MerchantAggregateRepository;

        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors
        
        public EstateManagementManager(IEstateManagementRepository estateManagementRepository,
                                       IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                       IAggregateRepository<ContractAggregate,DomainEvent> contractAggregateRepository,
                                       IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                       IModelFactory modelFactory)
        {
            this.EstateManagementRepository = estateManagementRepository;
            this.EstateAggregateRepository = estateAggregateRepository;
            this.ContractAggregateRepository = contractAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
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
                                                CancellationToken cancellationToken){
            ContractAggregate contractAgggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);
            if (contractAgggregate.IsCreated == false){
                throw new NotFoundException($"No contract found with Id [{estateId}]");
            }
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
                                            CancellationToken cancellationToken){

            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false){
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
            if (merchantAggregate.IsCreated == false)
            {
                throw new NotFoundException($"No merchant found with Id [{merchantId}]");
            }

            Merchant merchantModel = await this.EstateManagementRepository.GetMerchant(estateId, merchantId, cancellationToken);

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
        
        public async Task<List<TransactionFee>> GetTransactionFeesForProduct(Guid estateId,
                                                                             Guid merchantId,
                                                                             Guid contractId,
                                                                             Guid productId,
                                                                             CancellationToken cancellationToken)
        {
            return await this.EstateManagementRepository.GetTransactionFeesForProduct(estateId, merchantId, contractId, productId, cancellationToken);
        }

        public async Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken){
            return await this.EstateManagementRepository.GetFileDetails(estateId, fileId, cancellationToken);
        }

        #endregion
    }
}
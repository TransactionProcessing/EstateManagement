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
    using Contract = Models.Contract.Contract;

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

        public async Task<List<Contract>> GetContracts(Guid estateId,
                                                       CancellationToken cancellationToken)
        {
            List<Contract> contracts = await this.EstateManagementRepository.GetContracts(estateId, cancellationToken);

            return contracts;
        }

        public async Task<Contract> GetContract(Guid estateId,
                                                Guid contractId,
                                                CancellationToken cancellationToken){
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);
            if (contractAggregate.IsCreated == false){
                throw new NotFoundException($"No contract found with Id [{estateId}]");
            }
            Contract contractModel = contractAggregate.GetContract();

            return contractModel;
        }

        public async Task<Estate> GetEstate(Guid estateId,
                                            CancellationToken cancellationToken){

            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false){
                throw new NotFoundException($"No estate found with Id [{estateId}]");
            }

            Estate estateModel = estateAggregate.GetEstate();
            
            return estateModel;
        }

        public async Task<Merchant> GetMerchant(Guid estateId,
                                                Guid merchantId,
                                                CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);
            if (merchantAggregate.IsCreated == false)
            {
                throw new NotFoundException($"No merchant found with Id [{merchantId}]");
            }

            Merchant merchantModel = merchantAggregate.GetMerchant();

            return merchantModel;
        }
        
        public async Task<List<Contract>> GetMerchantContracts(Guid estateId,
                                                               Guid merchantId,
                                                               CancellationToken cancellationToken)
        {
            List<Contract> contractModels = await this.EstateManagementRepository.GetMerchantContracts(estateId, merchantId, cancellationToken);

            return contractModels;
        }

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
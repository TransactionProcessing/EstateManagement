namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using OperatorAggregate;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.Exceptions;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
    using Contract = Models.Contract.Contract;
    using Operator = Models.Estate.Operator;

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

        private readonly IAggregateRepository<OperatorAggregate, DomainEvent> OperatorAggregateRepository;

        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors
        
        public EstateManagementManager(IEstateManagementRepository estateManagementRepository,
                                       IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                       IAggregateRepository<ContractAggregate,DomainEvent> contractAggregateRepository,
                                       IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                       IModelFactory modelFactory,
                                       IAggregateRepository<OperatorAggregate, DomainEvent> operatorAggregateRepository)
        {
            this.EstateManagementRepository = estateManagementRepository;
            this.EstateAggregateRepository = estateAggregateRepository;
            this.ContractAggregateRepository = contractAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.OperatorAggregateRepository = operatorAggregateRepository;
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

            if (estateModel.Operators != null){
                foreach (Operator @operator in estateModel.Operators){
                    OperatorAggregate operatorAggregate = await this.OperatorAggregateRepository.GetLatestVersion(@operator.OperatorId, cancellationToken);
                    @operator.Name = operatorAggregate.Name;
                }
            }

            return estateModel;
        }

        public async Task<List<Estate>> GetEstates(Guid estateId,
                                                   CancellationToken cancellationToken){
            Estate estateModel = await this.EstateManagementRepository.GetEstate(estateId, cancellationToken);

            return new List<Estate>(){
                                         estateModel
                                     };
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

            if (merchantModel.Operators != null){
                foreach (Models.Merchant.Operator @operator in merchantModel.Operators){
                    OperatorAggregate operatorAggregate = await this.OperatorAggregateRepository.GetLatestVersion(@operator.OperatorId, cancellationToken);
                    @operator.Name = operatorAggregate.Name;
                }
            }

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
            List<Merchant> merchants = await this.EstateManagementRepository.GetMerchants(estateId, cancellationToken);

            if (merchants == null || merchants.Any() == false)
            {
                throw new NotFoundException($"No Merchants found for estate Id {estateId}");
            }

            return merchants;
        }
        
        public async Task<List<TransactionFee>> GetTransactionFeesForProduct(Guid estateId,
                                                                             Guid merchantId,
                                                                             Guid contractId,
                                                                             Guid productId,
                                                                             CancellationToken cancellationToken)
        {
            // TODO: this will need updated to handle merchant specific fees when that is available

            ContractAggregate contract = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);

            if (contract.IsCreated == false){
                throw new NotFoundException($"No contract found with Id [{contractId}]");
            }

            List<Product> products = contract.GetProducts();

            Product product = products.SingleOrDefault(p => p.ProductId == productId);

            if (product == null){
                throw new NotFoundException($"No product found with Id [{productId}] on contract Id [{contractId}]");
            }

            return product.TransactionFees;

        }

        public async Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken){
            return await this.EstateManagementRepository.GetFileDetails(estateId, fileId, cancellationToken);
        }

        #endregion
    }
}
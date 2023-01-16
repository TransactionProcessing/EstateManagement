namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ContractAggregate;
    using EstateAggregate;
    using Models.Contract;
    using Models.Estate;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    
    public class ContractDomainService : IContractDomainService
    {
        #region Fields
        
        private readonly IAggregateRepository<ContractAggregate, DomainEvent> ContractAggregateRepository;

        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        #endregion

        #region Constructors

        public ContractDomainService(IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                     IAggregateRepository<ContractAggregate, DomainEvent> contractAggregateRepository)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.ContractAggregateRepository = contractAggregateRepository;
        }

        #endregion

        #region Methods

        public async Task AddProductToContract(Guid productId,
                                               Guid contractId,
                                               String productName,
                                               String displayText,
                                               Decimal? value,
                                               ProductType productType,
                                               CancellationToken cancellationToken)
        {
            // Get the contract aggregate
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);

            // Check for a duplicate
            if (contractAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Contract Id [{contractId}] must be created to add products");
            }

            if (value.HasValue)
            {
                contractAggregate.AddFixedValueProduct(productId, productName, displayText, value.Value,productType);
            }
            else
            {
                contractAggregate.AddVariableValueProduct(productId, productName, displayText,productType);
            }

            await this.ContractAggregateRepository.SaveChanges(contractAggregate, cancellationToken);
        }

        public async Task AddTransactionFeeForProductToContract(Guid transactionFeeId,
                                                                Guid contractId,
                                                                Guid productId,
                                                                String description,
                                                                CalculationType calculationType,
                                                                FeeType feeType,
                                                                Decimal value,
                                                                CancellationToken cancellationToken)
        {
            // Get the contract aggregate
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);

            // Check for a duplicate
            if (contractAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Contract Id [{contractId}] must be created to add products");
            }

            List<Product> products = contractAggregate.GetProducts();
            Product product = products.SingleOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product Id [{productId}] not added to contract [{contractAggregate.Description}]");
            }

            contractAggregate.AddTransactionFee(product, transactionFeeId, description, calculationType, feeType, value);

            await this.ContractAggregateRepository.SaveChanges(contractAggregate, cancellationToken);
        }

        public async Task DisableTransactionFeeForProduct(Guid transactionFeeId,
                                                          Guid contractId,
                                                          Guid productId,
                                                          CancellationToken cancellationToken)
        {
            // Get the contract aggregate
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);
            
            contractAggregate.DisableTransactionFee(productId, transactionFeeId);

            await this.ContractAggregateRepository.SaveChanges(contractAggregate, cancellationToken);
        }
        
        public async Task CreateContract(Guid contractId,
                                         Guid estateId,
                                         Guid operatorId,
                                         String description,
                                         CancellationToken cancellationToken)
        {
            // Validate the estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);

            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException("Unable to create a contract for an estate that is not created");
            }

            // Validate the operator
            Estate estate = estateAggregate.GetEstate();
            if (estate.Operators == null || estate.Operators.Any(o => o.OperatorId == operatorId) == false)
            {
                throw new InvalidOperationException($"Unable to create a contract for an operator that is not setup on estate [{estate.Name}]");
            }

            // Get the contract aggregate
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);

            // Check for a duplicate
            if (contractAggregate.IsCreated)
            {
                throw new InvalidOperationException($"Contract Id [{contractId}] already created for estate [{estate.Name}]");
            }

            contractAggregate.Create(estateId, operatorId, description);

            await this.ContractAggregateRepository.SaveChanges(contractAggregate, cancellationToken);
        }

        #endregion
    }
}
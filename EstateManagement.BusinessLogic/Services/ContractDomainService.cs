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
    using Shared.EventStore.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IContractDomainService" />
    public class ContractDomainService : IContractDomainService
    {
        #region Fields

        /// <summary>
        /// The contract aggregate repository
        /// </summary>
        private readonly IAggregateRepository<ContractAggregate, DomainEventRecord.DomainEvent> ContractAggregateRepository;

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent> EstateAggregateRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDomainService"/> class.
        /// </summary>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        /// <param name="contractAggregateRepository">The contract aggregate repository.</param>
        public ContractDomainService(IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent> estateAggregateRepository,
                                     IAggregateRepository<ContractAggregate, DomainEventRecord.DomainEvent> contractAggregateRepository)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.ContractAggregateRepository = contractAggregateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the product to contract.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.InvalidOperationException">Contract Id [{contractId}] must be created to add products</exception>
        public async Task AddProductToContract(Guid productId,
                                               Guid contractId,
                                               String productName,
                                               String displayText,
                                               Decimal? value,
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
                contractAggregate.AddFixedValueProduct(productId, productName, displayText, value.Value);
            }
            else
            {
                contractAggregate.AddVariableValueProduct(productId, productName, displayText);
            }

            await this.ContractAggregateRepository.SaveChanges(contractAggregate, cancellationToken);
        }

        /// <summary>
        /// Adds the transaction fee for product to contract.
        /// </summary>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="feeType">Type of the fee.</param>
        /// <param name="value">The value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="InvalidOperationException">
        /// Contract Id [{contractId}] must be created to add products
        /// or
        /// Product Id [{productId}] not added to contract [{contractAggregate.Description}]
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Contract Id [{contractId}] must be created to add products
        /// or
        /// Product Id [{productId}] not added to contract [{contractAggregate.Description}]</exception>
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

        /// <summary>
        /// Disables the transaction fee for product.
        /// </summary>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
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
        
        /// <summary>
        /// Creates the contract.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Unable to create a contract for an estate that is not created
        /// or
        /// Unable to create a contract for an operator that is not setup on estate [{estate.Name}]
        /// or
        /// Contract Id [{contractId}] already created for estate [{estate.Name}]
        /// </exception>
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
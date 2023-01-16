namespace EstateManagement.ContractAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contract.DomainEvents;
    using Models.Contract;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventStore;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Aggregate" />
    public class ContractAggregate : Aggregate
    {
        #region Fields

        private readonly List<Product> Products;

        #endregion

        #region Constructors

        [ExcludeFromCodeCoverage]
        public ContractAggregate()
        {
            // Nothing here
            this.Products = new List<Product>();
        }

        private ContractAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Products = new List<Product>();
        }

        #endregion

        #region Properties

        public String Description { get; private set; }

        public Guid EstateId { get; private set; }

        public Boolean IsCreated { get; private set; }

        public Guid OperatorId { get; private set; }

        #endregion

        #region Methods

        public void AddFixedValueProduct(Guid productId,
                                         String productName,
                                         String displayText,
                                         Decimal value,
                                         ProductType productType) {
            Guard.ThrowIfInvalidGuid(productId, typeof(ArgumentNullException), "Product Id cannot be an empty Guid");
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");
            Guard.ThrowIfZero(value, typeof(ArgumentOutOfRangeException), "Product value must not be zero");
            Guard.ThrowIfNegative(value, typeof(ArgumentOutOfRangeException), "Product value must not be negative");

            // Check product not already added
            if (this.Products.Any(p => p.Name == productName)) {
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            FixedValueProductAddedToContractEvent fixedValueProductAddedToContractEvent =
                new(this.AggregateId, this.EstateId, productId, productName, displayText, value, (Int32)productType);

            this.ApplyAndAppend(fixedValueProductAddedToContractEvent);
        }

        public void AddTransactionFee(Product product,
                                      Guid transactionFeeId,
                                      String description,
                                      CalculationType calculationType,
                                      FeeType feeType,
                                      Decimal value)
        {
            Guard.ThrowIfInvalidGuid(transactionFeeId, typeof(ArgumentNullException), "Transaction Fee Id cannot be an empty Guid");
            Guard.ThrowIfNull(product, typeof(ArgumentNullException), "Product to add fee for cannot be null");
            Guard.ThrowIfNullOrEmpty(description, typeof(ArgumentNullException), "Transaction Fee description must not be null or empty");
            Guard.ThrowIfZero(value, typeof(ArgumentOutOfRangeException), "Transaction Fee value cannot be zero");
            Guard.ThrowIfNegative(value, typeof(ArgumentOutOfRangeException), "Transaction Fee value cannot be negative");

            if (this.Products.Any(p => p.ProductId == product.ProductId) == false)
            {
                throw new InvalidOperationException($"Product Id {product.ProductId} is not a valid product on this contract");
            }

            Guard.ThrowIfInvalidEnum(typeof(CalculationType), calculationType, nameof(calculationType));
            Guard.ThrowIfInvalidEnum(typeof(FeeType), feeType, nameof(feeType));

            TransactionFeeForProductAddedToContractEvent transactionFeeForProductAddedToContractEvent =
                new TransactionFeeForProductAddedToContractEvent(this.AggregateId, this.EstateId, product.ProductId, transactionFeeId, description, (Int32)calculationType, (Int32)feeType, value);

            this.ApplyAndAppend(transactionFeeForProductAddedToContractEvent);
        }

        public void DisableTransactionFee(Guid productId,
                                          Guid transactionFeeId)
        {
            if (this.Products.Any(p => p.ProductId == productId) == false)
            {
                throw new InvalidOperationException($"Product Id {productId} is not a valid product on this contract");
            }

            Product product = this.Products.Single(p => p.ProductId == productId);

            if (product.TransactionFees.Any(f => f.TransactionFeeId == transactionFeeId) == false)
            {
                throw new InvalidOperationException($"Transaction Fee Id {transactionFeeId} is not a valid for product {product.Name} on this contract");
            }

            TransactionFeeForProductDisabledEvent transactionFeeForProductDisabledEvent = new TransactionFeeForProductDisabledEvent(this.AggregateId,
                                                                                                                                       this.EstateId,
                                                                                                                                       productId,
                                                                                                                                       transactionFeeId);

            this.ApplyAndAppend(transactionFeeForProductDisabledEvent);
        }

        private void PlayEvent(TransactionFeeForProductDisabledEvent domainEvent)
        {
            // Find the product
            Product product = this.Products.Single(p => p.ProductId == domainEvent.ProductId);
            TransactionFee transactionFee = product.TransactionFees.Single(t => t.TransactionFeeId == domainEvent.TransactionFeeId);

            transactionFee.IsEnabled = false;

        }
        
        public void AddVariableValueProduct(Guid productId,
                                            String productName,
                                            String displayText,
                                            ProductType productType)
        {
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");

            // Check product not already added
            if (this.Products.Any(p => p.Name == productName))
            {
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            VariableValueProductAddedToContractEvent variableValueProductAddedToContractEvent =
                new(this.AggregateId, this.EstateId, productId, productName, displayText, (Int32)productType);

            this.ApplyAndAppend(variableValueProductAddedToContractEvent);
        }

        public static ContractAggregate Create(Guid aggregateId)
        {
            return new ContractAggregate(aggregateId);
        }

        public void Create(Guid estateId,
                           Guid operatorId,
                           String description)
        {
            Guard.ThrowIfInvalidGuid(estateId, typeof(ArgumentNullException), "Estate Id must not be an empty Guid");
            Guard.ThrowIfInvalidGuid(operatorId, typeof(ArgumentNullException), "Operator Id must not be an empty Guid");
            Guard.ThrowIfNullOrEmpty(description, typeof(ArgumentNullException), "Contract description must not be null or empty");

            ContractCreatedEvent contractCreatedEvent = new ContractCreatedEvent(this.AggregateId, estateId, operatorId, description);
            this.ApplyAndAppend(contractCreatedEvent);
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <returns></returns>
        public Contract GetContract()
        {
            Contract contractModel = new Contract();

            contractModel.EstateId = this.EstateId;
            contractModel.IsCreated = this.IsCreated;
            contractModel.OperatorId = this.OperatorId;
            contractModel.Description = this.Description;
            contractModel.ContractId = this.AggregateId;

            return contractModel;
        }

        public List<Product> GetProducts()
        {
            return this.Products;
        }

        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       EstateId = Guid.NewGuid() // TODO: Populate
                   };
        }

        public override void PlayEvent(IDomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        private void PlayEvent(ContractCreatedEvent domainEvent)
        {
            this.IsCreated = true;
            this.OperatorId = domainEvent.OperatorId;
            this.EstateId = domainEvent.EstateId;
            this.Description = domainEvent.Description;
        }

        private void PlayEvent(FixedValueProductAddedToContractEvent domainEvent)
        {
            this.Products.Add(new Product
                              {
                                  ProductId = domainEvent.ProductId,
                                  Name = domainEvent.ProductName,
                                  DisplayText = domainEvent.DisplayText,
                                  Value = domainEvent.Value,
                                  ProductType = (ProductType)domainEvent.ProductType
                              });
        }

        private void PlayEvent(VariableValueProductAddedToContractEvent domainEvent)
        {
            this.Products.Add(new Product
                              {
                                  ProductId = domainEvent.ProductId,
                                  Name = domainEvent.ProductName,
                                  DisplayText = domainEvent.DisplayText,
                                  ProductType = (ProductType)domainEvent.ProductType
            });
        }

        private void PlayEvent(TransactionFeeForProductAddedToContractEvent domainEvent)
        {
            // Find the product
            Product product = this.Products.Single(p => p.ProductId == domainEvent.ProductId);

            product.TransactionFees.Add(new TransactionFee
                                        {
                                            Description = domainEvent.Description,
                                            CalculationType = (Models.Contract.CalculationType)domainEvent.CalculationType,
                                            TransactionFeeId = domainEvent.TransactionFeeId,
                                            Value = domainEvent.Value,
                                            IsEnabled = true,
                                            FeeType = (Models.Contract.FeeType)domainEvent.FeeType
                                        });
        }

        #endregion
    }
}
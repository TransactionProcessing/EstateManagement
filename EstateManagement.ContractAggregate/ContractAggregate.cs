namespace EstateManagement.ContractAggregate{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contract.DomainEvents;
    using Models.Contract;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;

    public static class ContractAggregateExtensions{
        #region Methods

        public static void AddFixedValueProduct(this ContractAggregate aggregate,
                                                Guid productId,
                                                String productName,
                                                String displayText,
                                                Decimal value,
                                                ProductType productType){
            Guard.ThrowIfInvalidGuid(productId, typeof(ArgumentNullException), "Product Id cannot be an empty Guid");
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");
            Guard.ThrowIfZero(value, typeof(ArgumentOutOfRangeException), "Product value must not be zero");
            Guard.ThrowIfNegative(value, typeof(ArgumentOutOfRangeException), "Product value must not be negative");

            // Check product not already added
            if (aggregate.Products.Any(p => p.Name == productName)){
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            FixedValueProductAddedToContractEvent fixedValueProductAddedToContractEvent =
                new(aggregate.AggregateId, aggregate.EstateId, productId, productName, displayText, value, (Int32)productType);

            aggregate.ApplyAndAppend(fixedValueProductAddedToContractEvent);
        }

        public static void AddTransactionFee(this ContractAggregate aggregate,
                                             Product product,
                                             Guid transactionFeeId,
                                             String description,
                                             CalculationType calculationType,
                                             FeeType feeType,
                                             Decimal value){
            Guard.ThrowIfInvalidGuid(transactionFeeId, typeof(ArgumentNullException), "Transaction Fee Id cannot be an empty Guid");
            Guard.ThrowIfNull(product, typeof(ArgumentNullException), "Product to add fee for cannot be null");
            Guard.ThrowIfNullOrEmpty(description, typeof(ArgumentNullException), "Transaction Fee description must not be null or empty");
            Guard.ThrowIfZero(value, typeof(ArgumentOutOfRangeException), "Transaction Fee value cannot be zero");
            Guard.ThrowIfNegative(value, typeof(ArgumentOutOfRangeException), "Transaction Fee value cannot be negative");

            if (aggregate.Products.Any(p => p.ProductId == product.ProductId) == false){
                throw new InvalidOperationException($"Product Id {product.ProductId} is not a valid product on this contract");
            }

            Guard.ThrowIfInvalidEnum(typeof(CalculationType), calculationType, nameof(calculationType));
            Guard.ThrowIfInvalidEnum(typeof(FeeType), feeType, nameof(feeType));

            TransactionFeeForProductAddedToContractEvent transactionFeeForProductAddedToContractEvent =
                new TransactionFeeForProductAddedToContractEvent(aggregate.AggregateId, aggregate.EstateId, product.ProductId, transactionFeeId, description, (Int32)calculationType, (Int32)feeType, value);

            aggregate.ApplyAndAppend(transactionFeeForProductAddedToContractEvent);
        }

        public static void AddVariableValueProduct(this ContractAggregate aggregate,
                                                   Guid productId,
                                                   String productName,
                                                   String displayText,
                                                   ProductType productType){
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");

            // Check product not already added
            if (aggregate.Products.Any(p => p.Name == productName)){
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            VariableValueProductAddedToContractEvent variableValueProductAddedToContractEvent =
                new(aggregate.AggregateId, aggregate.EstateId, productId, productName, displayText, (Int32)productType);

            aggregate.ApplyAndAppend(variableValueProductAddedToContractEvent);
        }

        public static void Create(this ContractAggregate aggregate,
                                  Guid estateId,
                                  Guid operatorId,
                                  String description){
            Guard.ThrowIfInvalidGuid(estateId, typeof(ArgumentNullException), "Estate Id must not be an empty Guid");
            Guard.ThrowIfInvalidGuid(operatorId, typeof(ArgumentNullException), "Operator Id must not be an empty Guid");
            Guard.ThrowIfNullOrEmpty(description, typeof(ArgumentNullException), "Contract description must not be null or empty");

            ContractCreatedEvent contractCreatedEvent = new ContractCreatedEvent(aggregate.AggregateId, estateId, operatorId, description);
            aggregate.ApplyAndAppend(contractCreatedEvent);
        }

        public static void DisableTransactionFee(this ContractAggregate aggregate,
                                                 Guid productId,
                                                 Guid transactionFeeId){
            if (aggregate.Products.Any(p => p.ProductId == productId) == false){
                throw new InvalidOperationException($"Product Id {productId} is not a valid product on this contract");
            }

            Product product = aggregate.Products.Single(p => p.ProductId == productId);

            if (product.TransactionFees.Any(f => f.TransactionFeeId == transactionFeeId) == false){
                throw new InvalidOperationException($"Transaction Fee Id {transactionFeeId} is not a valid for product {product.Name} on this contract");
            }

            TransactionFeeForProductDisabledEvent transactionFeeForProductDisabledEvent = new TransactionFeeForProductDisabledEvent(aggregate.AggregateId,
                                                                                                                                    aggregate.EstateId,
                                                                                                                                    productId,
                                                                                                                                    transactionFeeId);

            aggregate.ApplyAndAppend(transactionFeeForProductDisabledEvent);
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <returns></returns>
        public static Contract GetContract(this ContractAggregate aggregate){
            Contract contractModel = new Contract();

            contractModel.EstateId = aggregate.EstateId;
            contractModel.IsCreated = aggregate.IsCreated;
            contractModel.OperatorId = aggregate.OperatorId;
            contractModel.Description = aggregate.Description;
            contractModel.ContractId = aggregate.AggregateId;

            return contractModel;
        }

        public static List<Product> GetProducts(this ContractAggregate aggregate){
            return aggregate.Products;
        }

        public static void PlayEvent(this ContractAggregate aggregate, TransactionFeeForProductDisabledEvent domainEvent){
            // Find the product
            Product product = aggregate.Products.Single(p => p.ProductId == domainEvent.ProductId);
            TransactionFee transactionFee = product.TransactionFees.Single(t => t.TransactionFeeId == domainEvent.TransactionFeeId);

            transactionFee.IsEnabled = false;
        }

        public static void PlayEvent(this ContractAggregate aggregate, ContractCreatedEvent domainEvent){
            aggregate.IsCreated = true;
            aggregate.OperatorId = domainEvent.OperatorId;
            aggregate.EstateId = domainEvent.EstateId;
            aggregate.Description = domainEvent.Description;
        }

        public static void PlayEvent(this ContractAggregate aggregate, FixedValueProductAddedToContractEvent domainEvent){
            aggregate.Products.Add(new Product{
                                                  ProductId = domainEvent.ProductId,
                                                  Name = domainEvent.ProductName,
                                                  DisplayText = domainEvent.DisplayText,
                                                  Value = domainEvent.Value,
                                                  ProductType = (ProductType)domainEvent.ProductType
                                              });
        }

        public static void PlayEvent(this ContractAggregate aggregate, VariableValueProductAddedToContractEvent domainEvent){
            aggregate.Products.Add(new Product{
                                                  ProductId = domainEvent.ProductId,
                                                  Name = domainEvent.ProductName,
                                                  DisplayText = domainEvent.DisplayText,
                                                  ProductType = (ProductType)domainEvent.ProductType
                                              });
        }

        public static void PlayEvent(this ContractAggregate aggregate, TransactionFeeForProductAddedToContractEvent domainEvent){
            // Find the product
            Product product = aggregate.Products.Single(p => p.ProductId == domainEvent.ProductId);

            product.TransactionFees.Add(new TransactionFee{
                                                              Description = domainEvent.Description,
                                                              CalculationType = (CalculationType)domainEvent.CalculationType,
                                                              TransactionFeeId = domainEvent.TransactionFeeId,
                                                              Value = domainEvent.Value,
                                                              IsEnabled = true,
                                                              FeeType = (FeeType)domainEvent.FeeType
                                                          });
        }

        #endregion
    }

    public record ContractAggregate : Aggregate{
        #region Fields

        internal readonly List<Product> Products;

        #endregion

        #region Constructors

        [ExcludeFromCodeCoverage]
        public ContractAggregate(){
            // Nothing here
            this.Products = new List<Product>();
        }

        private ContractAggregate(Guid aggregateId){
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Products = new List<Product>();
        }

        #endregion

        #region Properties

        public String Description{ get; internal set; }

        public Guid EstateId{ get; internal set; }

        public Boolean IsCreated{ get; internal set; }

        public Guid OperatorId{ get; internal set; }

        #endregion

        #region Methods

        public static ContractAggregate Create(Guid aggregateId){
            return new ContractAggregate(aggregateId);
        }

        public override void PlayEvent(IDomainEvent domainEvent) => ContractAggregateExtensions.PlayEvent(this, (dynamic)domainEvent);

        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata(){
            return new{
                          EstateId = Guid.NewGuid() // TODO: Populate
                      };
        }

        #endregion
    }
}
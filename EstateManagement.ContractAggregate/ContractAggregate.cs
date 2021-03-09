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

        /// <summary>
        /// The products
        /// </summary>
        private readonly List<Product> Products;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ContractAggregate()
        {
            // Nothing here
            this.Products = new List<Product>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private ContractAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Products = new List<Product>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCreated { get; private set; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.InvalidOperationException">Product Name {productName} has already been added to the contract</exception>
        public void AddFixedValueProduct(Guid productId, 
                                         String productName,
                                         String displayText,
                                         Decimal value)
        {
            Guard.ThrowIfInvalidGuid(productId, typeof(ArgumentNullException), "Product Id cannot be an empty Guid");
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");
            Guard.ThrowIfZero(value, typeof(ArgumentOutOfRangeException), "Product value must not be zero");
            Guard.ThrowIfNegative(value, typeof(ArgumentOutOfRangeException), "Product value must not be negative");

            // Check product not already added
            if (this.Products.Any(p => p.Name == productName))
            {
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            FixedValueProductAddedToContractEvent fixedValueProductAddedToContractEvent =
                new FixedValueProductAddedToContractEvent(this.AggregateId, this.EstateId, productId, productName, displayText, value);
            this.ApplyAndAppend(fixedValueProductAddedToContractEvent);
        }

        /// <summary>
        /// Adds the transaction fee.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="feeType">Type of the fee.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="InvalidOperationException">Product Id {product.ProductId} is not a valid product on this contract</exception>
        /// <exception cref="System.InvalidOperationException">Product Id {productId} is not a valid product on this contract</exception>
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

        /// <summary>
        /// Disables the transaction fee.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
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

        /// <summary>
        /// Adds the variable value product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="displayText">The display text.</param>
        /// <exception cref="System.InvalidOperationException">Product Name {productName} has already been added to the contract</exception>
        public void AddVariableValueProduct(Guid productId,
                                            String productName,
                                            String displayText)
        {
            Guard.ThrowIfNullOrEmpty(productName, typeof(ArgumentNullException), "Product Name must not be null or empty");
            Guard.ThrowIfNullOrEmpty(displayText, typeof(ArgumentNullException), "Product Display Text must not be null or empty");

            // Check product not already added
            if (this.Products.Any(p => p.Name == productName))
            {
                throw new InvalidOperationException($"Product Name {productName} has already been added to the contract");
            }

            VariableValueProductAddedToContractEvent variableValueProductAddedToContractEvent =
                new VariableValueProductAddedToContractEvent(this.AggregateId, this.EstateId, productId, productName, displayText);
            this.ApplyAndAppend(variableValueProductAddedToContractEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static ContractAggregate Create(Guid aggregateId)
        {
            return new ContractAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
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

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            return this.Products;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       EstateId = Guid.NewGuid() // TODO: Populate
                   };
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        public override void PlayEvent(IDomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(ContractCreatedEvent domainEvent)
        {
            this.IsCreated = true;
            this.OperatorId = domainEvent.OperatorId;
            this.EstateId = domainEvent.EstateId;
            this.Description = domainEvent.Description;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(FixedValueProductAddedToContractEvent domainEvent)
        {
            this.Products.Add(new Product
                              {
                                  ProductId = domainEvent.ProductId,
                                  Name = domainEvent.ProductName,
                                  DisplayText = domainEvent.DisplayText,
                                  Value = domainEvent.Value
                              });
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(VariableValueProductAddedToContractEvent domainEvent)
        {
            this.Products.Add(new Product
                              {
                                  ProductId = domainEvent.ProductId,
                                  Name = domainEvent.ProductName,
                                  DisplayText = domainEvent.DisplayText,
                              });
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
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
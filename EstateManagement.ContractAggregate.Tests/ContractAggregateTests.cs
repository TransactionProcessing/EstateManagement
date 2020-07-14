using System;
using Xunit;

namespace EstateManagement.ContractAggregate.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Contract;
    using Shouldly;
    using Testing;

    public class ContractAggregateTests
    {
        [Fact]
        public void ContractAggregate_CanBeCreated_IsCreated()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);

            aggregate.AggregateId.ShouldBe(TestData.ContractId);
        }

        [Fact]
        public void ContractAggregate_Create_IsCreated()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            aggregate.AggregateId.ShouldBe(TestData.ContractId);
            aggregate.EstateId.ShouldBe(TestData.EstateId);
            aggregate.OperatorId.ShouldBe(TestData.OperatorId);
            aggregate.Description.ShouldBe(TestData.ContractDescription);
            aggregate.IsCreated.ShouldBeTrue();
        }

        [Fact]
        public void ContractAggregate_Create_InvalidEstateId_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Create(Guid.Empty, TestData.OperatorId, TestData.ContractDescription);
                                                });
        }

        [Fact]
        public void ContractAggregate_Create_InvalidOperatorId_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Create(TestData.EstateId, Guid.Empty, TestData.ContractDescription);
                                                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContractAggregate_Create_InvalidDescription_ErrorThrown(String description)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Create(TestData.EstateId, TestData.OperatorId, description);
                                                });
        }

        [Fact]
        public void ContractAggregate_AddFixedValueProduct_ProductAdded()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);

            List<Product> products = aggregate.GetProducts();
            products.Count.ShouldBe(1);
            products.First().ProductId.ShouldNotBe(Guid.Empty);
            products.First().Name.ShouldBe(TestData.ProductName);
            products.First().DisplayText.ShouldBe(TestData.ProductDisplayText);
            products.First().Value.ShouldBe(TestData.ProductFixedValue);

        }

        [Fact]
        public void ContractAggregate_AddFixedValueProduct_DuplicateProduct_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);

            Should.Throw<InvalidOperationException>(() =>
                                                {
                                                    aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);
                                                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContractAggregate_AddFixedValueProduct_InvalidProductName_ErrorThrown(String productName)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddFixedValueProduct(TestData.ProductId, productName, TestData.ProductDisplayText, TestData.ProductFixedValue);
                                                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContractAggregate_AddFixedValueProduct_InvalidProductDisplayText_ErrorThrown(String displayText)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, displayText, TestData.ProductFixedValue);
                                                });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ContractAggregate_AddFixedValueProduct_InvalidProductValue_ErrorThrown(Decimal value)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Should.Throw<ArgumentOutOfRangeException>(() =>
                                                {
                                                    aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, value);
                                                });
        }

        [Fact]
        public void ContractAggregate_AddVariableValueProduct_ProductAdded()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            products.Count.ShouldBe(1);
            products.First().ProductId.ShouldNotBe(Guid.Empty);
            products.First().Name.ShouldBe(TestData.ProductName);
            products.First().DisplayText.ShouldBe(TestData.ProductDisplayText);
            products.First().Value.ShouldBeNull();

        }

        [Fact]
        public void ContractAggregate_AddVariableValueProduct_DuplicateProduct_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContractAggregate_AddVariableValueProduct_InvalidProductName_ErrorThrown(String productName)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.AddVariableValueProduct(TestData.ProductId, productName, TestData.ProductDisplayText);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ContractAggregate_AddVariableValueProduct_InvalidProductDisplayText_ErrorThrown(String displayText)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, displayText);
            });
        }

        [Fact]
        public void ContractAggregate_GetContract_ContractReturned()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Contract contract = aggregate.GetContract();

            contract.EstateId.ShouldBe(TestData.EstateId);
            contract.ContractId.ShouldBe(TestData.ContractId);
            contract.Description.ShouldBe(TestData.ContractDescription);
            contract.IsCreated.ShouldBeTrue();
            contract.OperatorId.ShouldBe(TestData.OperatorId);
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_FixedValueProduct_TransactionFeeAdded(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);
            
            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType, TestData.TransactionFeeValue);

            List<Product> productsAfterFeeAdded = aggregate.GetProducts();
            Product productWithFees = productsAfterFeeAdded.Single();

            productWithFees.TransactionFees.ShouldHaveSingleItem();
            TransactionFee fee = productWithFees.TransactionFees.Single();
            fee.Description.ShouldBe(TestData.TransactionFeeDescription);
            fee.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
            fee.CalculationType.ShouldBe(calculationType);
            fee.Value.ShouldBe(TestData.TransactionFeeValue);
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_FixedValueProduct_InvalidFeeId_ErrorThrown(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(product, Guid.Empty, TestData.TransactionFeeDescription, calculationType, TestData.TransactionFeeValue);
                                                });
        }

        [Theory]
        [InlineData(CalculationType.Fixed, null)]
        [InlineData(CalculationType.Fixed, "")]
        [InlineData(CalculationType.Percentage, null)]
        [InlineData(CalculationType.Percentage, "")]
        public void ContractAggregate_AddTransactionFee_FixedValueProduct_InvalidFeeDescription_ErrorThrown(CalculationType calculationType, String feeDescription)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddFixedValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(product, TestData.TransactionFeeId, feeDescription, calculationType, TestData.TransactionFeeValue);
                                                });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_VariableValueProduct_TransactionFeeAdded(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType, TestData.TransactionFeeValue);

            List<Product> productsAfterFeeAdded = aggregate.GetProducts();
            Product productWithFees = productsAfterFeeAdded.Single();

            productWithFees.TransactionFees.ShouldHaveSingleItem();
            TransactionFee fee = productWithFees.TransactionFees.Single();
            fee.Description.ShouldBe(TestData.TransactionFeeDescription);
            fee.TransactionFeeId.ShouldBe(TestData.TransactionFeeId);
            fee.CalculationType.ShouldBe(calculationType);
            fee.Value.ShouldBe(TestData.TransactionFeeValue);
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_VariableValueProduct_InvalidFeeId_ErrorThrown(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(product, Guid.Empty, TestData.TransactionFeeDescription, calculationType, TestData.TransactionFeeValue);
                                                });
        }

        [Theory]
        [InlineData(CalculationType.Fixed, null)]
        [InlineData(CalculationType.Fixed, "")]
        [InlineData(CalculationType.Percentage, null)]
        [InlineData(CalculationType.Percentage, "")]
        public void ContractAggregate_AddTransactionFee_VariableValueProduct_InvalidFeeDescription_ErrorThrown(CalculationType calculationType, String feeDescription)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(product, TestData.TransactionFeeId, feeDescription, calculationType, TestData.TransactionFeeValue);
                                                });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_NullProduct_ErrorThrown(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(null, TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType, TestData.TransactionFeeValue);
                                                });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public void ContractAggregate_AddTransactionFee_ProductNotFound_ErrorThrown(CalculationType calculationType)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddFixedValueProduct(TestData.ProductId,TestData.ProductName, TestData.ProductDisplayText, TestData.ProductFixedValue);

            Should.Throw<InvalidOperationException>(() =>
                                                {
                                                    aggregate.AddTransactionFee(new Product(), TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType,TestData.TransactionFeeValue);
                                                });
        }

        [Fact]
        public void ContractAggregate_AddTransactionFee_FixedValueProduct_InvalidCalculationType_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentOutOfRangeException>(() =>
                                                      {
                                                          aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, (CalculationType)99, TestData.TransactionFeeValue);
                                                      });
        }

        [Theory]
        [InlineData(CalculationType.Fixed,0)]
        [InlineData(CalculationType.Percentage,0)]
        [InlineData(CalculationType.Fixed, -1)]
        [InlineData(CalculationType.Percentage, -1)]
        public void ContractAggregate_AddTransactionFee_VariableValueProduct_InvalidFeeValue_ErrorThrown(CalculationType calculationType, Decimal feeValue)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentOutOfRangeException>(() =>
                                                      {
                                                          aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType, feeValue);
                                                      });
        }

        [Theory]
        [InlineData(CalculationType.Fixed, 0)]
        [InlineData(CalculationType.Percentage, 0)]
        [InlineData(CalculationType.Fixed, -1)]
        [InlineData(CalculationType.Percentage, -1)]
        public void ContractAggregate_AddTransactionFee_FixedValueProduct_InvalidFeeValue_ErrorThrown(CalculationType calculationType, Decimal feeValue)
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentOutOfRangeException>(() =>
                                                      {
                                                          aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, calculationType, feeValue);
                                                      });
        }

        [Fact]
        public void ContractAggregate_AddTransactionFee_VariableValueProduct_InvalidCalculationType_ErrorThrown()
        {
            ContractAggregate aggregate = ContractAggregate.Create(TestData.ContractId);
            aggregate.Create(TestData.EstateId, TestData.OperatorId, TestData.ContractDescription);
            aggregate.AddVariableValueProduct(TestData.ProductId, TestData.ProductName, TestData.ProductDisplayText);

            List<Product> products = aggregate.GetProducts();
            Product product = products.Single();

            Should.Throw<ArgumentOutOfRangeException>(() =>
                                                      {
                                                          aggregate.AddTransactionFee(product, TestData.TransactionFeeId, TestData.TransactionFeeDescription, (CalculationType)99, TestData.TransactionFeeValue);
                                                      });
        }
    }
}

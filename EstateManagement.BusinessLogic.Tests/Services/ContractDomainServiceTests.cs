using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services;
    using ContractAggregate;
    using EstateAggregate;
    using Models.Contract;
    using Moq;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ContractDomainServiceTests
    {
        [Fact]
        public void ContractDomainService_CreateContract_ContractIsCreated()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(TestData.EstateAggregateWithOperator());
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.NotThrow(async () =>
                            {
                                await domainService.CreateContract(TestData.ContractId,
                                                                   TestData.EstateId,
                                                                   TestData.OperatorId,
                                                                   TestData.ContractDescription,
                                                                   CancellationToken.None);
                            });
        }

        [Fact]
        public void ContractDomainService_CreateContract_ContractAlreadyCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(TestData.EstateAggregateWithOperator());

            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregate);
            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.CreateContract(TestData.ContractId,
                                                                                           TestData.EstateId,
                                                                                           TestData.OperatorId,
                                                                                           TestData.ContractDescription,
                                                                                           CancellationToken.None);
                                                    });
        }

        [Fact]
        public void ContractDomainService_CreateContract_EstateNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.CreateContract(TestData.ContractId,
                                                                                           TestData.EstateId,
                                                                                           TestData.OperatorId,
                                                                                           TestData.ContractDescription,
                                                                                           CancellationToken.None);
                                                    });
        }

        [Fact]
        public void ContractDomainService_CreateContract_NoOperatorCreatedForEstate_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.CreateContract(TestData.ContractId,
                                                                                           TestData.EstateId,
                                                                                           TestData.OperatorId,
                                                                                           TestData.ContractDescription,
                                                                                           CancellationToken.None);
                                                    });
        }

        [Fact]
        public void ContractDomainService_CreateContract_OperatorNotFoundForEstate_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.CreateContract(TestData.ContractId,
                                                                                           TestData.EstateId,
                                                                                           TestData.OperatorId2,
                                                                                           TestData.ContractDescription,
                                                                                           CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_FixedValue_ProductAddedToContract()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.NotThrow(async () =>
                            {
                                await domainService.AddProductToContract(TestData.ProductId,
                                                                         TestData.ContractId,
                                                                         TestData.ProductName,
                                                                         TestData.ProductDisplayText,
                                                                         TestData.ProductFixedValue,
                                                                         CancellationToken.None);
                            });
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_FixedValue_ContractNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                            {
                                await domainService.AddProductToContract(TestData.ProductId,
                                                                         TestData.ContractId,
                                                                         TestData.ProductName,
                                                                         TestData.ProductDisplayText,
                                                                         TestData.ProductFixedValue,
                                                                         CancellationToken.None);
                            });
        }
        
        [Fact]
        public async Task ContractDomainService_AddProductToContract_VariableValue_ProductAddedToContract()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.NotThrow(async () =>
            {
                await domainService.AddProductToContract(TestData.ProductId,
                                                         TestData.ContractId,
                                                         TestData.ProductName,
                                                         TestData.ProductDisplayText,
                                                         null,
                                                         CancellationToken.None);
            });
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_VariableValue_ContractNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AddProductToContract(TestData.ProductId,
                                                         TestData.ContractId,
                                                         TestData.ProductName,
                                                         TestData.ProductDisplayText,
                                                         null,
                                                         CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_TransactionFeeIsAddedToProduct(CalculationType calculationType)
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(TestData.CreatedContractAggregateWithAProduct);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);
            Should.NotThrow(async () =>
                            {
                                await domainService.AddTransactionFeeForProductToContract(TestData.TransactionFeeId,
                                                                                          TestData.ContractId,
                                                                                          TestData.ProductId,
                                                                                          TestData.TransactionFeeDescription,
                                                                                          calculationType,
                                                                                          TestData.TransactionFeeValue,
                                                                                          CancellationToken.None);
                            });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_ContractNotCreated_ErrorThrown(CalculationType calculationType)
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(TestData.EmptyContractAggregate);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);
            Should.Throw<InvalidOperationException>(async () =>
                            {
                                await domainService.AddTransactionFeeForProductToContract(TestData.TransactionFeeId,
                                                                                          TestData.ContractId,
                                                                                          TestData.ProductId,
                                                                                          TestData.TransactionFeeDescription,
                                                                                          calculationType,
                                                                                          TestData.TransactionFeeValue,
                                                                                          CancellationToken.None);
                            });
        }

        [Theory]
        [InlineData(CalculationType.Fixed)]
        [InlineData(CalculationType.Percentage)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_ProductNotFound_ErrorThrown(CalculationType calculationType)
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            Mock<IAggregateRepository<ContractAggregate>> contractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate>>();
            contractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(TestData.CreatedContractAggregateWithAProduct);

            ContractDomainService domainService = new ContractDomainService(estateAggregateRepository.Object, contractAggregateRepository.Object);
            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.AddTransactionFeeForProductToContract(TestData.TransactionFeeId,
                                                                                                                  TestData.ContractId,
                                                                                                                  Guid.Parse("63662476-6C0F-42A8-BFD6-0C2F4B4D3144"), 
                                                                                                                  TestData.TransactionFeeDescription,
                                                                                                                  calculationType,
                                                                                                                  TestData.TransactionFeeValue,
                                                                                                                  CancellationToken.None);
                                                    });
        }
    }
}

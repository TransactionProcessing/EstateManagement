using System;
using EstateManagement.BusinessLogic.Requests;
using Shared.EventStore.EventStore;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Core;
    using BusinessLogic.Services;
    using ContractAggregate;
    using EstateAggregate;
    using Models.Contract;
    using Moq;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shouldly;
    using Testing;
    using Xunit;

    
    public class ContractDomainServiceTests {
        private ContractDomainService DomainService;
        private Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;
        private Mock<IAggregateRepository<ContractAggregate, DomainEvent>> ContractAggregateRepository;
        private Mock<IEventStoreContext> EventStoreContext;
        public ContractDomainServiceTests() {
            EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            ContractAggregateRepository = new Mock<IAggregateRepository<ContractAggregate, DomainEvent>>();
            EventStoreContext = new Mock<IEventStoreContext>();
            this.DomainService = new ContractDomainService(EstateAggregateRepository.Object, ContractAggregateRepository.Object, EventStoreContext.Object);
        }

        [Fact]
        public async Task ContractDomainService_CreateContract_ContractIsCreated()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(TestData.Aggregates.EstateAggregateWithOperator());
            
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));
            ContractAggregateRepository.Setup(c => c.SaveChanges(It.IsAny<ContractAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_CreateContract_DuplicateContractNameForOperator_ResultFailed()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.Aggregates.EstateAggregateWithOperator());
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));
            String queryResult =
                "{\r\n  \"total\": 1,\r\n  \"contractId\": \"3015e4d0-e9a9-49e5-bd55-a5492f193b62\"\r\n}";
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_CreateContract_ContractAlreadyCreated_ResultFailed()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(TestData.Aggregates.EstateAggregateWithOperator());

            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));
            
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_CreateContract_EstateNotCreated_ResultFailed()
        {
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyEstateAggregate));
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_CreateContract_NoOperatorCreatedForEstate_ResultFailed()
        {
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ContractDomainService_CreateContract_OperatorNotFoundForEstate_ResultFailed()
        {
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.CreateContractCommand command = TestData.Commands.CreateContractCommand;
            Result result = await this.DomainService.CreateContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_FixedValue_ProductAddedToContract()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));

            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));
            ContractAggregateRepository.Setup(c => c.SaveChanges(It.IsAny<ContractAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_FixedValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ContractDomainService_AddProductToContract_FixedValue_ContractNotCreated_ErrorThrown()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_FixedValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_VariableValue_ProductAddedToContract()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));

            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));
            ContractAggregateRepository.Setup(c => c.SaveChanges(It.IsAny<ContractAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_VariableValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ContractDomainService_AddProductToContract_VariableValue_ContractNotCreated_ErrorThrown()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_VariableValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_VariableValue_EstateNotCreated_ErrorThrown()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyEstateAggregate));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_VariableValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ContractDomainService_AddProductToContract_FixedValue_EstateNotCreated_ErrorThrown()
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EmptyEstateAggregate));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddProductToContractCommand command = TestData.Commands.AddProductToContractCommand_FixedValue;
            Result result = await this.DomainService.AddProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Theory]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_TransactionFeeIsAddedToProduct(DataTransferObjects.Responses.Contract.CalculationType calculationType, DataTransferObjects.Responses.Contract.FeeType feeType)
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));

            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregateWithAProduct()));
            ContractAggregateRepository.Setup(c => c.SaveChanges(It.IsAny<ContractAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddTransactionFeeForProductToContractCommand command =
                TestData.Commands.AddTransactionFeeForProductToContractCommand(calculationType, feeType);
            Result result = await this.DomainService.AddTransactionFeeForProductToContract(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Theory]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_ContractNotCreated_ErrorThrown(DataTransferObjects.Responses.Contract.CalculationType calculationType, DataTransferObjects.Responses.Contract.FeeType feeType)
        {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(Result.Success(TestData.Aggregates.EmptyContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddTransactionFeeForProductToContractCommand command =
                TestData.Commands.AddTransactionFeeForProductToContractCommand(calculationType,feeType);
            Result result = await this.DomainService.AddTransactionFeeForProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Theory]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.Merchant)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Fixed, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        [InlineData(DataTransferObjects.Responses.Contract.CalculationType.Percentage, DataTransferObjects.Responses.Contract.FeeType.ServiceProvider)]
        public async Task ContractDomainService_AddTransactionFeeForProductToContract_ProductNotFound_ErrorThrown(
            DataTransferObjects.Responses.Contract.CalculationType calculationType,
            DataTransferObjects.Responses.Contract.FeeType feeType) {
            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(TestData.Aggregates.CreatedContractAggregate()));

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.AddTransactionFeeForProductToContractCommand command =
                TestData.Commands.AddTransactionFeeForProductToContractCommand(calculationType, feeType);
            Result result = await this.DomainService.AddTransactionFeeForProductToContract(command, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Theory]
        [InlineData(CalculationType.Fixed, FeeType.Merchant)]
        [InlineData(CalculationType.Percentage, FeeType.Merchant)]
        [InlineData(CalculationType.Fixed, FeeType.ServiceProvider)]
        [InlineData(CalculationType.Percentage, FeeType.ServiceProvider)]
        public async Task ContractDomainService_DisableTransactionFeeForProduct_TransactionFeeDisabled(
            CalculationType calculationType,
            FeeType feeType) {

            EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.Aggregates.EstateAggregateWithOperator());

            ContractAggregateRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.Aggregates.CreatedContractAggregateWithAProductAndTransactionFee(calculationType, feeType));
            ContractAggregateRepository.Setup(c => c.SaveChanges(It.IsAny<ContractAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            EventStoreContext.Setup(c => c.RunTransientQuery(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("{\r\n  \"total\": 0,\r\n  \"contractId\": \"\"\r\n}");

            ContractCommands.DisableTransactionFeeForProductCommand command = TestData.Commands.DisableTransactionFeeForProductCommand;
            Result result = await this.DomainService.DisableTransactionFeeForProduct(command, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
    }
}

namespace EstateManagement.BusinessLogic.Tests.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using EstateAggregate;
using Moq;
using OperatorAggregate;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.Aggregate;
using Shouldly;
using Testing;
using Xunit;

public class OperatorDomainServiceTests{

    private IOperatorDomainService OperatorDomainService;
    private Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;
    private Mock<IAggregateRepository<OperatorAggregate, DomainEvent>> OperatorAggregateRepository;

    public OperatorDomainServiceTests(){
        this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
        this.OperatorAggregateRepository= new Mock<IAggregateRepository<OperatorAggregate, DomainEvent>>();
        this.OperatorDomainService = new OperatorDomainService(this.EstateAggregateRepository.Object,
                                                               this.OperatorAggregateRepository.Object);
    }

    [Fact]
    public async Task OperatorDomainService_CreateOperator_OperatorIsCreated(){
        this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(TestData.CreatedEstateAggregate);

        this.OperatorAggregateRepository.Setup(o => o.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyOperatorAggregate);

        await this.OperatorDomainService.CreateOperator(TestData.CreateOperatorCommand, CancellationToken.None);
    }

    [Fact]
    public async Task OperatorDomainService_CreateOperator_EstateNotCreated_ExceptionThrown()
    {
        this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EmptyEstateAggregate);

        Should.Throw<InvalidOperationException>(async () => {
                                                    await this.OperatorDomainService.CreateOperator(TestData.CreateOperatorCommand, CancellationToken.None);
                                                });

    }

    [Fact]
    public async Task OperatorDomainService_CreateOperator_OperatorAlreadyCreated_ExceptionThrown()
    {
        this.EstateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.CreatedEstateAggregate);

        this.OperatorAggregateRepository.Setup(o => o.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedOperatorAggregate);

        Should.Throw<InvalidOperationException>(async () => {
                                                    await this.OperatorDomainService.CreateOperator(TestData.CreateOperatorCommand, CancellationToken.None);
                                                });
    }

    [Fact]
    public async Task OperatorDomainService_UpdateOperator_OperatorIsUpdated()
    {
        this.OperatorAggregateRepository.Setup(o => o.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedOperatorAggregate);

        await this.OperatorDomainService.UpdateOperator(TestData.UpdateOperatorCommand, CancellationToken.None);
    }

    [Fact]
    public async Task OperatorDomainService_UpdateOperator_OperatorNotCreated_ExceptionThrown()
    {
        this.OperatorAggregateRepository.Setup(o => o.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyOperatorAggregate);
        
        Should.Throw<InvalidOperationException>(async () => {
            await this.OperatorDomainService.UpdateOperator(TestData.UpdateOperatorCommand, CancellationToken.None);
        });
    }
}
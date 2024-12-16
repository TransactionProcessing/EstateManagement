using System.Threading.Tasks;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using BusinessLogic.EventHandling;
using Estate.DomainEvents;
using Moq;
using Repository;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class EstateDomainEventHandlerTests
{
    #region Methods

    private Mock<IEstateReportingRepository> EstateReportingRepository;

    private EstateDomainEventHandler DomainEventHandler;

    public EstateDomainEventHandlerTests() {
        Logger.Initialise(NullLogger.Instance);
        this.EstateReportingRepository = new Mock<IEstateReportingRepository>();

        this.DomainEventHandler= new EstateDomainEventHandler(this.EstateReportingRepository.Object);
    }
    [Fact]
    public void EstateDomainEventHandler_EstateCreatedEvent_EventIsHandled()
    {
        EstateCreatedEvent estateCreatedEvent = TestData.EstateCreatedEvent;
        this.EstateReportingRepository
            .Setup(r => r.CreateReadModel(It.IsAny<EstateCreatedEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success);
    
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(estateCreatedEvent, CancellationToken.None); });
    }

    [Fact]
    public async Task EstateDomainEventHandler_EstateCreatedEvent_CreateReadModelFailed_EventIsHandled()
    {
        EstateCreatedEvent estateCreatedEvent = TestData.EstateCreatedEvent;
        this.EstateReportingRepository
            .Setup(r => r.CreateReadModel(It.IsAny<EstateCreatedEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure);

        var result = await this.DomainEventHandler.Handle(estateCreatedEvent, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public void EstateDomainEventHandler_EstateReferenceAllocatedEvent_EventIsHandled()
    {
        EstateReferenceAllocatedEvent estateReferenceAllocatedEvent = TestData.EstateReferenceAllocatedEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(estateReferenceAllocatedEvent, CancellationToken.None); });
    }

    [Fact]
    public void EstateDomainEventHandler_SecurityUserAddedEvent_EventIsHandled()
    {
        SecurityUserAddedToEstateEvent securityUserAddedEvent = TestData.EstateSecurityUserAddedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(securityUserAddedEvent, CancellationToken.None); });
    }

    #endregion
}
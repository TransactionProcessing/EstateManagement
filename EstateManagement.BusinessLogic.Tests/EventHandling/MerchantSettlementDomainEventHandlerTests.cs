using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.EventHandling;
using Estate.DomainEvents;
using MediatR;
using Moq;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class MerchantSettlementDomainEventHandlerTests{

    private readonly MerchantSettlementDomainEventHandler DomainEventHandler;
    private Mock<IMediator> Mediator;
    public MerchantSettlementDomainEventHandlerTests(){
        Logger.Initialise(NullLogger.Instance);
        this.Mediator = new Mock<IMediator>();
        this.DomainEventHandler = new MerchantSettlementDomainEventHandler(this.Mediator.Object);
    }

    [Fact]
    public async Task MerchantSettlementDomainEventHandler_Handle_MerchantFeeSettledEvent_EventIsHandled()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<IRequest<Result>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
        Should.NotThrow(async () =>
                        {
                            await this.DomainEventHandler.Handle(TestData.MerchantFeeSettledEvent, CancellationToken.None);
                        });
        this.Mediator.Verify(m => m.Send(It.IsAny<IRequest<Result>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void MerchantSettlementDomainEventHandler_EstateCreatedEvent_EventIsHandled()
    {
        EstateCreatedEvent domainEvent = TestData.EstateCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }
}
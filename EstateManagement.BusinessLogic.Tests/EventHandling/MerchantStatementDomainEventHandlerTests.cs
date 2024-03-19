namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.EventHandling;
using MediatR;
using Moq;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class MerchantStatementDomainEventHandlerTests
{
    private Mock<IMediator> Mediator;

    private MerchantStatementDomainEventHandler DomainEventHandler;
    public MerchantStatementDomainEventHandlerTests()
    {
        Logger.Initialise(NullLogger.Instance);
        this.Mediator = new Mock<IMediator>();
        this.DomainEventHandler = new MerchantStatementDomainEventHandler(this.Mediator.Object);
    }

    [Fact]
    public async Task MerchantStatementDomainEventHandler_Handle_StatementGeneratedEvent_EventIsHandled()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Should.NotThrow(async () =>
                        {
                            await this.DomainEventHandler.Handle(TestData.StatementGeneratedEvent, CancellationToken.None);
                        });
        this.Mediator.Verify(m=> m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MerchantStatementDomainEventHandler_Handle_TransactionHasBeenCompletedEvent_EventIsHandled()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Should.NotThrow(async () =>
                        {
                            await this.DomainEventHandler.Handle(TestData.TransactionHasBeenCompletedEvent, CancellationToken.None);
                        });
        this.Mediator.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

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
        this.Mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Should.NotThrow(async () =>
                        {
                            await this.DomainEventHandler.Handle(TestData.MerchantFeeSettledEvent, CancellationToken.None);
                        });
        this.Mediator.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
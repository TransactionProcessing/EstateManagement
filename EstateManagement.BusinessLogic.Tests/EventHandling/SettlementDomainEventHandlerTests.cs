namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using BusinessLogic.EventHandling;
using Moq;
using Repository;
using Shared.Logger;
using Shouldly;
using Testing;
using TransactionProcessor.Settlement.DomainEvents;
using TransactionProcessor.Transaction.DomainEvents;
using Xunit;

public class SettlementDomainEventHandlerTests
{
    private Mock<IEstateReportingRepository> EstateReportingRepository;

    private SettlementDomainEventHandler DomainEventHandler;

    public SettlementDomainEventHandlerTests() {
        Logger.Initialise(NullLogger.Instance);
        this.EstateReportingRepository = new Mock<IEstateReportingRepository>();
        this.DomainEventHandler = new SettlementDomainEventHandler(this.EstateReportingRepository.Object);
    }
        
    [Fact]
    public void SettlementDomainEventHandler_SettlementCreatedForDateEvent_EventIsHandled()
    {
        SettlementCreatedForDateEvent domainEvent = TestData.SettlementCreatedForDateEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void SettlementDomainEventHandler_MerchantFeeAddedPendingSettlementEvent_EventIsHandled()
    {
        MerchantFeeAddedPendingSettlementEvent domainEvent = TestData.MerchantFeeAddedPendingSettlementEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void SettlementDomainEventHandler_MerchantFeeAddedToTransactionEvent_EventIsHandled()
    {
        SettledMerchantFeeAddedToTransactionEvent domainEvent = TestData.MerchantFeeAddedToTransactionEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void SettlementDomainEventHandler_MerchantFeeSettledEvent_EventIsHandled()
    {
        MerchantFeeSettledEvent domainEvent = TestData.MerchantFeeSettledEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void SettlementDomainEventHandler_SettlementCompletedEvent_EventIsHandled()
    {
        SettlementCompletedEvent domainEvent = TestData.SettlementCompletedEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void SettlementDomainEventHandler_SettlementProcessingStartedEvent_EventIsHandled()
    {
        SettlementProcessingStartedEvent domainEvent = TestData.SettlementProcessingStartedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }
}
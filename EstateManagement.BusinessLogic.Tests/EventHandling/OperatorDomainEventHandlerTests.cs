﻿namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using BusinessLogic.EventHandling;
using EstateManagement.Estate.DomainEvents;
using Moq;
using Operator.DomainEvents;
using Repository;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class OperatorDomainEventHandlerTests{
    private Mock<IEstateReportingRepository> EstateReportingRepository;
    private OperatorDomainEventHandler DomainEventHandler;

    public OperatorDomainEventHandlerTests(){
        Logger.Initialise(NullLogger.Instance);
        this.EstateReportingRepository = new Mock<IEstateReportingRepository>();
        this.DomainEventHandler = new OperatorDomainEventHandler(this.EstateReportingRepository.Object);
    }

    [Fact]
    public void OperatorDomainEventHandler_OperatorCreatedEvent_EventIsHandled()
    {
        OperatorCreatedEvent operatorCreatedEvent = TestData.OperatorCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(operatorCreatedEvent, CancellationToken.None); });
    }

    [Fact]
    public void MerchantDomainEventHandler_EstateCreatedEvent_EventIsHandled()
    {
        EstateCreatedEvent domainEvent = TestData.EstateCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }
}
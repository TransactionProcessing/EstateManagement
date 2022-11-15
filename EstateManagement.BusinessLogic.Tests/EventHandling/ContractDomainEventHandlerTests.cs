namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using BusinessLogic.EventHandling;
using Contract.DomainEvents;
using Moq;
using Repository;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class ContractDomainEventHandlerTests
{
    #region Methods

    private Mock<IEstateReportingRepository> EstateReportingRepository;

    private ContractDomainEventHandler DomainEventHandler;
    public ContractDomainEventHandlerTests() {
        Logger.Initialise(NullLogger.Instance);
        this.EstateReportingRepository= new Mock<IEstateReportingRepository>();
        this.DomainEventHandler = new ContractDomainEventHandler(this.EstateReportingRepository.Object);
    }
        
    [Fact]
    public void ContractDomainEventHandler_ContractCreatedEvent_EventIsHandled()
    {
        ContractCreatedEvent contractCreatedEvent = TestData.ContractCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(contractCreatedEvent, CancellationToken.None); });
    }

    [Fact]
    public void ContractDomainEventHandler_FixedValueProductAddedToContractEvent_EventIsHandled()
    {
        FixedValueProductAddedToContractEvent fixedValueProductAddedToContractEvent = TestData.FixedValueProductAddedToContractEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(fixedValueProductAddedToContractEvent, CancellationToken.None); });
    }

    [Fact]
    public void ContractDomainEventHandler_TransactionFeeForProductAddedToContractEvent_EventIsHandled()
    {
        TransactionFeeForProductAddedToContractEvent transactionFeeForProductAddedToContractEvent = TestData.TransactionFeeForProductAddedToContractEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionFeeForProductAddedToContractEvent, CancellationToken.None); });
    }

    [Fact]
    public void ContractDomainEventHandler_TransactionFeeForProductDisabledEvent_EventIsHandled()
    {
        TransactionFeeForProductDisabledEvent transactionFeeForProductDisabledEvent = TestData.TransactionFeeForProductDisabledEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(transactionFeeForProductDisabledEvent, CancellationToken.None); });
    }

    [Fact]
    public void ContractDomainEventHandler_VariableValueProductAddedToContractEvent_EventIsHandled()
    {
        VariableValueProductAddedToContractEvent variableValueProductAddedToContractEvent = TestData.VariableValueProductAddedToContractEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(variableValueProductAddedToContractEvent, CancellationToken.None); });
    }

    #endregion
}
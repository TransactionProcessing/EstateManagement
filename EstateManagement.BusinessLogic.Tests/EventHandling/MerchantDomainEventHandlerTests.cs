using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Events;
    using EstateManagement.Merchant.DomainEvents;
    using EstateManagement.MerchantStatement.DomainEvents;
    using Events;
    using MediatR;
    using MerchantAggregate;
    using Moq;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using TransactionProcessor.Transaction.DomainEvents;
    using Xunit;

    public class MerchantDomainEventHandlerTests
    {
        private Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> MerchantAggregateRepository;
        private Mock<IEstateManagementRepository> EstateManagementRepository;
        private Mock<IEstateReportingRepository> EstateReportingRepository;
        private Mock<IMediator> Mediator;

        private MerchantDomainEventHandler DomainEventHandler;

        public MerchantDomainEventHandlerTests() {
            Logger.Initialise(NullLogger.Instance);

            this.Mediator = new Mock<IMediator>();
            this.MerchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();
            this.EstateManagementRepository = new Mock<IEstateManagementRepository>();
            this.EstateReportingRepository = new Mock<IEstateReportingRepository>();

            this.DomainEventHandler = new MerchantDomainEventHandler(MerchantAggregateRepository.Object,
                                                                                           EstateManagementRepository.Object,
                                                                                           EstateReportingRepository.Object,
                                                                                           Mediator.Object);
        }

        [Fact]
        public async Task MerchantDomainEventHandler_Handle_CallbackReceivedEnrichedEvent_EventIsHandled()
        {
            EstateManagementRepository.Setup(e => e.GetMerchantFromReference(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts());
            
            CallbackReceivedEnrichedEvent domainEvent = TestData.CallbackReceivedEnrichedEvent;

            Should.NotThrow(async () =>
                            {
                                await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None);
                            });

        }

        #region Methods

        [Fact]
        public void MerchantDomainEventHandler_AddressAddedEvent_EventIsHandled()
        {
            AddressAddedEvent addressAddedEvent = TestData.AddressAddedEvent;
            
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(addressAddedEvent, CancellationToken.None); });
        }
        
        [Fact]
        public void MerchantDomainEventHandler_ContactAddedEvent_EventIsHandled()
        {
            ContactAddedEvent contactAddedEvent = TestData.ContactAddedEvent;

            
            

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(contactAddedEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_MerchantReferenceAllocatedEvent_EventIsHandled()
        {
            MerchantReferenceAllocatedEvent merchantReferenceAllocatedEvent = TestData.MerchantReferenceAllocatedEvent;
            
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(merchantReferenceAllocatedEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_DeviceAddedToMerchantEvent_EventIsHandled()
        {
            DeviceAddedToMerchantEvent deviceAddedToMerchantEvent = TestData.DeviceAddedToMerchantEvent;
            
            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(deviceAddedToMerchantEvent, CancellationToken.None); });
        }
        
        [Fact]
        public void MerchantDomainEventHandler_MerchantCreatedEvent_EventIsHandled()
        {
            MerchantCreatedEvent merchantCreatedEvent = TestData.MerchantCreatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(merchantCreatedEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_OperatorAssignedToMerchantEvent_EventIsHandled()
        {
            OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent = TestData.OperatorAssignedToMerchantEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(operatorAssignedToMerchantEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_SecurityUserAddedEvent_EventIsHandled()
        {
            SecurityUserAddedToMerchantEvent merchantSecurityUserAddedEvent = TestData.MerchantSecurityUserAddedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(merchantSecurityUserAddedEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_SettlementScheduleChangedEvent_EventIsHandled()
        {
            SettlementScheduleChangedEvent settlementScheduleChangedEvent = TestData.SettlementScheduleChangedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(settlementScheduleChangedEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_SettlementGeneratedEvent_EventIsHandled()
        {
            StatementGeneratedEvent statementGeneratedEvent = TestData.StatementGeneratedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(statementGeneratedEvent, CancellationToken.None); });
        }

        [Fact]
        public void SettlementDomainEventHandler_TransactionHasBeenCompletedEvent_EventIsHandled()
        {
            TransactionHasBeenCompletedEvent domainEvent = TestData.TransactionHasBeenCompletedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void SettlementDomainEventHandler_ContractAddedToMerchantEvent_EventIsHandled()
        {
            ContractAddedToMerchantEvent domainEvent = TestData.ContractAddedToMerchantEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        #endregion
    }
}

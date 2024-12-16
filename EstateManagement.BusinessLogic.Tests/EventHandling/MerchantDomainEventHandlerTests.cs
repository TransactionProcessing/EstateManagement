using System;
using System.Threading.Tasks;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Events;
    using EstateManagement.BusinessLogic.Requests;
    using EstateManagement.Estate.DomainEvents;
    using EstateManagement.Merchant.DomainEvents;
    using EstateManagement.MerchantStatement.DomainEvents;
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
        public async Task MerchantDomainEventHandler_Handle_CallbackReceivedEnrichedEvent_Deposit_EventIsHandled()
        {
            EstateManagementRepository.Setup(e => e.GetMerchantFromReference(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(Result.Success(TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts()));
            this.Mediator.Setup(m => m.Send(It.IsAny<MerchantCommands.MakeMerchantDepositCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            CallbackReceivedEnrichedEvent domainEvent = TestData.CallbackReceivedEnrichedEventDeposit;

            var result = await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();

        }

        [Fact]
        public async Task MerchantDomainEventHandler_Handle_CallbackReceivedEnrichedEvent_OtherType_EventIsHandled()
        {
            EstateManagementRepository.Setup(e => e.GetMerchantFromReference(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts());

            CallbackReceivedEnrichedEvent domainEvent = TestData.CallbackReceivedEnrichedEventOtherType;

            var result = await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task MerchantDomainEventHandler_Handle_CallbackReceivedEnrichedEvent_Deposit_GetMerchantFailed_ResultIsFailure()
        {
            EstateManagementRepository.Setup(e => e.GetMerchantFromReference(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure());

            CallbackReceivedEnrichedEvent domainEvent = TestData.CallbackReceivedEnrichedEventDeposit;

            var result = await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();

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
        public void MerchantDomainEventHandler_TransactionHasBeenCompletedEvent_EventIsHandled()
        {
            TransactionHasBeenCompletedEvent domainEvent = TestData.TransactionHasBeenCompletedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_MerchantNameUpdatedEvent_EventIsHandled()
        {
            MerchantNameUpdatedEvent domainEvent = TestData.MerchantNameUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_DeviceSwappedForMerchantEvent_EventIsHandled()
        {
            DeviceSwappedForMerchantEvent domainEvent = TestData.DeviceSwappedForMerchantEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_OperatorRemovedFromMerchantEvent_EventIsHandled()
        {
            OperatorRemovedFromMerchantEvent domainEvent = TestData.OperatorRemovedFromMerchantEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantAddressLine1UpdatedEvent_EventIsHandled()
        {
            MerchantAddressLine1UpdatedEvent domainEvent = TestData.MerchantAddressLine1UpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantAddressLine2UpdatedEvent_EventIsHandled()
        {
            MerchantAddressLine2UpdatedEvent domainEvent = TestData.MerchantAddressLine2UpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantAddressLine3UpdatedEvent_EventIsHandled()
        {
            MerchantAddressLine3UpdatedEvent domainEvent = TestData.MerchantAddressLine3UpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantAddressLine4UpdatedEvent_EventIsHandled()
        {
            MerchantAddressLine4UpdatedEvent domainEvent = TestData.MerchantAddressLine4UpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantCountyUpdatedEvent_EventIsHandled()
        {
            MerchantCountyUpdatedEvent domainEvent = TestData.MerchantCountyUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantRegionUpdatedEvent_EventIsHandled()
        {
            MerchantRegionUpdatedEvent domainEvent = TestData.MerchantRegionUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantTownUpdatedEvent_EventIsHandled()
        {
            MerchantTownUpdatedEvent domainEvent = TestData.MerchantTownUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantPostalCodeUpdatedEvent_EventIsHandled()
        {
            MerchantPostalCodeUpdatedEvent domainEvent = TestData.MerchantPostalCodeUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantContactNameUpdatedEvent_EventIsHandled()
        {
            MerchantContactNameUpdatedEvent domainEvent = TestData.MerchantContactNameUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantContactEmailAddressUpdatedEvent_EventIsHandled()
        {
            MerchantContactEmailAddressUpdatedEvent domainEvent = TestData.MerchantContactEmailAddressUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }
        [Fact]
        public void MerchantDomainEventHandler_MerchantContactPhoneNumberUpdatedEvent_EventIsHandled()
        {
            MerchantContactPhoneNumberUpdatedEvent domainEvent = TestData.MerchantContactPhoneNumberUpdatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_ContractAddedToMerchantEvent_EventIsHandled()
        {
            ContractAddedToMerchantEvent domainEvent = TestData.ContractAddedToMerchantEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void MerchantDomainEventHandler_EstateCreatedEvent_EventIsHandled()
        {
            EstateCreatedEvent domainEvent = TestData.EstateCreatedEvent;

            Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
        }

        #endregion
    }
}

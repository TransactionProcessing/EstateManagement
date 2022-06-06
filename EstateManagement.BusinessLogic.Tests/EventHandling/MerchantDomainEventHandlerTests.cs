using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.EventHandling
{
    using System.Threading;
    using BusinessLogic.EventHandling;
    using Events;
    using MediatR;
    using MerchantAggregate;
    using Moq;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantDomainEventHandlerTests
    {
        [Fact]
        public async Task MerchantDomainEventHandler_Handle_CallbackReceivedEnrichedEvent_EventIsHandled()
        {
            Mock<IAggregateRepository<MerchantAggregate, DomainEvent>> merchantAggregateRepository =
                new Mock<IAggregateRepository<MerchantAggregate, DomainEvent>>();
            Mock<IEstateManagementRepository> estateManagementRepository = new Mock<IEstateManagementRepository>();
            estateManagementRepository.Setup(e => e.GetMerchantFromReference(It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(TestData.MerchantModelWithAddressesContactsDevicesAndOperators());
            Mock<IMediator> mediator = new Mock<IMediator>();

            MerchantDomainEventHandler domainEventHandler = new MerchantDomainEventHandler(merchantAggregateRepository.Object,
                                                                                           estateManagementRepository.Object,
                                                                                           mediator.Object);

            CallbackReceivedEnrichedEvent domainEvent = TestData.CallbackReceivedEnrichedEvent;

            Should.NotThrow(async () =>
                            {
                                await domainEventHandler.Handle(domainEvent, CancellationToken.None);
                            });

        }
    }
}

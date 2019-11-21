namespace EstateManagement.BusinessLogic.Tests.CommandHandler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Commands;
    using BusinessLogic.Services;
    using CommandHandlers;
    using Commands;
    using EstateAggregate;
    using MerchantAggregate;
    using Moq;
    using Services;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class CommandRouterTests
    {
        [Fact]
        public void CommandRouter_CreateEstateCommand_IsRouted()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EstateAggregate());
            estateAggregateRepository.Setup(e => e.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            ICommandRouter router = new CommandRouter(aggregateRepositoryManager.Object, merchantDomainService.Object);

            CreateEstateCommand command = TestData.CreateEstateCommand;

            Should.NotThrow(async () =>
                            {
                                await router.Route(command, CancellationToken.None);
                            });
        }

        [Fact]
        public void CommandRouter_CreateMerchantCommand_IsRouted()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EstateAggregate());
            estateAggregateRepository.Setup(e => e.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            merchantDomainService.Setup(e => e.CreateMerchant(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<Guid>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<Guid>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<String>(),
                                                              It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            ICommandRouter router = new CommandRouter(aggregateRepositoryManager.Object, merchantDomainService.Object);

            CreateMerchantCommand command = TestData.CreateMerchantCommand;

            Should.NotThrow(async () =>
                            {
                                await router.Route(command, CancellationToken.None);
                            });

        }
    }
}

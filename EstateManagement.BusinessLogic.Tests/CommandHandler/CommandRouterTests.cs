namespace EstateManagement.BusinessLogic.Tests.CommandHandler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Commands;
    using EstateAggregate;
    using Moq;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;
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
            ICommandRouter router = new CommandRouter(estateAggregateRepository.Object);

            CreateEstateCommand command = TestData.CreateEstateCommand;

            Should.NotThrow(async () =>
                            {
                                await router.Route(command, CancellationToken.None);
                            });
            
        }
    }
}

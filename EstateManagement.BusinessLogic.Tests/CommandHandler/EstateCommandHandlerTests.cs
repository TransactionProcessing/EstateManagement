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
    using Moq;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateCommandHandlerTests
    {
        [Fact]
        public void EstateCommandHandler_CreateEstateCommand_IsHandled()
        {
            Mock<IEstateDomainService> estateDomainService = new Mock<IEstateDomainService>();
            ICommandHandler handler = new EstateCommandHandler(estateDomainService.Object);

            CreateEstateCommand command = TestData.CreateEstateCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(command, CancellationToken.None);
                            });

        }
    }
}
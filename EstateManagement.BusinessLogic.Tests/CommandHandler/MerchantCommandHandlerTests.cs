namespace EstateManagement.BusinessLogic.Tests.CommandHandler
{
    using System.Threading;
    using BusinessLogic.Commands;
    using BusinessLogic.Services;
    using CommandHandlers;
    using Moq;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantCommandHandlerTests
    {
        [Fact]
        public void MerchantCommandHandler_CreateMerchantCommand_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            ICommandHandler handler = new MerchantCommandHandler(merchantDomainService.Object);

            CreateMerchantCommand command = TestData.CreateMerchantCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(command, CancellationToken.None);
                            });

        }
    }
}
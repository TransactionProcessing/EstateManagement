namespace EstateManagement.BusinessLogic.Tests.CommandHandler
{
    using System.Threading;
    using BusinessLogic.Services;
    using Moq;
    using RequestHandlers;
    using Requests;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantRequestHandlerTests
    {
        [Fact]
        public void MerchantRequestHandler_CreateMerchantRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);
            
            Should.NotThrow(async () =>
                            {
                                await handler.Handle(TestData.CreateMerchantCommand, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_AssignOperatorToMerchantRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.AssignOperatorToMerchantCommand request = TestData.AssignOperatorToMerchantCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_CreateMerchantUserRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.CreateMerchantUserCommand request = TestData.CreateMerchantUserCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_AddMerchantDeviceRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.AddMerchantDeviceCommand command = TestData.AddMerchantDeviceCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(command, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_MakeMerchantDepositRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.MakeMerchantDepositCommand request = TestData.MakeMerchantDepositCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_MakeMerchantWithdrawalRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.MakeMerchantWithdrawalCommand request = TestData.MakeMerchantWithdrawalCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_SetMerchantSettlementScheduleRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            SetMerchantSettlementScheduleRequest request = TestData.SetMerchantSettlementScheduleRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_SwapMerchantDeviceRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            SwapMerchantDeviceRequest request = TestData.SwapMerchantDeviceRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_AddMerchantContractRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MerchantCommands.AddMerchantContractCommand request = TestData.AddMerchantContractCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }
    }
}
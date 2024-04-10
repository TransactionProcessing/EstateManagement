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

            AssignOperatorToMerchantRequest request = TestData.AssignOperatorToMerchantRequest;

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

            CreateMerchantUserRequest request = TestData.CreateMerchantUserRequest;

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

            AddMerchantDeviceRequest request = TestData.AddMerchantDeviceRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_MakeMerchantDepositRequest_IsHandled()
        {
            Mock<IMerchantDomainService> merchantDomainService = new Mock<IMerchantDomainService>();
            MerchantRequestHandler handler = new MerchantRequestHandler(merchantDomainService.Object);

            MakeMerchantDepositRequest request = TestData.MakeMerchantDepositRequest;

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

            MakeMerchantWithdrawalRequest request = TestData.MakeMerchantWithdrawalRequest;

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

            AddMerchantContractRequest request = TestData.AddMerchantContractRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }
    }
}
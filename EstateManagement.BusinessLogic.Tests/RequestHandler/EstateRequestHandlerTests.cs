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

    public class EstateRequestHandlerTests
    {
        [Fact]
        public void EstateRequestHandler_CreateEstateRequest_IsHandled()
        {
            Mock<IEstateDomainService> estateDomainService = new Mock<IEstateDomainService>();
            EstateRequestHandler handler = new EstateRequestHandler(estateDomainService.Object);

            CreateEstateRequest request = TestData.CreateEstateRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void EstateRequestHandler_AddOperatorToEstateRequest_IsHandled()
        {
            Mock<IEstateDomainService> estateDomainService = new Mock<IEstateDomainService>();
            EstateRequestHandler handler = new EstateRequestHandler(estateDomainService.Object);

            AddOperatorToEstateRequest request = TestData.AddOperatorToEstateRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void EstateRequestHandler_CreateEstateUserRequest_IsHandled()
        {
            Mock<IEstateDomainService> estateDomainService = new Mock<IEstateDomainService>();
            EstateRequestHandler handler = new EstateRequestHandler(estateDomainService.Object);

            CreateEstateUserRequest request = TestData.CreateEstateUserRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }
    }
}
﻿using System;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.RequestHandler
{
    using System.Threading;
    using BusinessLogic.Services;
    using Moq;
    using RequestHandlers;
    using Requests;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantStatementRequestHandlerTests
    {
        [Fact]
        public void MerchantStatementRequestHandler_AddTransactionToMerchantStatementRequest_IsHandled()
        {
            Mock<IMerchantStatementDomainService> merchantDomainService = new Mock<IMerchantStatementDomainService>(MockBehavior.Strict);
            merchantDomainService.Setup(m => m.AddTransactionToStatement(It.IsAny<Guid>(),
                                                                         It.IsAny<Guid>(),
                                                                         It.IsAny<DateTime>(),
                                                                         It.IsAny<Decimal?>(),
                                                                         It.IsAny<Boolean>(),
                                                                         It.IsAny<Guid>(),
                                                                         It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            MerchantStatementRequestHandler handler = new MerchantStatementRequestHandler(merchantDomainService.Object);

            AddTransactionToMerchantStatementRequest request = TestData.AddTransactionToMerchantStatementRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantStatementRequestHandler_AddSettledFeeToMerchantStatementRequest_IsHandled()
        {
            Mock<IMerchantStatementDomainService> merchantDomainService = new Mock<IMerchantStatementDomainService>(MockBehavior.Strict);
            merchantDomainService.Setup(m => m.AddSettledFeeToStatement(It.IsAny<Guid>(),
                                                                         It.IsAny<Guid>(),
                                                                         It.IsAny<DateTime>(),
                                                                         It.IsAny<Decimal>(),
                                                                         It.IsAny<Guid>(),
                                                                         It.IsAny<Guid>(),
                                                                         It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            MerchantStatementRequestHandler handler = new MerchantStatementRequestHandler(merchantDomainService.Object);

            AddSettledFeeToMerchantStatementRequest request = TestData.AddSettledFeeToMerchantStatementRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantStatementRequestHandler_GenerateMerchantStatementCommand_IsHandled()
        {
            Mock<IMerchantStatementDomainService> merchantDomainService = new Mock<IMerchantStatementDomainService>(MockBehavior.Strict);
            merchantDomainService.Setup(m => m.GenerateStatement(It.IsAny<MerchantCommands.GenerateMerchantStatementCommand>(),It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(TestData.MerchantStatementId);
            MerchantStatementRequestHandler handler = new MerchantStatementRequestHandler(merchantDomainService.Object);

            MerchantCommands.GenerateMerchantStatementCommand request = TestData.GenerateMerchantStatementCommand;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantStatementRequestHandler_EmailMerchantStatementRequest_IsHandled()
        {
            Mock<IMerchantStatementDomainService> merchantDomainService = new Mock<IMerchantStatementDomainService>(MockBehavior.Strict);
            merchantDomainService.Setup(m => m.EmailStatement(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask);
            MerchantStatementRequestHandler handler = new MerchantStatementRequestHandler(merchantDomainService.Object);

            EmailMerchantStatementRequest request = TestData.EmailMerchantStatementRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });

        }
    }
}

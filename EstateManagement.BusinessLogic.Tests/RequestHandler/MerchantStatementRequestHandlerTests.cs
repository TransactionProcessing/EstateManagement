﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void MerchantStatementRequestHandler_CreateMerchantRequest_IsHandled()
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
    }
}

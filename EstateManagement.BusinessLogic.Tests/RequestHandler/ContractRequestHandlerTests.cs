using System;
using System.Collections.Generic;
using System.Text;

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

    public class ContractRequestHandlerTests
    {
        [Fact]
        public void ContractRequestHandler_CreateContractRequest_IsHandled()
        {
            Mock<IContractDomainService> contractDomainService = new Mock<IContractDomainService>();
            ContractRequestHandler handler = new ContractRequestHandler(contractDomainService.Object);

            CreateContractRequest request = TestData.CreateContractRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void ContractRequestHandler_AddProductToContractRequest_IsHandled()
        {
            Mock<IContractDomainService> contractDomainService = new Mock<IContractDomainService>();
            ContractRequestHandler handler = new ContractRequestHandler(contractDomainService.Object);

            AddProductToContractRequest request = TestData.AddProductToContractRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void ContractRequestHandler_AddTransactionFeeForProductToContractRequest_IsHandled()
        {
            Mock<IContractDomainService> contractDomainService = new Mock<IContractDomainService>();
            ContractRequestHandler handler = new ContractRequestHandler(contractDomainService.Object);

            AddTransactionFeeForProductToContractRequest request = TestData.AddTransactionFeeForProductToContractRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void ContractRequestHandler_DisableTransactionFeeForProductRequest_IsHandled()
        {
            Mock<IContractDomainService> contractDomainService = new Mock<IContractDomainService>();
            ContractRequestHandler handler = new ContractRequestHandler(contractDomainService.Object);

            DisableTransactionFeeForProductRequest request = TestData.DisableTransactionFeeForProductRequest;

            Should.NotThrow(async () =>
                            {
                                await handler.Handle(request, CancellationToken.None);
                            });
        }
    }
}

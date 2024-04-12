namespace EstateManagement.BusinessLogic.Tests.CommandHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using BusinessLogic.Services;
    using Manger;
    using Models.Merchant;
    using Moq;
    using RequestHandlers;
    using Requests;
    using Shared.Exceptions;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantRequestHandlerTests{
        private MerchantRequestHandler MerchantRequestHandler;

        private Mock<IMerchantDomainService> MerchantDomainService;
        private Mock<IEstateManagementManager> EstateManagementManager;
        public MerchantRequestHandlerTests(){
            this.MerchantDomainService = new Mock<IMerchantDomainService>();
            this.EstateManagementManager = new Mock<IEstateManagementManager>();
            this.MerchantRequestHandler = new MerchantRequestHandler(this.MerchantDomainService.Object, this.EstateManagementManager.Object);
        }
        [Fact]
        public void MerchantRequestHandler_CreateMerchantCommand_IsHandled()
        {
            Should.NotThrow(async () =>
                          {
                              await this.MerchantRequestHandler.Handle(TestData.CreateMerchantCommand, CancellationToken.None);
                          });

        }

        [Fact]
        public void MerchantRequestHandler_AssignOperatorToMerchantCommand_IsHandled()
        {
            MerchantCommands.AssignOperatorToMerchantCommand request = TestData.AssignOperatorToMerchantCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_CreateMerchantUserCommand_IsHandled()
        {
            MerchantCommands.CreateMerchantUserCommand request = TestData.CreateMerchantUserCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_AddMerchantDeviceCommand_IsHandled()
        {
            MerchantCommands.AddMerchantDeviceCommand command = TestData.AddMerchantDeviceCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(command, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_MakeMerchantDepositCommand_IsHandled()
        {
            MerchantCommands.MakeMerchantDepositCommand request = TestData.MakeMerchantDepositCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_MakeMerchantWithdrawalCommand_IsHandled()
        {
            MerchantCommands.MakeMerchantWithdrawalCommand request = TestData.MakeMerchantWithdrawalCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        //[Fact]
        //public void MerchantRequestHandler_SetMerchantSettlementScheduleRequest_IsHandled()
        //{
        //    SetMerchantSettlementScheduleRequest request = TestData.SetMerchantSettlementScheduleRequest;

        //    Should.NotThrow(async () =>
        //                    {
        //                        await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
        //                    });

        //}

        [Fact]
        public void MerchantRequestHandler_SwapMerchantDeviceCommand_IsHandled()
        {
            MerchantCommands.SwapMerchantDeviceCommand request = TestData.SwapMerchantDeviceCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_AddMerchantContractCommand_IsHandled()
        {
            MerchantCommands.AddMerchantContractCommand request = TestData.AddMerchantContractCommand;

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });

        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantsQuery_IsHandled(){
            MerchantQueries.GetMerchantsQuery request = new MerchantQueries.GetMerchantsQuery(TestData.EstateId);

            this.EstateManagementManager.Setup(m => m.GetMerchants(It.IsAny<Guid>(),
                                                                  It.IsAny<CancellationToken>())).ReturnsAsync(new List<Merchant>{
                                                                                                                                     TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts()
                                                                                                                                 });

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantsQuery_NullMerchants_IsHandled()
        {
            MerchantQueries.GetMerchantsQuery request = new MerchantQueries.GetMerchantsQuery(TestData.EstateId);

            List<Merchant> merchants = null;
            this.EstateManagementManager.Setup(m => m.GetMerchants(It.IsAny<Guid>(),
                                                                   It.IsAny<CancellationToken>())).ReturnsAsync(merchants);

            Should.Throw<NotFoundException>(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantsQuery_EmptyMerchants_IsHandled()
        {
            MerchantQueries.GetMerchantsQuery request = new MerchantQueries.GetMerchantsQuery(TestData.EstateId);

            List<Merchant> merchants = new List<Merchant>();
            this.EstateManagementManager.Setup(m => m.GetMerchants(It.IsAny<Guid>(),
                                                                   It.IsAny<CancellationToken>())).ReturnsAsync(merchants);

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantContractsQuery_IsHandled()
        {
            MerchantQueries.GetMerchantContractsQuery request = new MerchantQueries.GetMerchantContractsQuery(TestData.EstateId, TestData.MerchantId);

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantQuery_IsHandled()
        {
            MerchantQueries.GetMerchantQuery request = new MerchantQueries.GetMerchantQuery(TestData.EstateId, TestData.MerchantId);

            this.EstateManagementManager.Setup(m => m.GetMerchant(It.IsAny<Guid>(),
                                                                  It.IsAny<Guid>(),
                                                                  It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantModelWithAddressesContactsDevicesAndOperatorsAndContracts());

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetMerchantQuery_NullMerchantReturned_IsHandled()
        {
            MerchantQueries.GetMerchantQuery request = new MerchantQueries.GetMerchantQuery(TestData.EstateId, TestData.MerchantId);
            Merchant merchant = null;
            this.EstateManagementManager.Setup(m => m.GetMerchant(It.IsAny<Guid>(),
                                                                  It.IsAny<Guid>(),
                                                                  It.IsAny<CancellationToken>())).ReturnsAsync(merchant);

            Should.Throw<NotFoundException>(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantRequestHandler_GetTransactionFeesForProductQuery_IsHandled()
        {
            MerchantQueries.GetTransactionFeesForProductQuery request = new MerchantQueries.GetTransactionFeesForProductQuery(TestData.EstateId, TestData.MerchantId, TestData.ContractId, TestData.ProductId);

            Should.NotThrow(async () =>
                            {
                                await this.MerchantRequestHandler.Handle(request, CancellationToken.None);
                            });
        }
    }
}
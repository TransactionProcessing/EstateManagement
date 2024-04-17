namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Responses.Merchant;
    using EstateManagement.Database.Entities;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using EstateManagement.Models.Estate;
    using Manger;
    using MediatR;
    using Models.Contract;
    using Models.Merchant;
    using Requests;
    using Services;
    using Shared.Exceptions;
    using Merchant = Models.Merchant.Merchant;

    public class MerchantRequestHandler : IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand,Guid>,
                                          IRequestHandler<MerchantCommands.AddMerchantContractCommand>,
                                          IRequestHandler<MerchantCommands.CreateMerchantCommand,Guid>,
                                          IRequestHandler<MerchantCommands.AssignOperatorToMerchantCommand, Guid>,
                                          IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Guid>,
                                          IRequestHandler<MerchantCommands.CreateMerchantUserCommand, Guid>,
                                          IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Guid>, 
                                          IRequestHandler<MerchantCommands.MakeMerchantWithdrawalCommand, Guid>,
                                          IRequestHandler<MerchantQueries.GetMerchantQuery, Models.Merchant.Merchant>,
                                          IRequestHandler<MerchantQueries.GetMerchantContractsQuery, List<Models.Contract.Contract>>,
                                          IRequestHandler<MerchantQueries.GetMerchantsQuery, List<Models.Merchant.Merchant>>,
                                          IRequestHandler<MerchantQueries.GetTransactionFeesForProductQuery, List<Models.Contract.TransactionFee>>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantCommand>,
                                          IRequestHandler<MerchantCommands.AddMerchantAddressCommand>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantAddressCommand>,
                                          IRequestHandler<MerchantCommands.AddMerchantContactCommand>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantContactCommand>,
                                          IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand>
    {
        #region Fields

        private readonly IMerchantDomainService MerchantDomainService;

        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        #region Constructors

        public MerchantRequestHandler(IMerchantDomainService merchantDomainService, 
                                      IEstateManagementManager estateManagementManager){
            this.MerchantDomainService = merchantDomainService;
            this.EstateManagementManager = estateManagementManager;
        }

        #endregion

        #region Methods
        
        public async Task<Guid> Handle(MerchantCommands.AssignOperatorToMerchantCommand command,
                                       CancellationToken cancellationToken) {
            Guid result = await this.MerchantDomainService.AssignOperatorToMerchant(command, cancellationToken);
            return result;
        }

        public async Task<Guid> Handle(MerchantCommands.CreateMerchantUserCommand command,
                                       CancellationToken cancellationToken) {
            Guid userId = await this.MerchantDomainService.CreateMerchantUser(command, cancellationToken);

            return userId;
        }

        public async Task<Guid> Handle(MerchantCommands.AddMerchantDeviceCommand command,
                                       CancellationToken cancellationToken)
        {
            Guid result = await this.MerchantDomainService.AddDeviceToMerchant(command, cancellationToken);
            return result;
        }

        public async Task<Guid> Handle(MerchantCommands.MakeMerchantDepositCommand command,
                                       CancellationToken cancellationToken) {
            Guid depositId = await this.MerchantDomainService.MakeMerchantDeposit(command, cancellationToken);

            return depositId;
        }
        
        public async Task<Guid> Handle(MerchantCommands.SwapMerchantDeviceCommand command,
                                       CancellationToken cancellationToken) {
            Guid deviceId = await this.MerchantDomainService.SwapMerchantDevice(command, cancellationToken);
            return deviceId;
        }

        public async Task<Guid> Handle(MerchantCommands.MakeMerchantWithdrawalCommand command,
                                       CancellationToken cancellationToken) {
            Guid withdrawalId = await this.MerchantDomainService.MakeMerchantWithdrawal(command, cancellationToken);
            return withdrawalId;
        }

        #endregion

        public async Task Handle(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.AddContractToMerchant(command, cancellationToken);
        }

        public async Task<Guid> Handle(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken){
            Guid result = await this.MerchantDomainService.CreateMerchant(command, cancellationToken);
            return result;
        }

        public async Task<Models.Merchant.Merchant> Handle(MerchantQueries.GetMerchantQuery query, CancellationToken cancellationToken){
            Models.Merchant.Merchant merchant = await this.EstateManagementManager.GetMerchant(query.EstateId, query.MerchantId, cancellationToken);

            //if (merchant == null){
            //    throw new NotFoundException($"Merchant not found with estate Id {query.EstateId} and merchant Id {query.MerchantId}");
            //}

            return merchant;
        }

        public async Task<List<Models.Contract.Contract>> Handle(MerchantQueries.GetMerchantContractsQuery query, CancellationToken cancellationToken){
            List<Models.Contract.Contract> contracts = await this.EstateManagementManager.GetMerchantContracts(query.EstateId, query.MerchantId, cancellationToken);
            return contracts;
        }

        public async Task<List<Merchant>> Handle(MerchantQueries.GetMerchantsQuery query, CancellationToken cancellationToken){
            List<Merchant> merchants = await this.EstateManagementManager.GetMerchants(query.EstateId, cancellationToken);
            
            return merchants;
        }

        public async Task<List<TransactionFee>> Handle(MerchantQueries.GetTransactionFeesForProductQuery query, CancellationToken cancellationToken){
            List<TransactionFee> transactionFees =
                    await this.EstateManagementManager.GetTransactionFeesForProduct(query.EstateId, query.MerchantId, query.ContractId, query.ProductId, cancellationToken);

            return transactionFees;
        }

        public async Task Handle(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.UpdateMerchant(command, cancellationToken);
        }

        public async Task Handle(MerchantCommands.AddMerchantAddressCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.AddMerchantAddress(command, cancellationToken);
        }

        public async Task Handle(MerchantCommands.UpdateMerchantAddressCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.UpdateMerchantAddress(command, cancellationToken);
        }

        public async Task Handle(MerchantCommands.AddMerchantContactCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.AddMerchantContact(command, cancellationToken);
        }

        public async Task Handle(MerchantCommands.UpdateMerchantContactCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.UpdateMerchantContact(command, cancellationToken);
        }

        public async Task Handle(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken){
            await this.MerchantDomainService.RemoveOperatorFromMerchant(command, cancellationToken);
        }
    }
}
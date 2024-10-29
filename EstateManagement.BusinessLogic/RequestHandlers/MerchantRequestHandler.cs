using EstateManagement.Models.Contract;
using SimpleResults;

namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Manger;
    using MediatR;
    using Requests;
    using Services;
    using Merchant = Models.Merchant.Merchant;

    public class MerchantRequestHandler : IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand, Result>,
                                          IRequestHandler<MerchantCommands.AddMerchantContractCommand, Result>,
                                          IRequestHandler<MerchantCommands.CreateMerchantCommand, Result>,
                                          IRequestHandler<MerchantCommands.AssignOperatorToMerchantCommand, Result>,
                                          IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Result>,
                                          IRequestHandler<MerchantCommands.CreateMerchantUserCommand, Result>,
                                          IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Result>, 
                                          IRequestHandler<MerchantCommands.MakeMerchantWithdrawalCommand, Result>,
                                          IRequestHandler<MerchantQueries.GetMerchantQuery, Result<Merchant>>,
                                          IRequestHandler<MerchantQueries.GetMerchantContractsQuery, Result<List<Models.Contract.Contract>>>,
                                          IRequestHandler<MerchantQueries.GetMerchantsQuery, Result<List<Merchant>>>,
                                          IRequestHandler<MerchantQueries.GetTransactionFeesForProductQuery, Result<List<Models.Contract.ContractProductTransactionFee>>>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantCommand, Result>,
                                          IRequestHandler<MerchantCommands.AddMerchantAddressCommand, Result>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantAddressCommand, Result>,
                                          IRequestHandler<MerchantCommands.AddMerchantContactCommand, Result>,
                                          IRequestHandler<MerchantCommands.UpdateMerchantContactCommand, Result>,
                                          IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand, Result>,
                                          IRequestHandler<MerchantCommands.RemoveMerchantContractCommand, Result>
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
        
        public async Task<Result> Handle(MerchantCommands.AssignOperatorToMerchantCommand command,
                                         CancellationToken cancellationToken) {
            return await this.MerchantDomainService.AssignOperatorToMerchant(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.CreateMerchantUserCommand command,
                                         CancellationToken cancellationToken) {
            return await this.MerchantDomainService.CreateMerchantUser(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AddMerchantDeviceCommand command,
                                         CancellationToken cancellationToken)
        {
            return await this.MerchantDomainService.AddDeviceToMerchant(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.MakeMerchantDepositCommand command,
                                         CancellationToken cancellationToken) {
            return await this.MerchantDomainService.MakeMerchantDeposit(command, cancellationToken);
        }
        
        public async Task<Result> Handle(MerchantCommands.SwapMerchantDeviceCommand command,
                                         CancellationToken cancellationToken) {
            return await this.MerchantDomainService.SwapMerchantDevice(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.MakeMerchantWithdrawalCommand command,
                                         CancellationToken cancellationToken) {
            return await this.MerchantDomainService.MakeMerchantWithdrawal(command, cancellationToken);
        }

        #endregion

        public async Task<Result> Handle(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.AddContractToMerchant(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.CreateMerchant(command, cancellationToken);
        }

        public async Task<Result<Merchant>> Handle(MerchantQueries.GetMerchantQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetMerchant(query.EstateId, query.MerchantId, cancellationToken);
        }

        public async Task<Result<List<Models.Contract.Contract>>> Handle(MerchantQueries.GetMerchantContractsQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetMerchantContracts(query.EstateId, query.MerchantId, cancellationToken);
        }

        public async Task<Result<List<Merchant>>> Handle(MerchantQueries.GetMerchantsQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetMerchants(query.EstateId, cancellationToken);
        }

        public async Task<Result<List<ContractProductTransactionFee>>> Handle(MerchantQueries.GetTransactionFeesForProductQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetTransactionFeesForProduct(query.EstateId, query.MerchantId, query.ContractId, query.ProductId, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.UpdateMerchant(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AddMerchantAddressCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.AddMerchantAddress(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.UpdateMerchantAddressCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.UpdateMerchantAddress(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AddMerchantContactCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.AddMerchantContact(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.UpdateMerchantContactCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.UpdateMerchantContact(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.RemoveOperatorFromMerchant(command, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.RemoveMerchantContractCommand command, CancellationToken cancellationToken){
            return await this.MerchantDomainService.RemoveContractFromMerchant(command, cancellationToken);
        }
    }
}
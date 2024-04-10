namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Responses.Merchant;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MediatR;
    using Requests;
    using Services;
    using SwapMerchantDeviceRequest = Requests.SwapMerchantDeviceRequest;

    public class MerchantRequestHandler : IRequestHandler<SetMerchantSettlementScheduleRequest>,
                                          IRequestHandler<SwapMerchantDeviceRequest>,
                                          IRequestHandler<MerchantCommands.AddMerchantContractCommand>,
                                          IRequestHandler<MerchantCommands.CreateMerchantCommand,Guid>,
                                          IRequestHandler<MerchantCommands.AssignOperatorToMerchantCommand, Guid>,
                                          IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Guid>,
                                          IRequestHandler<MerchantCommands.CreateMerchantUserCommand, Guid>,
                                          IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Guid>, 
                                          IRequestHandler<MerchantCommands.MakeMerchantWithdrawalCommand, Guid>{
        #region Fields

        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        public MerchantRequestHandler(IMerchantDomainService merchantDomainService) {
            this.MerchantDomainService = merchantDomainService;
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

        public async Task Handle(SetMerchantSettlementScheduleRequest request,
                                       CancellationToken cancellationToken) {
            await this.MerchantDomainService.SetMerchantSettlementSchedule(request.EstateId, request.MerchantId, request.SettlementSchedule, cancellationToken);
        }

        public async Task Handle(SwapMerchantDeviceRequest request,
                                       CancellationToken cancellationToken) {
            await this.MerchantDomainService.SwapMerchantDevice(request.EstateId,
                                                                request.MerchantId,
                                                                request.DeviceId,
                                                                request.OriginalDeviceIdentifier,
                                                                request.NewDeviceIdentifier,
                                                                cancellationToken);
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
    }
}
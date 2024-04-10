namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Responses.Merchant;
    using MediatR;
    using Requests;
    using Services;

    public class MerchantRequestHandler : IRequestHandler<CreateMerchantUserRequest, Guid>,
                                          IRequestHandler<AddMerchantDeviceRequest>,
                                          IRequestHandler<MakeMerchantDepositRequest, Guid>,
                                          IRequestHandler<SetMerchantSettlementScheduleRequest>,
                                          IRequestHandler<SwapMerchantDeviceRequest>,
                                          IRequestHandler<MakeMerchantWithdrawalRequest, Guid>,
                                          IRequestHandler<AddMerchantContractRequest>,
                                          IRequestHandler<CreateMerchantCommand,Guid>,
                                          IRequestHandler<AssignOperatorToMerchantCommand>
    {
        #region Fields

        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        public MerchantRequestHandler(IMerchantDomainService merchantDomainService) {
            this.MerchantDomainService = merchantDomainService;
        }

        #endregion

        #region Methods
        
        public async Task Handle(AssignOperatorToMerchantCommand request,
                                 CancellationToken cancellationToken) {
            //await this.MerchantDomainService.AssignOperatorToMerchant(request.EstateId,
            //                                                          request.MerchantId,
            //                                                          request.OperatorId,
            //                                                          request.MerchantNumber,
            //                                                          request.TerminalNumber,
            //                                                          cancellationToken);
        }

        public async Task<Guid> Handle(CreateMerchantUserRequest request,
                                       CancellationToken cancellationToken) {
            Guid userId = await this.MerchantDomainService.CreateMerchantUser(request.EstateId,
                                                                              request.MerchantId,
                                                                              request.EmailAddress,
                                                                              request.Password,
                                                                              request.GivenName,
                                                                              request.MiddleName,
                                                                              request.FamilyName,
                                                                              cancellationToken);

            return userId;
        }

        public async Task Handle(AddMerchantDeviceRequest request,
                                       CancellationToken cancellationToken) {
            await this.MerchantDomainService.AddDeviceToMerchant(request.EstateId, request.MerchantId, request.DeviceId, request.DeviceIdentifier, cancellationToken);
        }

        public async Task<Guid> Handle(MakeMerchantDepositRequest request,
                                       CancellationToken cancellationToken) {
            Guid depositId = await this.MerchantDomainService.MakeMerchantDeposit(request.EstateId,
                                                                                  request.MerchantId,
                                                                                  request.Source,
                                                                                  request.Reference,
                                                                                  request.DepositDateTime,
                                                                                  request.Amount,
                                                                                  cancellationToken);

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

        public async Task<Guid> Handle(MakeMerchantWithdrawalRequest request,
                                       CancellationToken cancellationToken) {
            Guid withdrawalId = await this.MerchantDomainService.MakeMerchantWithdrawal(request.EstateId,
                                                                                        request.MerchantId,
                                                                                        request.WithdrawalDateTime,
                                                                                        request.Amount,
                                                                                        cancellationToken);

            return withdrawalId;
        }

        #endregion

        public async Task Handle(AddMerchantContractRequest request, CancellationToken cancellationToken){
            await this.MerchantDomainService.AddContractToMerchant(request.EstateId, request.MerchantId, request.ContractId, cancellationToken);
        }

        public async Task<Guid> Handle(CreateMerchantCommand request, CancellationToken cancellationToken){
            var result = await this.MerchantDomainService.CreateMerchant(request, cancellationToken);
            return result;
        }
    }
}
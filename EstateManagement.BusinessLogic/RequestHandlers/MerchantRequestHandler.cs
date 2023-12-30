namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    public class MerchantRequestHandler : IRequestHandler<CreateMerchantRequest>,
                                          IRequestHandler<AssignOperatorToMerchantRequest>,
                                          IRequestHandler<CreateMerchantUserRequest, Guid>,
                                          IRequestHandler<AddMerchantDeviceRequest>,
                                          IRequestHandler<MakeMerchantDepositRequest, Guid>,
                                          IRequestHandler<SetMerchantSettlementScheduleRequest>,
                                          IRequestHandler<SwapMerchantDeviceRequest>,
                                          IRequestHandler<MakeMerchantWithdrawalRequest, Guid>,
                                          IRequestHandler<AddMerchantContractRequest>{
        #region Fields

        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        public MerchantRequestHandler(IMerchantDomainService merchantDomainService) {
            this.MerchantDomainService = merchantDomainService;
        }

        #endregion

        #region Methods

        public async Task Handle(CreateMerchantRequest request,
                                       CancellationToken cancellationToken) {
            await this.MerchantDomainService.CreateMerchant(request.EstateId,
                                                            request.MerchantId,
                                                            request.Name,
                                                            request.AddressId,
                                                            request.AddressLine1,
                                                            request.AddressLine2,
                                                            request.AddressLine3,
                                                            request.AddressLine4,
                                                            request.Town,
                                                            request.Region,
                                                            request.PostalCode,
                                                            request.Country,
                                                            request.ContactId,
                                                            request.ContactName,
                                                            request.ContactPhoneNumber,
                                                            request.ContactEmailAddress,
                                                            request.SettlementSchedule,
                                                            request.CreateDateTime,
                                                            cancellationToken);
        }

        public async Task Handle(AssignOperatorToMerchantRequest request,
                                       CancellationToken cancellationToken) {
            await this.MerchantDomainService.AssignOperatorToMerchant(request.EstateId,
                                                                      request.MerchantId,
                                                                      request.OperatorId,
                                                                      request.MerchantNumber,
                                                                      request.TerminalNumber,
                                                                      cancellationToken);
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
    }
}
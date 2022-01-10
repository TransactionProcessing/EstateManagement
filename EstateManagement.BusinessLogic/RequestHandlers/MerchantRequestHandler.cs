﻿namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateMerchantUserRequest, System.Guid}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AddMerchantDeviceRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateMerchantRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AssignOperatorToMerchantRequest, System.String}" />
    public class MerchantRequestHandler : IRequestHandler<CreateMerchantRequest>,
                                          IRequestHandler<AssignOperatorToMerchantRequest>,
                                          IRequestHandler<CreateMerchantUserRequest, Guid>,
                                          IRequestHandler<AddMerchantDeviceRequest>,
                                          IRequestHandler<MakeMerchantDepositRequest, Guid>,
                                          IRequestHandler<SetMerchantSettlementScheduleRequest>,
                                          IRequestHandler<SwapMerchantDeviceRequest>
    {
        #region Fields

        /// <summary>
        /// The merchant domain service
        /// </summary>
        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantRequestHandler" /> class.
        /// </summary>
        /// <param name="merchantDomainService">The merchant domain service.</param>
        public MerchantRequestHandler(IMerchantDomainService merchantDomainService)
        {
            this.MerchantDomainService = merchantDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Unit> Handle(CreateMerchantRequest request,
                                         CancellationToken cancellationToken)
        {
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
                                                            cancellationToken);

            return new Unit();
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Unit> Handle(AssignOperatorToMerchantRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.AssignOperatorToMerchant(request.EstateId,
                                                                      request.MerchantId,
                                                                      request.OperatorId,
                                                                      request.MerchantNumber,
                                                                      request.TerminalNumber,
                                                                      cancellationToken);

            return new Unit();
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Guid> Handle(CreateMerchantUserRequest request,
                                       CancellationToken cancellationToken)
        {
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

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Unit> Handle(AddMerchantDeviceRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.AddDeviceToMerchant(request.EstateId, request.MerchantId, request.DeviceId, request.DeviceIdentifier, cancellationToken);

            return new Unit();
        }

        #endregion

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<Guid> Handle(MakeMerchantDepositRequest request,
                                       CancellationToken cancellationToken)
        {
            Guid depositId = await this.MerchantDomainService.MakeMerchantDeposit(request.EstateId,
                                                                                  request.MerchantId,
                                                                                  request.Source,
                                                                                  request.Reference,
                                                                                  request.DepositDateTime,
                                                                                  request.Amount,
                                                                                  cancellationToken);

            return depositId;
        }

        public async Task<Unit> Handle(SetMerchantSettlementScheduleRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.SetMerchantSettlementSchedule(request.EstateId, request.MerchantId, request.SettlementSchedule, cancellationToken);

            return new Unit();
        }

        public async Task<Unit> Handle(SwapMerchantDeviceRequest request, CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.SwapMerchantDevice(request.EstateId, request.MerchantId, request.DeviceId, request.OriginalDeviceIdentifier, request.NewDeviceIdentifier,cancellationToken);

            return new Unit();
        }
    }
}
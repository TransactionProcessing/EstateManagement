namespace EstateManagement.BusinessLogic.RequestHandlers
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
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateMerchantRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AssignOperatorToMerchantRequest, System.String}" />
    public class MerchantRequestHandler : IRequestHandler<CreateMerchantRequest, String>, IRequestHandler<AssignOperatorToMerchantRequest, String>
    {
        #region Fields

        /// <summary>
        /// The merchant domain service
        /// </summary>
        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantRequestHandler"/> class.
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
        public async Task<String> Handle(CreateMerchantRequest request,
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
                                                            cancellationToken);

            return string.Empty;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<String> Handle(AssignOperatorToMerchantRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.AssignOperatorToMerchant(request.EstateId,
                                                                      request.MerchantId,
                                                                      request.OperatorId,
                                                                      request.MerchantNumber,
                                                                      request.TerminalNumber,
                                                                      cancellationToken);

            return string.Empty;
        }

        #endregion
    }
}
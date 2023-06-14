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
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateEstateUserRequest, System.Guid}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateEstateRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AddOperatorToEstateRequest, System.String}" />
    public class EstateRequestHandler : IRequestHandler<CreateEstateRequest>,
                                        IRequestHandler<AddOperatorToEstateRequest>,
                                        IRequestHandler<CreateEstateUserRequest,Guid>
    {
        #region Fields

        /// <summary>
        /// The estate domain service
        /// </summary>
        private readonly IEstateDomainService EstateDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateRequestHandler" /> class.
        /// </summary>
        /// <param name="estateDomainService">The estate domain service.</param>
        public EstateRequestHandler(IEstateDomainService estateDomainService)
        {
            this.EstateDomainService = estateDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(CreateEstateRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.EstateDomainService.CreateEstate(request.EstateId, request.Name, cancellationToken);
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Guid> Handle(CreateEstateUserRequest request,
                                       CancellationToken cancellationToken)
        {
            Guid userId = await this.EstateDomainService.CreateEstateUser(request.EstateId,
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
        public async Task Handle(AddOperatorToEstateRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.EstateDomainService.AddOperatorToEstate(request.EstateId,
                                                               request.OperatorId,
                                                               request.Name,
                                                               request.RequireCustomMerchantNumber,
                                                               request.RequireCustomTerminalNumber,
                                                               cancellationToken);
        }

        #endregion
    }
}
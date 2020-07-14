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
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.CreateContractRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AddProductToContractRequest, System.String}" />
    /// <seealso cref="MediatR.IRequestHandler{EstateManagement.BusinessLogic.Requests.AddTransactionFeeForProductToContractRequest, System.String}" />
    public class ContractRequestHandler : IRequestHandler<CreateContractRequest, String>,
                                          IRequestHandler<AddProductToContractRequest, String>,
                                          IRequestHandler<AddTransactionFeeForProductToContractRequest, String>
    {
        #region Fields

        /// <summary>
        /// The estate domain service
        /// </summary>
        private readonly IContractDomainService ContractDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateRequestHandler" /> class.
        /// </summary>
        /// <param name="contractDomainService">The contract domain service.</param>
        public ContractRequestHandler(IContractDomainService contractDomainService)
        {
            this.ContractDomainService = contractDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<String> Handle(CreateContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.CreateContract(request.ContractId, request.EstateId, request.OperatorId, request.Description, cancellationToken);

            return string.Empty;
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<String> Handle(AddProductToContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.AddProductToContract(request.ProductId,
                                                                  request.ContractId,
                                                                  request.ProductName,
                                                                  request.DisplayText,
                                                                  request.Value,
                                                                  cancellationToken);

            return string.Empty;
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<String> Handle(AddTransactionFeeForProductToContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.AddTransactionFeeForProductToContract(request.TransactionFeeId,
                                                                                   request.ContractId,
                                                                                   request.ProductId,
                                                                                   request.Description,
                                                                                   request.CalculationType,
                                                                                   request.Value,
                                                                                   cancellationToken);
            return string.Empty;
        }

        #endregion
    }
}
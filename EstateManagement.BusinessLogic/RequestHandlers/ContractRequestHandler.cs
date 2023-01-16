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
    public class ContractRequestHandler : IRequestHandler<CreateContractRequest>,
                                          IRequestHandler<AddProductToContractRequest>,
                                          IRequestHandler<AddTransactionFeeForProductToContractRequest>,
                                          IRequestHandler<DisableTransactionFeeForProductRequest>
    {
        #region Fields

        private readonly IContractDomainService ContractDomainService;

        #endregion

        #region Constructors

        public ContractRequestHandler(IContractDomainService contractDomainService)
        {
            this.ContractDomainService = contractDomainService;
        }

        #endregion

        #region Methods

        public async Task<Unit> Handle(CreateContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.CreateContract(request.ContractId, request.EstateId, request.OperatorId, request.Description, cancellationToken);

            return new Unit();
        }
        
        public async Task<Unit> Handle(AddProductToContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.AddProductToContract(request.ProductId,
                                                                  request.ContractId,
                                                                  request.ProductName,
                                                                  request.DisplayText,
                                                                  request.Value,
                                                                  request.ProductType,
                                                                  cancellationToken);

            return new Unit();
        }
        
        public async Task<Unit> Handle(AddTransactionFeeForProductToContractRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.AddTransactionFeeForProductToContract(request.TransactionFeeId,
                                                                                   request.ContractId,
                                                                                   request.ProductId,
                                                                                   request.Description,
                                                                                   request.CalculationType,
                                                                                   request.FeeType,
                                                                                   request.Value,
                                                                                   cancellationToken);
            return new Unit();
        }

        #endregion

        public async Task<Unit> Handle(DisableTransactionFeeForProductRequest request,
                                         CancellationToken cancellationToken)
        {
            await this.ContractDomainService.DisableTransactionFeeForProduct(request.TransactionFeeId, request.ContractId, request.ProductId, cancellationToken);

            return new Unit();
        }
    }
}
namespace EstateManagement.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using AddProductToContractRequest = BusinessLogic.Requests.AddProductToContractRequest;
    using AddProductToContractRequestDTO = DataTransferObjects.Requests.AddProductToContractRequest;
    using CreateContractRequest = BusinessLogic.Requests.CreateContractRequest;
    using CreateContractRequestDTO = DataTransferObjects.Requests.CreateContractRequest;
    using AddTransactionFeeForProductToContractRequestDTO = DataTransferObjects.Requests.AddTransactionFeeForProductToContractRequest;
    using AddTransactionFeeForProductToContractRequest = BusinessLogic.Requests.AddTransactionFeeForProductToContractRequest;
    using EstateManagement.Factories;
    using EstateManagement.BusinessLogic.Manger;
    using Models.Contract;
    using Shared.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(ContractController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    public class ContractController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        private readonly IEstateManagementManager EstateManagementManager;

        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public ContractController(IMediator mediator,
                                  IEstateManagementManager estateManagementManager,
                                  IModelFactory modelFactory)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="includeProducts">if set to <c>true</c> [include products].</param>
        /// <param name="includeProductsWithFees">if set to <c>true</c> [include products with fees].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{contractId}")]
        public async Task<IActionResult> GetContract([FromRoute] Guid estateId, 
                                                     [FromRoute] Guid contractId,
                                                     [FromQuery] Boolean includeProducts,
                                                     [FromQuery] Boolean includeProductsWithFees,
                                                     CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Contract contract = await this.EstateManagementManager.GetContract(estateId, contractId, includeProducts, includeProductsWithFees, cancellationToken);

            if (contract == null)
            {
                throw new NotFoundException($"Contract not found with estate Id {estateId} and contract Id {contractId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(contract));
        }

        /// <summary>
        /// Adds the product to contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="addProductToContractRequest">The add product to contract request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{contractId}/products")]
        public async Task<IActionResult> AddProductToContract([FromRoute] Guid estateId,
                                                              [FromRoute] Guid contractId,
                                                              [FromBody] AddProductToContractRequestDTO addProductToContractRequest,
                                                              CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Guid productId = Guid.NewGuid();
            
            // Create the command
            AddProductToContractRequest command = AddProductToContractRequest.Create(contractId , estateId, productId, addProductToContractRequest.ProductName, addProductToContractRequest.DisplayText,
                                                                                     addProductToContractRequest.Value);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{ContractController.ControllerRoute}/{contractId}/products/{productId}",
                                new AddProductToContractResponse
                                {
                                    EstateId = estateId,
                                    ContractId = contractId,
                                    ProductId = productId
                                });
        }

        /// <summary>
        /// Adds the transaction fee for product to contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="addTransactionFeeForProductToContractRequest">The add transaction fee for product to contract request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{contractId}/products/{productId}/transactionFees")]
        public async Task<IActionResult> AddTransactionFeeForProductToContract([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromBody] AddTransactionFeeForProductToContractRequestDTO addTransactionFeeForProductToContractRequest,
                                                                               CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Guid transactionFeeId = Guid.NewGuid();

            Models.Contract.CalculationType calculationType = (Models.Contract.CalculationType)addTransactionFeeForProductToContractRequest.CalculationType;
            Models.Contract.FeeType feeType = (Models.Contract.FeeType)addTransactionFeeForProductToContractRequest.FeeType;

            // Create the command
            AddTransactionFeeForProductToContractRequest command = AddTransactionFeeForProductToContractRequest.Create(contractId,estateId,
                                                                                                                       productId,
                                                                                                                       transactionFeeId,
                                                                                                                       addTransactionFeeForProductToContractRequest.Description,
                                                                                                                       calculationType,
                                                                                                                       feeType,
                                                                                                                       addTransactionFeeForProductToContractRequest.Value);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{ContractController.ControllerRoute}/{contractId}/products/{productId}/transactionFees/{transactionFeeId}",
                                new AddTransactionFeeForProductToContractResponse
                                {
                                    EstateId = estateId,
                                    ContractId = contractId,
                                    ProductId = productId,
                                    TransactionFeeId = transactionFeeId
                                });
        }

        [HttpPost]
        [Route("{contractId}/products/{productId}/transactionFees/{transactionFeeId}")]
        public async Task<IActionResult> DisableTransactionFeeForProduct([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromRoute] Guid transactionFeeId,
                                                                               CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            //// Create the command
            //AddTransactionFeeForProductToContractRequest command = AddTransactionFeeForProductToContractRequest.Create(contractId, estateId,
            //                                                                                                           productId,
            //                                                                                                           transactionFeeId,
            //                                                                                                           addTransactionFeeForProductToContractRequest.Description,
            //                                                                                                           calculationType,
            //                                                                                                           addTransactionFeeForProductToContractRequest.Value);

            // Route the command
            //await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Ok($"{ContractController.ControllerRoute}/{contractId}/products/{productId}/transactionFees/{transactionFeeId}");
        }

        /// <summary>
        /// Creates the contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createContractRequest">The create contract request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateContract([FromRoute] Guid estateId,
                                                        [FromBody] CreateContractRequestDTO createContractRequest,
                                                        CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] {string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName}) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            Guid contractId = Guid.NewGuid();

            // Create the command
            CreateContractRequest command = CreateContractRequest.Create(contractId, estateId, createContractRequest.OperatorId, createContractRequest.Description);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{ContractController.ControllerRoute}/{contractId}",
                                new CreateContractResponse
                                {
                                    EstateId = estateId,
                                    OperatorId = command.OperatorId,
                                    ContractId = contractId
                                });
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "contracts";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/estates/{estateid}/" + ContractController.ControllerName;

        #endregion
    }
}
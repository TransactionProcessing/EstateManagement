using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Shared.EventStore.Aggregate;
using SimpleResults;

namespace EstateManagement.Controllers.v2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Contract;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using AddProductToContractRequestDTO = DataTransferObjects.Requests.Contract.AddProductToContractRequest;
    using CreateContractRequestDTO = DataTransferObjects.Requests.Contract.CreateContractRequest;
    using AddTransactionFeeForProductToContractRequestDTO = DataTransferObjects.Requests.Contract.AddTransactionFeeForProductToContractRequest;
    using EstateManagement.Factories;
    using EstateManagement.BusinessLogic.Manger;
    using Models.Contract;
    using Shared.Exceptions;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Shared.General;
    using static EstateManagement.BusinessLogic.Requests.ContractCommands;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(ControllerRoute)]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        private ClaimsPrincipal UserOverride;
        internal void SetContextOverride(HttpContext ctx) {
            this.UserOverride = ctx.User;
        }

        #region Constructors

        public ContractController(IMediator mediator,
                                  IEstateManagementManager estateManagementManager)
        {
            Mediator = mediator;
            EstateManagementManager = estateManagementManager;
        }

        #endregion

        internal ClaimsPrincipal GetUser()
        {
            return this.UserOverride switch
            {
                null => this.HttpContext.User,
                _ => this.UserOverride
            };
        }

        #region Methods

        private Result StandardSecurityChecks(Guid estateId) {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.GetUser(), "EstateId", estateId.ToString());

            string estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.GetUser(), new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return Result.Forbidden("User role is not valid");
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return Result.Forbidden("User estate id claim is not valid");
            }

            return Result.Success();
        }

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
        [SwaggerResponse(200, "OK", typeof(ContractResponse))]
        [SwaggerResponseExample(200, typeof(ContractResponseExample))]
        public async Task<ActionResult<Result<ContractResponse>>> GetContract([FromRoute] Guid estateId,
                                                     [FromRoute] Guid contractId,
                                                     CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;

            ContractQueries.GetContractQuery query = new ContractQueries.GetContractQuery(estateId, contractId);
            Result<Contract> result = await Mediator.Send(query, cancellationToken);
            if (result.IsFailed) {
                var x = result.ToActionResult().Result;
                return x;
            }

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result<List<ContractResponse>>>> GetContracts([FromRoute] Guid estateId,
                                                     CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;
            ContractQueries.GetContractsQuery query = new ContractQueries.GetContractsQuery(estateId);

            Result<List<Contract>> result = await Mediator.Send(query, cancellationToken);
            return ModelFactory.ConvertFrom(result.Data).ToActionResult();

        }

        /// <summary>
        /// Adds the product to contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="addProductToContractRequest">The add product to contract request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{contractId}/products")]
        [SwaggerResponse(201, "Created", typeof(AddProductToContractResponse))]
        [SwaggerResponseExample(201, typeof(AddProductToContractResponseExample))]
        public async Task<ActionResult<Result>> AddProductToContract([FromRoute] Guid estateId,
                                                              [FromRoute] Guid contractId,
                                                              [FromBody] AddProductToContractRequestDTO addProductToContractRequest,
                                                              CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;

            Guid productId = Guid.NewGuid();

            // Create the command
            AddProductToContractCommand command =
                new(estateId, contractId, productId,
                    addProductToContractRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);
            return result.ToActionResult();
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
        [HttpPatch]
        [Route("{contractId}/products/{productId}/transactionFees")]
        [SwaggerResponse(201, "Created", typeof(AddTransactionFeeForProductToContractResponse))]
        [SwaggerResponseExample(201, typeof(AddTransactionFeeForProductToContractResponseExample))]
        public async Task<ActionResult<Result>> AddTransactionFeeForProductToContract([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromBody] AddTransactionFeeForProductToContractRequestDTO addTransactionFeeForProductToContractRequest,
                                                                               CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;

            Guid transactionFeeId = Guid.NewGuid();

            // Create the command
            AddTransactionFeeForProductToContractCommand command =
                new(estateId, contractId, productId, transactionFeeId, addTransactionFeeForProductToContractRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        /// <summary>
        /// Disables the transaction fee for product.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{contractId}/products/{productId}/transactionFees/{transactionFeeId}")]
        [SwaggerResponse(200, "OK")]
        public async Task<ActionResult<Result>> DisableTransactionFeeForProduct([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromRoute] Guid transactionFeeId,
                                                                               CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;

            // Create the command
            DisableTransactionFeeForProductCommand command = new(contractId, estateId, productId, transactionFeeId);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResult();
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
        [SwaggerResponse(201, "Created", typeof(CreateContractResponse))]
        [SwaggerResponseExample(201, typeof(CreateContractResponseExample))]
        public async Task<ActionResult<Result>> CreateContract([FromRoute] Guid estateId,
                                                        [FromBody] CreateContractRequestDTO createContractRequest,
                                                        CancellationToken cancellationToken)
        {
            Result securityChecksResult = StandardSecurityChecks(estateId);
            if (securityChecksResult.IsFailed)
                return securityChecksResult.ToActionResult().Result;

            Guid contractId = Guid.NewGuid();

            // Create the command
            CreateContractCommand command = new(estateId, contractId, createContractRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResult();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "contracts";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/estates/{estateid}/" + ControllerName;

        #endregion
    }
}
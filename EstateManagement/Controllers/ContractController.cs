using EstateManagement.DataTransferObjects.Requests.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SimpleResults;

namespace EstateManagement.Controllers
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
    using EstateManagement.Factories;
    using EstateManagement.BusinessLogic.Manger;
    using Models.Contract;
    using Shared.Exceptions;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Shared.General;
    using Microsoft.AspNetCore.Components.Forms;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ExcludeFromCodeCoverage]
    [Route(ContractController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase {
        public EstateManagement.Controllers.v2.ContractController V2ContractController;

        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        #region Constructors

        public ContractController(IMediator mediator,
                                  IEstateManagementManager estateManagementManager)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
            this.V2ContractController = new v2.ContractController(this.Mediator, this.EstateManagementManager);
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
        [SwaggerResponse(200, "OK", typeof(ContractResponse))]
        [SwaggerResponseExample(200, typeof(ContractResponseExample))]
        public async Task<IActionResult> GetContract([FromRoute] Guid estateId, 
                                                     [FromRoute] Guid contractId,
                                                     CancellationToken cancellationToken) {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.GetContract(estateId, contractId, cancellationToken);

            return ActionResultHelpers.HandleResult(result, $"Contract not found with estate Id {estateId} and contract Id {contractId}");
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
        public async Task<IActionResult> GetContracts([FromRoute] Guid estateId,
                                                     CancellationToken cancellationToken)
        {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.GetContracts(estateId, cancellationToken);

            return ActionResultHelpers.HandleResult(result, String.Empty);

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
        public async Task<IActionResult> AddProductToContract([FromRoute] Guid estateId,
                                                              [FromRoute] Guid contractId,
                                                              [FromBody] AddProductToContractRequest addProductToContractRequest,
                                                              CancellationToken cancellationToken)
        {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.AddProductToContract(estateId, contractId, addProductToContractRequest, cancellationToken);

            return ActionResultHelpers.HandleResult(result, String.Empty);
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
        public async Task<IActionResult> AddTransactionFeeForProductToContract([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromBody] AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest,
                                                                               CancellationToken cancellationToken)
        {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.AddTransactionFeeForProductToContract(
                estateId, contractId, productId, addTransactionFeeForProductToContractRequest, cancellationToken);

            return ActionResultHelpers.HandleResult(result, String.Empty);
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
        public async Task<IActionResult> DisableTransactionFeeForProduct([FromRoute] Guid estateId,
                                                                               [FromRoute] Guid contractId,
                                                                               [FromRoute] Guid productId,
                                                                               [FromRoute] Guid transactionFeeId,
                                                                               CancellationToken cancellationToken)
        {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.DisableTransactionFeeForProduct(estateId, contractId, productId, transactionFeeId, cancellationToken);

            return ActionResultHelpers.HandleResult(result, String.Empty);
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
        public async Task<IActionResult> CreateContract([FromRoute] Guid estateId,
                                                        [FromBody] CreateContractRequest createContractRequest,
                                                        CancellationToken cancellationToken)
        {
            this.V2ContractController.SetContextOverride(this.HttpContext);
            var result = await this.V2ContractController.CreateContract(estateId, createContractRequest, cancellationToken);

            return ActionResultHelpers.HandleResult(result, String.Empty);
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

    public static class ActionResultHelpers{
        //public static IActionResult HandleResult<T>(ActionResult<Result<T>> result, String notFoundMessage)
        //{
        //    if (result.Result.IsSuccess())
        //    {
        //        OkObjectResult ok = result.Result as OkObjectResult;
        //        Result<T> x = ok.Value as Result<T>;
                
        //        return  new OkObjectResult(x.Data);
        //    }

        //    if (result.Result is NotFoundObjectResult)
        //    {
        //        throw new NotFoundException(notFoundMessage);
        //    }

        //    ObjectResult r = result.Result as ObjectResult;
        //    if (r.StatusCode == 403)
        //        return new ForbidResult();

        //    return new BadRequestResult();
        //}

        public static IActionResult HandleResult(IActionResult result, String notFoundMessage) {

            if (result.GetType().Name == nameof(OkObjectResult)) {
                OkObjectResult ok = result as OkObjectResult;
                Type type = ok.Value.GetType();
                dynamic convertedObj = Convert.ChangeType(ok.Value, ok.Value.GetType());

                if (convertedObj.GetType().GetProperty("Data") != null)
                {
                    // convertedObj has a property named "Data"
                    return new OkObjectResult(convertedObj.Data);
                }

                // convertedObj does not have a property named "Data"
                return new OkResult();


            }

            IActionResult x = result.GetType().Name switch {
                nameof(BadRequestObjectResult) => new BadRequestResult(),
                nameof(NotFoundObjectResult) => throw new NotFoundException(notFoundMessage),
                nameof(UnauthorizedObjectResult) => new UnauthorizedResult(),
                nameof(ConflictObjectResult) => new ConflictResult(),
                nameof(ForbidResult) => new ForbidResult(),
                //nameof(OkObjectResult) => new OkObjectResult(result.)
                _ => result
            };
            return x;


            //if (result.Result.IsSuccess())
            //{
            //    return new OkResult();
            //}

            //if (result.Result is NotFoundObjectResult)
            //{
            //    throw new NotFoundException(notFoundMessage);
            //}

            //ObjectResult r = result.Result as ObjectResult;
            //if (r.StatusCode == 403)
            //    return new ForbidResult();

            //return new BadRequestResult();
        }
    }
}
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    using BusinessLogic.Manger;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Merchant;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.Contract;
    using Models.Merchant;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    [Route(ControllerRoute)]
    [ApiController]
    [Authorize]
    public class MerchantController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The estate management manager
        /// </summary>
        private readonly IEstateManagementManager EstateManagementManager;

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        #endregion

        #region Constructors

        public MerchantController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private ClaimsPrincipal UserOverride;
        internal void SetContextOverride(HttpContext ctx)
        {
            this.UserOverride = ctx.User;
        }

        internal ClaimsPrincipal GetUser()
        {
            return this.UserOverride switch
            {
                null => this.HttpContext.User,
                _ => this.UserOverride
            };
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("")]
        [SwaggerResponse(201, "Created", typeof(CreateMerchantResponse))]
        [SwaggerResponseExample(201, typeof(CreateMerchantResponseExample))]
        public async Task<IActionResult> CreateMerchant([FromRoute] Guid estateId,
                                                        [FromBody] CreateMerchantRequest createMerchantRequest,
                                                        CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.CreateMerchantCommand command = new(estateId, createMerchantRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();

        }

        [HttpPatch]
        [Route("{merchantId}/operators")]
        [ProducesResponseType(typeof(AssignOperatorResponse), 201)]
        [SwaggerResponse(201, "Created", typeof(AssignOperatorResponse))]
        [SwaggerResponseExample(201, typeof(AssignOperatorResponseExample))]
        public async Task<IActionResult> AssignOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        AssignOperatorRequest assignOperatorRequest,
                                                        CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.AssignOperatorToMerchantCommand command = new(estateId, merchantId, assignOperatorRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpDelete]
        [Route("{merchantId}/operators/{operatorId}")]
        public async Task<IActionResult> RemoveOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromRoute] Guid operatorId,
                                                        CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.RemoveOperatorFromMerchantCommand command = new(estateId, merchantId, operatorId);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPatch]
        [Route("{merchantId}/devices")]
        [SwaggerResponse(201, "Created", typeof(AddMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> AddDevice([FromRoute] Guid estateId,
                                                   [FromRoute] Guid merchantId,
                                                   [FromBody] AddMerchantDeviceRequest addMerchantDeviceRequest,
                                                   CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.AddMerchantDeviceCommand command = new(estateId, merchantId, addMerchantDeviceRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPatch]
        [Route("{merchantId}/contracts")]
        public async Task<IActionResult> AddContract([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromBody] AddMerchantContractRequest addMerchantContractRequest,
                                                     CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.AddMerchantContractCommand command = new(estateId, merchantId, addMerchantContractRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpDelete]
        [Route("{merchantId}/contracts/{contractId}")]
        public async Task<IActionResult> RemoveContract([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromRoute] Guid contractId,
                                                        CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.RemoveMerchantContractCommand command = new(estateId, merchantId, contractId);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPatch]
        [Route("{merchantId}/users")]
        [SwaggerResponse(201, "Created", typeof(CreateMerchantUserResponse))]
        [SwaggerResponseExample(201, typeof(CreateMerchantUserResponseExample))]
        public async Task<IActionResult> CreateMerchantUser([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] CreateMerchantUserRequest createMerchantUserRequest,
                                                            CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.CreateMerchantUserCommand command = new(estateId, merchantId, createMerchantUserRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPost]
        [Route("{merchantId}/deposits")]
        [SwaggerResponse(201, "Created", typeof(MakeMerchantDepositResponse))]
        [SwaggerResponseExample(201, typeof(MakeMerchantDepositResponseExample))]
        public async Task<IActionResult> MakeDeposit([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromBody] MakeMerchantDepositRequest makeMerchantDepositRequest,
                                                     CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            // This will always be a manual deposit as auto ones come in via another route
            MerchantCommands.MakeMerchantDepositCommand command = new(estateId, merchantId, Models.MerchantDepositSource.Manual, makeMerchantDepositRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPost]
        [Route("{merchantId}/withdrawals")]
        //[SwaggerResponse(201, "Created", typeof(MakeMerchantDepositResponse))]
        //[SwaggerResponseExample(201, typeof(MakeMerchantDepositResponseExample))]
        public async Task<IActionResult> MakeWithdrawal([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromBody] MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest,
                                                        CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.MakeMerchantWithdrawalCommand command = new(estateId, merchantId, makeMerchantWithdrawalRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();

        }

        [HttpPatch]
        [Route("{merchantId}/devices/{deviceIdentifier}")]
        [SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> SwapMerchantDevice([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromRoute] string deviceIdentifier,
                                                            [FromBody] SwapMerchantDeviceRequest swapMerchantDeviceRequest,
                                                            CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.SwapMerchantDeviceCommand command = new(estateId, merchantId, deviceIdentifier, swapMerchantDeviceRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPost]
        [Route("{merchantId}/statements")]
        [SwaggerResponse(201, "Created", typeof(GenerateMerchantStatementResponse))]
        [SwaggerResponseExample(201, typeof(GenerateMerchantStatementResponseExample))]
        public async Task<IActionResult> GenerateMerchantStatement([FromRoute] Guid estateId,
                                                                   [FromRoute] Guid merchantId,
                                                                   [FromBody] GenerateMerchantStatementRequest generateMerchantStatementRequest,
                                                                   CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.GenerateMerchantStatementCommand command = new(estateId, merchantId, generateMerchantStatementRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpGet]
        [Route("{merchantId}")]
        [SwaggerResponse(200, "OK", typeof(MerchantResponse))]
        [SwaggerResponseExample(200, typeof(MerchantResponseExample))]
        public async Task<IActionResult> GetMerchant([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantQueries.GetMerchantQuery query = new(estateId, merchantId);

            // Route the query
            Result<Merchant> result = await Mediator.Send(query, cancellationToken);
            if (result.IsFailed)
                return result.ToActionResultX();
            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }
        
        [Route("{merchantId}/contracts")]
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> GetMerchantContracts([FromRoute] Guid estateId,
                                                              [FromRoute] Guid merchantId,
                                                              CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantQueries.GetMerchantContractsQuery query = new(estateId, merchantId);

            Result<List<Models.Contract.Contract>> result = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(List<MerchantResponse>))]
        [SwaggerResponseExample(200, typeof(MerchantResponseListExample))]
        public async Task<IActionResult> GetMerchants([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantQueries.GetMerchantsQuery query = new(estateId);

            Result<List<Merchant>> result = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }

        [Route("{merchantId}/contracts/{contractId}/products/{productId}/transactionFees")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>), 200)]
        [SwaggerResponse(200, "OK", typeof(List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>))]
        [SwaggerResponseExample(200, typeof(ContractProductTransactionFeeResponseListExample))]
        public async Task<IActionResult> GetTransactionFeesForProduct([FromRoute] Guid estateId,
                                                                      [FromRoute] Guid merchantId,
                                                                      [FromRoute] Guid contractId,
                                                                      [FromRoute] Guid productId,
                                                                      CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantQueries.GetTransactionFeesForProductQuery query = new(estateId, merchantId, contractId, productId);

            List<Models.Contract.ContractProductTransactionFee> transactionFees = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(transactionFees).ToActionResultX();
        }


        private bool PerformMerchantUserChecks(Guid estateId, Guid merchantId)
        {

            if (PerformStandardChecks(estateId) == false)
            {
                return false;
            }

            string merchantRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName"))
                ? "Merchant"
                : Environment.GetEnvironmentVariable("MerchantRoleName");

            if (this.GetUser().IsInRole(merchantRoleName) == false)
                return true;

            if (ClaimsHelper.IsUserRolesValid(this.GetUser(), new[] { merchantRoleName }) == false)
            {
                return false;
            }

            Claim merchantIdClaim = ClaimsHelper.GetUserClaim(this.GetUser(), "MerchantId");

            if (ClaimsHelper.ValidateRouteParameter(merchantId, merchantIdClaim) == false)
            {
                return false;
            }

            return true;
        }

        private bool PerformStandardChecks(Guid estateId)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(GetUser(), "EstateId", estateId.ToString());

            string estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(GetUser(), new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return false;
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return false;
            }

            return true;
        }


        [HttpPatch]
        [Route("{merchantId}")]
        [SwaggerResponse(204, "No Content")]
        public async Task<IActionResult> UpdateMerchant([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromBody] UpdateMerchantRequest updateMerchantRequest,
                                                        CancellationToken cancellationToken)
        {

            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.UpdateMerchantCommand command = new(estateId, merchantId, updateMerchantRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [Route("{merchantId}/addresses")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> AddMerchantAddress([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] DataTransferObjects.Requests.Merchant.Address addAddressRequest,
                                                            CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.AddMerchantAddressCommand command = new(estateId, merchantId, addAddressRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [Route("{merchantId}/addresses/{addressId}")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> UpdateMerchantAddress([FromRoute] Guid estateId,
                                                               [FromRoute] Guid merchantId,
                                                               [FromRoute] Guid addressId,
                                                               [FromBody] DataTransferObjects.Requests.Merchant.Address updateAddressRequest,
                                                               CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.UpdateMerchantAddressCommand command = new(estateId, merchantId, addressId, updateAddressRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [Route("{merchantId}/contacts")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> AddMerchantContact([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] DataTransferObjects.Requests.Merchant.Contact addContactRrequest,
                                                            CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.AddMerchantContactCommand command = new(estateId, merchantId, addContactRrequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [Route("{merchantId}/contacts/{contactId}")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> UpdateMerchantContact([FromRoute] Guid estateId,
                                                               [FromRoute] Guid merchantId,
                                                               [FromRoute] Guid contactId,
                                                               [FromBody] DataTransferObjects.Requests.Merchant.Contact updateContactRequest,
                                                               CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantCommands.UpdateMerchantContactCommand command = new(estateId, merchantId, contactId, updateContactRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "merchants";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/estates/{estateid}/" + ControllerName;

        #endregion
    }

    public static class ResultExtensions {
        public static IActionResult ToActionResultX(this Result result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);

            return result.Status switch
            {
                ResultStatus.Invalid => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                ResultStatus.Unauthorized => new UnauthorizedObjectResult(result),
                ResultStatus.Conflict => new ConflictObjectResult(result),
                ResultStatus.Failure => CreateObjectResult(result, HttpStatusCode.InternalServerError),
                ResultStatus.CriticalError => CreateObjectResult(result, HttpStatusCode.InternalServerError),
                ResultStatus.Forbidden => new ForbidResult(),
                _ => CreateObjectResult(result, HttpStatusCode.NotImplemented)

            };
        }

        internal static IActionResult ToActionResultX<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);

            return result.Status switch
            {
                ResultStatus.Invalid => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                ResultStatus.Unauthorized => new UnauthorizedObjectResult(result),
                ResultStatus.Conflict => new ConflictObjectResult(result),
                ResultStatus.Failure => CreateObjectResult(result, HttpStatusCode.InternalServerError),
                ResultStatus.CriticalError => CreateObjectResult(result, HttpStatusCode.InternalServerError),
                ResultStatus.Forbidden => new ForbidResult(),
                _ => CreateObjectResult(result, HttpStatusCode.NotImplemented)

            };
        }

        internal static ObjectResult CreateObjectResult(Result result,
                                                        HttpStatusCode statusCode)
        {
            ObjectResult or = new ObjectResult(result);
            or.StatusCode = (Int32)statusCode;
            return or;
        }

        internal static ObjectResult CreateObjectResult<T>(Result<T> result,
                                                           HttpStatusCode statusCode)
        {
            ObjectResult or = new ObjectResult(result);
            or.StatusCode = (Int32)statusCode;
            return or;
        }
    }
}
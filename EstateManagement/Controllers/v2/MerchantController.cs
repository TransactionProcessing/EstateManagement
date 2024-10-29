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
        public async Task<ActionResult<Result>> CreateMerchant([FromRoute] Guid estateId,
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
            return result.ToActionResult();

        }

        [HttpPatch]
        [Route("{merchantId}/operators")]
        [ProducesResponseType(typeof(AssignOperatorResponse), 201)]
        [SwaggerResponse(201, "Created", typeof(AssignOperatorResponse))]
        [SwaggerResponseExample(201, typeof(AssignOperatorResponseExample))]
        public async Task<ActionResult<Result>> AssignOperator([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpDelete]
        [Route("{merchantId}/operators/{operatorId}")]
        public async Task<ActionResult<Result>> RemoveOperator([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPatch]
        [Route("{merchantId}/devices")]
        [SwaggerResponse(201, "Created", typeof(AddMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<ActionResult<Result>> AddDevice([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPatch]
        [Route("{merchantId}/contracts")]
        public async Task<ActionResult<Result>> AddContract([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpDelete]
        [Route("{merchantId}/contracts/{contractId}")]
        public async Task<ActionResult<Result>> RemoveContract([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPatch]
        [Route("{merchantId}/users")]
        [SwaggerResponse(201, "Created", typeof(CreateMerchantUserResponse))]
        [SwaggerResponseExample(201, typeof(CreateMerchantUserResponseExample))]
        public async Task<ActionResult<Result>> CreateMerchantUser([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPost]
        [Route("{merchantId}/deposits")]
        [SwaggerResponse(201, "Created", typeof(MakeMerchantDepositResponse))]
        [SwaggerResponseExample(201, typeof(MakeMerchantDepositResponseExample))]
        public async Task<ActionResult<Result>> MakeDeposit([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPost]
        [Route("{merchantId}/withdrawals")]
        //[SwaggerResponse(201, "Created", typeof(MakeMerchantDepositResponse))]
        //[SwaggerResponseExample(201, typeof(MakeMerchantDepositResponseExample))]
        public async Task<ActionResult<Result>> MakeWithdrawal([FromRoute] Guid estateId,
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
            return result.ToActionResult();

        }

        [HttpPatch]
        [Route("{merchantId}/devices/{deviceIdentifier}")]
        [SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<ActionResult<Result>> SwapMerchantDevice([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpPost]
        [Route("{merchantId}/statements")]
        [SwaggerResponse(201, "Created", typeof(GenerateMerchantStatementResponse))]
        [SwaggerResponseExample(201, typeof(GenerateMerchantStatementResponseExample))]
        public async Task<ActionResult<Result>> GenerateMerchantStatement([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [HttpGet]
        [Route("{merchantId}")]
        [SwaggerResponse(200, "OK", typeof(MerchantResponse))]
        [SwaggerResponseExample(200, typeof(MerchantResponseExample))]
        public async Task<ActionResult<Result<MerchantResponse>>> GetMerchant([FromRoute] Guid estateId,
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

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        [Route("{merchantId}/contracts")]
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result<List<ContractResponse>>>> GetMerchantContracts([FromRoute] Guid estateId,
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

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(List<MerchantResponse>))]
        [SwaggerResponseExample(200, typeof(MerchantResponseListExample))]
        public async Task<ActionResult<Result<List<MerchantResponse>>>> GetMerchants([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            bool isRequestAllowed = PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return Forbid();
            }

            MerchantQueries.GetMerchantsQuery query = new(estateId);

            Result<List<Merchant>> result = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        [Route("{merchantId}/contracts/{contractId}/products/{productId}/transactionFees")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>), 200)]
        [SwaggerResponse(200, "OK", typeof(List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>))]
        [SwaggerResponseExample(200, typeof(ContractProductTransactionFeeResponseListExample))]
        public async Task<ActionResult<Result<List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>>>> GetTransactionFeesForProduct([FromRoute] Guid estateId,
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

            return ModelFactory.ConvertFrom(transactionFees).ToActionResult();
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
        public async Task<ActionResult<Result>> UpdateMerchant([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [Route("{merchantId}/addresses")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result>> AddMerchantAddress([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [Route("{merchantId}/addresses/{addressId}")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result>> UpdateMerchantAddress([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [Route("{merchantId}/contacts")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result>> AddMerchantContact([FromRoute] Guid estateId,
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
            return result.ToActionResult();
        }

        [Route("{merchantId}/contacts/{contactId}")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<ActionResult<Result>> UpdateMerchantContact([FromRoute] Guid estateId,
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
            return result.ToActionResult();
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
}
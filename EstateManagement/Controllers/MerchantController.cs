namespace EstateManagement.Controllers
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
    using Microsoft.AspNetCore.Mvc;
    using Models.Contract;
    using Models.Merchant;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    
    [ExcludeFromCodeCoverage]
    [Route(MerchantController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public class MerchantController : ControllerBase {
        public EstateManagement.Controllers.v2.MerchantController V2MerchantController;

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
            this.Mediator = mediator;
            this.V2MerchantController = new v2.MerchantController(mediator);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.CreateMerchant(estateId, createMerchantRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);

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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result =
                await this.V2MerchantController.AssignOperator(estateId, merchantId, assignOperatorRequest,
                    cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpDelete]
        [Route("{merchantId}/operators/{operatorId}")]
        public async Task<IActionResult> RemoveOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromRoute] Guid operatorId,
                                                        CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.RemoveOperator(estateId, merchantId, operatorId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.AddDevice(estateId, merchantId, addMerchantDeviceRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpPatch]
        [Route("{merchantId}/contracts")]
        public async Task<IActionResult> AddContract([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromBody] AddMerchantContractRequest addMerchantContractRequest,
                                                     CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.AddContract(estateId, merchantId, addMerchantContractRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpDelete]
        [Route("{merchantId}/contracts/{contractId}")]
        public async Task<IActionResult> RemoveContract([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromRoute] Guid contractId,
                                                     CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.RemoveContract(estateId, merchantId, contractId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.CreateMerchantUser(estateId, merchantId, createMerchantUserRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.MakeDeposit(estateId, merchantId, makeMerchantDepositRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.MakeWithdrawal(estateId, merchantId, makeMerchantWithdrawalRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);

        }

        [HttpPatch]
        [Route("{merchantId}/devices/{deviceIdentifier}")]
        [SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> SwapMerchantDevice([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromRoute] String deviceIdentifier,
                                                            [FromBody] SwapMerchantDeviceRequest swapMerchantDeviceRequest,
                                                            CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.SwapMerchantDevice(estateId, merchantId, deviceIdentifier, swapMerchantDeviceRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.GenerateMerchantStatement(estateId, merchantId, generateMerchantStatementRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpGet]
        [Route("{merchantId}")]
        [SwaggerResponse(200, "OK", typeof(MerchantResponse))]
        [SwaggerResponseExample(200, typeof(MerchantResponseExample))]
        public async Task<IActionResult> GetMerchant([FromRoute] Guid estateId,
                                                      [FromRoute] Guid merchantId,
                                                      CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.GetMerchant(estateId, merchantId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [Route("{merchantId}/contracts")]
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> GetMerchantContracts([FromRoute] Guid estateId,
                                                              [FromRoute] Guid merchantId,
                                                              CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.GetMerchantContracts(estateId, merchantId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(List<MerchantResponse>))]
        [SwaggerResponseExample(200, typeof(MerchantResponseListExample))]
        public async Task<IActionResult> GetMerchants([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.GetMerchants(estateId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.GetTransactionFeesForProduct(estateId, merchantId, contractId, productId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }


        private Boolean PerformMerchantUserChecks(Guid estateId, Guid merchantId)
        {

            if (this.PerformStandardChecks(estateId) == false){
                return false;
            }

            String merchantRoleName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MerchantRoleName"))
                ? "Merchant"
                : Environment.GetEnvironmentVariable("MerchantRoleName");

            if (this.User.IsInRole(merchantRoleName) == false)
                return true;

            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { merchantRoleName }) == false)
            {
                return false;
            }

            Claim merchantIdClaim = ClaimsHelper.GetUserClaim(this.User, "MerchantId");

            if (ClaimsHelper.ValidateRouteParameter(merchantId, merchantIdClaim) == false)
            {
                return false;
            }

            return true;
        }

        private Boolean PerformStandardChecks(Guid estateId)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
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
                                                       CancellationToken cancellationToken){

            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.UpdateMerchant(estateId, merchantId, updateMerchantRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [Route("{merchantId}/addresses")]
        [HttpPatch]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> AddMerchantAddress([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] DataTransferObjects.Requests.Merchant.Address addAddressRequest,
                                                            CancellationToken cancellationToken){
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.AddMerchantAddress(estateId, merchantId, addAddressRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.UpdateMerchantAddress(estateId, merchantId, addressId, updateAddressRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.AddMerchantContact(estateId, merchantId, addContactRrequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2MerchantController.SetContextOverride(this.HttpContext);
            var result = await this.V2MerchantController.UpdateMerchantContact(estateId, merchantId, contactId, updateContactRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "merchants";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/estates/{estateid}/" + MerchantController.ControllerName;

        #endregion
    }
}
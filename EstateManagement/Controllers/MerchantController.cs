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
            this.Mediator = mediator;
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.CreateMerchantCommand command = new(estateId, createMerchantRequest);

            // Route the command
            Guid merchantId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{command.RequestDto}",
                                new CreateMerchantResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId
                                });

        }

        [HttpPost]
        [Route("{merchantId}/operators")]
        [ProducesResponseType(typeof(AssignOperatorResponse), 201)]
        [SwaggerResponse(201, "Created", typeof(AssignOperatorResponse))]
        [SwaggerResponseExample(201, typeof(AssignOperatorResponseExample))]
        public async Task<IActionResult> AssignOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        AssignOperatorRequest assignOperatorRequest,
                                                        CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AssignOperatorToMerchantCommand command = new(estateId, merchantId, assignOperatorRequest);

            // Route the command
            Guid operatorId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new AssignOperatorResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    OperatorId = operatorId
                                });
        }

        [HttpPost]
        [Route("{merchantId}/devices")]
        [SwaggerResponse(201, "Created", typeof(AddMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> AddDevice([FromRoute] Guid estateId,
                                                   [FromRoute] Guid merchantId,
                                                   [FromBody] AddMerchantDeviceRequest addMerchantDeviceRequest,
                                                   CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AddMerchantDeviceCommand command = new(estateId, merchantId, addMerchantDeviceRequest);

            // Route the command
            Guid deviceId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new AddMerchantDeviceResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DeviceId = deviceId
                                });
        }

        [HttpPost]
        [Route("{merchantId}/contracts")]
        public async Task<IActionResult> AddContract([FromRoute] Guid estateId,
                                                     [FromRoute] Guid merchantId,
                                                     [FromBody] AddMerchantContractRequest addMerchantContractRequest,
                                                     CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AddMerchantContractCommand command = new(estateId, merchantId, addMerchantContractRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Ok();
        }

        [HttpPost]
        [Route("{merchantId}/users")]
        [SwaggerResponse(201, "Created", typeof(CreateMerchantUserResponse))]
        [SwaggerResponseExample(201, typeof(CreateMerchantUserResponseExample))]
        public async Task<IActionResult> CreateMerchantUser([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] CreateMerchantUserRequest createMerchantUserRequest,
                                                            CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.CreateMerchantUserCommand command = new(estateId, merchantId, createMerchantUserRequest);

            // Route the command
            Guid userId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}/users/{userId}",
                                new CreateMerchantUserResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    UserId = userId
                                });
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            // This will always be a manual deposit as auto ones come in via another route
            MerchantCommands.MakeMerchantDepositCommand command = new(estateId, merchantId, Models.MerchantDepositSource.Manual, makeMerchantDepositRequest);

            Guid depositId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new MakeMerchantDepositResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DepositId = depositId
                                });
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.MakeMerchantWithdrawalCommand command = new(estateId, merchantId, makeMerchantWithdrawalRequest);

            // Route the command
            Guid withdrawalId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new MakeMerchantWithdrawalResponse()
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    WithdrawalId = withdrawalId
                                });

        }

        [HttpPatch]
        [Route("{merchantId}/devices")]
        [SwaggerResponse(201, "Created", typeof(SwapMerchantDeviceResponse))]
        [SwaggerResponseExample(201, typeof(AddMerchantDeviceResponseExample))]
        public async Task<IActionResult> SwapMerchantDevice([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] SwapMerchantDeviceRequest swapMerchantDeviceRequest,
                                                            CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.SwapMerchantDeviceCommand command = new(estateId, merchantId, swapMerchantDeviceRequest);

            Guid deviceId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new SwapMerchantDeviceResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DeviceId = deviceId
                                });
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.GenerateMerchantStatementCommand command = new(estateId, merchantId, generateMerchantStatementRequest);

            // Route the command
            Guid merchantStatementId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new GenerateMerchantStatementResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    MerchantStatementId = merchantStatementId
                                });
        }

        [HttpGet]
        [Route("{merchantId}")]
        [SwaggerResponse(200, "OK", typeof(MerchantResponse))]
        [SwaggerResponseExample(200, typeof(MerchantResponseExample))]
        public async Task<IActionResult> GetMerchant([FromRoute] Guid estateId,
                                                      [FromRoute] Guid merchantId,
                                                      CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantQueries.GetMerchantQuery query = new (estateId, merchantId);

            // Route the query
            Merchant merchant = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(merchant));
        }

        [Route("{merchantId}/contracts")]
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        [SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> GetMerchantContracts([FromRoute] Guid estateId,
                                                              [FromRoute] Guid merchantId,
                                                              CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantQueries.GetMerchantContractsQuery query = new (estateId, merchantId);

            List<Models.Contract.Contract> contracts = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(contracts));
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(List<MerchantResponse>))]
        [SwaggerResponseExample(200, typeof(MerchantResponseListExample))]
        public async Task<IActionResult> GetMerchants([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantQueries.GetMerchantsQuery query = new (estateId);

            List<Merchant> merchants = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(merchants));
        }

        [Route("{merchantId}/contracts/{contractId}/products/{productId}/transactionFees")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ContractProductTransactionFee>), 200)]
        [SwaggerResponse(200, "OK", typeof(List<ContractProductTransactionFee>))]
        [SwaggerResponseExample(200, typeof(ContractProductTransactionFeeResponseListExample))]
        public async Task<IActionResult> GetTransactionFeesForProduct([FromRoute] Guid estateId,
                                                                      [FromRoute] Guid merchantId,
                                                                      [FromRoute] Guid contractId,
                                                                      [FromRoute] Guid productId,
                                                                      CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformMerchantUserChecks(estateId, merchantId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantQueries.GetTransactionFeesForProductQuery query = new (estateId, merchantId, contractId, productId);

            List<TransactionFee> transactionFees = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(transactionFees));
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
        public async Task<ActionResult> UpdateMerchant([FromRoute] Guid estateId,
                                                       [FromRoute] Guid merchantId,
                                                       [FromBody] UpdateMerchantRequest updateMerchantRequest,
                                                       CancellationToken cancellationToken){

            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.UpdateMerchantCommand command = new (estateId, merchantId, updateMerchantRequest);
            
            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
        }

        [Route("{merchantId}/addresses")]
        [HttpPost]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> AddMerchantAddress([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] DataTransferObjects.Requests.Merchant.Address addAddressRequest,
                                                            CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AddMerchantAddressCommand command = new (estateId,merchantId, addAddressRequest);

            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.UpdateMerchantAddressCommand command = new(estateId, merchantId, addressId, updateAddressRequest);

            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
        }

        [Route("{merchantId}/contacts")]
        [HttpPost]
        //[SwaggerResponse(200, "OK", typeof(List<ContractResponse>))]
        //[SwaggerResponseExample(200, typeof(ContractResponseListExample))]
        public async Task<IActionResult> AddMerchantContact([FromRoute] Guid estateId,
                                                            [FromRoute] Guid merchantId,
                                                            [FromBody] DataTransferObjects.Requests.Merchant.Contact addContactRrequest,
                                                            CancellationToken cancellationToken)
        {
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AddMerchantContactCommand command = new(estateId, merchantId, addContactRrequest);

            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
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
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.UpdateMerchantContactCommand command = new(estateId, merchantId, contactId,updateContactRequest);

            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
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
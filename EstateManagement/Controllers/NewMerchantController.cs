using EstateManagement.BusinessLogic.Manger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagement.Controllers
{
    using System;
    using System.Net;
    using System.Security.AccessControl;
    using System.Threading.Tasks;
    using System.Threading;
    using Shared.General;
    using System.Security.Claims;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Responses.Merchant;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using NuGet.Packaging.Signing;

    public partial class MerchantController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [SwaggerResponse(201, "Created", typeof(CreateMerchantResponse))]
        [SwaggerResponseExample(201, typeof(CreateMerchantResponseExample))]
        public async Task<IActionResult> CreateMerchant([FromRoute] Guid estateId,
                                                        [FromBody] CreateMerchantRequest createMerchantRequest,
                                                        CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false){
                return this.Forbid();
            }

            MerchantCommands.CreateMerchantCommand command = new (estateId, createMerchantRequest);

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
                                                        CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AssignOperatorToMerchantCommand command = new (estateId, merchantId, assignOperatorRequest);

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
                                                   CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.AddMerchantDeviceCommand command = new (estateId, merchantId,addMerchantDeviceRequest);

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
                                                     CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }
            MerchantCommands.AddMerchantContractCommand command = new (estateId, merchantId, addMerchantContractRequest);

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
                                                            CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            MerchantCommands.CreateMerchantUserCommand command = new (estateId, merchantId, createMerchantUserRequest);

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
                                                     CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false)
            {
                return this.Forbid();
            }

            // This will always be a manual deposit as auto ones come in via another route
            MerchantCommands.MakeMerchantDepositCommand command = new (estateId, merchantId, Models.MerchantDepositSource.Manual, makeMerchantDepositRequest);

            Guid depositId  = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new MakeMerchantDepositResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    DepositId = depositId
                                });
        }

        public async Task<IActionResult> MakeWithdrawal([FromRoute] Guid estateId,
                                                        [FromRoute] Guid merchantId,
                                                        [FromBody] MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest,
                                                        CancellationToken cancellationToken){
            Boolean isRequestAllowed = this.PerformStandardChecks(estateId);
            if (isRequestAllowed == false){
                return this.Forbid();
            }

            MerchantCommands.MakeMerchantWithdrawalCommand command = new(estateId, merchantId, makeMerchantWithdrawalRequest);

            // Route the command
            Guid withdrawalId = await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new MakeMerchantWithdrawalResponse(){
                                                                        EstateId = estateId,
                                                                        MerchantId = merchantId,
                                                                        WithdrawalId = withdrawalId
                                                                    });

        }

        private Boolean PerformStandardChecks(Guid estateId)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false){
                return false;
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return false;
            }

            return true;
        }

    }
}

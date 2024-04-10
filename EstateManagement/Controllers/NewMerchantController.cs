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

            CreateMerchantCommand command = new (estateId, createMerchantRequest);

            // Route the command
            var merchantId = await this.Mediator.Send(command, cancellationToken);

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

            AssignOperatorToMerchantCommand command = new (estateId, merchantId, assignOperatorRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{MerchantController.ControllerRoute}/{merchantId}",
                                new AssignOperatorResponse
                                {
                                    EstateId = estateId,
                                    MerchantId = merchantId,
                                    OperatorId = assignOperatorRequest.OperatorId
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

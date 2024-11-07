using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.BusinessLogic.Requests;
using EstateManagement.DataTransferObjects.Responses.File;
using MediatR;
using SimpleResults;

namespace EstateManagement.Controllers.v2
{
    using EstateManagement.BusinessLogic.Manger;
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Factories;
    using Models.File;
    using Shared.General;
    using Microsoft.AspNetCore.Http;

    //[Route("api/[controller]")]
    //[ApiController]
    [ExcludeFromCodeCoverage]
    [Route(ControllerRoute)]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IMediator Mediator;

        public FileController(IMediator mediator)
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

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "files";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/estates/{estateid}/" + ControllerName;

        #endregion

        [HttpGet]
        [Route("{fileId}")]
        //[SwaggerResponse(200, "OK", typeof(ContractResponse))]
        //[SwaggerResponseExample(200, typeof(ContractResponseExample))]
        public async Task<IActionResult> GetFile([FromRoute] Guid estateId,
                                                 [FromRoute] Guid fileId,
                                                 CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.GetUser(), "EstateId", estateId.ToString());

            string estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.GetUser(), new[] { string.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return Forbid();
            }

            FileQueries.GetFileQuery query = new FileQueries.GetFileQuery(estateId, fileId);
            Result<File> result = await Mediator.Send(query, cancellationToken);


            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }

    }
}

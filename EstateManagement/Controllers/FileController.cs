using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagement.Controllers
{
    using EstateManagement.BusinessLogic.Manger;
    using MediatR;
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Examples;
    using DataTransferObjects.Responses;
    using Factories;
    using Models.File;
    using Shared.Exceptions;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    //[Route("api/[controller]")]
    //[ApiController]
    [ExcludeFromCodeCoverage]
    [Route(FileController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IEstateManagementManager EstateManagementManager;

        public FileController(IEstateManagementManager estateManagementManager)
        {
            this.EstateManagementManager = estateManagementManager;
        }

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "files";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/estates/{estateid}/" + FileController.ControllerName;

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

            File file = await this.EstateManagementManager.GetFileDetails(estateId, fileId, cancellationToken);
            
            return this.Ok(ModelFactory.ConvertFrom(file));
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.DataTransferObjects.Responses.File;
using MediatR;

namespace EstateManagement.Controllers
{
    using EstateManagement.BusinessLogic.Manger;
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Factories;
    using Models.File;
    using Shared.General;
    using SimpleResults;

    //[Route("api/[controller]")]
    //[ApiController]
    [ExcludeFromCodeCoverage]
    [Route(FileController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase {
        private EstateManagement.Controllers.v2.FileController V2FileController;

        private readonly IEstateManagementManager EstateManagementManager;

        public FileController(IEstateManagementManager estateManagementManager, IMediator mediator)
        {
            this.EstateManagementManager = estateManagementManager;
            this.V2FileController = new v2.FileController(mediator);
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
            this.V2FileController.SetContextOverride(this.HttpContext);
            ActionResult<Result<FileDetailsResponse>> result = await this.V2FileController.GetFile(estateId, fileId, cancellationToken);

            return ActionResultHelpers.HandleResult(result, "");
        }

    }
}

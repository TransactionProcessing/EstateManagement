namespace EstateManagement.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Commands;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using EstateManagement.BusinessLogic.CommandHandlers;
    using Microsoft.AspNetCore.Mvc;
    using Shared.DomainDrivenDesign.CommandHandling;

    [Route(EstateController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    public class EstateController : ControllerBase
    {
        private readonly ICommandRouter CommandRouter;

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "estates";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + EstateController.ControllerName;

        #endregion

        public EstateController(CommandRouter commandRouter)
        {
            this.CommandRouter = commandRouter;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateEstate([FromBody] CreateEstateRequest createEstateRequest, CancellationToken cancellationToken)
        {
            Guid estateId = Guid.NewGuid();

            // Create the command
            CreateEstateCommand command = CreateEstateCommand.Create(estateId, createEstateRequest.EstateName);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{EstateController.ControllerRoute}/{estateId}",
                                new CreateEstateResponse
                                {
                                    EstateId = estateId
                                });
        }
    }
}
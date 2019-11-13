namespace EstateManagement.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.CommandHandlers;
    using BusinessLogic.Commands;
    using BusinessLogic.Manger;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Factories;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route(EstateController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    public class EstateController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        /// <summary>
        /// The estate managment manager
        /// </summary>
        private readonly IEstateManagmentManager EstateManagmentManager;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="estateManagmentManager">The estate managment manager.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateController(ICommandRouter commandRouter,
                                IEstateManagmentManager estateManagmentManager,
                                IModelFactory modelFactory)
        {
            this.CommandRouter = commandRouter;
            this.EstateManagmentManager = estateManagmentManager;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="createEstateRequest">The create estate request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateEstate([FromBody] CreateEstateRequest createEstateRequest,
                                                      CancellationToken cancellationToken)
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

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Estate not found with estate Id {estateId}</exception>
        [HttpGet]
        [Route("{estateId}")]
        public async Task<IActionResult> GetEstate([FromRoute] Guid estateId,
                                                   CancellationToken cancellationToken)
        {
            Estate estate = await this.EstateManagmentManager.GetEstate(estateId, cancellationToken);

            if (estate == null)
            {
                throw new NotFoundException($"Estate not found with estate Id {estateId}");
            }

            return this.Ok(this.ModelFactory.ConvertFrom(estate));
        }

        #endregion

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
    }
}
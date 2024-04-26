namespace EstateManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manger;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Estate;
    using DataTransferObjects.Responses.Estate;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Shared.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Models.Estate;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    
    [ExcludeFromCodeCoverage]
    [Route(EstateController.ControllerRoute)]
    [ApiController]
    [Authorize]
    public class EstateController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// The estate management manager
        /// </summary>
        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        #region Constructors

        public EstateController(IMediator mediator,
                                IEstateManagementManager estateManagementManager)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
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
        [SwaggerResponse(201, "Created", typeof(CreateEstateResponse))]
        [SwaggerResponseExample(201, typeof(CreateEstateResponseExample))]
        public async Task<IActionResult> CreateEstate([FromBody] CreateEstateRequest createEstateRequest,
                                                      CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            Guid estateId = createEstateRequest.EstateId;

            // Create the command
            EstateCommands.CreateEstateCommand command = new(createEstateRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

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
        [SwaggerResponse(200, "OK", typeof(EstateResponse))]
        [SwaggerResponseExample(200, typeof(EstateResponseExample))]
        public async Task<IActionResult> GetEstate([FromRoute] Guid estateId,
                                                   CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }
            
            EstateQueries.GetEstateQuery query = new(estateId);

            Estate estate = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(estate));
        }

        [HttpGet]
        [Route("{estateId}/all")]
        [SwaggerResponse(200, "OK", typeof(List<EstateResponse>))]
        [SwaggerResponseExample(200, typeof(EstatesResponseExample))]
        public async Task<IActionResult> GetEstates([FromRoute] Guid estateId,
                                                   CancellationToken cancellationToken)
        {
            // Get the Estate Id claim from the user
            Claim estateIdClaim = ClaimsHelper.GetUserClaim(this.User, "EstateId", estateId.ToString());

            String estateRoleName = Environment.GetEnvironmentVariable("EstateRoleName");
            if (ClaimsHelper.IsUserRolesValid(this.User, new[] { String.IsNullOrEmpty(estateRoleName) ? "Estate" : estateRoleName }) == false)
            {
                return this.Forbid();
            }

            if (ClaimsHelper.ValidateRouteParameter(estateId, estateIdClaim) == false)
            {
                return this.Forbid();
            }

            EstateQueries.GetEstatesQuery query = new(estateId);

            List<Estate> estates = await this.Mediator.Send(query, cancellationToken);

            Estate estate = estates.Single();

            EstateResponse estateResponse = ModelFactory.ConvertFrom(estate);
            return this.Ok(new List<EstateResponse>() {estateResponse});
        }

        /// <summary>
        /// Creates the estate user.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createEstateUserRequest">The create estate user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{estateId}/users")]
        [SwaggerResponse(201, "Created", typeof(CreateEstateUserResponse))]
        [SwaggerResponseExample(201, typeof(CreateEstateUserResponseExample))]
        public async Task<IActionResult> CreateEstateUser([FromRoute] Guid estateId,
                                                          [FromBody] CreateEstateUserRequest createEstateUserRequest,
                                                          CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            // Create the command
            EstateCommands.CreateEstateUserCommand command = new(estateId, createEstateUserRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Ok();
        }

        [HttpPost]
        [Route("{estateId}/operators")]
        public async Task<IActionResult> AssignOperator([FromRoute] Guid estateId, [FromBody] AssignOperatorRequest assignOperatorRequest, CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            // Create the command
            EstateCommands.AddOperatorToEstateCommand command = new(estateId, assignOperatorRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Ok();
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
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
    using Common;
    using Common.Examples;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Shared.Exceptions;
    using EstateManagement.BusinessLogic.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Models.Estate;
    using Shared.General;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using CreateEstateRequest = BusinessLogic.Requests.CreateEstateRequest;
    using CreateEstateRequestDTO = DataTransferObjects.Requests.CreateEstateRequest;
    using CreateEstateUserRequest = BusinessLogic.Requests.CreateEstateUserRequest;
    using CreateEstateUserRequestDTO = DataTransferObjects.Requests.CreateEstateUserRequest;

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
        public async Task<IActionResult> CreateEstate([FromBody] CreateEstateRequestDTO createEstateRequest,
                                                      CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            Guid estateId = createEstateRequest.EstateId;

            // Create the command
            CreateEstateRequest request = CreateEstateRequest.Create(estateId, createEstateRequest.EstateName);

            // Route the command
            await this.Mediator.Send(request, cancellationToken);

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
            
            Estate estate = await this.EstateManagementManager.GetEstate(estateId, cancellationToken);

            if (estate == null)
            {
                throw new NotFoundException($"Estate not found with estate Id {estateId}");
            }

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

            List<Estate> estates = await this.EstateManagementManager.GetEstates(estateId, cancellationToken);

            if (estates.Any() == false)
            {
                throw new NotFoundException($"Estate not found with estate Id {estateId}");
            }

            var estate = estates.Single();

            EstateResponse estateReponse = ModelFactory.ConvertFrom(estate);
            return this.Ok(new List<EstateResponse>() {estateReponse});
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
                                                          [FromBody] CreateEstateUserRequestDTO createEstateUserRequest,
                                                          CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            // Create the command
            CreateEstateUserRequest request = CreateEstateUserRequest.Create(estateId, createEstateUserRequest.EmailAddress,
                                                                             createEstateUserRequest.Password,
                                                                             createEstateUserRequest.GivenName,
                                                                             createEstateUserRequest.MiddleName,
                                                                             createEstateUserRequest.FamilyName);

            // Route the command
            Guid userId = await this.Mediator.Send(request, cancellationToken);

            // return the result
            return this.Created($"{EstateController.ControllerRoute}/{estateId}/users/{userId}",
                                new CreateEstateUserResponse
                                {
                                    EstateId = estateId,
                                    UserId = userId
                                });
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
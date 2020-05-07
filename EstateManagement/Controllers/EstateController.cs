namespace EstateManagement.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manger;
    using Common;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.Exceptions;
    using EstateManagement.BusinessLogic.Requests;
    using Microsoft.AspNetCore.Authorization;
    using CreateEstateRequest = BusinessLogic.Requests.CreateEstateRequest;
    using CreateEstateRequestDTO = DataTransferObjects.Requests.CreateEstateRequest;
    using CreateEstateUserRequest = BusinessLogic.Requests.CreateEstateUserRequest;
    using CreateEstateUserRequestDTO = DataTransferObjects.Requests.CreateEstateUserRequest;

    [ExcludeFromCodeCoverage]
    [Route(EstateController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
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

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateController" /> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="estateManagementManager">The estate management manager.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateController(IMediator mediator,
                                IEstateManagementManager estateManagementManager,
                                IModelFactory modelFactory)
        {
            this.Mediator = mediator;
            this.EstateManagementManager = estateManagementManager;
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

            return this.Ok(this.ModelFactory.ConvertFrom(estate));
        }

        [HttpPost]
        [Route("{estateId}/users")]
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
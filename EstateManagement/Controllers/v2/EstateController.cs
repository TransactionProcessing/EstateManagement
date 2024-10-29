﻿using SimpleResults;

namespace EstateManagement.Controllers.v2
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
    using Microsoft.AspNetCore.Http;

    [ExcludeFromCodeCoverage]
    [Route(ControllerRoute)]
    [ApiController]
    [Authorize]
    public class EstateController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        #endregion

        #region Constructors

        public EstateController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private ClaimsPrincipal UserOverride;
        internal void SetContextOverride(HttpContext ctx)
        {
            this.UserOverride = ctx.User;
        }

        internal ClaimsPrincipal GetUser() {
            return this.UserOverride switch
            {
                null => this.HttpContext.User,
                _ => this.UserOverride
            };
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
        public async Task<ActionResult<Result>> CreateEstate([FromBody] CreateEstateRequest createEstateRequest,
                                                      CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.GetUser()))
            {
                return Forbid();
            }

            // Create the command
            EstateCommands.CreateEstateCommand command = new(createEstateRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            return result.ToActionResult();
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
        public async Task<ActionResult<Result<EstateResponse>>> GetEstate([FromRoute] Guid estateId,
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

            EstateQueries.GetEstateQuery query = new(estateId);

            Result<Estate> result = await Mediator.Send(query, cancellationToken);
            if (result.IsFailed)
            {
                return result.ToActionResult().Result;
            }

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        [HttpGet]
        [Route("{estateId}/all")]
        [SwaggerResponse(200, "OK", typeof(List<EstateResponse>))]
        [SwaggerResponseExample(200, typeof(EstatesResponseExample))]
        public async Task<ActionResult<Result<List<EstateResponse>>>> GetEstates([FromRoute] Guid estateId,
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

            EstateQueries.GetEstatesQuery query = new(estateId);

            Result<List<Estate>> result = await Mediator.Send(query, cancellationToken);
            if (result.IsFailed)
            {
                return result.ToActionResult().Result;
            }

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        /// <summary>
        /// Creates the estate user.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createEstateUserRequest">The create estate user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>/// <returns></returns>
        [HttpPatch]
        [Route("{estateId}/users")]
        [SwaggerResponse(201, "Created", typeof(CreateEstateUserResponse))]
        [SwaggerResponseExample(201, typeof(CreateEstateUserResponseExample))]
        public async Task<ActionResult<Result>> CreateEstateUser([FromRoute] Guid estateId,
                                                                 [FromBody] CreateEstateUserRequest createEstateUserRequest,
                                                                 CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.GetUser()))
            {
                return Forbid();
            }

            // Create the command
            EstateCommands.CreateEstateUserCommand command = new(estateId, createEstateUserRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResult();
        }

        [HttpPatch]
        [Route("{estateId}/operators")]
        public async Task<ActionResult<Result>> AssignOperator([FromRoute] Guid estateId, [FromBody] AssignOperatorRequest assignOperatorRequest, CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.GetUser()))
            {
                return Forbid();
            }

            // Create the command
            EstateCommands.AddOperatorToEstateCommand command = new(estateId, assignOperatorRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResult();
        }

        [HttpDelete]
        [Route("{estateId}/operators/{operatorId}")]
        public async Task<ActionResult<Result>> RemoveOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid operatorId,
                                                        CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.GetUser()))
            {
                return Forbid();
            }

            EstateCommands.RemoveOperatorFromEstateCommand command = new(estateId, operatorId);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResult();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "estates";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/" + ControllerName;

        #endregion
    }
}
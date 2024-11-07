using SimpleResults;

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
    public class EstateController : ControllerBase {
        public EstateManagement.Controllers.v2.EstateController V2EstateController;

        #region Fields

        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;
        
        #endregion

        #region Constructors

        public EstateController(IMediator mediator)
        {
            this.Mediator = mediator;
            this.V2EstateController = new v2.EstateController(mediator);
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
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.CreateEstate(createEstateRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.GetEstate(estateId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpGet]
        [Route("{estateId}/all")]
        [SwaggerResponse(200, "OK", typeof(List<EstateResponse>))]
        [SwaggerResponseExample(200, typeof(EstatesResponseExample))]
        public async Task<IActionResult> GetEstates([FromRoute] Guid estateId,
                                                   CancellationToken cancellationToken)
        {
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.GetEstates(estateId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
        public async Task<IActionResult> CreateEstateUser([FromRoute] Guid estateId,
                                                          [FromBody] CreateEstateUserRequest createEstateUserRequest,
                                                          CancellationToken cancellationToken)
        {
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.CreateEstateUser(estateId, createEstateUserRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpPatch]
        [Route("{estateId}/operators")]
        public async Task<IActionResult> AssignOperator([FromRoute] Guid estateId, [FromBody] AssignOperatorRequest assignOperatorRequest, CancellationToken cancellationToken)
        {
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.AssignOperator(estateId, assignOperatorRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpDelete]
        [Route("{estateId}/operators/{operatorId}")]
        public async Task<IActionResult> RemoveOperator([FromRoute] Guid estateId,
                                                        [FromRoute] Guid operatorId,
                                                        CancellationToken cancellationToken)
        {
            this.V2EstateController.SetContextOverride(this.HttpContext);
            var result = await this.V2EstateController.RemoveOperator(estateId, operatorId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
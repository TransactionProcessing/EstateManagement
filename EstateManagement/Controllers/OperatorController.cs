using System;
using System.Threading.Tasks;

namespace EstateManagement.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Operator;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Models.Operator;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    [Route(OperatorController.ControllerRoute)]
    [ApiController]
    public class OperatorController : ControllerBase {
        private EstateManagement.Controllers.v2.OperatorController V2OperatorController;
        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public OperatorController(IMediator mediator)
        {
            this.Mediator = mediator;
            this.V2OperatorController = new v2.OperatorController(mediator);
        }

        /// <summary>
        /// Creates the operator.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createOperatorRequest">The create operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponse(201, "Created", typeof(CreateOperatorResponse))]
        [SwaggerResponseExample(201, typeof(CreateOperatorResponseExample))]
        public async Task<IActionResult> CreateOperator([FromRoute] Guid estateId,  [FromBody] CreateOperatorRequest createOperatorRequest, CancellationToken cancellationToken)
        {
            this.V2OperatorController.SetContextOverride(this.HttpContext);
            var result = await this.V2OperatorController.CreateOperator(estateId, createOperatorRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpPost]
        [Route("{operatorId}")]
        [SwaggerResponse(200, "OK")]
        public async Task<IActionResult> UpdateOperator([FromRoute] Guid estateId, [FromRoute] Guid operatorId, [FromBody] UpdateOperatorRequest updateOperatorRequest, CancellationToken cancellationToken)
        {
            this.V2OperatorController.SetContextOverride(this.HttpContext);
            var result = await this.V2OperatorController.UpdateOperator(estateId, operatorId, updateOperatorRequest, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }


        [HttpGet]
        [Route("{operatorId}")]
        [SwaggerResponse(200, "OK", typeof(OperatorResponse))]
        [SwaggerResponseExample(201, typeof(OperatorResponseExample))]
        public async Task<IActionResult> GetOperator([FromRoute] Guid estateId,
                                                     [FromRoute] Guid operatorId,
                                                     CancellationToken cancellationToken)
        {
            this.V2OperatorController.SetContextOverride(this.HttpContext);
            var result = await this.V2OperatorController.GetOperator(estateId, operatorId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(OperatorResponse))]
        [SwaggerResponseExample(201, typeof(OperatorResponseExample))]
        public async Task<IActionResult> GetOperators([FromRoute] Guid estateId,
                                                     CancellationToken cancellationToken)
        {
            this.V2OperatorController.SetContextOverride(this.HttpContext);
            var result = await this.V2OperatorController.GetOperators(estateId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "operators";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/estates/{estateid}/" + OperatorController.ControllerName;

        #endregion
    }
}

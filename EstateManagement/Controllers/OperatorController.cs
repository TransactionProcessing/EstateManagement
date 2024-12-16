using System;
using System.Threading.Tasks;
using Shared.Results;
using SimpleResults;

namespace EstateManagement.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Operator;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.Operator;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    [Route(ControllerRoute)]
    [ApiController]
    public class OperatorController : ControllerBase
    {
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
            Mediator = mediator;
        }

        private ClaimsPrincipal UserOverride;
        internal void SetContextOverride(HttpContext ctx)
        {
            UserOverride = ctx.User;
        }

        internal ClaimsPrincipal GetUser()
        {
            return UserOverride switch
            {
                null => HttpContext.User,
                _ => UserOverride
            };
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
        public async Task<IActionResult> CreateOperator([FromRoute] Guid estateId, [FromBody] CreateOperatorRequest createOperatorRequest, CancellationToken cancellationToken)
        {
            // Create the command
            OperatorCommands.CreateOperatorCommand command = new OperatorCommands.CreateOperatorCommand(estateId, createOperatorRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }

        [HttpPost]
        [Route("{operatorId}")]
        [SwaggerResponse(200, "OK")]
        public async Task<IActionResult> UpdateOperator([FromRoute] Guid estateId, [FromRoute] Guid operatorId, [FromBody] UpdateOperatorRequest updateOperatorRequest, CancellationToken cancellationToken)
        {
            // Create the command
            OperatorCommands.UpdateOperatorCommand command = new OperatorCommands.UpdateOperatorCommand(estateId, operatorId, updateOperatorRequest);

            // Route the command
            Result result = await Mediator.Send(command, cancellationToken);

            // return the result
            return result.ToActionResultX();
        }


        [HttpGet]
        [Route("{operatorId}")]
        [SwaggerResponse(200, "OK", typeof(OperatorResponse))]
        [SwaggerResponseExample(201, typeof(OperatorResponseExample))]
        public async Task<IActionResult> GetOperator([FromRoute] Guid estateId,
                                                     [FromRoute] Guid operatorId,
                                                     CancellationToken cancellationToken)
        {
            // Create the command
            OperatorQueries.GetOperatorQuery query = new(estateId, operatorId);

            // Route the command
            Operator @operator = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(@operator).ToActionResultX();
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(200, "OK", typeof(OperatorResponse))]
        [SwaggerResponseExample(201, typeof(OperatorResponseExample))]
        public async Task<IActionResult> GetOperators([FromRoute] Guid estateId,
                                                      CancellationToken cancellationToken)
        {
            // Create the command
            OperatorQueries.GetOperatorsQuery query = new(estateId);

            // Route the command
            List<Operator> @operatorList = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(@operatorList).ToActionResultX();
        }

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "operators";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/estates/{estateid}/" + ControllerName;

        #endregion
    }
}

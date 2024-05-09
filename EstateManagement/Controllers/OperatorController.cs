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
            this.Mediator = mediator;
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
            // Create the command
            OperatorCommands.CreateOperatorCommand command = new OperatorCommands.CreateOperatorCommand(estateId, createOperatorRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);
            
            // return the result
            return this.Created($"{OperatorController.ControllerRoute}/{createOperatorRequest.OperatorId}",
                                new CreateOperatorResponse
                                {
                                    EstateId = estateId,
                                    OperatorId = createOperatorRequest.OperatorId
                                });
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
            Operator @operator = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(@operator));
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
            List<Operator> @operatorList = await this.Mediator.Send(query, cancellationToken);

            return this.Ok(ModelFactory.ConvertFrom(@operatorList));
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

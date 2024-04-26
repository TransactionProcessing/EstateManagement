using System;
using System.Threading.Tasks;

namespace EstateManagement.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using BusinessLogic.Requests;
    using Common.Examples;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Operator;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateOperator([FromBody] CreateOperatorRequest createOperatorRequest, CancellationToken cancellationToken)
        {
            // Create the command
            OperatorCommands.CreateOperatorCommand command = new OperatorCommands.CreateOperatorCommand(createOperatorRequest);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);
            
            // return the result
            return this.Created($"{OperatorController.ControllerRoute}/{createOperatorRequest.OperatorId}",
                                new CreateOperatorResponse
                                {
                                    EstateId = createOperatorRequest.EstateId,
                                    OperatorId = createOperatorRequest.OperatorId
                                });
        }

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "operators";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + OperatorController.ControllerName;

        #endregion
    }
}

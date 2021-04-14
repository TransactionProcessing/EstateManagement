using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateManagement.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using BusinessLogic.Requests;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Shared.DomainDrivenDesign.CommandHandling;

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
        [ProducesResponseType(typeof(CreateOperatorResponse), 201)]
        public async Task<IActionResult> CreateOperator([FromRoute] Guid estateId, [FromBody] CreateOperatorRequest createOperatorRequest, CancellationToken cancellationToken)
        {
            Guid operatorId = Guid.NewGuid();

            // Create the command
            AddOperatorToEstateRequest command = AddOperatorToEstateRequest.Create(estateId, operatorId,
                                                                         createOperatorRequest.Name,
                                                                         createOperatorRequest.RequireCustomMerchantNumber.Value,
                                                                         createOperatorRequest.RequireCustomTerminalNumber.Value);

            // Route the command
            await this.Mediator.Send(command, cancellationToken);

            // return the result
            return this.Created($"{OperatorController.ControllerRoute}/{operatorId}",
                                new CreateOperatorResponse
                                {
                                    EstateId = estateId,
                                    OperatorId = operatorId
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
        private const String ControllerRoute = "api/estates/{estateid}/" + OperatorController.ControllerName;

        #endregion
    }
}

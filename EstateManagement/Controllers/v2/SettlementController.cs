using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Requests;
using EstateManagement.DataTransferObjects.Responses.Settlement;
using EstateManagement.Factories;
using EstateManagement.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;

namespace EstateManagement.Controllers.v2 {
    [Route(SettlementController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SettlementController : ControllerBase {
        private readonly IMediator Mediator;

        #region Fields

        #endregion

        #region Constructors

        public SettlementController(IMediator mediator) {
            this.Mediator = mediator;
        }

        #endregion

        #region Methods

        [Route("{settlementId}")]
        [HttpGet]
        public async Task<ActionResult<Result<SettlementResponse>>> GetSettlement([FromRoute] Guid estateId,
            [FromQuery] Guid merchantId,
            [FromRoute] Guid settlementId,
            CancellationToken cancellationToken) {
            SettlementQueries.GetSettlementQuery query =
                new SettlementQueries.GetSettlementQuery(estateId, merchantId, settlementId);
            Result<SettlementModel> result = await this.Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<Result<List<SettlementResponse>>>> GetSettlements([FromRoute] Guid estateId,
            [FromQuery] Guid? merchantId,
            [FromQuery(Name = "start_date")] String startDate,
            [FromQuery(Name = "end_date")] String endDate,
            CancellationToken cancellationToken) {
            SettlementQueries.GetSettlementsQuery query =
                new SettlementQueries.GetSettlementsQuery(estateId, merchantId, startDate, endDate);
            Result<List<SettlementModel>> result = await this.Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResult();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "settlements";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/v2/estates/{estateId}/" + SettlementController.ControllerName;

        #endregion
    }
}
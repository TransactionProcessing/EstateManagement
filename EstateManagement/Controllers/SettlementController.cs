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
using Shared.Results;
using SimpleResults;

namespace EstateManagement.Controllers
{
    [Route(ControllerRoute)]
    [Authorize]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SettlementController : ControllerBase
    {
        private readonly IMediator Mediator;

        #region Fields

        #endregion

        #region Constructors

        public SettlementController(IMediator mediator)
        {
            Mediator = mediator;
        }

        #endregion

        #region Methods

        [Route("{settlementId}")]
        [HttpGet]
        public async Task<IActionResult> GetSettlement([FromRoute] Guid estateId,
                                                       [FromQuery] Guid merchantId,
                                                       [FromRoute] Guid settlementId,
                                                       CancellationToken cancellationToken)
        {
            SettlementQueries.GetSettlementQuery query =
                new SettlementQueries.GetSettlementQuery(estateId, merchantId, settlementId);
            Result<SettlementModel> result = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetSettlements([FromRoute] Guid estateId,
                                                        [FromQuery] Guid? merchantId,
                                                        [FromQuery(Name = "start_date")] string startDate,
                                                        [FromQuery(Name = "end_date")] string endDate,
                                                        CancellationToken cancellationToken)
        {
            SettlementQueries.GetSettlementsQuery query =
                new SettlementQueries.GetSettlementsQuery(estateId, merchantId, startDate, endDate);
            Result<List<SettlementModel>> result = await Mediator.Send(query, cancellationToken);

            return ModelFactory.ConvertFrom(result.Data).ToActionResultX();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const string ControllerName = "settlements";

        /// <summary>
        /// The controller route
        /// </summary>
        private const string ControllerRoute = "api/v2/estates/{estateId}/" + ControllerName;

        #endregion
    }
}
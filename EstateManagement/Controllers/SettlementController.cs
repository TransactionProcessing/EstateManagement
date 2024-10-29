using MediatR;

namespace EstateManagement.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.BusinessLogic.Manger;
    using EstateManagement.Database.Entities;
    using EstateManagement.Factories;
    using EstateManagement.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route(SettlementController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SettlementController : ControllerBase {
        #region Fields

        private EstateManagement.Controllers.v2.SettlementController V2SettlementController;

        private readonly IReportingManager ReportingManager;

        #endregion

        #region Constructors

        public SettlementController(IReportingManager reportingManager, IMediator mediator) {
            this.ReportingManager = reportingManager;
            this.V2SettlementController = new v2.SettlementController(mediator);
        }

        #endregion

        #region Methods

        [Route("{settlementId}")]
        [HttpGet]
        public async Task<IActionResult> GetSettlement([FromRoute] Guid estateId,
                                                       [FromQuery] Guid merchantId,
                                                       [FromRoute] Guid settlementId,
                                                       CancellationToken cancellationToken) {
            var result = await this.V2SettlementController.GetSettlement(estateId, merchantId, settlementId, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetSettlements([FromRoute] Guid estateId,
                                                        [FromQuery] Guid? merchantId,
                                                        [FromQuery(Name = "start_date")] String startDate,
                                                        [FromQuery(Name = "end_date")] String endDate,
                                                        CancellationToken cancellationToken) {
            var result = await this.V2SettlementController.GetSettlements(estateId, merchantId, startDate, endDate, cancellationToken);
            return ActionResultHelpers.HandleResult(result, String.Empty);
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
        private const String ControllerRoute = "api/estates/{estateId}/" + SettlementController.ControllerName;

        #endregion
    }
}
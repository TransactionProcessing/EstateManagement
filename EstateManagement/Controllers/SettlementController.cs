using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Manger;
using EstateManagement.Factories;
using EstateManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route(SettlementController.ControllerRoute)]
[Authorize]
[ApiController]
[ExcludeFromCodeCoverage]
public class SettlementController : ControllerBase
{
    #region Fields

    private readonly IModelFactory ModelFactory;

    private readonly IReportingManager ReportingManager;

    #endregion

    #region Constructors

    public SettlementController(IReportingManager reportingManager,
                                IModelFactory modelFactory)
    {
        this.ReportingManager = reportingManager;
        this.ModelFactory = modelFactory;
    }

    #endregion

    #region Methods

    [Route("{settlementId}")]
    [HttpGet]
    public async Task<IActionResult> GetSettlement([FromRoute] Guid estateId,
                                                   [FromQuery] Guid? merchantId,
                                                   [FromRoute] Guid settlementId,
                                                   CancellationToken cancellationToken)
    {
        SettlementModel model = await this.ReportingManager.GetSettlement(estateId, merchantId, settlementId, cancellationToken);

        return this.Ok(this.ModelFactory.ConvertFrom(model));
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetSettlements([FromRoute] Guid estateId,
                                                    [FromQuery] Guid? merchantId,
                                                    [FromQuery(Name = "start_date")] String startDate,
                                                    [FromQuery(Name = "end_date")] String endDate,
                                                    CancellationToken cancellationToken)
    {
        List<SettlementModel> model = await this.ReportingManager.GetSettlements(estateId, merchantId, startDate, endDate, cancellationToken);

        return this.Ok(this.ModelFactory.ConvertFrom(model));
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
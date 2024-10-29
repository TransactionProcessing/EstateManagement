using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Manger;
using EstateManagement.BusinessLogic.Requests;
using EstateManagement.Models;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.RequestHandlers;

public class SettlementRequestHandler : IRequestHandler<SettlementQueries.GetSettlementQuery,  Result<SettlementModel>>,
    IRequestHandler<SettlementQueries.GetSettlementsQuery, Result<List<SettlementModel>>>
{
    private readonly IReportingManager ReportingManager;

    public SettlementRequestHandler(IReportingManager reportingManager) {
        this.ReportingManager = reportingManager;
    }
    public async Task<Result<SettlementModel>> Handle(SettlementQueries.GetSettlementQuery request,
                                                      CancellationToken cancellationToken) {
        return await this.ReportingManager.GetSettlement(request.EstateId, request.MerchantId, request.SettlementId, cancellationToken);
    }

    public async Task<Result<List<SettlementModel>>> Handle(SettlementQueries.GetSettlementsQuery request,
                                                            CancellationToken cancellationToken) {
        return await this.ReportingManager.GetSettlements(request.EstateId, request.MerchantId, request.StartDate, request.EndDate, cancellationToken);
    }
}
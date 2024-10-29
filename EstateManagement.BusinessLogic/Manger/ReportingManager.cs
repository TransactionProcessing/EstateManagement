using SimpleResults;

namespace EstateManagement.BusinessLogic.Manger;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Repository;

public class ReportingManager : IReportingManager
{
    #region Fields

    /// <summary>
    /// The repository
    /// </summary>
    private readonly IEstateReportingRepository Repository;

    /// <summary>
    /// The repository for reports
    /// </summary>
    private readonly IEstateReportingRepositoryForReports RepositoryForReports;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportingManager" /> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="repositoryForReports">The repository for reports.</param>
    public ReportingManager(IEstateReportingRepository repository,
                            IEstateReportingRepositoryForReports repositoryForReports)
    {
        this.Repository = repository;
        this.RepositoryForReports = repositoryForReports;
    }

    #endregion

    #region Methods

    public async Task<Result<SettlementModel>> GetSettlement(Guid estateId,
                                                             Guid merchantId,
                                                             Guid settlementId,
                                                             CancellationToken cancellationToken)
    {
        return await this.RepositoryForReports.GetSettlement(estateId, merchantId, settlementId, cancellationToken);
    }

    public async Task<Result<List<SettlementModel>>> GetSettlements(Guid estateId,
                                                            Guid? merchantId,
                                                            String startDate,
                                                            String endDate,
                                                            CancellationToken cancellationToken)
    {
        return await this.RepositoryForReports.GetSettlements(estateId, merchantId, startDate, endDate, cancellationToken);
    }

    #endregion
}
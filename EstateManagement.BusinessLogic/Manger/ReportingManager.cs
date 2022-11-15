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

    public async Task<SettlementModel> GetSettlement(Guid estateId,
                                                     Guid? merchantId,
                                                     Guid settlementId,
                                                     CancellationToken cancellationToken)
    {
        SettlementModel model = await this.RepositoryForReports.GetSettlement(estateId, merchantId, settlementId, cancellationToken);

        return model;
    }

    public async Task<List<SettlementModel>> GetSettlements(Guid estateId,
                                                            Guid? merchantId,
                                                            String startDate,
                                                            String endDate,
                                                            CancellationToken cancellationToken)
    {
        List<SettlementModel> model = await this.RepositoryForReports.GetSettlements(estateId, merchantId, startDate, endDate, cancellationToken);

        return model;
    }

    #endregion
}
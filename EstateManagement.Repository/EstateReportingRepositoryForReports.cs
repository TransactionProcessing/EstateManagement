using SimpleResults;

namespace EstateManagement.Repository;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.Contexts;
using Database.ViewEntities;
using Microsoft.EntityFrameworkCore;
using Models;

[ExcludeFromCodeCoverage]
public class EstateReportingRepositoryForReports : IEstateReportingRepositoryForReports
{
    #region Fields

    /// <summary>
    /// The database context factory
    /// </summary>
    private readonly Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> DbContextFactory;

    private const String ConnectionStringIdentifier = "EstateReportingReadModel";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EstateReportingRepository" /> class.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    public EstateReportingRepositoryForReports(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> dbContextFactory)
    {
        this.DbContextFactory = dbContextFactory;
    }

    #endregion

    #region Methods

    public async Task<Result<SettlementModel>> GetSettlement(Guid estateId,
                                                             Guid merchantId,
                                                             Guid settlementId,
                                                             CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId, EstateReportingRepositoryForReports.ConnectionStringIdentifier, cancellationToken);

        IQueryable<SettlementView> query = context.SettlementsView.Where(t => t.EstateId == estateId && t.SettlementId == settlementId
                                                                         && t.MerchantId == merchantId).AsQueryable();
        
        var result = query.AsEnumerable().GroupBy(t => new {
                                                               t.SettlementId,
                                                               t.SettlementDate,
                                                               t.IsCompleted
                                                           }).SingleOrDefault();

        if (result == null)
            return Result.NotFound($"Settlement with Id {settlementId} not found");

        SettlementModel model = new SettlementModel
                                {
                                    SettlementDate = result.Key.SettlementDate,
                                    SettlementId = result.Key.SettlementId,
                                    NumberOfFeesSettled = result.Count(),
                                    ValueOfFeesSettled = result.Sum(x => x.CalculatedValue),
                                    IsCompleted = result.Key.IsCompleted
                                };

        result.ToList().ForEach(f => model.SettlementFees.Add(new SettlementFeeModel
                                                              {
                                                                  SettlementDate = f.SettlementDate,
                                                                  SettlementId = f.SettlementId,
                                                                  CalculatedValue = f.CalculatedValue,
                                                                  MerchantId = f.MerchantId,
                                                                  MerchantName = f.MerchantName,
                                                                  FeeDescription = f.FeeDescription,
                                                                  IsSettled = f.IsSettled,
                                                                  TransactionId = f.TransactionId,
                                                                  OperatorIdentifier = f.OperatorIdentifier
                                                              }));

        return Result.Success(model);
    }

    public async Task<Result<List<SettlementModel>>> GetSettlements(Guid estateId,
                                                                    Guid? merchantId,
                                                                    String startDate,
                                                                    String endDate,
                                                                    CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId,EstateReportingRepositoryForReports.ConnectionStringIdentifier, cancellationToken);

        DateTime queryStartDate = DateTime.ParseExact(startDate, "yyyyMMdd", null);
        DateTime queryEndDate = DateTime.ParseExact(endDate, "yyyyMMdd", null);

        IQueryable<SettlementView> query = context.SettlementsView.Where(t => t.EstateId == estateId &&
                                                                              t.SettlementDate >= queryStartDate.Date && t.SettlementDate <= queryEndDate.Date)
                                                  .AsQueryable();

        if (merchantId.HasValue)
        {
            query = query.Where(t => t.MerchantId == merchantId);
        }

        List<SettlementModel> result = await query.GroupBy(t => new
        {
            t.SettlementId,
            t.SettlementDate,
            t.IsCompleted
        }).Select(t => new SettlementModel
        {
            SettlementId = t.Key.SettlementId,
            SettlementDate = t.Key.SettlementDate,
            NumberOfFeesSettled = t.Count(),
            ValueOfFeesSettled = t.Sum(x => x.CalculatedValue),
            IsCompleted = t.Key.IsCompleted
        }).OrderByDescending(t => t.SettlementDate)
                                                  .ToListAsync(cancellationToken);
        
        return Result.Success(result);
    }

    #endregion
}
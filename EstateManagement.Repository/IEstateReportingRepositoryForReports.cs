﻿using SimpleResults;

namespace EstateManagement.Repository;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

public interface IEstateReportingRepositoryForReports
{
    #region Methods

    Task<Result<SettlementModel>> GetSettlement(Guid estateId,
                                                Guid merchantId,
                                                Guid settlementId,
                                                CancellationToken cancellationToken);

    Task<Result<List<SettlementModel>>> GetSettlements(Guid estateId,
                                                      Guid? merchantId,
                                                      String startDate,
                                                      String endDate,
                                                      CancellationToken cancellationToken);

    #endregion
}
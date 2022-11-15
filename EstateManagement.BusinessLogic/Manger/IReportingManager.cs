using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Manger
{
    using Models;

    public interface IReportingManager
    {
        #region Methods

        /// <summary>
        /// Gets the settlement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settlementId">The settlement identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<SettlementModel> GetSettlement(Guid estateId,
                                            Guid? merchantId,
                                            Guid settlementId,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the settlements.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<SettlementModel>> GetSettlements(Guid estateId,
                                                   Guid? merchantId,
                                                   String startDate,
                                                   String endDate,
                                                   CancellationToken cancellationToken);

        #endregion
    }
}

namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IMerchantStatementDomainService
    {
        #region Methods

        /// <summary>
        /// Adds the transaction to statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transactionDateTime">The transaction date time.</param>
        /// <param name="transactionAmount">The transaction amount.</param>
        /// <param name="isAuthorised">if set to <c>true</c> [is authorised].</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddTransactionToStatement(Guid estateId,
                                       Guid merchantId,
                                       DateTime transactionDateTime,
                                       Decimal? transactionAmount,
                                       Boolean isAuthorised,
                                       Guid transactionId,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Adds the settled fee to statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settledDateTime">The settled date time.</param>
        /// <param name="settledAmount">The settled amount.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="settledFeeId">The settled fee identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddSettledFeeToStatement(Guid estateId,
                                       Guid merchantId,
                                       DateTime settledDateTime,
                                       Decimal settledAmount,
                                       Guid transactionId,
                                       Guid settledFeeId,
                                       CancellationToken cancellationToken);

        #endregion
    }
}
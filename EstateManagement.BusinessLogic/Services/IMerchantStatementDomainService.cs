﻿namespace EstateManagement.BusinessLogic.Services
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

        /// <summary>
        /// Generates the statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="statementDate">The statement date.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Guid> GenerateStatement(Guid estateId,
                                     Guid merchantId,
                                     DateTime statementDate, 
                                     CancellationToken cancellationToken);

        /// <summary>
        /// Emails the statement.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="merchantStatementId">The merchant statement identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task EmailStatement(Guid estateId,
                            Guid merchantId,
                            Guid merchantStatementId,
                            CancellationToken cancellationToken);

        #endregion
    }
}
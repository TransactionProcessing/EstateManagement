namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Requests;

    /// <summary>
    /// 
    /// </summary>
    public interface IMerchantStatementDomainService
    {
        #region Methods

        Task AddTransactionToStatement(Guid estateId,
                                       Guid merchantId,
                                       DateTime transactionDateTime,
                                       Decimal? transactionAmount,
                                       Boolean isAuthorised,
                                       Guid transactionId,
                                       CancellationToken cancellationToken);

        Task AddSettledFeeToStatement(Guid estateId,
                                       Guid merchantId,
                                       DateTime settledDateTime,
                                       Decimal settledAmount,
                                       Guid transactionId,
                                       Guid settledFeeId,
                                       CancellationToken cancellationToken);

        Task<Guid> GenerateStatement(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken);

        Task EmailStatement(Guid estateId,
                            Guid merchantId,
                            Guid merchantStatementId,
                            CancellationToken cancellationToken);

        #endregion
    }
}
using SimpleResults;

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

        Task<Result> AddTransactionToStatement(AddTransactionToMerchantStatementRequest command, CancellationToken cancellationToken);

        Task<Result> AddSettledFeeToStatement(AddSettledFeeToMerchantStatementRequest command, CancellationToken cancellationToken);

        Task<Result> GenerateStatement(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken);

        Task<Result> EmailStatement(EmailMerchantStatementRequest command, CancellationToken cancellationToken);

        #endregion
    }
}
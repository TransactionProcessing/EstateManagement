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

        Task<Result> AddTransactionToStatement(MerchantStatementCommands.AddTransactionToMerchantStatementCommand command, CancellationToken cancellationToken);

        Task<Result> AddSettledFeeToStatement(MerchantStatementCommands.AddSettledFeeToMerchantStatementCommand command, CancellationToken cancellationToken);

        Task<Result> GenerateStatement(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken);

        Task<Result> EmailStatement(MerchantStatementCommands.EmailMerchantStatementCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
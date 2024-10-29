using EstateManagement.BusinessLogic.Requests;

namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Contract;
    using SimpleResults;

    /// <summary>
    /// 
    /// </summary>
    public interface IContractDomainService
    {
        #region Methods

        Task<Result> AddProductToContract(ContractCommands.AddProductToContractCommand command, CancellationToken cancellationToken);

        Task<Result> AddTransactionFeeForProductToContract(ContractCommands.AddTransactionFeeForProductToContractCommand command, CancellationToken cancellationToken);
        
        Task<Result> DisableTransactionFeeForProduct(ContractCommands.DisableTransactionFeeForProductCommand command, CancellationToken cancellationToken);

        Task<Result> CreateContract(ContractCommands.CreateContractCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
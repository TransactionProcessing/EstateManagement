using EstateManagement.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Models.Contract;

public class DummyContractDomainService : IContractDomainService
{
    public async Task<Result> AddProductToContract(ContractCommands.AddProductToContractCommand command, CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddTransactionFeeForProductToContract(ContractCommands.AddTransactionFeeForProductToContractCommand command,
                                                                    CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> DisableTransactionFeeForProduct(ContractCommands.DisableTransactionFeeForProductCommand command,
                                                              CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result> CreateContract(ContractCommands.CreateContractCommand command,
                                             CancellationToken cancellationToken) =>
        Result.Success();
}
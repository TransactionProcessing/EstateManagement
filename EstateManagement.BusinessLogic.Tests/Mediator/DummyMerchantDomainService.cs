using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Models;
using Requests;

public class DummyMerchantDomainService : IMerchantDomainService {
    public async Task<Result> CreateMerchant(MerchantCommands.CreateMerchantCommand command,
                                             CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AssignOperatorToMerchant(MerchantCommands.AssignOperatorToMerchantCommand command,
                                                       CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> CreateMerchantUser(MerchantCommands.CreateMerchantUserCommand command,
                                                 CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command,
                                                  CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command,
                                                 CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command,
                                                  CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command,
                                                     CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command,
                                                    CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand command,
                                             CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddMerchantAddress(MerchantCommands.AddMerchantAddressCommand command,
                                                 CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantAddressCommand command,
                                                    CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddMerchantContact(MerchantCommands.AddMerchantContactCommand command,
                                                 CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantContactCommand command,
                                                    CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command,
                                                         CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveMerchantContractCommand command,
                                                         CancellationToken cancellationToken) => Result.Success();
}
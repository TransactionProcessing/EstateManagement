namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Models;
using Requests;

public class DummyMerchantDomainService : IMerchantDomainService
{
    public async Task<Guid> CreateMerchant(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken){ 
        return Guid.NewGuid();
    }

    public async Task<Guid> AssignOperatorToMerchant(MerchantCommands.AssignOperatorToMerchantCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task<Guid> CreateMerchantUser(MerchantCommands.CreateMerchantUserCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task<Guid> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task SwapMerchantDevice(Guid estateId, Guid merchantId, Guid deviceId, String originalDeviceIdentifier, String newDeviceIdentifier, CancellationToken cancellationToken){
        
    }

    public async Task<Guid> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task<Guid> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task SetMerchantSettlementSchedule(Guid estateId, Guid merchantId, SettlementSchedule settlementSchedule, CancellationToken cancellationToken){
        
    }

    public async Task AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken){
        
    }
}
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

    public async Task SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command, CancellationToken cancellationToken){

    }
    
    public async Task<Guid> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task<Guid> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken){
        
    }

    public async Task UpdateMerchant(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken){
        
    }

    public async Task AddMerchantAddress(MerchantCommands.AddMerchantAddressCommand command, CancellationToken cancellationToken){
        
    }

    public async Task UpdateMerchantAddress(MerchantCommands.UpdateMerchantAddressCommand command, CancellationToken cancellationToken){
        
    }

    public async Task AddMerchantContact(MerchantCommands.AddMerchantContactCommand command, CancellationToken cancellationToken){
        
    }

    public async Task UpdateMerchantContact(MerchantCommands.UpdateMerchantContactCommand command, CancellationToken cancellationToken){
        
    }

    public async Task RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken){
        
    }

    public async Task RemoveContractFromMerchant(MerchantCommands.RemoveMerchantContractCommand command, CancellationToken cancellationToken){
        
    }
}
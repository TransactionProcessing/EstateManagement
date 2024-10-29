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
    public interface IMerchantDomainService
    {
        #region Methods
        Task<Result> CreateMerchant(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken);
        Task<Result> AssignOperatorToMerchant(MerchantCommands.AssignOperatorToMerchantCommand command, CancellationToken cancellationToken);
        Task<Result> CreateMerchantUser(MerchantCommands.CreateMerchantUserCommand command, CancellationToken cancellationToken);
        Task<Result> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command, CancellationToken cancellationToken);
        Task<Result> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command, CancellationToken cancellationToken);
        Task<Result> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken);
        Task<Result> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command, CancellationToken cancellationToken);
        Task<Result> AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken);
        Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken);
        Task<Result> AddMerchantAddress(MerchantCommands.AddMerchantAddressCommand command, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantAddressCommand command, CancellationToken cancellationToken);
        Task<Result> AddMerchantContact(MerchantCommands.AddMerchantContactCommand command, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantContactCommand command, CancellationToken cancellationToken);
        Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken);
        Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveMerchantContractCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
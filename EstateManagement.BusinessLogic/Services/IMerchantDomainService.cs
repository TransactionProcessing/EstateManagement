﻿namespace EstateManagement.BusinessLogic.Services
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
        Task<Guid> CreateMerchant(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken);
        
        Task<Guid> AssignOperatorToMerchant(MerchantCommands.AssignOperatorToMerchantCommand command, CancellationToken cancellationToken);

        Task<Guid> CreateMerchantUser(MerchantCommands.CreateMerchantUserCommand command, CancellationToken cancellationToken);

        Task<Guid> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command, CancellationToken cancellationToken);

        Task<Guid> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command, CancellationToken cancellationToken);
        
        Task<Guid> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken);

        Task<Guid> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command, CancellationToken cancellationToken);

        Task SetMerchantSettlementSchedule(Guid estateId,
                                           Guid merchantId,
                                           Models.SettlementSchedule settlementSchedule,
                                           CancellationToken cancellationToken);

        Task AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
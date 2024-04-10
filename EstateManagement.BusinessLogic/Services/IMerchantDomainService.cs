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
        Task<Guid> CreateMerchant(CreateMerchantCommand command, CancellationToken cancellationToken);
        
        Task AssignOperatorToMerchant(AssignOperatorToMerchantCommand command, CancellationToken cancellationToken);

        Task<Guid> CreateMerchantUser(Guid estateId,
                                      Guid merchantId,
                                      String emailAddress,
                                      String password,
                                      String givenName,
                                      String middleName,
                                      String familyName,
                                      CancellationToken cancellationToken);

        Task AddDeviceToMerchant(Guid estateId,
                                         Guid merchantId,
                                         Guid deviceId,
                                         String deviceIdentifier,
                                         CancellationToken cancellationToken);

        Task SwapMerchantDevice(Guid estateId,
            Guid merchantId,
            Guid deviceId,
            String originalDeviceIdentifier,
            String newDeviceIdentifier,
            CancellationToken cancellationToken);
        
        Task<Guid> MakeMerchantDeposit(Guid estateId,
                                 Guid merchantId,
                                 Models.MerchantDepositSource source,
                                 String reference,
                                 DateTime depositDateTime,
                                 Decimal amount,
                                 CancellationToken cancellationToken);

        Task<Guid> MakeMerchantWithdrawal(Guid estateId,
                                       Guid merchantId,
                                       DateTime withdrawalDateTime,
                                       Decimal amount,
                                       CancellationToken cancellationToken);

        Task SetMerchantSettlementSchedule(Guid estateId,
                                           Guid merchantId,
                                           Models.SettlementSchedule settlementSchedule,
                                           CancellationToken cancellationToken);

        Task AddContractToMerchant(Guid estateId,
                                   Guid merchantId,
                                   Guid contractId,
                                   CancellationToken cancellationToken);

        #endregion
    }
}
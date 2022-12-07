namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /// <summary>
    /// 
    /// </summary>
    public interface IMerchantDomainService
    {
        #region Methods
        
        Task CreateMerchant(Guid estateId,
                            Guid merchantId,
                            String name,
                            Guid addressId,
                            String addressLine1,
                            String addressLine2,
                            String addressLine3,
                            String addressLine4,
                            String town,
                            String region,
                            String postalCode,
                            String country,
                            Guid contactId,
                            String contactName,
                            String contactPhoneNumber,
                            String contactEmailAddress,
                            Models.SettlementSchedule settlementSchedule,
                            DateTime createDateTime,
                            CancellationToken cancellationToken);

        Task AssignOperatorToMerchant(Guid estateId,
                                      Guid merchantId,
                                      Guid operatorId,
                                      String merchantNumber,
                                      String terminalNumber,
                                      CancellationToken cancellationToken);

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

        #endregion
    }
}
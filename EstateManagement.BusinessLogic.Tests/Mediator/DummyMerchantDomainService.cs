namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Models;
using Requests;

public class DummyMerchantDomainService : IMerchantDomainService
{
    public async Task<Guid> CreateMerchant(CreateMerchantCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }

    public async Task AssignOperatorToMerchant(AssignOperatorToMerchantCommand command, CancellationToken cancellationToken){
        
    }

    public async Task CreateMerchant(Guid estateId,
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
                                     SettlementSchedule settlementSchedule,
                                     DateTime createDateTime,
                                     CancellationToken cancellationToken) {
    }
    
    public async Task<Guid> CreateMerchantUser(Guid estateId,
                                               Guid merchantId,
                                               String emailAddress,
                                               String password,
                                               String givenName,
                                               String middleName,
                                               String familyName,
                                               CancellationToken cancellationToken) {
        return Guid.NewGuid();
    }

    public async Task AddDeviceToMerchant(Guid estateId,
                                          Guid merchantId,
                                          Guid deviceId,
                                          String deviceIdentifier,
                                          CancellationToken cancellationToken) {
    }

    public async Task SwapMerchantDevice(Guid estateId,
                                         Guid merchantId,
                                         Guid deviceId,
                                         String originalDeviceIdentifier,
                                         String newDeviceIdentifier,
                                         CancellationToken cancellationToken) {
    }

    public async Task<Guid> MakeMerchantDeposit(Guid estateId,
                                                Guid merchantId,
                                                MerchantDepositSource source,
                                                String reference,
                                                DateTime depositDateTime,
                                                Decimal amount,
                                                CancellationToken cancellationToken) {
        return Guid.NewGuid();
    }

    public async Task<Guid> MakeMerchantWithdrawal(Guid estateId,
                                                   Guid merchantId,
                                                   DateTime withdrawalDateTime,
                                                   Decimal amount,
                                                   CancellationToken cancellationToken) {
        return Guid.NewGuid();
    }

    public async Task SetMerchantSettlementSchedule(Guid estateId,
                                                    Guid merchantId,
                                                    SettlementSchedule settlementSchedule,
                                                    CancellationToken cancellationToken) {
    }

    public async Task AddContractToMerchant(Guid estateId, Guid merchantId, Guid contractId, CancellationToken cancellationToken){
        
    }
}
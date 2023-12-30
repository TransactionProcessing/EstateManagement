using Shared.DomainDrivenDesign.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.Merchant.DomainEvents
{
    public record AddressAddedEvent(Guid MerchantId,
                                    Guid EstateId,
                                    Guid AddressId,
                                    String AddressLine1,
                                    String AddressLine2,
                                    String AddressLine3,
                                    String AddressLine4,
                                    String Town,
                                    String Region,
                                    String PostalCode,
                                    String Country) : DomainEvent(MerchantId, Guid.NewGuid());

    public record AutomaticDepositMadeEvent(Guid MerchantId,
                                            Guid EstateId,
                                            Guid DepositId,
                                            String Reference,
                                            DateTime DepositDateTime,
                                            Decimal Amount) : DomainEvent(MerchantId, Guid.NewGuid());

    public record ContactAddedEvent(Guid MerchantId,
                                    Guid EstateId,
                                    Guid ContactId,
                                    String ContactName,
                                    String ContactPhoneNumber,
                                    String ContactEmailAddress) : DomainEvent(MerchantId, Guid.NewGuid());

    public record DeviceAddedToMerchantEvent(Guid MerchantId,
                                             Guid EstateId,
                                             Guid DeviceId,
                                             String DeviceIdentifier) : DomainEvent(MerchantId, Guid.NewGuid());

    public record DeviceSwappedForMerchantEvent(Guid MerchantId,
                                                Guid EstateId,
                                                Guid DeviceId,
                                                String OriginalDeviceIdentifier,
                                                String NewDeviceIdentifier) : DomainEvent(MerchantId, Guid.NewGuid());

    public record ManualDepositMadeEvent(Guid MerchantId,
                                         Guid EstateId,
                                         Guid DepositId,
                                         String Reference,
                                         DateTime DepositDateTime,
                                         Decimal Amount) : DomainEvent(MerchantId, Guid.NewGuid());

    public record MerchantCreatedEvent(Guid MerchantId,
                                       Guid EstateId,
                                       String MerchantName,
                                       DateTime DateCreated) : DomainEvent(MerchantId, Guid.NewGuid());
    public record MerchantDepositListCreatedEvent(Guid MerchantId,
                                                  Guid EstateId,
                                                  DateTime DateCreated) : DomainEvent(MerchantId, Guid.NewGuid());

    public record MerchantReferenceAllocatedEvent(Guid MerchantId,
                                                  Guid EstateId,
                                                  String MerchantReference) : DomainEvent(MerchantId, Guid.NewGuid());

    public record OperatorAssignedToMerchantEvent(Guid MerchantId,
                                                  Guid EstateId,
                                                  Guid OperatorId,
                                                  String Name,
                                                  String MerchantNumber,
                                                  String TerminalNumber) : DomainEvent(MerchantId, Guid.NewGuid());

    public record SecurityUserAddedToMerchantEvent(Guid MerchantId,
                                                   Guid EstateId,
                                                   Guid SecurityUserId,
                                                   String EmailAddress) : DomainEvent(MerchantId, Guid.NewGuid());

    public record SettlementScheduleChangedEvent(Guid MerchantId,
                                                 Guid EstateId,
                                                 Int32 SettlementSchedule,
                                                 DateTime NextSettlementDate) : DomainEvent(MerchantId, Guid.NewGuid());

    public record WithdrawalMadeEvent(Guid MerchantId,
                                      Guid EstateId,
                                      Guid WithdrawalId,
                                      DateTime WithdrawalDateTime,
                                      Decimal Amount) : DomainEvent(MerchantId, Guid.NewGuid());

    public record ContractAddedToMerchantEvent(Guid MerchantId,
                                             Guid EstateId,
                                             Guid ContractId) : DomainEvent(MerchantId, Guid.NewGuid());

    public record ContractProductAddedToMerchantEvent(Guid MerchantId, Guid EstateId, Guid ContractId, 
                                                      Guid ContractProductId) : DomainEvent(MerchantId, Guid.NewGuid());
}

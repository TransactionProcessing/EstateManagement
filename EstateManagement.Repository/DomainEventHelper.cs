namespace EstateManagement.Repository;

using System;
using System.Linq;
using System.Reflection;
using Shared.DomainDrivenDesign.EventSourcing;

public static class DomainEventHelper{
    #region Methods

    public static Guid GetContractId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "ContractId");

    public static Guid GetContractProductId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "ProductId");

    public static Guid GetContractProductTransactionFeeId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FeeId");

    public static Guid GetEstateId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "EstateId");
    public static Guid GetOperatorId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "OperatorId");

    public static Guid GetFileId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FileId");

    public static Guid GetFileImportLogId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FileImportLogId");

    public static Guid GetMerchantId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "MerchantId");
    public static Guid GetAddressId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "AddressId");

    public static Guid GetContactId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "ContactId");

    public static T GetProperty<T>(IDomainEvent domainEvent, String propertyName){
        try{
            var f = domainEvent.GetType()
                               .GetProperties()
                               .SingleOrDefault(p => p.Name == propertyName);

            if (f != null){
                return (T)f.GetValue(domainEvent);
            }
        }
        catch{
            // ignored
        }

        return default(T);
    }

    public static T GetPropertyIgnoreCase<T>(IDomainEvent domainEvent, String propertyName){
        try{
            var f = domainEvent.GetType()
                               .GetProperties()
                               .SingleOrDefault(p => String.Compare(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) == 0);

            if (f != null){
                return (T)f.GetValue(domainEvent);
            }
        }
        catch{
            // ignored
        }

        return default(T);
    }

    public static Guid GetSettlementId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "SettlementId");

    public static Guid GetStatementHeaderId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "MerchantStatementId");

    public static Guid GetTransactionId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "TransactionId");

    public static Guid GetVoucherId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "VoucherId");

    public static Boolean HasProperty(IDomainEvent domainEvent,
                                      String propertyName){
        PropertyInfo propertyInfo = domainEvent.GetType()
                                               .GetProperties()
                                               .SingleOrDefault(p => p.Name == propertyName);

        return propertyInfo != null;
    }

    #endregion
}
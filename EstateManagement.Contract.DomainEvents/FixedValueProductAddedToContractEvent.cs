namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;
    
    public record FixedValueProductAddedToContractEvent(Guid ContractId,
                                                        Guid EstateId,
                                                        Guid ProductId,
                                                        String ProductName,
                                                        String DisplayText,
                                                        Decimal Value,
                                                        Int32 ProductType) : DomainEvent(ContractId, Guid.NewGuid());
}
namespace EstateManagement.Contract.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record VariableValueProductAddedToContractEvent(Guid ContractId,
                                                           Guid EstateId,
                                                           Guid ProductId,
                                                           String ProductName,
                                                           String DisplayText,
                                                           Int32 ProductType) : DomainEvent(ContractId, Guid.NewGuid());
}
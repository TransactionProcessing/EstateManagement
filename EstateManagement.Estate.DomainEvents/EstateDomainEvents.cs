namespace EstateManagement.Estate.DomainEvents{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record EstateCreatedEvent(Guid EstateId,
                                     String EstateName) : DomainEvent(EstateId, Guid.NewGuid());

    public record EstateReferenceAllocatedEvent(Guid EstateId, String EstateReference) : DomainEvent(EstateId, Guid.NewGuid());

    public record OperatorAddedToEstateEvent(Guid EstateId,
                                             Guid OperatorId,
                                             String Name,
                                             Boolean RequireCustomMerchantNumber,
                                             Boolean RequireCustomTerminalNumber) : DomainEvent(EstateId, Guid.NewGuid());

    public record SecurityUserAddedToEstateEvent(Guid EstateId,
                                                 Guid SecurityUserId,
                                                 String EmailAddress) : DomainEvent(EstateId, Guid.NewGuid());
}
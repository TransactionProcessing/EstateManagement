namespace EstateManagement.Operator.DomainEvents
{
    using Shared.DomainDrivenDesign.EventSourcing;

    public record OperatorCreatedEvent(Guid OperatorId,
                                       Guid EstateId,
                                       String Name,
                                       Boolean RequireCustomMerchantNumber,
                                       Boolean RequireCustomTerminalNumber) : DomainEvent(OperatorId, Guid.NewGuid());
}

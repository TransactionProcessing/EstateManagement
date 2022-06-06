namespace EstateManagement.Estate.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record EstateReferenceAllocatedEvent : DomainEvent
    {
        public EstateReferenceAllocatedEvent(Guid aggregateId,String estateReference) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = aggregateId;
            this.EstateReference = estateReference;
        }

        #region Properties
        
        public Guid EstateId { get; init; }
        
        public String EstateReference { get; init; }

        #endregion
    }
}
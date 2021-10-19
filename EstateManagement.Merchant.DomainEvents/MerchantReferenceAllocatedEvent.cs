namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record MerchantReferenceAllocatedEvent : DomainEventRecord.DomainEvent
    {
        public MerchantReferenceAllocatedEvent(Guid aggregateId,
                                               Guid estateId, String merchantReference) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.MerchantReference = merchantReference;
        }

        #region Properties

        public Guid EstateId { get; init; }
        public Guid MerchantId { get; init; }
        public String MerchantReference { get; init; }

        #endregion
    }
}
namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;
    
    public record ManualDepositMadeEvent : DomainEvent
    {
        #region Constructors

        public ManualDepositMadeEvent(Guid aggregateId,
                                       Guid estateId,
                                       Guid depositId,
                                       String reference,
                                       DateTime depositDateTime,
                                       Decimal amount) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.DepositId = depositId;
            this.Reference = reference;
            this.DepositDateTime = depositDateTime;
            this.Amount = amount;
        }

        #endregion

        #region Properties

        public Decimal Amount { get; init; }

        public DateTime DepositDateTime { get; init; }

        public Guid DepositId { get; init; }

        public Guid EstateId { get; init; }

        public Guid MerchantId { get; init; }

        public String Reference { get; init; }

        #endregion
    }
}
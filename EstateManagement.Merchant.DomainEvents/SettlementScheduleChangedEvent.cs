namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record SettlementScheduleChangedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="settlementSchedule">The settlement schedule</param>
        /// <param name="nextSettlementDate"></param>
        public SettlementScheduleChangedEvent(Guid aggregateId,
                                              Guid estateId,
                                              Int32 settlementSchedule,
                                              DateTime nextSettlementDate) : base(aggregateId, Guid.NewGuid())
        {
            this.NextSettlementDate = nextSettlementDate;
            this.MerchantId = aggregateId;
            this.EstateId = estateId;
            this.SettlementSchedule = settlementSchedule;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        public DateTime NextSettlementDate { get; init; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; init; }

        
        public Int32 SettlementSchedule { get; init; }

        #endregion
    }
}
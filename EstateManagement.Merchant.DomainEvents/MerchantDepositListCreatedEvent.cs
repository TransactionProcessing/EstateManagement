namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record MerchantDepositListCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="dateCreated">The date created.</param>
        public MerchantDepositListCreatedEvent(Guid aggregateId,
                                               Guid estateId,
                                               DateTime dateCreated) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.DateCreated = dateCreated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        public DateTime DateCreated { get; init; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; init; }

        #endregion
    }
}
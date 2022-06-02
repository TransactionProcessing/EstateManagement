namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    
    public record MerchantCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="dateCreated">The date created.</param>
        public MerchantCreatedEvent(Guid aggregateId,
                                     Guid estateId,
                                     String merchantName,
                                     DateTime dateCreated) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.MerchantId = aggregateId;
            this.MerchantName = merchantName;
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

        /// <summary>
        /// Gets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        public String MerchantName { get; init; }

        #endregion
    }
}
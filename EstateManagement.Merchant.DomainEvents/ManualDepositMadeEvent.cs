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
    [JsonObject]
    public class ManualDepositMadeEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualDepositMadeEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ManualDepositMadeEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualDepositMadeEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="depositId">The deposit identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        private ManualDepositMadeEvent(Guid aggregateId,
                                       Guid eventId,
                                       Guid estateId,
                                       Guid depositId,
                                       String reference,
                                       DateTime depositDateTime,
                                       Decimal amount) : base(aggregateId, eventId)
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

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [JsonProperty]
        public Decimal Amount { get; }

        /// <summary>
        /// Gets the deposit date time.
        /// </summary>
        /// <value>
        /// The deposit date time.
        /// </value>
        [JsonProperty]
        public DateTime DepositDateTime { get; }

        /// <summary>
        /// Gets the deposit identifier.
        /// </summary>
        /// <value>
        /// The deposit identifier.
        /// </value>
        [JsonProperty]
        public Guid DepositId { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        [JsonProperty]
        public Guid MerchantId { get; }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        [JsonProperty]
        public String Reference { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="depositId">The deposit identifier.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static ManualDepositMadeEvent Create(Guid aggregateId,
                                                    Guid estateId,
                                                    Guid depositId,
                                                    String reference,
                                                    DateTime depositDateTime,
                                                    Decimal amount)
        {
            return new ManualDepositMadeEvent(aggregateId, Guid.NewGuid(), estateId, depositId, reference, depositDateTime, amount);
        }

        #endregion
    }
}
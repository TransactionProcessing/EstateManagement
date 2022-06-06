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

    public record OperatorAssignedToMerchantEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorAssignedToMerchantEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        public OperatorAssignedToMerchantEvent(Guid aggregateId,
                                                Guid estateId,
                                                Guid operatorId,
                                                String name,
                                                String merchantNumber,
                                                String terminalNumber) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.MerchantNumber = merchantNumber;
            this.TerminalNumber = terminalNumber;
            this.MerchantId = aggregateId;
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

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; init; }

        /// <summary>
        /// Gets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        public String MerchantNumber { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; init; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; init; }

        /// <summary>
        /// Gets the terminal number.
        /// </summary>
        /// <value>
        /// The terminal number.
        /// </value>
        public String TerminalNumber { get; init; }

        #endregion
    }
}
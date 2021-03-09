namespace EstateManagement.Estate.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    public record OperatorAddedToEstateEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors



        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        public OperatorAddedToEstateEvent(Guid aggregateId,
                                           Guid operatorId,
                                           String name,
                                           Boolean requireCustomMerchantNumber,
                                           Boolean requireCustomTerminalNumber) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = aggregateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.RequireCustomMerchantNumber = requireCustomMerchantNumber;
            this.RequireCustomTerminalNumber = requireCustomTerminalNumber;
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
        /// Gets a value indicating whether [require custom merchant number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom merchant number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomMerchantNumber { get; init; }

        /// <summary>
        /// Gets a value indicating whether [require custom terminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom terminal number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomTerminalNumber { get; init; }

        #endregion

        
    }
}
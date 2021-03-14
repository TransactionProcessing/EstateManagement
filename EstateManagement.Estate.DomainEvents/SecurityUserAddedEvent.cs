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
    public record SecurityUserAddedToEstateEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        public SecurityUserAddedToEstateEvent(Guid aggregateId,
                                       Guid securityUserId,
                                       String emailAddress) : base(aggregateId, Guid.NewGuid())
        {
            this.EstateId = aggregateId;
            this.SecurityUserId = securityUserId;
            this.EmailAddress = emailAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; init; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        /// <summary>
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        public Guid SecurityUserId { get; init; }

        #endregion

        
    }
}
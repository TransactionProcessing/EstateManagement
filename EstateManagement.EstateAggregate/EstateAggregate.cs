namespace EstateManagement.EstateAggregate
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Estate.DomainEvents;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventStore.Aggregate" />
    public class EstateAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public EstateAggregate()
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private EstateAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        public String EstateName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCreated { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static EstateAggregate Create(Guid aggregateId)
        {
            return new EstateAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the specified estate name.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        public void Create(String estateName)
        {
            Guard.ThrowIfNullOrEmpty(estateName, typeof(ArgumentNullException), "Estate name must be provided when registering a new estate");

            this.CheckEstateHasNotAlreadyBeenCreated();

            EstateCreatedEvent estateCreatedEvent = EstateCreatedEvent.Create(this.AggregateId, estateName);

            this.ApplyAndPend(estateCreatedEvent);
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       EstateId = this.AggregateId
                   };
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Checks the estate has not already been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">Estate with name {this.EstateName} has already been created</exception>
        private void CheckEstateHasNotAlreadyBeenCreated()
        {
            if (this.IsCreated)
            {
                throw new InvalidOperationException($"Estate with name {this.EstateName} has already been created");
            }
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(EstateCreatedEvent domainEvent)
        {
            this.EstateName = domainEvent.EstateName;
            this.IsCreated = true;
        }

        #endregion
    }
}
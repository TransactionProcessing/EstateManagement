namespace EstateManagement.OperatorAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Operator.DomainEvents;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;

    public static class OperatorAggregateExtensions
    {
        public static EstateManagement.Models.Operator.Operator GetOperator(this OperatorAggregate aggregate){
            return new EstateManagement.Models.Operator.Operator{
                                                                    OperatorId = aggregate.AggregateId,
                                                                    RequireCustomTerminalNumber = aggregate.RequireCustomTerminalNumber,
                                                                    Name = aggregate.Name,
                                                                    RequireCustomMerchantNumber = aggregate.RequireCustomMerchantNumber
                                                                };
        }

        public static void PlayEvent(this OperatorAggregate aggregate, OperatorCreatedEvent domainEvent)
        {
            aggregate.IsCreated = true;
            aggregate.Name = domainEvent.Name;
            aggregate.RequireCustomMerchantNumber = domainEvent.RequireCustomMerchantNumber;
            aggregate.RequireCustomTerminalNumber = domainEvent.RequireCustomTerminalNumber;
        }

        public static void Create(this OperatorAggregate aggregate,
            Guid estateId,
            String name,
            Boolean requireCustomMerchantNumber,
            Boolean requireCustomTerminalNumber)
        {
            Guard.ThrowIfInvalidGuid(estateId, typeof(ArgumentNullException), "Estate Id must not be an empty Guid");
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), "Operator name must not be null or empty");

            OperatorCreatedEvent operatorCreatedEvent = new(aggregate.AggregateId, estateId, name, requireCustomMerchantNumber, requireCustomTerminalNumber);

            aggregate.ApplyAndAppend(operatorCreatedEvent);
        }
    }

    public record OperatorAggregate : Aggregate
    {
        public Boolean IsCreated { get; internal set; }

        public String Name { get; internal set; }
        public Boolean RequireCustomMerchantNumber { get; internal set; }
        public Boolean RequireCustomTerminalNumber { get; internal set; }

        public static OperatorAggregate Create(Guid aggregateId)
        {
            return new OperatorAggregate(aggregateId);
        }

        #region Constructors

        [ExcludeFromCodeCoverage]
        public OperatorAggregate()
        {
        }

        private OperatorAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        public override void PlayEvent(IDomainEvent domainEvent) => OperatorAggregateExtensions.PlayEvent(this, (dynamic)domainEvent);

        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       EstateId = Guid.NewGuid() // TODO: Populate
                   };
        }
    }
}

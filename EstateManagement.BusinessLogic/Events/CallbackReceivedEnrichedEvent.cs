namespace EstateManagement.BusinessLogic.Events
{
    using System;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    public record CallbackReceivedEnrichedEvent : DomainEventRecord.DomainEvent
    {
        [JsonProperty("typeString")]
        public String TypeString { get; set; }
        [JsonProperty("messageFormat")]
        public Int32 MessageFormat { get; set; }
        [JsonProperty("callbackMessage")]
        public String CallbackMessage { get; set; }
        [JsonProperty("estateid")]
        public Guid EstateId { get; set; }
        [JsonProperty("reference")]
        public String Reference { get; set; }

        public CallbackReceivedEnrichedEvent(Guid aggregateId) : base(aggregateId, Guid.NewGuid())
        {
        }
    }
}

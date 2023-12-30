namespace EstateManagement.MerchantContractAggregate
{
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;

    public static class MerchantContractAggregateExtensions{

    }

    public record MerchantContractAggregate : Aggregate
    {
        public Guid ContractId { get; set; }
        public Guid MerchantId{ get; set; }

        public override void PlayEvent(IDomainEvent domainEvent){
            throw new NotImplementedException();
        }

        protected override Object GetMetadata(){
            throw new NotImplementedException();
        }
    }
}

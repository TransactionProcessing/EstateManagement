namespace EstateManagement.EstateAggregate.Tests
{
    using Shouldly;
    using Xunit;

    public class EstateAggregateTests
    {
        [Fact]
        public void EstateAggregate_CanBeCreated_IsCreated()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
        }
    }
}

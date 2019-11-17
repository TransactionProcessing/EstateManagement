namespace EstateManagement.EstateAggregate.Tests
{
    using System;
    using EstateManagement.Testing;
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

        [Fact]
        public void EstateAggregate_Create_IsCreated()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
            aggregate.EstateName.ShouldBe(TestData.EstateName);
            aggregate.IsCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EstateAggregate_Create_InvalidEstateName_ErrorThrown(String estateName)
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Create(estateName);
                                                });
            
            exception.Message.ShouldContain("Estate name must be provided when registering a new estate");
        }

        [Fact]
        public void EstateAggregate_Create_EstateAlreadyCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.Create(TestData.EstateName);
                                                                                  });

            exception.Message.ShouldContain($"Estate with name {TestData.EstateName} has already been created");
        }
    }
}

namespace EstateManagement.EstateAggregate.Tests
{
    using System;
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
        public void EstateAggregate_Register_IsCreated()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Register(TestData.EstateName);

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
            aggregate.EstateName.ShouldBe(TestData.EstateName);
            aggregate.IsCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EstateAggregate_Register_InvalidEstateName_ErrorThrown(String estateName)
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            ArgumentNullException exception = Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Register(estateName);
                                                });
            
            exception.Message.ShouldContain("Estate name must be provided when registering a new estate");
        }

        [Fact]
        public void EstateAggregate_Register_EstateAlreadyCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Register(TestData.EstateName);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.Register(TestData.EstateName);
                                                                                  });

            exception.Message.ShouldContain($"Estate with name {TestData.EstateName} has already been created");
        }
    }
}

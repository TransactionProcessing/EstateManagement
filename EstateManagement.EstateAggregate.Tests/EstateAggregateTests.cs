namespace EstateManagement.EstateAggregate.Tests
{
    using System;
    using System.Linq;
    using EstateManagement.Testing;
    using Models;
    using Models.Estate;
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
            aggregate.GenerateReference();

            aggregate.AggregateId.ShouldBe(TestData.EstateId);
            aggregate.EstateName.ShouldBe(TestData.EstateName);
            aggregate.IsCreated.ShouldBeTrue();
            aggregate.EstateReference.ShouldBe(TestData.EstateReference);
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
        public void EstateAggregate_Create_EstateAlreadyCreated_NoErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            Should.NotThrow(() =>
                            {
                                aggregate.Create(TestData.EstateName);
                            });
        }

        [Fact]
        public void EstateAggregate_GenerateReference_CalledTwice_NoErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.GenerateReference();

            Should.NotThrow(() =>
                            {
                                aggregate.GenerateReference();
                            });
        }

        [Fact]
        public void EstateAggregate_GetEstate_NoOperators_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.GenerateReference();
            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Reference.ShouldBe(TestData.EstateReference);
            model.Operators.ShouldBeNull();
        }

        [Fact]
        public void EstateAggregate_GetEstate_WithAnOperator_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.GenerateReference();
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Reference.ShouldBe(TestData.EstateReference);
            model.Operators.ShouldHaveSingleItem();
            
            Operator @operator =model.Operators.Single();
            @operator.OperatorId.ShouldBe(TestData.OperatorId);
            @operator.Name.ShouldBe(TestData.OperatorName);
            @operator.RequireCustomMerchantNumber.ShouldBe(TestData.RequireCustomMerchantNumberFalse);
            @operator.RequireCustomTerminalNumber.ShouldBe(TestData.RequireCustomTerminalNumberFalse);
        }

        [Fact]
        public void EstateAggregate_GetEstate_NoSecurityUsers_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.GenerateReference();
            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Reference.ShouldBe(TestData.EstateReference);
            model.Operators.ShouldBeNull();
            model.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void EstateAggregate_GetEstate_WithASecurityUser_EstateIsReturned()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.GenerateReference();
            aggregate.AddSecurityUser(TestData.SecurityUserId,TestData.EstateUserEmailAddress);

            Estate model = aggregate.GetEstate();

            model.EstateId.ShouldBe(TestData.EstateId);
            model.Name.ShouldBe(TestData.EstateName);
            model.Reference.ShouldBe(TestData.EstateReference);
            model.SecurityUsers.ShouldHaveSingleItem();

            SecurityUser securityUser = model.SecurityUsers.Single();
            securityUser.SecurityUserId.ShouldBe(TestData.SecurityUserId);
            securityUser.EmailAddress.ShouldBe(TestData.EstateUserEmailAddress);
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorIsAdded()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);

            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_EstateNotCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain("Estate has not been created");
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorWithIdAlreadyAdded_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain($"Duplicate operator details are not allowed, an operator already exists on this estate with Id [{TestData.OperatorId}]");
        }

        [Fact]
        public void EstateAggregate_AddOperatorToEstate_OperatorWithNameAlreadyAdded_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddOperator(TestData.OperatorId, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                  {
                                                                                      aggregate.AddOperator(TestData.OperatorId2, TestData.OperatorName, TestData.RequireCustomMerchantNumberFalse, TestData.RequireCustomTerminalNumberFalse);
                                                                                  });

            exception.Message.ShouldContain($"Duplicate operator details are not allowed, an operator already exists on this estate with Name [{TestData.OperatorName}]");
        }

        [Fact]
        public void EstateAggregate_AddSecurityUserToEstate_SecurityUserIsAdded()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);
            aggregate.Create(TestData.EstateName);
            aggregate.AddSecurityUser(TestData.SecurityUserId, TestData.EstateUserEmailAddress);
        }

        [Fact]
        public void EstateAggregate_AddSecurityUserToEstate_EstateNotCreated_ErrorThrown()
        {
            EstateAggregate aggregate = EstateAggregate.Create(TestData.EstateId);

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
                                                                                          {
                                                                                              aggregate.AddSecurityUser(TestData.SecurityUserId, TestData.EstateUserEmailAddress);
                                                                                          });

            exception.Message.ShouldContain("Estate has not been created");
        }
    }
}

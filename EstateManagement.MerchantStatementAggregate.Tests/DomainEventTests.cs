using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.MerchantStatementAggregate.Tests
{
    using MerchantStatement.DomainEvents;
    using Shouldly;
    using Testing;
    using Xunit;

    public class DomainEventTests
    {
        [Fact]
        public void StatementCreatedEvent_CanBeCreated_IsCreated()
        {
            StatementCreatedEvent statementCreatedEvent = new StatementCreatedEvent(TestData.MerchantStatementId,
                                                                                    TestData.EstateId,
                                                                                    TestData.MerchantId,
                                                                                    TestData.StatementCreateDate);

            statementCreatedEvent.ShouldNotBeNull();
            statementCreatedEvent.AggregateId.ShouldBe(TestData.MerchantStatementId);
            statementCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            statementCreatedEvent.EstateId.ShouldBe(TestData.EstateId);
            statementCreatedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            statementCreatedEvent.DateCreated.ShouldBe(TestData.StatementCreateDate);
        }

        [Fact]
        public void TransactionAddedToStatementEvent_CanBeCreated_IsCreated()
        {
            TransactionAddedToStatementEvent transactionAddedToStatementEvent = new TransactionAddedToStatementEvent(TestData.MerchantStatementId,
                 TestData.EstateId,
                 TestData.MerchantId,
                 TestData.Transaction1.TransactionId,
                 TestData.Transaction1.DateTime,
                 TestData.Transaction1.Amount,
                 TestData.Transaction1.OperatorId);

            transactionAddedToStatementEvent.ShouldNotBeNull();
            transactionAddedToStatementEvent.AggregateId.ShouldBe(TestData.MerchantStatementId);
            transactionAddedToStatementEvent.EventId.ShouldNotBe(Guid.Empty);
            transactionAddedToStatementEvent.EstateId.ShouldBe(TestData.EstateId);
            transactionAddedToStatementEvent.MerchantId.ShouldBe(TestData.MerchantId);
            transactionAddedToStatementEvent.TransactionId.ShouldBe(TestData.Transaction1.TransactionId);
            transactionAddedToStatementEvent.TransactionDateTime.ShouldBe(TestData.Transaction1.DateTime);
            transactionAddedToStatementEvent.TransactionValue.ShouldBe(TestData.Transaction1.Amount);
            transactionAddedToStatementEvent.OperatorId.ShouldBe(TestData.Transaction1.OperatorId);
        }

        [Fact]
        public void SettledFeeAddedToStatementEvent_CanBeCreated_IsCreated()
        {
            SettledFeeAddedToStatementEvent settledFeeAddedToStatementEvent = new SettledFeeAddedToStatementEvent(TestData.MerchantStatementId,
                TestData.EstateId,
                TestData.MerchantId,
                TestData.SettledFee1.SettledFeeId,
                TestData.SettledFee1.TransactionId,
                TestData.SettledFee1.DateTime,
                TestData.SettledFee1.Amount);

            settledFeeAddedToStatementEvent.ShouldNotBeNull();
            settledFeeAddedToStatementEvent.AggregateId.ShouldBe(TestData.MerchantStatementId);
            settledFeeAddedToStatementEvent.EventId.ShouldNotBe(Guid.Empty);
            settledFeeAddedToStatementEvent.EstateId.ShouldBe(TestData.EstateId);
            settledFeeAddedToStatementEvent.MerchantId.ShouldBe(TestData.MerchantId);
            settledFeeAddedToStatementEvent.TransactionId.ShouldBe(TestData.SettledFee1.TransactionId);
            settledFeeAddedToStatementEvent.SettledDateTime.ShouldBe(TestData.SettledFee1.DateTime);
            settledFeeAddedToStatementEvent.SettledValue.ShouldBe(TestData.SettledFee1.Amount);
            settledFeeAddedToStatementEvent.SettledFeeId.ShouldBe(TestData.SettledFee1.SettledFeeId);
        }

        [Fact]
        public void StatementGeneratedEvent_CanBeCreated_IsCreated()
        {
            StatementGeneratedEvent statementGeneratedEvent = new StatementGeneratedEvent(TestData.MerchantStatementId,
                                                                                          TestData.EstateId,
                                                                                          TestData.MerchantId,
                                                                                          TestData.StatementGeneratedDate);

            statementGeneratedEvent.ShouldNotBeNull();
            statementGeneratedEvent.AggregateId.ShouldBe(TestData.MerchantStatementId);
            statementGeneratedEvent.EventId.ShouldNotBe(Guid.Empty);
            statementGeneratedEvent.EstateId.ShouldBe(TestData.EstateId);
            statementGeneratedEvent.MerchantId.ShouldBe(TestData.MerchantId);
            statementGeneratedEvent.DateGenerated.ShouldBe(TestData.StatementGeneratedDate);
        }
    }
}

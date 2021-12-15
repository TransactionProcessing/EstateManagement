﻿namespace EstateManagement.MerchantStatementAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MerchantStatement.DomainEvents;
    using Models.MerchantStatement;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;

    public class MerchantStatementAggregate : Aggregate
    {
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return null;
        }

        public override void PlayEvent(IDomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        public static MerchantStatementAggregate Create(Guid aggregateId)
        {
            return new MerchantStatementAggregate(aggregateId);
        }

        public void CreateStatement(Guid estateId, Guid merchantId, DateTime createdDateTime)
        {
            // Just return if already created
            if (this.IsCreated)
                return;

            Guard.ThrowIfInvalidGuid(estateId, nameof(estateId));
            Guard.ThrowIfInvalidGuid(merchantId, nameof(merchantId));

            StatementCreatedEvent statementCreatedEvent = new StatementCreatedEvent(this.AggregateId, estateId, merchantId, createdDateTime);

            this.ApplyAndAppend(statementCreatedEvent);
        }

        public void AddTransactionToStatement(Transaction transaction)
        {
            this.EnsureStatementHasBeenCreated();
            this.EnsureTransactionHasNotAlreadyBeenAddedToStatement(transaction);
            this.EnsureStatementHasNotAlreadyBeenGenerated();

            TransactionAddedToStatementEvent transactionAddedToStatementEvent = new TransactionAddedToStatementEvent(this.AggregateId,
                this.EstateId,
                this.MerchantId,
                transaction.TransactionId,
                transaction.DateTime,
                transaction.Amount);

            this.ApplyAndAppend(transactionAddedToStatementEvent);
        }

        private void EnsureStatementHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException("Statement has not been created");
            }
        }

        private void EnsureStatementHasNotAlreadyBeenGenerated()
        {
            if (this.IsGenerated == true)
            {
                throw new InvalidOperationException("Statement has already been generated");
            }
        }

        private void EnsureTransactionHasNotAlreadyBeenAddedToStatement(Transaction transaction)
        {
            if (this.Transactions.Any(t => t.TransactionId == transaction.TransactionId))
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} already added to statement {this.AggregateId}");
            }
        }

        private void EnsureTransactionHasBeenAddedToStatement(Guid transactionId)
        {
            if (this.Transactions.Any(t => t.TransactionId == transactionId) == false)
            {
                throw new InvalidOperationException($"Transaction {transactionId} has not been added to statement {this.AggregateId}");
            }
        }

        private void EnsureSettledFeeHasNotAlreadyBeenAddedToStatement(SettledFee settledFee)
        {
            if (this.SettledFees.Any(t => t.SettledFeeId == settledFee.SettledFeeId))
            {
                throw new InvalidOperationException($"Settled Fee {settledFee.SettledFeeId} already added to statement {this.AggregateId}");
            }
        }

        public void AddSettledFeeToStatement(SettledFee settledFee)
        {
            this.EnsureStatementHasBeenCreated();
            this.EnsureTransactionHasBeenAddedToStatement(settledFee.TransactionId);
            this.EnsureSettledFeeHasNotAlreadyBeenAddedToStatement(settledFee);
            this.EnsureStatementHasNotAlreadyBeenGenerated();

            SettledFeeAddedToStatementEvent settledFeeAddedToStatementEvent =
                new SettledFeeAddedToStatementEvent(this.AggregateId,
                                                    this.EstateId,
                                                    this.MerchantId,
                                                    settledFee.SettledFeeId,
                                                    settledFee.TransactionId,
                                                    settledFee.DateTime,
                                                    settledFee.Amount);

            this.ApplyAndAppend(settledFeeAddedToStatementEvent);
        }

        public void GenerateStatement(DateTime generatedDateTime)
        {
            this.EnsureStatementHasBeenCreated();
            this.EnsureStatementHasNotAlreadyBeenGenerated();

            if (this.Transactions.Any() == false)
            {
                throw new InvalidOperationException("Statement has no transactions added");
            }

            if (this.Transactions.Any() == true && this.SettledFees.Any() == false)
            {
                throw new InvalidOperationException("Statement has transactions added with no matching settled fee");
            }

            StatementGeneratedEvent statementGeneratedEvent = new StatementGeneratedEvent(this.AggregateId, this.EstateId, this.MerchantId, generatedDateTime);

            this.ApplyAndAppend(statementGeneratedEvent);
        }

        public MerchantStatement GetStatement(Boolean includeStatementLines = false)
        {
            MerchantStatement merchantStatement = new MerchantStatement
                                                  { 
                                                      EstateId = this.EstateId,
                                                      MerchantId = this.MerchantId,
                                                      MerchantStatementId = this.AggregateId,
                                                      IsCreated = this.IsCreated,
                                                      IsGenerated = this.IsGenerated,
                                                      StatementCreatedDateTime = this.CreatedDateTime,
                                                      StatementGeneratedDateTime = this.GeneratedDateTime
                                                  };

            if (includeStatementLines)
            {
                foreach (Transaction transaction in this.Transactions)
                {
                    merchantStatement.AddStatementLine(new StatementLine
                                                       {
                                                           Amount = transaction.Amount,
                                                           DateTime = transaction.DateTime,
                                                           Description = String.Empty,
                                                           LineType = 1 // Transaction
                                                       });
                }

                foreach (SettledFee settledFee in this.SettledFees)
                {
                    merchantStatement.AddStatementLine(new StatementLine
                                                       {
                                                           Amount = settledFee.Amount,
                                                           DateTime = settledFee.DateTime,
                                                           Description = String.Empty,
                                                           LineType = 2 // Settled Fee
                                                       });
                }
            }

            return merchantStatement;
        }

        [ExcludeFromCodeCoverage]
        public MerchantStatementAggregate()
        {
            // Nothing here
            this.Transactions = new List<Transaction>();
            this.SettledFees = new List<SettledFee>();
        }

        private MerchantStatementAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Transactions = new List<Transaction>();
            this.SettledFees = new List<SettledFee>();
        }

        private Boolean IsCreated;

        private Boolean IsGenerated;

        private Guid EstateId;

        private Guid MerchantId;

        private DateTime CreatedDateTime;

        private DateTime GeneratedDateTime;

        private List<Transaction> Transactions;

        private List<SettledFee> SettledFees;

        private void PlayEvent(StatementCreatedEvent domainEvent)
        {
            this.IsCreated = true;
            this.EstateId = domainEvent.EstateId;
            this.MerchantId = domainEvent.MerchantId;
            this.CreatedDateTime = domainEvent.DateCreated;
        }

        private void PlayEvent(TransactionAddedToStatementEvent domainEvent)
        {
            this.Transactions.Add(new Transaction
                                  {
                                      Amount = domainEvent.TransactionValue,
                                      DateTime = domainEvent.TransactionDateTime,
                                      TransactionId = domainEvent.TransactionId
                                  });
        }

        private void PlayEvent(SettledFeeAddedToStatementEvent domainEvent)
        {
            this.SettledFees.Add(new SettledFee
                                 {
                                     Amount = domainEvent.SettledValue,
                                     DateTime = domainEvent.SettledDateTime,
                                     SettledFeeId = domainEvent.SettledFeeId,
                                     TransactionId = domainEvent.TransactionId
                                 });
        }

        private void PlayEvent(StatementGeneratedEvent domainEvent)
        {
            this.IsGenerated = true;
            this.GeneratedDateTime = domainEvent.DateGenerated;
        }
    }
}
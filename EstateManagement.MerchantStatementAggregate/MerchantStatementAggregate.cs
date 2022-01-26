namespace EstateManagement.MerchantStatementAggregate
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.Aggregate.Aggregate" />
    public class MerchantStatementAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The created date time
        /// </summary>
        private DateTime CreatedDateTime;

        /// <summary>
        /// The emailed date time
        /// </summary>
        private DateTime EmailedDateTime;

        /// <summary>
        /// The email message identifier
        /// </summary>
        private Guid EmailMessageId;

        /// <summary>
        /// The estate identifier
        /// </summary>
        private Guid EstateId;

        /// <summary>
        /// The generated date time
        /// </summary>
        private DateTime GeneratedDateTime;

        /// <summary>
        /// The has been emailed
        /// </summary>
        private Boolean HasBeenEmailed;

        /// <summary>
        /// The is created
        /// </summary>
        private Boolean IsCreated;

        /// <summary>
        /// The is generated
        /// </summary>
        private Boolean IsGenerated;

        /// <summary>
        /// The merchant identifier
        /// </summary>
        private Guid MerchantId;

        /// <summary>
        /// The settled fees
        /// </summary>
        private readonly List<SettledFee> SettledFees;

        /// <summary>
        /// The transactions
        /// </summary>
        private readonly List<Transaction> Transactions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MerchantStatementAggregate()
        {
            // Nothing here
            this.Transactions = new List<Transaction>();
            this.SettledFees = new List<SettledFee>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantStatementAggregate"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private MerchantStatementAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Transactions = new List<Transaction>();
            this.SettledFees = new List<SettledFee>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the settled fee to statement.
        /// </summary>
        /// <param name="statementId">The statement identifier.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settledFee">The settled fee.</param>
        public void AddSettledFeeToStatement(Guid statementId,
                                             DateTime createdDate,
                                             Guid estateId,
                                             Guid merchantId,
                                             SettledFee settledFee)
        {
            // Create statement id required
            this.CreateStatement(statementId, createdDate, estateId, merchantId);

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

        /// <summary>
        /// Creates the statement.
        /// </summary>
        /// <param name="statementId">The statement identifier.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        private void CreateStatement(Guid statementId,
                                     DateTime createdDate,
                                     Guid estateId,
                                     Guid merchantId)
        {
            if (this.NumberOfHistoricalEvents == 0)
            {
                Guard.ThrowIfInvalidGuid(statementId, nameof(statementId));
                Guard.ThrowIfInvalidGuid(estateId, nameof(estateId));
                Guard.ThrowIfInvalidGuid(merchantId, nameof(merchantId));

                StatementCreatedEvent statementCreatedEvent = new StatementCreatedEvent(statementId, estateId, merchantId, createdDate);

                this.ApplyAndAppend(statementCreatedEvent);
            }
        }

        /// <summary>
        /// Adds the transaction to statement.
        /// </summary>
        /// <param name="statementId">The statement identifier.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transaction">The transaction.</param>
        public void AddTransactionToStatement(Guid statementId,
                                              DateTime createdDate,
                                              Guid estateId,
                                              Guid merchantId,
                                              Transaction transaction)
        {
            // Create statement id required
            this.CreateStatement(statementId, createdDate, estateId, merchantId);

            TransactionAddedToStatementEvent transactionAddedToStatementEvent = new TransactionAddedToStatementEvent(this.AggregateId,
                this.EstateId,
                this.MerchantId,
                transaction.TransactionId,
                transaction.DateTime,
                transaction.Amount);

            this.ApplyAndAppend(transactionAddedToStatementEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static MerchantStatementAggregate Create(Guid aggregateId)
        {
            return new MerchantStatementAggregate(aggregateId);
        }

        /// <summary>
        /// Emails the statement.
        /// </summary>
        /// <param name="emailedDateTime">The emailed date time.</param>
        /// <param name="messageId">The message identifier.</param>
        public void EmailStatement(DateTime emailedDateTime,
                                   Guid messageId)
        {
            this.EnsureStatementHasBeenCreated();
            this.EnsureStatementHasBeenGenerated();

            StatementEmailedEvent statementEmailedEvent = new StatementEmailedEvent(this.AggregateId, this.EstateId, this.MerchantId, emailedDateTime, messageId);

            this.ApplyAndAppend(statementEmailedEvent);
        }

        /// <summary>
        /// Generates the statement.
        /// </summary>
        /// <param name="generatedDateTime">The generated date time.</param>
        /// <exception cref="System.InvalidOperationException">Statement has no transactions or settled fees</exception>
        public void GenerateStatement(DateTime generatedDateTime)
        {
            this.EnsureStatementHasNotAlreadyBeenGenerated();

            if (this.Transactions.Any() == false && this.SettledFees.Any() == false)
            {
                throw new InvalidOperationException("Statement has no transactions or settled fees");
            }

            StatementGeneratedEvent statementGeneratedEvent = new StatementGeneratedEvent(this.AggregateId, this.EstateId, this.MerchantId, generatedDateTime);

            this.ApplyAndAppend(statementGeneratedEvent);
        }

        /// <summary>
        /// Gets the statement.
        /// </summary>
        /// <param name="includeStatementLines">if set to <c>true</c> [include statement lines].</param>
        /// <returns></returns>
        public MerchantStatement GetStatement(Boolean includeStatementLines = false)
        {
            MerchantStatement merchantStatement = new MerchantStatement
                                                  {
                                                      EstateId = this.EstateId,
                                                      MerchantId = this.MerchantId,
                                                      MerchantStatementId = this.AggregateId,
                                                      IsCreated = this.IsCreated,
                                                      IsGenerated = this.IsGenerated,
                                                      HasBeenEmailed = this.HasBeenEmailed,
                                                      StatementCreatedDateTime = this.CreatedDateTime,
                                                      StatementGeneratedDateTime = this.GeneratedDateTime
                                                  };

            if (includeStatementLines)
            {
                foreach (Transaction transaction in this.Transactions)
                {
                    merchantStatement.AddStatementLine(new MerchantStatementLine
                                                       {
                                                           Amount = transaction.Amount,
                                                           DateTime = transaction.DateTime,
                                                           Description = string.Empty,
                                                           LineType = 1 // Transaction
                                                       });
                }

                foreach (SettledFee settledFee in this.SettledFees)
                {
                    merchantStatement.AddStatementLine(new MerchantStatementLine
                                                       {
                                                           Amount = settledFee.Amount,
                                                           DateTime = settledFee.DateTime,
                                                           Description = string.Empty,
                                                           LineType = 2 // Settled Fee
                                                       });
                }
            }

            return merchantStatement;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        public override void PlayEvent(IDomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return null;
        }

        /// <summary>
        /// Ensures the statement has been created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Statement has not been created</exception>
        private void EnsureStatementHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException("Statement has not been created");
            }
        }

        /// <summary>
        /// Ensures the statement has been generated.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Statement has not been generated</exception>
        private void EnsureStatementHasBeenGenerated()
        {
            if (this.IsGenerated == false)
            {
                throw new InvalidOperationException("Statement has not been generated");
            }
        }

        /// <summary>
        /// Ensures the statement has not already been generated.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Statement has already been generated</exception>
        private void EnsureStatementHasNotAlreadyBeenGenerated()
        {
            if (this.IsGenerated)
            {
                throw new InvalidOperationException("Statement has already been generated");
            }
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(StatementCreatedEvent domainEvent)
        {
            this.IsCreated = true;
            this.EstateId = domainEvent.EstateId;
            this.MerchantId = domainEvent.MerchantId;
            this.CreatedDateTime = domainEvent.DateCreated;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TransactionAddedToStatementEvent domainEvent)
        {
            this.EstateId = domainEvent.EstateId;
            this.MerchantId = domainEvent.MerchantId;

            this.Transactions.Add(new Transaction
                                  {
                                      Amount = domainEvent.TransactionValue,
                                      DateTime = domainEvent.TransactionDateTime,
                                      TransactionId = domainEvent.TransactionId
                                  });
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(SettledFeeAddedToStatementEvent domainEvent)
        {
            this.EstateId = domainEvent.EstateId;
            this.MerchantId = domainEvent.MerchantId;

            this.SettledFees.Add(new SettledFee
                                 {
                                     Amount = domainEvent.SettledValue,
                                     DateTime = domainEvent.SettledDateTime,
                                     SettledFeeId = domainEvent.SettledFeeId,
                                     TransactionId = domainEvent.TransactionId
                                 });
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(StatementGeneratedEvent domainEvent)
        {
            this.IsGenerated = true;
            this.GeneratedDateTime = domainEvent.DateGenerated;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(StatementEmailedEvent domainEvent)
        {
            this.HasBeenEmailed = true;
            this.EmailedDateTime = domainEvent.DateEmailed;
            this.EmailMessageId = domainEvent.MessageId;
        }

        #endregion
    }
}
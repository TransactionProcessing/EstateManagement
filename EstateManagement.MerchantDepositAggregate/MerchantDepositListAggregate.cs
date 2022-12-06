namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using EstateManagement.MerchantDepositListAggregate;
    using Merchant.DomainEvents;
    using Models;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Aggregate" />
    public class MerchantDepositListAggregate : Aggregate
    {
        #region Fields

        private readonly List<Deposit> Deposits;

        private readonly List<Withdrawal> Withdrawals;

        #endregion

        #region Constructors

        [ExcludeFromCodeCoverage]
        public MerchantDepositListAggregate() {
            // Nothing here
            this.Deposits = new List<Deposit>();
            this.Withdrawals = new List<Withdrawal>();
        }

        private MerchantDepositListAggregate(Guid aggregateId) {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Deposits = new List<Deposit>();
            this.Withdrawals = new List<Withdrawal>();
        }

        #endregion

        #region Properties

        public DateTime DateCreated { get; private set; }

        public Guid EstateId { get; private set; }

        public Boolean IsCreated { get; private set; }

        #endregion

        #region Methods

        public static MerchantDepositListAggregate Create(Guid aggregateId) {
            return new MerchantDepositListAggregate(aggregateId);
        }

        public void Create(MerchantAggregate merchantAggregate,
                           DateTime dateCreated) {
            this.EnsureMerchantHasBeenCreated(merchantAggregate);
            // Ensure this aggregate has not already been created
            if (this.IsCreated)
                return;

            MerchantDepositListCreatedEvent merchantDepositListCreatedEvent =
                new MerchantDepositListCreatedEvent(this.AggregateId, merchantAggregate.EstateId, dateCreated);

            this.ApplyAndAppend(merchantDepositListCreatedEvent);
        }

        public List<Models.Merchant.Deposit> GetDeposits() {
            List<Models.Merchant.Deposit> deposits = new List<Models.Merchant.Deposit>();
            if (this.Deposits.Any()) {
                this.Deposits.ForEach(d => deposits.Add(new Models.Merchant.Deposit {
                                                                                        Source = d.Source,
                                                                                        DepositDateTime = d.DepositDateTime,
                                                                                        DepositId = d.DepositId,
                                                                                        Amount = d.Amount,
                                                                                        Reference = d.Reference
                                                                                    }));
            }

            return deposits;
        }

        public List<Models.Merchant.Withdrawal> GetWithdrawals()
        {
            List<Models.Merchant.Withdrawal> withdrawals = new List<Models.Merchant.Withdrawal>();
            if (this.Withdrawals.Any())
            {
                this.Withdrawals.ForEach(d => withdrawals.Add(new Models.Merchant.Withdrawal()
                                                              {
                                                                  WithdrawalDateTime = d.WithdrawalDateTime,
                                                                  WithdrawalId= d.WithdrawalId,
                                                                  Amount = d.Amount
                                                              }));
            }

            return withdrawals;
        }

        public void MakeDeposit(MerchantDepositSource source,
                                String reference,
                                DateTime depositDateTime,
                                PositiveMoney amount) {
            String depositData = $"{depositDateTime.ToString("yyyyMMdd hh:mm:ss.fff")}-{reference}-{amount:N2}-{source}";
            Guid depositId = this.GenerateGuidFromString(depositData);

            this.EnsureMerchantDepositListHasBeenCreated();
            this.EnsureNotDuplicateDeposit(depositId);
            // TODO: Change amount to a value object (PositiveAmount VO)
            this.EnsureDepositSourceHasBeenSet(source);

            if (source == MerchantDepositSource.Manual) {
                ManualDepositMadeEvent manualDepositMadeEvent =
                    new ManualDepositMadeEvent(this.AggregateId, this.EstateId, depositId, reference, depositDateTime, amount.Value);
                this.ApplyAndAppend(manualDepositMadeEvent);
            }
            else if (source == MerchantDepositSource.Automatic) {
                AutomaticDepositMadeEvent automaticDepositMadeEvent =
                    new AutomaticDepositMadeEvent(this.AggregateId, this.EstateId, depositId, reference, depositDateTime, amount.Value);
                this.ApplyAndAppend(automaticDepositMadeEvent);
            }
        }

        public void MakeWithdrawal(DateTime withdrawalDateTime,
                                   PositiveMoney amount) {
            String depositData = $"{withdrawalDateTime.ToString("yyyyMMdd hh:mm:ss.fff")}-{amount:N2}";
            Guid withdrawalId = this.GenerateGuidFromString(depositData);

            this.EnsureMerchantDepositListHasBeenCreated();
            this.EnsureNotDuplicateWithdrawal(withdrawalId);

            WithdrawalMadeEvent withdrawalMadeEvent = new(this.AggregateId, this.EstateId, withdrawalId, withdrawalDateTime, amount.Value);
            this.ApplyAndAppend(withdrawalMadeEvent);
        }

        public override void PlayEvent(IDomainEvent domainEvent) {
            this.PlayEvent((dynamic)domainEvent);
        }

        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata() {
            return new {
                           this.EstateId,
                           MerchantId = this.AggregateId
                       };
        }

        private void EnsureDepositSourceHasBeenSet(MerchantDepositSource merchantDepositSource) {
            if (merchantDepositSource == MerchantDepositSource.NotSet) {
                throw new InvalidOperationException("Merchant deposit source must be set");
            }
        }

        private void EnsureMerchantDepositListHasBeenCreated() {
            if (this.IsCreated == false) {
                throw new InvalidOperationException("Merchant Deposit List has not been created");
            }
        }

        private void EnsureMerchantHasBeenCreated(MerchantAggregate merchantAggregate) {
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException("Merchant has not been created");
            }
        }

        private void EnsureNotDuplicateDeposit(Guid depositId) {
            if (this.Deposits.Any(d => d.DepositId == depositId)) {
                throw new InvalidOperationException($"Deposit Id [{depositId}] already made for merchant [{this.AggregateId}]");
            }
        }

        private void EnsureNotDuplicateWithdrawal(Guid withdrawalId) {
            if (this.Withdrawals.Any(d => d.WithdrawalId == withdrawalId)) {
                throw new InvalidOperationException($"Withdrawal Id [{withdrawalId}] already made for merchant [{this.AggregateId}]");
            }
        }

        private Guid GenerateGuidFromString(String input) {
            using(SHA256 sha256Hash = SHA256.Create()) {
                //Generate hash from the key
                Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                Byte[] j = bytes.Skip(Math.Max(0, bytes.Count() - 16)).ToArray(); //Take last 16

                //Create our Guid.
                return new Guid(j);
            }
        }

        private void PlayEvent(MerchantDepositListCreatedEvent merchantDepositListCreatedEvent) {
            this.IsCreated = true;
            this.EstateId = merchantDepositListCreatedEvent.EstateId;
            this.DateCreated = merchantDepositListCreatedEvent.DateCreated;
            this.AggregateId = merchantDepositListCreatedEvent.AggregateId;
        }

        private void PlayEvent(ManualDepositMadeEvent manualDepositMadeEvent) {
            Deposit deposit = Deposit.Create(manualDepositMadeEvent.DepositId,
                                             MerchantDepositSource.Manual,
                                             manualDepositMadeEvent.Reference,
                                             manualDepositMadeEvent.DepositDateTime,
                                             manualDepositMadeEvent.Amount);
            this.Deposits.Add(deposit);
        }

        private void PlayEvent(AutomaticDepositMadeEvent automaticDepositMadeEvent) {
            Deposit deposit = Deposit.Create(automaticDepositMadeEvent.DepositId,
                                             MerchantDepositSource.Automatic,
                                             automaticDepositMadeEvent.Reference,
                                             automaticDepositMadeEvent.DepositDateTime,
                                             automaticDepositMadeEvent.Amount);
            this.Deposits.Add(deposit);
        }

        private void PlayEvent(WithdrawalMadeEvent withdrawalMadeEvent) {
            Withdrawal withdrawal = Withdrawal.Create(withdrawalMadeEvent.WithdrawalId, withdrawalMadeEvent.WithdrawalDateTime, withdrawalMadeEvent.Amount);
            this.Withdrawals.Add(withdrawal);
        }

        #endregion
    }
}
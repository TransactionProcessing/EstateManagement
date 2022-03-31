namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Merchant.DomainEvents;
    using Models;
    using Models.Merchant;
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

        /// <summary>
        /// The addresses
        /// </summary>
        private readonly List<Address> Addresses;

        /// <summary>
        /// The contacts
        /// </summary>
        private readonly List<Contact> Contacts;

        /// <summary>
        /// The deposits
        /// </summary>
        private readonly List<Deposit> Deposits;

        /// <summary>
        /// The devices
        /// </summary>
        private readonly Dictionary<Guid, String> Devices;

        /// <summary>
        /// The operators
        /// </summary>
        private readonly List<Operator> Operators;

        /// <summary>
        /// The security users
        /// </summary>
        private readonly List<SecurityUser> SecurityUsers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDepositListAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MerchantDepositListAggregate()
        {
            // Nothing here
            this.Deposits = new List<Deposit>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDepositListAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private MerchantDepositListAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Deposits = new List<Deposit>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCreated { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static MerchantDepositListAggregate Create(Guid aggregateId)
        {
            return new MerchantDepositListAggregate(aggregateId);
        }

        public void Create(MerchantAggregate merchantAggregate,
                           DateTime dateCreated)
        {
            this.EnsureMerchantHasBeenCreated(merchantAggregate);
            // Ensure this aggregate has not already been created
            if (this.IsCreated)
                return;

            MerchantDepositListCreatedEvent merchantDepositListCreatedEvent =
                new MerchantDepositListCreatedEvent(this.AggregateId, merchantAggregate.EstateId, dateCreated);

            this.ApplyAndAppend(merchantDepositListCreatedEvent);
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <returns></returns>
        public List<Models.Merchant.Deposit> GetDeposits()
        {
            List<Models.Merchant.Deposit> deposits = new List<Models.Merchant.Deposit>();
            if (this.Deposits.Any())
            {
                this.Deposits.ForEach(d => deposits.Add(new Models.Merchant.Deposit
                                                        {
                                                            Source = d.Source,
                                                            DepositDateTime = d.DepositDateTime,
                                                            DepositId = d.DepositId,
                                                            Amount = d.Amount,
                                                            Reference = d.Reference
                                                        }));
            }

            return deposits;
        }

        /// <summary>
        /// Makes the deposit.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        public void MakeDeposit(MerchantDepositSource source,
                                String reference,
                                DateTime depositDateTime,
                                PositiveMoney amount)
        {
            String depositData = $"{depositDateTime.ToString("yyyyMMdd hh:mm:ss.fff")}-{reference}-{amount:N2}-{source}";
            Guid depositId = this.GenerateGuidFromString(depositData);

            this.EnsureMerchantDepositListHasBeenCreated();
            this.EnsureNotDuplicateDeposit(depositId);
            // TODO: Change amount to a value object (PositiveAmount VO)
            this.EnsureDepositSourceHasBeenSet(source);

            if (source == MerchantDepositSource.Manual)
            {
                ManualDepositMadeEvent manualDepositMadeEvent =
                    new ManualDepositMadeEvent(this.AggregateId, this.EstateId, depositId, reference, depositDateTime, amount.Value);
                this.ApplyAndAppend(manualDepositMadeEvent);
            }
            else if (source == MerchantDepositSource.Automatic)
            {
                AutomaticDepositMadeEvent automaticDepositMadeEvent =
                    new AutomaticDepositMadeEvent(this.AggregateId, this.EstateId, depositId, reference, depositDateTime, amount.Value);
                this.ApplyAndAppend(automaticDepositMadeEvent);
            }
        }

        private void EnsureMerchantDepositListHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException("Merchant Deposit List has not been created");
            }
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
            return new
                   {
                       this.EstateId,
                       MerchantId = this.AggregateId
                   };
        }

        /// <summary>
        /// Ensures the deposit source has been set.
        /// </summary>
        /// <param name="merchantDepositSource">The merchant deposit source.</param>
        /// <exception cref="InvalidOperationException">Merchant deposit source must be set</exception>
        private void EnsureDepositSourceHasBeenSet(MerchantDepositSource merchantDepositSource)
        {
            if (merchantDepositSource == MerchantDepositSource.NotSet)
            {
                throw new InvalidOperationException("Merchant deposit source must be set");
            }
        }

        /// <summary>
        /// Ensures the merchant has been created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Merchant {this.Name} has not been created</exception>
        private void EnsureMerchantHasBeenCreated(MerchantAggregate merchantAggregate)
        {
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException("Merchant has not been created");
            }
        }

        /// <summary>
        /// Ensures the not duplicate deposit.
        /// </summary>
        /// <param name="depositId">The deposit identifier.</param>
        /// <exception cref="InvalidOperationException">Deposit Id [{depositId}] already made for merchant [{this.Name}]</exception>
        private void EnsureNotDuplicateDeposit(Guid depositId)
        {
            if (this.Deposits.Any(d => d.DepositId == depositId))
            {
                throw new InvalidOperationException($"Deposit Id [{depositId}] already made for merchant [{this.AggregateId}]");
            }
        }

        /// <summary>
        /// Generates the unique identifier from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private Guid GenerateGuidFromString(String input)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                //Generate hash from the key
                Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                Byte[] j = bytes.Skip(Math.Max(0, bytes.Count() - 16)).ToArray(); //Take last 16

                //Create our Guid.
                return new Guid(j);
            }
        }

        private void PlayEvent(MerchantDepositListCreatedEvent merchantDepositListCreatedEvent)
        {
            this.IsCreated = true;
            this.EstateId = merchantDepositListCreatedEvent.EstateId;
            this.DateCreated = merchantDepositListCreatedEvent.DateCreated;
            this.AggregateId = merchantDepositListCreatedEvent.AggregateId;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="manualDepositMadeEvent">The manual deposit made event.</param>
        private void PlayEvent(ManualDepositMadeEvent manualDepositMadeEvent)
        {
            Deposit deposit = Deposit.Create(manualDepositMadeEvent.DepositId,
                                             MerchantDepositSource.Manual,
                                             manualDepositMadeEvent.Reference,
                                             manualDepositMadeEvent.DepositDateTime,
                                             manualDepositMadeEvent.Amount);
            this.Deposits.Add(deposit);
        }

        private void PlayEvent(AutomaticDepositMadeEvent automaticDepositMadeEvent)
        {
            Deposit deposit = Deposit.Create(automaticDepositMadeEvent.DepositId,
                                             MerchantDepositSource.Automatic,
                                             automaticDepositMadeEvent.Reference,
                                             automaticDepositMadeEvent.DepositDateTime,
                                             automaticDepositMadeEvent.Amount);
            this.Deposits.Add(deposit);
        }

        #endregion
    }
}
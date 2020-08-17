namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Merchant.DomainEvents;
    using Models;
    using Models.Merchant;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventStore;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Aggregate" />
    public class MerchantAggregate : Aggregate
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
        /// Initializes a new instance of the <see cref="MerchantAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MerchantAggregate()
        {
            // Nothing here
            this.Addresses = new List<Address>();
            this.Contacts = new List<Contact>();
            this.Deposits = new List<Deposit>();
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
            this.Devices = new Dictionary<Guid, String>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private MerchantAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Addresses = new List<Address>();
            this.Contacts = new List<Contact>();
            this.Deposits = new List<Deposit>();
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
            this.Devices = new Dictionary<Guid, String>();
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

        /// <summary>
        /// Gets the maximum devices.
        /// </summary>
        /// <value>
        /// The maximum devices.
        /// </value>
        public Int32 MaximumDevices { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="addressLine3">The address line3.</param>
        /// <param name="addressLine4">The address line4.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="country">The country.</param>
        public void AddAddress(Guid addressId,
                               String addressLine1,
                               String addressLine2,
                               String addressLine3,
                               String addressLine4,
                               String town,
                               String region,
                               String postalCode,
                               String country)
        {
            this.EnsureMerchantHasBeenCreated();

            AddressAddedEvent addressAddedEvent = AddressAddedEvent.Create(this.AggregateId,
                                                                           this.EstateId,
                                                                           addressId,
                                                                           addressLine1,
                                                                           addressLine2,
                                                                           addressLine3,
                                                                           addressLine4,
                                                                           town,
                                                                           region,
                                                                           postalCode,
                                                                           country);

            this.ApplyAndPend(addressAddedEvent);
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        public void AddContact(Guid contactId,
                               String contactName,
                               String contactPhoneNumber,
                               String contactEmailAddress)
        {
            this.EnsureMerchantHasBeenCreated();

            ContactAddedEvent contactAddedEvent =
                ContactAddedEvent.Create(this.AggregateId, this.EstateId, contactId, contactName, contactPhoneNumber, contactEmailAddress);

            this.ApplyAndPend(contactAddedEvent);
        }

        /// <summary>
        /// Adds the device.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        public void AddDevice(Guid deviceId,
                              String deviceIdentifier)
        {
            Guard.ThrowIfNullOrEmpty(deviceIdentifier, typeof(ArgumentNullException), "Device Identifier cannot be null or empty");

            this.EnsureMerchantHasBeenCreated();
            this.EnsureMerchantHasSpaceForDevice();
            // TODO: Reintroduce when merchant can request > 1 device
            //this.EnsureNotDuplicateDevice(deviceId, deviceIdentifier);

            DeviceAddedToMerchantEvent deviceAddedToMerchantEvent = DeviceAddedToMerchantEvent.Create(this.AggregateId, this.EstateId, deviceId, deviceIdentifier);

            this.ApplyAndPend(deviceAddedToMerchantEvent);
        }
        
        /// <summary>
        /// Adds the security user.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        public void AddSecurityUser(Guid securityUserId,
                                    String emailAddress)
        {
            this.EnsureMerchantHasBeenCreated();

            SecurityUserAddedEvent securityUserAddedEvent = SecurityUserAddedEvent.Create(this.AggregateId, this.EstateId, securityUserId, emailAddress);

            this.ApplyAndPend(securityUserAddedEvent);
        }

        public void AssignOperator(Guid operatorId,
                                   String operatorName,
                                   String merchantNumber,
                                   String terminalNumber)
        {
            this.EnsureMerchantHasBeenCreated();
            this.EnsureOperatorHasNotAlreadyBeenAssigned(operatorId);

            OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent =
                OperatorAssignedToMerchantEvent.Create(this.AggregateId, this.EstateId, operatorId, operatorName, merchantNumber, terminalNumber);

            this.ApplyAndPend(operatorAssignedToMerchantEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static MerchantAggregate Create(Guid aggregateId)
        {
            return new MerchantAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="dateCreated">The date created.</param>
        public void Create(Guid estateId,
                           String merchantName,
                           DateTime dateCreated)
        {
            // Ensure this merchant has not already been created
            this.EnsureMerchantNotAlreadyCreated();

            MerchantCreatedEvent merchantCreatedEvent = MerchantCreatedEvent.Create(this.AggregateId, estateId, merchantName, dateCreated);

            this.ApplyAndPend(merchantCreatedEvent);
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <returns></returns>
        public Merchant GetMerchant()
        {
            if (this.IsCreated == false)
            {
                return null;
            }

            Merchant merchantModel = new Merchant();

            merchantModel.EstateId = this.EstateId;
            merchantModel.MerchantId = this.AggregateId;
            merchantModel.MerchantName = this.Name;

            if (this.Addresses.Any())
            {
                merchantModel.Addresses = new List<Models.Merchant.Address>();
                this.Addresses.ForEach(a => merchantModel.Addresses.Add(new Models.Merchant.Address
                                                                        {
                                                                            AddressId = a.AddressId,
                                                                            Town = a.Town,
                                                                            Region = a.Region,
                                                                            PostalCode = a.PostalCode,
                                                                            Country = a.Country,
                                                                            AddressLine1 = a.AddressLine1,
                                                                            AddressLine4 = a.AddressLine4,
                                                                            AddressLine3 = a.AddressLine3,
                                                                            AddressLine2 = a.AddressLine2
                                                                        }));
            }

            if (this.Contacts.Any())
            {
                merchantModel.Contacts = new List<Models.Merchant.Contact>();
                this.Contacts.ForEach(c => merchantModel.Contacts.Add(new Models.Merchant.Contact
                                                                      {
                                                                          ContactId = c.ContactId,
                                                                          ContactPhoneNumber = c.ContactPhoneNumber,
                                                                          ContactEmailAddress = c.ContactEmailAddress,
                                                                          ContactName = c.ContactName
                                                                      }));
            }

            if (this.Operators.Any())
            {
                merchantModel.Operators = new List<Models.Merchant.Operator>();
                this.Operators.ForEach(o => merchantModel.Operators.Add(new Models.Merchant.Operator
                                                                        {
                                                                            OperatorId = o.OperatorId,
                                                                            Name = o.Name,
                                                                            TerminalNumber = o.TerminalNumber,
                                                                            MerchantNumber = o.MerchantNumber
                                                                        }));
            }

            if (this.SecurityUsers.Any())
            {
                merchantModel.SecurityUsers = new List<Models.SecurityUser>();
                this.SecurityUsers.ForEach(s => merchantModel.SecurityUsers.Add(new Models.SecurityUser
                                                                                {
                                                                                    SecurityUserId = s.SecurityUserId,
                                                                                    EmailAddress = s.EmailAddress
                                                                                }));
            }

            if (this.Devices.Any())
            {
                merchantModel.Devices = new Dictionary<Guid, String>();
                foreach ((Guid key, String value) in this.Devices)
                {
                    merchantModel.Devices.Add(key, value);
                }
            }

            if (this.Deposits.Any())
            {
                merchantModel.Deposits=new List<Models.Merchant.Deposit>();
                this.Deposits.ForEach(d => merchantModel.Deposits.Add(new Models.Merchant.Deposit
                                                                      {
                                                                          Source = d.Source,
                                                                          DepositDateTime = d.DepositDateTime,
                                                                          DepositId = d.DepositId,
                                                                          Amount = d.Amount,
                                                                          Reference = d.Reference
                                                                      }));
            }

            return merchantModel;
        }

        /// <summary>
        /// Makes the deposit.
        /// </summary>
        /// <param name="depositId">The deposit identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        public void MakeDeposit(Guid depositId,
                                MerchantDepositSource source,
                                String reference,
                                DateTime depositDateTime,
                                Decimal amount)
        {
            this.EnsureMerchantHasBeenCreated();
            this.EnsureNotDuplicateDeposit(depositId);
            // TODO: Change amount to a value object (PositiveAmount VO)
            this.EnsureDepositSourceHasBeenSet(source);

            if (source == MerchantDepositSource.Manual)
            {
                ManualDepositMadeEvent manualDepositMadeEvent =
                    ManualDepositMadeEvent.Create(this.AggregateId, this.EstateId, depositId, reference, depositDateTime, amount);
                this.ApplyAndPend(manualDepositMadeEvent);
            }
            else if (source == MerchantDepositSource.Automatic)
            {
                // TODO:
                throw new NotSupportedException("Automatic deposits are not yet supported");
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
                throw new InvalidOperationException($"Deposit Id [{depositId}] already made for merchant [{this.Name}]");
            }
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
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
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
        private void EnsureMerchantHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException("Merchant has not been created");
            }
        }

        ///// <summary>
        ///// Ensures the not duplicate device.
        ///// </summary>
        ///// <param name="deviceId">The device identifier.</param>
        ///// <param name="deviceIdentifier">The device identifier.</param>
        ///// <exception cref="InvalidOperationException">
        ///// Device Id {deviceId} already allocated to Merchant {this.Name}
        ///// or
        ///// Device Identifier {deviceIdentifier} already allocated to Merchant {this.Name}
        ///// </exception>
        //private void EnsureNotDuplicateDevice(Guid deviceId, String deviceIdentifier)
        //{
        //    if (this.Devices.Any(d => d.Key == deviceId))
        //    {
        //        throw new InvalidOperationException($"Device Id {deviceId} already allocated to Merchant {this.Name}");
        //    }

        //    if (this.Devices.Any(d => d.Value == deviceIdentifier))
        //    {
        //        throw new InvalidOperationException($"Device Identifier {deviceIdentifier} already allocated to Merchant {this.Name}");
        //    }
        //}

        /// <summary>
        /// Ensures the merchant has space for device.
        /// </summary>
        /// <exception cref="InvalidOperationException">Merchant {this.Name} already has the maximum devices allocated</exception>
        private void EnsureMerchantHasSpaceForDevice()
        {
            if (this.Devices.Count + 1 > this.MaximumDevices)
            {
                throw new InvalidOperationException($"Merchant {this.Name} already has the maximum devices allocated");
            }
        }

        /// <summary>
        /// Ensures the merchant not already created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Merchant {this.Name} is already created</exception>
        private void EnsureMerchantNotAlreadyCreated()
        {
            if (this.IsCreated)
            {
                throw new InvalidOperationException($"Merchant {this.Name} is already created");
            }
        }

        /// <summary>
        /// Ensures the operator has not already been assigned.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <exception cref="InvalidOperationException">Operator {operatorId} has already been assigned to merchant</exception>
        private void EnsureOperatorHasNotAlreadyBeenAssigned(Guid operatorId)
        {
            if (this.Operators.Any(o => o.OperatorId == operatorId))
            {
                throw new InvalidOperationException($"Operator {operatorId} has already been assigned to merchant");
            }
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="merchantCreatedEvent">The merchant created event.</param>
        private void PlayEvent(MerchantCreatedEvent merchantCreatedEvent)
        {
            this.IsCreated = true;
            this.EstateId = merchantCreatedEvent.EstateId;
            this.Name = merchantCreatedEvent.MerchantName;
            this.AggregateId = merchantCreatedEvent.AggregateId;
            this.DateCreated = merchantCreatedEvent.DateCreated;
            this.MaximumDevices = 1;
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

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="addressAddedEvent">The address added event.</param>
        private void PlayEvent(AddressAddedEvent addressAddedEvent)
        {
            Address address = Address.Create(addressAddedEvent.AddressId,
                                             addressAddedEvent.AddressLine1,
                                             addressAddedEvent.AddressLine2,
                                             addressAddedEvent.AddressLine3,
                                             addressAddedEvent.AddressLine4,
                                             addressAddedEvent.Town,
                                             addressAddedEvent.Region,
                                             addressAddedEvent.PostalCode,
                                             addressAddedEvent.Country);

            this.Addresses.Add(address);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="contactAddedEvent">The contact added event.</param>
        private void PlayEvent(ContactAddedEvent contactAddedEvent)
        {
            Contact contact = Contact.Create(contactAddedEvent.ContactId,
                                             contactAddedEvent.ContactName,
                                             contactAddedEvent.ContactPhoneNumber,
                                             contactAddedEvent.ContactEmailAddress);

            this.Contacts.Add(contact);
        }

        private void PlayEvent(OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent)
        {
            Operator @operator = Operator.Create(operatorAssignedToMerchantEvent.OperatorId,
                                                 operatorAssignedToMerchantEvent.Name,
                                                 operatorAssignedToMerchantEvent.MerchantNumber,
                                                 operatorAssignedToMerchantEvent.TerminalNumber);

            this.Operators.Add(@operator);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(SecurityUserAddedEvent domainEvent)
        {
            SecurityUser securityUser = SecurityUser.Create(domainEvent.SecurityUserId, domainEvent.EmailAddress);

            this.SecurityUsers.Add(securityUser);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(DeviceAddedToMerchantEvent domainEvent)
        {
            this.Devices.Add(domainEvent.DeviceId, domainEvent.DeviceIdentifier);
        }

        #endregion
    }
}
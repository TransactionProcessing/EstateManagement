namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using ContractAggregate;
    using Merchant.DomainEvents;
    using Models;
    using Models.Contract;
    using Models.Merchant;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;

    public static class MerchantAggregateExtensions{

        public static void UpdateMerchant(this MerchantAggregate aggregate, String name){
            aggregate.EnsureMerchantHasBeenCreated();

            if (String.Compare(name, aggregate.Name, StringComparison.InvariantCultureIgnoreCase) != 0 &&
                String.IsNullOrEmpty(name) == false){
                // Name has been updated to raise an event for this
                MerchantNameUpdatedEvent merchantNameUpdatedEvent = new MerchantNameUpdatedEvent(aggregate.AggregateId,
                                                                                                 aggregate.EstateId,
                                                                                                 name);
                aggregate.ApplyAndAppend(merchantNameUpdatedEvent);
            }
        }

        public static void AddAddress(this MerchantAggregate aggregate,
            Guid addressId,
                               String addressLine1,
                               String addressLine2,
                               String addressLine3,
                               String addressLine4,
                               String town,
                               String region,
                               String postalCode,
                               String country)
        {
            aggregate.EnsureMerchantHasBeenCreated();

            AddressAddedEvent addressAddedEvent = new AddressAddedEvent(aggregate.AggregateId,
                                                                        aggregate.EstateId,
                                                                        addressId,
                                                                        addressLine1,
                                                                        addressLine2,
                                                                        addressLine3,
                                                                        addressLine4,
                                                                        town,
                                                                        region,
                                                                        postalCode,
                                                                        country);

            aggregate.ApplyAndAppend(addressAddedEvent);
        }

        public static void GenerateReference(this MerchantAggregate aggregate)
        {
            // Just return as we already have a reference allocated
            if (String.IsNullOrEmpty(aggregate.MerchantReference) == false)
                return;

            aggregate.EnsureMerchantHasBeenCreated();

            String reference = String.Format("{0:X}", aggregate.AggregateId.GetHashCode());

            MerchantReferenceAllocatedEvent merchantReferenceAllocatedEvent = new MerchantReferenceAllocatedEvent(aggregate.AggregateId, aggregate.EstateId, reference);

            aggregate.ApplyAndAppend(merchantReferenceAllocatedEvent);
        }

        public static void AddContract(this MerchantAggregate aggregate, ContractAggregate contractAggregate){
            aggregate.EnsureMerchantHasBeenCreated();
            aggregate.EnsureContractHasNotAlreadyBeenAdded(contractAggregate.AggregateId);

            ContractAddedToMerchantEvent contractAddedToMerchantEvent = new ContractAddedToMerchantEvent(aggregate.AggregateId, aggregate.EstateId, contractAggregate.AggregateId);

            aggregate.ApplyAndAppend(contractAddedToMerchantEvent);

            foreach (Product product in contractAggregate.GetProducts()){
                ContractProductAddedToMerchantEvent contractProductAddedToMerchantEvent = new ContractProductAddedToMerchantEvent(aggregate.AggregateId,
                                                                                                                                  aggregate.EstateId,
                                                                                                                                  contractAggregate.AggregateId,
                                                                                                                                  product.ProductId);
                aggregate.ApplyAndAppend(contractProductAddedToMerchantEvent);
            }
        }

        public static void AddContact(this MerchantAggregate aggregate, 
                                      Guid contactId,
                                      String contactName,
                                      String contactPhoneNumber,
                                      String contactEmailAddress)
        {
            aggregate.EnsureMerchantHasBeenCreated();

            ContactAddedEvent contactAddedEvent =
                new ContactAddedEvent(aggregate.AggregateId, aggregate.EstateId, contactId, contactName, contactPhoneNumber, contactEmailAddress);

            aggregate.ApplyAndAppend(contactAddedEvent);
        }

        public static void AddDevice(this MerchantAggregate aggregate, 
                                     Guid deviceId,
                                     String deviceIdentifier)
        {
            Guard.ThrowIfNullOrEmpty(deviceIdentifier, typeof(ArgumentNullException), "Device Identifier cannot be null or empty");

            aggregate.EnsureMerchantHasBeenCreated();
            aggregate.EnsureMerchantHasSpaceForDevice();
            // TODO: Reintroduce when merchant can request > 1 device
            //aggregate.EnsureNotDuplicateDevice(deviceId, deviceIdentifier);

            DeviceAddedToMerchantEvent deviceAddedToMerchantEvent = new DeviceAddedToMerchantEvent(aggregate.AggregateId, aggregate.EstateId, deviceId, deviceIdentifier);

            aggregate.ApplyAndAppend(deviceAddedToMerchantEvent);
        }

        public static void SwapDevice(this MerchantAggregate aggregate, 
                                      Guid deviceId,
                                      String originalDeviceIdentifier,
                                      String newDeviceIdentifier)
        {
            Guard.ThrowIfNullOrEmpty(originalDeviceIdentifier, typeof(ArgumentNullException), "Original Device Identifier cannot be null or empty");
            Guard.ThrowIfNullOrEmpty(newDeviceIdentifier, typeof(ArgumentNullException), "New Device Identifier cannot be null or empty");

            aggregate.EnsureMerchantHasBeenCreated();
            aggregate.EnsureDeviceBelongsToMerchant(originalDeviceIdentifier);
            aggregate.EnsureDeviceDoesNotAlreadyBelongToMerchant(newDeviceIdentifier);

            DeviceSwappedForMerchantEvent deviceSwappedForMerchantEvent = new DeviceSwappedForMerchantEvent(
                                                                                                            aggregate.AggregateId, aggregate.EstateId,
                deviceId, originalDeviceIdentifier, newDeviceIdentifier);

            aggregate.ApplyAndAppend(deviceSwappedForMerchantEvent);
        }

        public static void AddSecurityUser(this MerchantAggregate aggregate, 
                                           Guid securityUserId,
                                           String emailAddress)
        {
            aggregate.EnsureMerchantHasBeenCreated();

            SecurityUserAddedToMerchantEvent securityUserAddedEvent = new SecurityUserAddedToMerchantEvent(aggregate.AggregateId, aggregate.EstateId, securityUserId, emailAddress);

            aggregate.ApplyAndAppend(securityUserAddedEvent);
        }
        
        public static void AssignOperator(this MerchantAggregate aggregate, 
                                          Guid operatorId,
                                          String operatorName,
                                          String merchantNumber,
                                          String terminalNumber)
        {
            aggregate.EnsureMerchantHasBeenCreated();
            aggregate.EnsureOperatorHasNotAlreadyBeenAssigned(operatorId);

            OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent =
                new OperatorAssignedToMerchantEvent(aggregate.AggregateId, aggregate.EstateId, operatorId, operatorName, merchantNumber, terminalNumber);

            aggregate.ApplyAndAppend(operatorAssignedToMerchantEvent);
        }

        public static Merchant GetMerchant(this MerchantAggregate aggregate)
        {
            if (aggregate.IsCreated == false)
            {
                return null;
            }

            Merchant merchantModel = new Merchant();

            merchantModel.EstateId = aggregate.EstateId;
            merchantModel.MerchantId = aggregate.AggregateId;
            merchantModel.MerchantName = aggregate.Name;
            merchantModel.Reference = aggregate.MerchantReference;
            merchantModel.SettlementSchedule = aggregate.SettlementSchedule;
            merchantModel.NextSettlementDueDate = aggregate.NextSettlementDueDate;

            if (aggregate.Addresses.Any())
            {
                merchantModel.Addresses = new List<Models.Merchant.Address>();
                aggregate.Addresses.ForEach(a => merchantModel.Addresses.Add(new Models.Merchant.Address
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

            if (aggregate.Contacts.Any())
            {
                merchantModel.Contacts = new List<Models.Merchant.Contact>();
                aggregate.Contacts.ForEach(c => merchantModel.Contacts.Add(new Models.Merchant.Contact
                                                                           {
                                                                               ContactId = c.ContactId,
                                                                               ContactPhoneNumber = c.ContactPhoneNumber,
                                                                               ContactEmailAddress = c.ContactEmailAddress,
                                                                               ContactName = c.ContactName
                                                                           }));
            }

            if (aggregate.Operators.Any())
            {
                merchantModel.Operators = new List<Models.Merchant.Operator>();
                aggregate.Operators.ForEach(o => merchantModel.Operators.Add(new Models.Merchant.Operator
                                                                             {
                                                                                 OperatorId = o.OperatorId,
                                                                                 Name = o.Name,
                                                                                 TerminalNumber = o.TerminalNumber,
                                                                                 MerchantNumber = o.MerchantNumber
                                                                             }));
            }

            if (aggregate.SecurityUsers.Any())
            {
                merchantModel.SecurityUsers = new List<Models.SecurityUser>();
                aggregate.SecurityUsers.ForEach(s => merchantModel.SecurityUsers.Add(new Models.SecurityUser
                                                                                     {
                                                                                         SecurityUserId = s.SecurityUserId,
                                                                                         EmailAddress = s.EmailAddress
                                                                                     }));
            }

            if (aggregate.Devices.Any())
            {
                merchantModel.Devices = new Dictionary<Guid, String>();
                foreach ((Guid key, String value) in aggregate.Devices)
                {
                    merchantModel.Devices.Add(key, value);
                }
            }

            if (aggregate.Contracts.Any()){
                merchantModel.Contracts = new List<Models.Merchant.Contract>();
                foreach (Contract aggregateContract in aggregate.Contracts){
                    Models.Merchant.Contract contract = new Models.Merchant.Contract();
                    contract.ContractId = aggregateContract.ContractId;
                    aggregateContract.ContractProducts.ForEach(cp => contract.ContractProducts.Add(cp));
                    merchantModel.Contracts.Add(contract);
                }
            }

            return merchantModel;
        }
        
        public static void SetSettlementSchedule(this MerchantAggregate aggregate, SettlementSchedule settlementSchedule)
        {
            // Check if there has actually been a change or not, if not ignore the request
            if (aggregate.SettlementSchedule == settlementSchedule)
                return;
            DateTime nextSettlementDate = DateTime.MinValue;
            if (settlementSchedule != SettlementSchedule.Immediate)
            {
                // Calculate next settlement date
                DateTime dateForCalculation = aggregate.NextSettlementDueDate;
                if (dateForCalculation == DateTime.MinValue)
                {
                    // No date set previously so use current date as start point
                    dateForCalculation = DateTime.Now.Date;
                }

                if (settlementSchedule == SettlementSchedule.Weekly)
                {
                    nextSettlementDate = dateForCalculation.AddDays(7);
                }
                else if (settlementSchedule == SettlementSchedule.Monthly)
                {
                    nextSettlementDate = dateForCalculation.AddMonths(1);
                }
            }

            SettlementScheduleChangedEvent settlementScheduleChangedEvent =
                new SettlementScheduleChangedEvent(aggregate.AggregateId, aggregate.EstateId, (Int32)settlementSchedule, nextSettlementDate);

            aggregate.ApplyAndAppend(settlementScheduleChangedEvent);
        }

        public static void Create(this MerchantAggregate aggregate, 
                           Guid estateId,
                           String merchantName,
                           DateTime dateCreated)
        {
            // Ensure this merchant has not already been created
            if (aggregate.IsCreated)
                return;

            MerchantCreatedEvent merchantCreatedEvent = new MerchantCreatedEvent(aggregate.AggregateId, estateId, merchantName, dateCreated);

            aggregate.ApplyAndAppend(merchantCreatedEvent);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, MerchantReferenceAllocatedEvent domainEvent)
        {
            aggregate.MerchantReference = domainEvent.MerchantReference;
        }

        private static void EnsureMerchantHasBeenCreated(this MerchantAggregate aggregate)
        {
            if (aggregate.IsCreated == false)
            {
                throw new InvalidOperationException("Merchant has not been created");
            }
        }

        private static void EnsureDeviceBelongsToMerchant(this MerchantAggregate aggregate, String originalDeviceIdentifier)
        {
            if (aggregate.Devices.ContainsValue(originalDeviceIdentifier) == false)
            {
                throw new InvalidOperationException("Merchant does not have this device allocated");
            }
        }

        private static void EnsureDeviceDoesNotAlreadyBelongToMerchant(this MerchantAggregate aggregate, String newDeviceIdentifier)
        {
            if (aggregate.Devices.ContainsValue(newDeviceIdentifier) == true)
            {
                throw new InvalidOperationException("Merchant already has this device allocated");
            }
        }
        
        private static void EnsureMerchantHasSpaceForDevice(this MerchantAggregate aggregate)
        {
            if (aggregate.Devices.Count + 1 > aggregate.MaximumDevices)
            {
                throw new InvalidOperationException($"Merchant {aggregate.Name} already has the maximum devices allocated");
            }
        }

        private static void EnsureOperatorHasNotAlreadyBeenAssigned(this MerchantAggregate aggregate, Guid operatorId)
        {
            if (aggregate.Operators.Any(o => o.OperatorId == operatorId))
            {
                throw new InvalidOperationException($"Operator {operatorId} has already been assigned to merchant");
            }
        }

        private static void EnsureContractHasNotAlreadyBeenAdded(this MerchantAggregate aggregate, Guid contractId)
        {
            if (aggregate.Contracts.Any(o => o.ContractId == contractId))
            {
                throw new InvalidOperationException($"Contract {contractId} has already been assigned to merchant");
            }
        }

        public static void PlayEvent(this MerchantAggregate aggregate, MerchantCreatedEvent merchantCreatedEvent)
        {
            aggregate.IsCreated = true;
            aggregate.EstateId = merchantCreatedEvent.EstateId;
            aggregate.Name = merchantCreatedEvent.MerchantName;
            aggregate.AggregateId = merchantCreatedEvent.AggregateId;
            aggregate.DateCreated = merchantCreatedEvent.DateCreated;
            aggregate.MaximumDevices = 1;
        }

        public static void PlayEvent(this MerchantAggregate aggregate, MerchantNameUpdatedEvent merchantNameUpdatedEvent){
            aggregate.Name = merchantNameUpdatedEvent.MerchantName;
        }

        public static void PlayEvent(this MerchantAggregate aggregate, AddressAddedEvent addressAddedEvent)
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

            aggregate.Addresses.Add(address);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, ContactAddedEvent contactAddedEvent)
        {
            Contact contact = Contact.Create(contactAddedEvent.ContactId,
                                             contactAddedEvent.ContactName,
                                             contactAddedEvent.ContactPhoneNumber,
                                             contactAddedEvent.ContactEmailAddress);

            aggregate.Contacts.Add(contact);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, OperatorAssignedToMerchantEvent operatorAssignedToMerchantEvent)
        {
            Operator @operator = Operator.Create(operatorAssignedToMerchantEvent.OperatorId,
                                                 operatorAssignedToMerchantEvent.Name,
                                                 operatorAssignedToMerchantEvent.MerchantNumber,
                                                 operatorAssignedToMerchantEvent.TerminalNumber);

            aggregate.Operators.Add(@operator);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, SecurityUserAddedToMerchantEvent domainEvent)
        {
            SecurityUser securityUser = SecurityUser.Create(domainEvent.SecurityUserId, domainEvent.EmailAddress);

            aggregate.SecurityUsers.Add(securityUser);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, SettlementScheduleChangedEvent domainEvent)
        {
            aggregate.SettlementSchedule = (SettlementSchedule)domainEvent.SettlementSchedule;
            aggregate.NextSettlementDueDate = domainEvent.NextSettlementDate;
        }

        public static void PlayEvent(this MerchantAggregate aggregate, ContractAddedToMerchantEvent domainEvent){
            aggregate.Contracts.Add(new Contract(domainEvent.ContractId));
        }

        public static void PlayEvent(this MerchantAggregate aggregate, ContractProductAddedToMerchantEvent domainEvent){
            Contract contract = aggregate.Contracts.Single(c => c.ContractId == domainEvent.ContractId);
            contract.AddContractProduct(domainEvent.ContractProductId);
        }

        public static void PlayEvent(this MerchantAggregate aggregate, DeviceSwappedForMerchantEvent domainEvent)
        {
            KeyValuePair<Guid, String> device = aggregate.Devices.Single(d => d.Value == domainEvent.OriginalDeviceIdentifier);
            aggregate.Devices.Remove(device.Key);

            aggregate.Devices.Add(domainEvent.DeviceId, domainEvent.NewDeviceIdentifier);

        }
        public static void PlayEvent(this MerchantAggregate aggregate, DeviceAddedToMerchantEvent domainEvent)
        {
            aggregate.Devices.Add(domainEvent.DeviceId, domainEvent.DeviceIdentifier);
        }
    }

    public record MerchantAggregate : Aggregate
    {
        #region Fields

        internal readonly List<Address> Addresses;

        internal readonly List<Contact> Contacts;

        internal readonly Dictionary<Guid, String> Devices;

        internal readonly List<Operator> Operators;

        internal readonly List<SecurityUser> SecurityUsers;

        internal readonly List<Contract> Contracts;

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
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
            this.Devices = new Dictionary<Guid, String>();
            this.Contracts = new List<Contract>();
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
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
            this.Devices = new Dictionary<Guid, String>();
            this.Contracts = new List<Contract>();
        }

        #endregion

        #region Properties

        public DateTime DateCreated { get; internal set; }

        public DateTime NextSettlementDueDate { get; internal set; }
        
        public Guid EstateId { get; internal set; }

        public Boolean IsCreated { get; internal set; }

        public Int32 MaximumDevices { get; internal set; }

        public String Name { get; internal set; }

        public String MerchantReference { get; internal set; }

        public SettlementSchedule SettlementSchedule { get; internal set; }

        #endregion

        #region Methods
        
        public static MerchantAggregate Create(Guid aggregateId)
        {
            return new MerchantAggregate(aggregateId);
        }
        
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       this.EstateId,
                       MerchantId = this.AggregateId
                   };
        }
        
        public override void PlayEvent(IDomainEvent domainEvent) => MerchantAggregateExtensions.PlayEvent(this, (dynamic)domainEvent);


        #endregion
    }
}
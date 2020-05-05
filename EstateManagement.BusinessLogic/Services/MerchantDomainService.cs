namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IMerchantDomainService" />
    public class MerchantDomainService : IMerchantDomainService
    {
        #region Fields

        /// <summary>
        /// The aggregate repository manager
        /// </summary>
        private readonly IAggregateRepositoryManager AggregateRepositoryManager;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDomainService" /> class.
        /// </summary>
        /// <param name="aggregateRepositoryManager">The aggregate repository manager.</param>
        public MerchantDomainService(IAggregateRepositoryManager aggregateRepositoryManager,
                                     ISecurityServiceClient securityServiceClient)
        {
            this.AggregateRepositoryManager = aggregateRepositoryManager;
            this.SecurityServiceClient = securityServiceClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="addressLine3">The address line3.</param>
        /// <param name="addressLine4">The address line4.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="country">The country.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.InvalidOperationException">Estate Id {estateId} has not been created</exception>
        public async Task CreateMerchant(Guid estateId,
                                         Guid merchantId,
                                         String name,
                                         Guid addressId,
                                         String addressLine1,
                                         String addressLine2,
                                         String addressLine3,
                                         String addressLine4,
                                         String town,
                                         String region,
                                         String postalCode,
                                         String country,
                                         Guid contactId,
                                         String contactName,
                                         String contactPhoneNumber,
                                         String contactEmailAddress,
                                         CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);

            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            // Reject Duplicate Merchant Names... is this needed ?

            // Create the merchant
            merchantAggregate.Create(estateId, name, DateTime.Now);

            // Add the address 
            merchantAggregate.AddAddress(addressId, addressLine1, addressLine2, addressLine3, addressLine4, town, region, postalCode, country);

            // Add the contact
            merchantAggregate.AddContact(contactId, contactName, contactPhoneNumber, contactEmailAddress);

            await merchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task AssignOperatorToMerchant(Guid estateId,
                                                   Guid merchantId,
                                                   Guid operatorId,
                                                   String merchantNumber,
                                                   String terminalNumber,
                                                   CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);

            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            // Is the operator valid for this estate
            Estate estate = estateAggregate.GetEstate();
            Operator @operator = estate.Operators?.SingleOrDefault(o => o.OperatorId == operatorId);
            if (@operator == null)
            {
                throw new InvalidOperationException($"Operator Id {operatorId} is not supported on Estate [{estate.Name}]");
            }

            // Operator has been validated, now check the rules of the operator against the passed in data
            if (@operator.RequireCustomMerchantNumber)
            {
                // requested addition must have a merchant number supplied
                if (String.IsNullOrEmpty(merchantNumber))
                {
                    throw new InvalidOperationException($"Operator Id {operatorId} requires that a merchant number is provided");
                }
            }

            if (@operator.RequireCustomTerminalNumber)
            {
                // requested addition must have a terminal number supplied
                if (String.IsNullOrEmpty(terminalNumber))
                {
                    throw new InvalidOperationException($"Operator Id {operatorId} requires that a terminal number is provided");
                }
            }

            // Assign the operator
            merchantAggregate.AssignOperator(operatorId, @operator.Name, merchantNumber, terminalNumber);

            await merchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task<Guid> CreateMerchantUser(Guid estateId,
                                                   Guid merchantId,
                                                   String emailAddress,
                                                   String password,
                                                   String givenName,
                                                   String middleName,
                                                   String familyName,
                                                   CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);
            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            CreateUserRequest createUserRequest = new CreateUserRequest
                                                  {
                                                      EmailAddress = emailAddress,
                                                      FamilyName = familyName,
                                                      GivenName = givenName,
                                                      MiddleName = middleName,
                                                      Password = password,
                                                      PhoneNumber = "123456", // Is this really needed :|
                                                      Roles = new List<String>(),
                                                      Claims = new Dictionary<String, String>()
                                                  };

            String merchantRoleName = Environment.GetEnvironmentVariable("MerchantRoleName");
            createUserRequest.Roles.Add(String.IsNullOrEmpty(merchantRoleName) ? "Merchant" : merchantRoleName);
            createUserRequest.Claims.Add("EstateId", estateId.ToString());
            createUserRequest.Claims.Add("MerchantId", merchantId.ToString());

            CreateUserResponse createUserResponse = await this.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken);

            // Add the user to the aggregate 
            merchantAggregate.AddSecurityUser(createUserResponse.UserId, emailAddress);

            // TODO: add a delete user here in case the aggregate add fails...

            await merchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);

            return createUserResponse.UserId;
        }

        /// <summary>
        /// Adds the device to merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddDeviceToMerchant(Guid estateId,
                                                      Guid merchantId,
                                                      Guid deviceId,
                                                      String deviceIdentifier,
                                                      CancellationToken cancellationToken)
        {
            IAggregateRepository<EstateAggregate> estateAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<EstateAggregate>(estateId);
            IAggregateRepository<MerchantAggregate> merchantAggregateRepository = this.AggregateRepositoryManager.GetAggregateRepository<MerchantAggregate>(estateId);
            MerchantAggregate merchantAggregate = await merchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await estateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.AddDevice(deviceId, deviceIdentifier);

            await merchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        #endregion
    }
}
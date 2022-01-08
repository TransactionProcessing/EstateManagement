namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Models;
    using Models.Estate;
    using Models.Merchant;
    using NLog;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventStore;
    using Shared.ValueObjects;
    using Operator = Models.Estate.Operator;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IMerchantDomainService" />
    public class MerchantDomainService : IMerchantDomainService
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent> EstateAggregateRepository;

        /// <summary>
        /// The merchant aggregate repository
        /// </summary>
        private readonly IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> MerchantAggregateRepository;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDomainService" /> class.
        /// </summary>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        /// <param name="merchantAggregateRepository">The merchant aggregate repository.</param>
        /// <param name="securityServiceClient">The security service client.</param>
        public MerchantDomainService(IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent> estateAggregateRepository,
                                     IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent> merchantAggregateRepository,
                                     ISecurityServiceClient securityServiceClient)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
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
                                         Models.SettlementSchedule settlementSchedule,
                                         CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            // Reject Duplicate Merchant Names... is this needed ?

            // Create the merchant
            if (merchantAggregate.IsCreated)
            {
                merchantAggregate.Create(estateId, name, merchantAggregate.DateCreated);
                merchantAggregate.GenerateReference();
            }
            else
            {
                merchantAggregate.Create(estateId, name, DateTime.Now);
                merchantAggregate.GenerateReference();

                // Add the address 
                merchantAggregate.AddAddress(addressId, addressLine1, addressLine2, addressLine3, addressLine4, town, region, postalCode, country);

                // Add the contact
                merchantAggregate.AddContact(contactId, contactName, contactPhoneNumber, contactEmailAddress);

                // Set the settlement schedule
                merchantAggregate.SetSettlementSchedule(settlementSchedule);
            }

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task AssignOperatorToMerchant(Guid estateId,
                                                   Guid merchantId,
                                                   Guid operatorId,
                                                   String merchantNumber,
                                                   String terminalNumber,
                                                   CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
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

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
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
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
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
            createUserRequest.Claims.Add("estateId", estateId.ToString());
            createUserRequest.Claims.Add("merchantId", merchantId.ToString());

            CreateUserResponse createUserResponse = await this.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken);

            // Add the user to the aggregate 
            merchantAggregate.AddSecurityUser(createUserResponse.UserId, emailAddress);

            // TODO: add a delete user here in case the aggregate add fails...

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);

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
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.AddDevice(deviceId, deviceIdentifier);
            
            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task SwapMerchantDevice(Guid estateId, Guid merchantId, Guid deviceId, string originalDeviceIdentifier,
            string newDeviceIdentifier, CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.SwapDevice(deviceId, originalDeviceIdentifier, newDeviceIdentifier);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        /// <summary>
        /// Makes the merchant deposit.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Merchant Id {merchantId} has not been created
        /// or
        /// Estate Id {estateId} has not been created
        /// </exception>
        public async Task<Guid> MakeMerchantDeposit(Guid estateId,
                                                    Guid merchantId,
                                                    Models.MerchantDepositSource source,
                                                    String reference,
                                                    DateTime depositDateTime,
                                                    Decimal depositAmount,
                                                    CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }
            
            PositiveMoney amount = PositiveMoney.Create(Money.Create(depositAmount));

            merchantAggregate.MakeDeposit(source,reference,depositDateTime,amount);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);

            Merchant merchant = merchantAggregate.GetMerchant();

            // Find the deposit
            Deposit deposit = merchant.Deposits.Single(d => d.Reference == reference && d.DepositDateTime == depositDateTime && d.Source == source &&
                                                                      d.Amount == amount.Value);

            return deposit.DepositId;
        }

        public async Task SetMerchantSettlementSchedule(Guid estateId,
                                                        Guid merchantId,
                                                        SettlementSchedule settlementSchedule,
                                                        CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.SetSettlementSchedule(settlementSchedule);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }
        
        #endregion
    }
}
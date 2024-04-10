namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ContractAggregate;
    using EstateAggregate;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MerchantAggregate;
    using Models;
    using Models.Merchant;
    using Requests;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.General;
    using Shared.Logger;
    using Shared.ValueObjects;
    using TransactionProcessor.Client;
    using TransactionProcessor.DataTransferObjects;
    using Estate = Models.Estate.Estate;
    using MerchantDepositSource = Models.MerchantDepositSource;
    using Operator = Models.Estate.Operator;
    using SettlementSchedule = DataTransferObjects.Responses.Merchant.SettlementSchedule;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IMerchantDomainService" />
    public class MerchantDomainService : IMerchantDomainService
    {
        #region Fields

        private readonly IAggregateRepository<EstateAggregate, DomainEvent> EstateAggregateRepository;

        private readonly IAggregateRepository<MerchantAggregate, DomainEvent> MerchantAggregateRepository;

        private readonly IAggregateRepository<MerchantDepositListAggregate, DomainEvent> MerchantDepositListAggregateRepository;

        private readonly IAggregateRepository<ContractAggregate, DomainEvent> ContractAggregateRepository;

        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly ITransactionProcessorClient TransactionProcessorClient;
        
        #endregion

        #region Constructors

        public MerchantDomainService(IAggregateRepository<EstateAggregate, DomainEvent> estateAggregateRepository,
                                     IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                     IAggregateRepository<MerchantDepositListAggregate, DomainEvent> merchantDepositListAggregateRepository,
                                     IAggregateRepository<ContractAggregate, DomainEvent> contractAggregateRepository,
                                     ISecurityServiceClient securityServiceClient,
                                     ITransactionProcessorClient transactionProcessorClient) {
            this.EstateAggregateRepository = estateAggregateRepository;
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.MerchantDepositListAggregateRepository = merchantDepositListAggregateRepository;
            this.ContractAggregateRepository = contractAggregateRepository;
            this.SecurityServiceClient = securityServiceClient;
            this.TransactionProcessorClient = transactionProcessorClient;
        }

        #endregion

        #region Methods

        public async Task AddDeviceToMerchant(Guid estateId,
                                              Guid merchantId,
                                              Guid deviceId,
                                              String deviceIdentifier,
                                              CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.AddDevice(deviceId, deviceIdentifier);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task AssignOperatorToMerchant(Guid estateId,
                                                   Guid merchantId,
                                                   Guid operatorId,
                                                   String merchantNumber,
                                                   String terminalNumber,
                                                   CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            // Is the operator valid for this estate
            Estate estate = estateAggregate.GetEstate();
            Operator @operator = estate.Operators?.SingleOrDefault(o => o.OperatorId == operatorId);
            if (@operator == null) {
                throw new InvalidOperationException($"Operator Id {operatorId} is not supported on Estate [{estate.Name}]");
            }

            // Operator has been validated, now check the rules of the operator against the passed in data
            if (@operator.RequireCustomMerchantNumber) {
                // requested addition must have a merchant number supplied
                if (String.IsNullOrEmpty(merchantNumber)) {
                    throw new InvalidOperationException($"Operator Id {operatorId} requires that a merchant number is provided");
                }
            }

            if (@operator.RequireCustomTerminalNumber) {
                // requested addition must have a terminal number supplied
                if (String.IsNullOrEmpty(terminalNumber)) {
                    throw new InvalidOperationException($"Operator Id {operatorId} requires that a terminal number is provided");
                }
            }

            // Assign the operator
            merchantAggregate.AssignOperator(operatorId, @operator.Name, merchantNumber, terminalNumber);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task<Guid> CreateMerchant(CreateMerchantCommand command, CancellationToken cancellationToken){
            // Convert the settlement schedule
            Models.SettlementSchedule settlementScheduleModel = command.RequestDto.SettlementSchedule switch
            {
                SettlementSchedule.Immediate => Models.SettlementSchedule.Immediate,
                SettlementSchedule.Monthly => Models.SettlementSchedule.Monthly,
                SettlementSchedule.Weekly => Models.SettlementSchedule.Weekly,
                _ => Models.SettlementSchedule.NotSet
            };

            // Check if we have been sent a merchant id to use
            Guid merchantId = command.RequestDto.MerchantId ?? Guid.NewGuid();
            
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {command.EstateId} has not been created");
            }

            // Reject Duplicate Merchant Names... is this needed ?

            // Create the merchant
            if (merchantAggregate.IsCreated)
            {
                merchantAggregate.Create(command.EstateId, command.RequestDto.Name, merchantAggregate.DateCreated);
                merchantAggregate.GenerateReference();
            }
            else
            {
                merchantAggregate.Create(command.EstateId, command.RequestDto.Name, command.RequestDto.CreatedDateTime.GetValueOrDefault(DateTime.Now));
                merchantAggregate.GenerateReference();

                Guid addressId = Guid.NewGuid();
                // Add the address 
                merchantAggregate.AddAddress(addressId, command.RequestDto.Address.AddressLine1, command.RequestDto.Address.AddressLine2, command.RequestDto.Address.AddressLine3,
                                             command.RequestDto.Address.AddressLine4, command.RequestDto.Address.Town, command.RequestDto.Address.Region,
                                             command.RequestDto.Address.PostalCode, command.RequestDto.Address.Country);

                // Add the contact
                Guid contactId = Guid.NewGuid();
                merchantAggregate.AddContact(contactId, command.RequestDto.Contact.ContactName, command.RequestDto.Contact.PhoneNumber, command.RequestDto.Contact.EmailAddress);

                // Set the settlement schedule
                merchantAggregate.SetSettlementSchedule(settlementScheduleModel);
            }

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);

            return merchantId;
        }
        
        public async Task<Guid> CreateMerchantUser(Guid estateId,
                                                   Guid merchantId,
                                                   String emailAddress,
                                                   String password,
                                                   String givenName,
                                                   String middleName,
                                                   String familyName,
                                                   CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            CreateUserRequest createUserRequest = new CreateUserRequest {
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

        public async Task<Guid> MakeMerchantDeposit(Guid estateId,
                                                    Guid merchantId,
                                                    MerchantDepositSource source,
                                                    String reference,
                                                    DateTime depositDateTime,
                                                    Decimal depositAmount,
                                                    CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            MerchantDepositListAggregate merchantDepositListAggregate = await this.MerchantDepositListAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            if (merchantDepositListAggregate.IsCreated == false) {
                merchantDepositListAggregate.Create(merchantAggregate, depositDateTime);
            }

            PositiveMoney amount = PositiveMoney.Create(Money.Create(depositAmount));

            merchantDepositListAggregate.MakeDeposit(source, reference, depositDateTime, amount);

            await this.MerchantDepositListAggregateRepository.SaveChanges(merchantDepositListAggregate, cancellationToken);

            List<Deposit> deposits = merchantDepositListAggregate.GetDeposits();

            // Find the deposit
            Deposit deposit = deposits.Single(d => d.Reference == reference && d.DepositDateTime == depositDateTime && d.Source == source && d.Amount == amount.Value);

            return deposit.DepositId;
        }

        public async Task<Guid> MakeMerchantWithdrawal(Guid estateId,
                                                 Guid merchantId,
                                                 DateTime withdrawalDateTime,
                                                 Decimal withdrawalAmount,
                                                 CancellationToken cancellationToken) {
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

            MerchantDepositListAggregate merchantDepositListAggregate = await this.MerchantDepositListAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            if (merchantDepositListAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant [{merchantId}] has not made any deposits yet");
            }

            // Now we need to check the merchants balance to ensure they have funds to withdraw
            this.TokenResponse = await this.GetToken(cancellationToken);
            //MerchantBalanceResponse merchantBalance = await this.TransactionProcessorClient.GetMerchantBalance(this.TokenResponse.AccessToken, estateId, merchantId, cancellationToken);
            //Decimal merchantBalance = await this.GetMerchantBalance(merchantId);
            MerchantBalanceResponse merchantBalance = await this.TransactionProcessorClient.GetMerchantBalance(this.TokenResponse.AccessToken, estateId, merchantId, cancellationToken);

            if (withdrawalAmount > merchantBalance.Balance) {
                throw new InvalidOperationException($"Not enough credit available for withdrawal of [{withdrawalAmount}]. Balance is {merchantBalance}");
            }

            // If we are here we have enough credit to withdraw
            PositiveMoney amount = PositiveMoney.Create(Money.Create(withdrawalAmount));

            merchantDepositListAggregate.MakeWithdrawal(withdrawalDateTime, amount);

            await this.MerchantDepositListAggregateRepository.SaveChanges(merchantDepositListAggregate, cancellationToken);

            List<Withdrawal> withdrawals = merchantDepositListAggregate.GetWithdrawals();

            // Find the withdrawal
            Withdrawal withdrawal = withdrawals.Single(d => d.WithdrawalDateTime == withdrawalDateTime && d.Amount == amount.Value);

            return withdrawal.WithdrawalId;
        }

        /// <summary>
        /// The token response
        /// </summary>
        private TokenResponse TokenResponse;

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private async Task<TokenResponse> GetToken(CancellationToken cancellationToken)
        {
            // Get a token to talk to the estate service
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");
            Logger.LogInformation($"Client Id is {clientId}");
            Logger.LogInformation($"Client Secret is {clientSecret}");

            if (this.TokenResponse == null)
            {
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            if (this.TokenResponse.Expires.UtcDateTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(2))
            {
                Logger.LogInformation($"Token is about to expire at {this.TokenResponse.Expires.DateTime:O}");
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            return this.TokenResponse;
        }

        public async Task SetMerchantSettlementSchedule(Guid estateId,
                                                        Guid merchantId,
                                                        Models.SettlementSchedule settlementSchedule,
                                                        CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.SetSettlementSchedule(settlementSchedule);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task AddContractToMerchant(Guid estateId, Guid merchantId, Guid contractId, CancellationToken cancellationToken){
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(contractId, cancellationToken);
            if (contractAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Contract Id {contractId} has not been created");
            }

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

            merchantAggregate.AddContract(contractAggregate);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        public async Task SwapMerchantDevice(Guid estateId,
                                             Guid merchantId,
                                             Guid deviceId,
                                             String originalDeviceIdentifier,
                                             String newDeviceIdentifier,
                                             CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {merchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            merchantAggregate.SwapDevice(deviceId, originalDeviceIdentifier, newDeviceIdentifier);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }
        
        #endregion
    }
}
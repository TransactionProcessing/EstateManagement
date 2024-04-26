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
    using MerchantAggregate;
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

        public async Task<Guid> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command, CancellationToken cancellationToken) {
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            Guid deviceId = Guid.NewGuid();
            validateResults.merchantAggregate.AddDevice(deviceId, command.RequestDto.DeviceIdentifier);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
            
            return deviceId;
        }

        public async Task<Guid> AssignOperatorToMerchant(MerchantCommands.AssignOperatorToMerchantCommand command,
                                                         CancellationToken cancellationToken) {
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            // Is the operator valid for this estate
            Estate estate = validateResults.estateAggregate.GetEstate();
            Operator @operator = estate.Operators?.SingleOrDefault(o => o.OperatorId == command.RequestDto.OperatorId);
            if (@operator == null) {
                throw new InvalidOperationException($"Operator Id {command.RequestDto.OperatorId} is not supported on Estate [{estate.Name}]");
            }

            // TODO: Reintroduce when we have an Operator Aggregate
            // https://github.com/TransactionProcessing/EstateManagement/issues/558
            // Operator has been validated, now check the rules of the operator against the passed in data
            //if (@operator.RequireCustomMerchantNumber) {
            //    // requested addition must have a merchant number supplied
            //    if (String.IsNullOrEmpty(command.RequestDto.MerchantNumber)) {
            //        throw new InvalidOperationException($"Operator Id {command.RequestDto.OperatorId} requires that a merchant number is provided");
            //    }
            //}

            //if (@operator.RequireCustomTerminalNumber) {
            //    // requested addition must have a terminal number supplied
            //    if (String.IsNullOrEmpty(command.RequestDto.TerminalNumber)) {
            //        throw new InvalidOperationException($"Operator Id {command.RequestDto.OperatorId} requires that a terminal number is provided");
            //    }
            //}

            // Assign the operator
            // TODO: Swap second parameter to name
            validateResults.merchantAggregate.AssignOperator(command.RequestDto.OperatorId, @operator.OperatorId.ToString(), command.RequestDto.MerchantNumber, command.RequestDto.TerminalNumber);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);

            return command.RequestDto.OperatorId;
        }

        private Models.SettlementSchedule ConvertSettlementSchedule(DataTransferObjects.Responses.Merchant.SettlementSchedule settlementSchedule) =>
            settlementSchedule switch{
                SettlementSchedule.Immediate => Models.SettlementSchedule.Immediate,
                SettlementSchedule.Monthly => Models.SettlementSchedule.Monthly,
                SettlementSchedule.Weekly => Models.SettlementSchedule.Weekly,
                _ => Models.SettlementSchedule.NotSet
            };

        public async Task<Guid> CreateMerchant(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken){
            Models.SettlementSchedule settlementSchedule = ConvertSettlementSchedule(command.RequestDto.SettlementSchedule);

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

                // Add the address 
                merchantAggregate.AddAddress(command.RequestDto.Address.AddressLine1, command.RequestDto.Address.AddressLine2, command.RequestDto.Address.AddressLine3,
                                             command.RequestDto.Address.AddressLine4, command.RequestDto.Address.Town, command.RequestDto.Address.Region,
                                             command.RequestDto.Address.PostalCode, command.RequestDto.Address.Country);

                // Add the contact
                merchantAggregate.AddContact(command.RequestDto.Contact.ContactName, command.RequestDto.Contact.PhoneNumber, command.RequestDto.Contact.EmailAddress);

                // Set the settlement schedule
                merchantAggregate.SetSettlementSchedule(settlementSchedule);
            }

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);

            return merchantId;
        }
        
        public async Task<Guid> CreateMerchantUser(MerchantCommands.CreateMerchantUserCommand command, CancellationToken cancellationToken) {
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            CreateUserRequest createUserRequest = new CreateUserRequest {
                                                                            EmailAddress = command.RequestDto.EmailAddress,
                                                                            FamilyName = command.RequestDto.FamilyName,
                                                                            GivenName = command.RequestDto.GivenName,
                                                                            MiddleName = command.RequestDto.MiddleName,
                                                                            Password = command.RequestDto.Password,
                                                                            PhoneNumber = "123456", // Is this really needed :|
                                                                            Roles = new List<String>(),
                                                                            Claims = new Dictionary<String, String>()
                                                                        };

            String merchantRoleName = Environment.GetEnvironmentVariable("MerchantRoleName");
            createUserRequest.Roles.Add(String.IsNullOrEmpty(merchantRoleName) ? "Merchant" : merchantRoleName);
            createUserRequest.Claims.Add("estateId", command.EstateId.ToString());
            createUserRequest.Claims.Add("merchantId", command.MerchantId.ToString());

            CreateUserResponse createUserResponse = await this.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken);

            // Add the user to the aggregate 
            validateResults.merchantAggregate.AddSecurityUser(createUserResponse.UserId, command.RequestDto.EmailAddress);

            // TODO: add a delete user here in case the aggregate add fails...

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);

            return createUserResponse.UserId;
        }

        public async Task<Guid> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken) {
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            MerchantDepositListAggregate merchantDepositListAggregate = await this.MerchantDepositListAggregateRepository.GetLatestVersion(command.MerchantId, cancellationToken);

            if (merchantDepositListAggregate.IsCreated == false) {
                merchantDepositListAggregate.Create(validateResults.merchantAggregate, command.RequestDto.DepositDateTime);
            }

            PositiveMoney amount = PositiveMoney.Create(Money.Create(command.RequestDto.Amount));

            merchantDepositListAggregate.MakeDeposit(command.DepositSource, command.RequestDto.Reference, command.RequestDto.DepositDateTime, amount);

            await this.MerchantDepositListAggregateRepository.SaveChanges(merchantDepositListAggregate, cancellationToken);

            List<Deposit> deposits = merchantDepositListAggregate.GetDeposits();

            // Find the deposit
            Deposit deposit = deposits.Single(d => d.Reference == command.RequestDto.Reference && d.DepositDateTime == command.RequestDto.DepositDateTime && d.Source == command.DepositSource && d.Amount == amount.Value);

            return deposit.DepositId;
        }

        public async Task<Guid> MakeMerchantWithdrawal(MerchantCommands.MakeMerchantWithdrawalCommand command,
                                                 CancellationToken cancellationToken) {
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            MerchantDepositListAggregate merchantDepositListAggregate = await this.MerchantDepositListAggregateRepository.GetLatestVersion(command.MerchantId, cancellationToken);

            if (merchantDepositListAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Merchant [{command.MerchantId}] has not made any deposits yet");
            }

            // Now we need to check the merchants balance to ensure they have funds to withdraw
            this.TokenResponse = await this.GetToken(cancellationToken);
            MerchantBalanceResponse merchantBalance = await this.TransactionProcessorClient.GetMerchantBalance(this.TokenResponse.AccessToken, command.EstateId, command.MerchantId, cancellationToken);

            if (command.RequestDto.Amount > merchantBalance.Balance) {
                throw new InvalidOperationException($"Not enough credit available for withdrawal of [{command.RequestDto.Amount}]. Balance is {merchantBalance}");
            }

            // If we are here we have enough credit to withdraw
            PositiveMoney amount = PositiveMoney.Create(Money.Create(command.RequestDto.Amount));

            merchantDepositListAggregate.MakeWithdrawal(command.RequestDto.WithdrawalDateTime, amount);

            await this.MerchantDepositListAggregateRepository.SaveChanges(merchantDepositListAggregate, cancellationToken);

            List<Withdrawal> withdrawals = merchantDepositListAggregate.GetWithdrawals();

            // Find the withdrawal
            Withdrawal withdrawal = withdrawals.Single(d => d.WithdrawalDateTime == command.RequestDto.WithdrawalDateTime && d.Amount == amount.Value);

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
        
        public async Task AddContractToMerchant(MerchantCommands.AddMerchantContractCommand command, CancellationToken cancellationToken){
            ContractAggregate contractAggregate = await this.ContractAggregateRepository.GetLatestVersion(command.RequestDto.ContractId, cancellationToken);
            if (contractAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Contract Id {command.RequestDto.ContractId} has not been created");
            }

            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.AddContract(contractAggregate);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task UpdateMerchant(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.UpdateMerchant(command.RequestDto.Name);

            Models.SettlementSchedule settlementSchedule = ConvertSettlementSchedule(command.RequestDto.SettlementSchedule);
            validateResults.merchantAggregate.SetSettlementSchedule(settlementSchedule);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task AddMerchantAddress(MerchantCommands.AddMerchantAddressCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.AddAddress(command.RequestDto.AddressLine1,
                                                         command.RequestDto.AddressLine2,
                                                         command.RequestDto.AddressLine3,
                                                         command.RequestDto.AddressLine4,
                                                         command.RequestDto.Town,
                                                         command.RequestDto.Region,
                                                         command.RequestDto.PostalCode,
                                                         command.RequestDto.Country);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task UpdateMerchantAddress(MerchantCommands.UpdateMerchantAddressCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.UpdateAddress(command.AddressId,
                                                         command.RequestDto.AddressLine1,
                                                         command.RequestDto.AddressLine2,
                                                         command.RequestDto.AddressLine3,
                                                         command.RequestDto.AddressLine4,
                                                         command.RequestDto.Town,
                                                         command.RequestDto.Region,
                                                         command.RequestDto.PostalCode,
                                                         command.RequestDto.Country);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task AddMerchantContact(MerchantCommands.AddMerchantContactCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.AddContact(command.RequestDto.ContactName,
                                                         command.RequestDto.PhoneNumber,
                                                         command.RequestDto.EmailAddress);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task UpdateMerchantContact(MerchantCommands.UpdateMerchantContactCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.UpdateContact(command.ContactId,
                                                            command.RequestDto.ContactName,
                                                            command.RequestDto.EmailAddress,
                                                         command.RequestDto.PhoneNumber
                                                         );

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.RemoveOperator(command.OperatorId);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        public async Task RemoveContractFromMerchant(MerchantCommands.RemoveMerchantContractCommand command, CancellationToken cancellationToken){
            (MerchantAggregate merchantAggregate, EstateAggregate estateAggregate) validateResults = await this.ValidateEstateAndMerchant(command.EstateId, command.MerchantId, cancellationToken);

            validateResults.merchantAggregate.RemoveContract(command.ContractId);

            await this.MerchantAggregateRepository.SaveChanges(validateResults.merchantAggregate, cancellationToken);
        }

        private async Task<(MerchantAggregate merchantAggregate, EstateAggregate estateAggregate)> ValidateEstateAndMerchant(Guid estateId, Guid merchantId, CancellationToken cancellationToken){
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

            return (merchantAggregate, estateAggregate);
        }

        public async Task SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command,
                                                   CancellationToken cancellationToken) {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(command.MerchantId, cancellationToken);

            // Check merchant has been created
            if (merchantAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Merchant Id {command.MerchantId} has not been created");
            }

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);
            if (estateAggregate.IsCreated == false) {
                throw new InvalidOperationException($"Estate Id {command.EstateId} has not been created");
            }
            
            merchantAggregate.SwapDevice(command.RequestDto.OriginalDeviceIdentifier, command.RequestDto.NewDeviceIdentifier);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }
        
        #endregion
    }
}
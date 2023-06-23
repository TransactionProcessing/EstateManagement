using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EstateManagement.Repository
{
    using System.Reflection;
    using Contract.DomainEvents;
    using Database.Contexts;
    using Database.Entities;
    using Database.ViewEntities;
    using Estate.DomainEvents;
    using FileProcessor.File.DomainEvents;
    using FileProcessor.FileImportLog.DomainEvents;
    using Merchant.DomainEvents;
    using MerchantStatement.DomainEvents;
    using Microsoft.EntityFrameworkCore;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.Exceptions;
    using Shared.Logger;
    using TransactionProcessor.Reconciliation.DomainEvents;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;

    public interface IEstateReportingRepository
    {
        #region Methods

        /// <summary>
        /// Adds the contract.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddContract(ContractCreatedEvent domainEvent,
                         CancellationToken cancellationToken);

        /// <summary>
        /// Adds the contract product.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddContractProduct(VariableValueProductAddedToContractEvent domainEvent,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Adds the contract product.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddContractProduct(FixedValueProductAddedToContractEvent domainEvent,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Adds the contract product transaction fee.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddContractProductTransactionFee(TransactionFeeForProductAddedToContractEvent domainEvent,
                                              CancellationToken cancellationToken);

        /// <summary>
        /// Adds the estate.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddEstate(EstateCreatedEvent domainEvent,
                       CancellationToken cancellationToken);

        /// <summary>
        /// Adds the estate operator.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddEstateOperator(OperatorAddedToEstateEvent domainEvent,
                               CancellationToken cancellationToken);

        /// <summary>
        /// Adds the estate security user.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddEstateSecurityUser(SecurityUserAddedToEstateEvent domainEvent,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Adds the fee details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFeeDetailsToTransaction(MerchantFeeAddedToTransactionEnrichedEvent domainEvent,
                                        CancellationToken cancellationToken);

        /// <summary>
        /// Adds the fee details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFeeDetailsToTransaction(ServiceProviderFeeAddedToTransactionEnrichedEvent domainEvent,
                                        CancellationToken cancellationToken);

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFile(FileCreatedEvent domainEvent,
                     CancellationToken cancellationToken);

        /// <summary>
        /// Adds the file import log.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFileImportLog(ImportLogCreatedEvent domainEvent,
                              CancellationToken cancellationToken);

        /// <summary>
        /// Adds the file line to file.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFileLineToFile(FileLineAddedEvent domainEvent,
                               CancellationToken cancellationToken);

        /// <summary>
        /// Adds the file to import log.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddFileToImportLog(FileAddedToImportLogEvent domainEvent,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Adds the generated voucher.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddGeneratedVoucher(VoucherGeneratedEvent domainEvent,
                                 CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchant(MerchantCreatedEvent domainEvent,
                         CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant address.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchantAddress(AddressAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant contact.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchantContact(ContactAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant device.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                               CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant operator.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchantOperator(OperatorAssignedToMerchantEvent domainEvent,
                                 CancellationToken cancellationToken);

        /// <summary>
        /// Adds the merchant security user.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMerchantSecurityUser(SecurityUserAddedToMerchantEvent domainEvent,
                                     CancellationToken cancellationToken);

        /// <summary>
        /// Adds the pending merchant fee to settlement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddPendingMerchantFeeToSettlement(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Adds the product details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Adds the settled fee to statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                      CancellationToken cancellationToken);

        /// <summary>
        /// Adds the settled merchant fee to settlement.
        /// </summary>
        /// <param name="settlementId">The settlement identifier.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddSettledMerchantFeeToSettlement(Guid settlementId,
                                               MerchantFeeAddedToTransactionEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                           CancellationToken cancellationToken);

        /// <summary>
        /// Adds the transaction to statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Completes the reconciliation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                    CancellationToken cancellationToken);

        /// <summary>
        /// Completes the transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                 CancellationToken cancellationToken);

        /// <summary>
        /// Creates the read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateReadModel(EstateCreatedEvent domainEvent,
                             CancellationToken cancellationToken);

        /// <summary>
        /// Creates the settlement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateSettlement(SettlementCreatedForDateEvent domainEvent,
                              CancellationToken cancellationToken);

        /// <summary>
        /// Creates the statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateStatement(StatementCreatedEvent domainEvent,
                             CancellationToken cancellationToken);

        /// <summary>
        /// Disables the contract product transaction fee.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task DisableContractProductTransactionFee(TransactionFeeForProductDisabledEvent domainEvent,
                                                  CancellationToken cancellationToken);

        /// <summary>
        /// Marks the merchant fee as settled.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task MarkMerchantFeeAsSettled(MerchantFeeSettledEvent domainEvent,
                                      CancellationToken cancellationToken);

        /// <summary>
        /// Marks the settlement as completed.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task MarkSettlementAsCompleted(SettlementCompletedEvent domainEvent,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Marks the statement as generated.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task MarkStatementAsGenerated(StatementGeneratedEvent domainEvent,
                                      CancellationToken cancellationToken);

        /// <summary>
        /// Records the transaction additional request data.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Records the transaction additional response data.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken);

        /// <summary>
        /// Starts the reconciliation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                 CancellationToken cancellationToken);

        /// <summary>
        /// Starts the transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task StartTransaction(TransactionHasStartedEvent domainEvent,
                              CancellationToken cancellationToken);

        /// <summary>
        /// Updates the estate.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateEstate(EstateReferenceAllocatedEvent domainEvent,
                          CancellationToken cancellationToken);

        /// <summary>
        /// Updates the file as complete.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateFileAsComplete(FileProcessingCompletedEvent domainEvent,
                                  CancellationToken cancellationToken);

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateMerchant(MerchantReferenceAllocatedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateMerchant(StatementGeneratedEvent domainEvent,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateMerchant(SettlementScheduleChangedEvent domainEvent,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the reconciliation overall totals.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateReconciliationOverallTotals(OverallTotalsRecordedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Updates the reconciliation status.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                        CancellationToken cancellationToken);

        /// <summary>
        /// Updates the reconciliation status.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                        CancellationToken cancellationToken);

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Updates the voucher issue details.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateVoucherIssueDetails(VoucherIssuedEvent domainEvent,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Updates the voucher redemption details.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateVoucherRedemptionDetails(VoucherFullyRedeemedEvent domainEvent,
                                            CancellationToken cancellationToken);

        #endregion
    }

    public class EstateReportingRepository : IEstateReportingRepository
    {
        #region Fields

        /// <summary>
        /// The additional request fields
        /// </summary>
        private readonly List<String> AdditionalRequestFields = new List<String>
                                                                {
                                                                    "Amount",
                                                                    "CustomerAccountNumber"
                                                                };

        /// <summary>
        /// The additional response fields
        /// </summary>
        private readonly List<String> AdditionalResponseFields = new List<String>();

        /// <summary>
        /// The database context factory
        /// </summary>
        private readonly Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> DbContextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateReportingRepository" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        public EstateReportingRepository(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> dbContextFactory)
        {
            this.DbContextFactory = dbContextFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the contract.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddContract(ContractCreatedEvent domainEvent,
                                      CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            Contract contract = new Contract
                                                 {
                                                     EstateReportingId = estate.EstateReportingId,
                                                     OperatorId = domainEvent.OperatorId,
                                                     ContractId = domainEvent.ContractId,
                                                     Description = domainEvent.Description
                                                 };

            await context.Contracts.AddAsync(contract, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the contract product.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddContractProduct(VariableValueProductAddedToContractEvent domainEvent,
                                             CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            ContractProduct contractProduct = new ContractProduct
            {
                ContractId = domainEvent.ContractId,
                ProductId = domainEvent.ProductId,
                DisplayText = domainEvent.DisplayText,
                ProductName = domainEvent.ProductName,
                Value = null,
                ProductType = domainEvent.ProductType
            };

            await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the contract product.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddContractProduct(FixedValueProductAddedToContractEvent domainEvent,
                                             CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            ContractProduct contractProduct = new ContractProduct
            {
                ContractId = domainEvent.ContractId,
                ProductId = domainEvent.ProductId,
                DisplayText = domainEvent.DisplayText,
                ProductName = domainEvent.ProductName,
                Value = domainEvent.Value,
                ProductType = domainEvent.ProductType
            };

            await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the contract product transaction fee.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddContractProductTransactionFee(TransactionFeeForProductAddedToContractEvent domainEvent,
                                                           CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            ContractProductTransactionFee contractProductTransactionFee = new ContractProductTransactionFee
            {
                ContractId = domainEvent.ContractId,
                ProductId = domainEvent.ProductId,
                Description = domainEvent.Description,
                Value = domainEvent.Value,
                TransactionFeeId = domainEvent.TransactionFeeId,
                CalculationType = domainEvent.CalculationType,
                IsEnabled = true,
                FeeType = domainEvent.FeeType
            };

            await context.ContractProductTransactionFees.AddAsync(contractProductTransactionFee, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the estate.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddEstate(EstateCreatedEvent domainEvent,
                                    CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            // Add the estate to the read model
            Estate estate = new Estate
                                             {
                                                 EstateId = domainEvent.EstateId,
                                                 Name = domainEvent.EstateName,
                                                 Reference = String.Empty
                                             };
            await context.Estates.AddAsync(estate, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the estate operator.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddEstateOperator(OperatorAddedToEstateEvent domainEvent,
                                            CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            EstateOperator estateOperator = new EstateOperator
            {
                EstateReportingId = estate.EstateReportingId,
                Name = domainEvent.Name,
                OperatorId = domainEvent.OperatorId,
                RequireCustomMerchantNumber = domainEvent.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = domainEvent.RequireCustomTerminalNumber
            };

            await context.EstateOperators.AddAsync(estateOperator, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task<EstateManagementGenericContext> GetContextFromDomainEvent(IDomainEvent domainEvent, CancellationToken cancellationToken){
            Guid estateId = DomainEventHelper.GetEstateId(domainEvent);
            if (estateId == Guid.Empty){
                throw new Exception($"Unable to resolve context for Domain Event {domainEvent.GetType()}");
            }
            return await this.DbContextFactory.GetContext(estateId, ConnectionStringIdentifier, cancellationToken);
        }

        /// <summary>
        /// Adds the estate security user.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddEstateSecurityUser(SecurityUserAddedToEstateEvent domainEvent,
                                                CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            EstateSecurityUser estateSecurityUser = new EstateSecurityUser
            {
                EstateReportingId = estate.EstateReportingId,
                EmailAddress = domainEvent.EmailAddress,
                SecurityUserId = domainEvent.SecurityUserId,
                CreatedDateTime = domainEvent.EventTimestamp.DateTime
            };

            await context.EstateSecurityUsers.AddAsync(estateSecurityUser, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task<Estate> LoadEstate(EstateManagementGenericContext context, IDomainEvent domainEvent){
            Guid estateId = DomainEventHelper.GetEstateId(domainEvent);
            return await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId);
        }

        /// <summary>
        /// Adds the fee details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task AddFeeDetailsToTransaction(MerchantFeeAddedToTransactionEnrichedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            TransactionFee transactionFee = new TransactionFee
            {
                FeeId = domainEvent.FeeId,
                CalculatedValue = domainEvent.CalculatedValue,
                CalculationType = domainEvent.FeeCalculationType,
                EventId = domainEvent.EventId,
                FeeType = 0,
                FeeValue = domainEvent.FeeValue,
                TransactionId = domainEvent.TransactionId
            };

            await context.TransactionFees.AddAsync(transactionFee, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the fee details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task AddFeeDetailsToTransaction(ServiceProviderFeeAddedToTransactionEnrichedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            Guid estateId = domainEvent.EstateId;

            EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId, ConnectionStringIdentifier, cancellationToken);

            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            TransactionFee transactionFee = new TransactionFee
            {
                FeeId = domainEvent.FeeId,
                CalculatedValue = domainEvent.CalculatedValue,
                CalculationType = domainEvent.FeeCalculationType,
                EventId = domainEvent.EventId,
                FeeType = 1,
                FeeValue = domainEvent.FeeValue,
                TransactionId = domainEvent.TransactionId
            };

            await context.TransactionFees.AddAsync(transactionFee, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddFile(FileCreatedEvent domainEvent,
                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            File file = new File
            {
                EstateReportingId = estate.EstateReportingId,
                MerchantId = domainEvent.MerchantId,
                FileImportLogId = domainEvent.FileImportLogId,
                UserId = domainEvent.UserId,
                FileId = domainEvent.FileId,
                FileProfileId = domainEvent.FileProfileId,
                FileLocation = domainEvent.FileLocation,
                FileReceivedDateTime = domainEvent.FileReceivedDateTime,
            };

            await context.Files.AddAsync(file, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the file import log.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddFileImportLog(ImportLogCreatedEvent domainEvent,
                                           CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            FileImportLog fileImportLog = new FileImportLog
            {
                EstateReportingId = estate.EstateReportingId,
                FileImportLogId = domainEvent.FileImportLogId,
                ImportLogDateTime = domainEvent.ImportLogDateTime
            };

            await context.FileImportLogs.AddAsync(fileImportLog, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the file line to file.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">File with Id {domainEvent.FileId} not found for estate Id {estateId}</exception>
        /// <exception cref="NotFoundException">File with Id {domainEvent.FileId} not found for estate Id {estateId}</exception>
        public async Task AddFileLineToFile(FileLineAddedEvent domainEvent,
                                            CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            File file = await context.Files.SingleOrDefaultAsync(f => f.FileId == domainEvent.FileId, cancellationToken:cancellationToken);

            if (file == null)
            {
                throw new NotFoundException($"File with Id {domainEvent.FileId} not found`");
            }

            FileLine fileLine = new FileLine
            {
                FileId = domainEvent.FileId,
                LineNumber = domainEvent.LineNumber,
                FileLineData = domainEvent.FileLine,
                Status = "P" // Pending
            };

            await context.FileLines.AddAsync(fileLine, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the file to import log.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Import log with Id {domainEvent.FileImportLogId} not found for estate Id {estateId}</exception>
        /// <exception cref="NotFoundException">Import log with Id {domainEvent.FileImportLogId} not found for estate Id {estateId}</exception>
        public async Task AddFileToImportLog(FileAddedToImportLogEvent domainEvent,
                                             CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            FileImportLog fileImportLog = await context.FileImportLogs.SingleOrDefaultAsync(f => f.FileImportLogId == domainEvent.FileImportLogId);

            if (fileImportLog == null)
            {
                throw new NotFoundException($"Import log with Id {domainEvent.FileImportLogId} not found");
            }

            FileImportLogFile fileImportLogFile = new FileImportLogFile
            {
                MerchantId = domainEvent.MerchantId,
                FileImportLogId = domainEvent.FileImportLogId,
                FileId = domainEvent.FileId,
                FilePath = domainEvent.FilePath,
                FileProfileId = domainEvent.FileProfileId,
                FileUploadedDateTime = domainEvent.FileUploadedDateTime,
                OriginalFileName = domainEvent.OriginalFileName,
                UserId = domainEvent.UserId
            };

            await context.FileImportLogFiles.AddAsync(fileImportLogFile, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the generated voucher.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddGeneratedVoucher(VoucherGeneratedEvent domainEvent,
                                              CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Voucher voucher = new Voucher
            {
                ExpiryDate = domainEvent.ExpiryDateTime,
                IsGenerated = true,
                IsIssued = false,
                OperatorIdentifier = domainEvent.OperatorIdentifier,
                Value = domainEvent.Value,
                VoucherCode = domainEvent.VoucherCode,
                VoucherId = domainEvent.VoucherId,
                TransactionId = domainEvent.TransactionId,
                GenerateDateTime = domainEvent.GeneratedDateTime
            };

            await context.Vouchers.AddAsync(voucher, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchant(MerchantCreatedEvent domainEvent,
                                      CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            Merchant merchant = new Merchant
                                                 {
                                                     EstateReportingId = estate.EstateReportingId,
                                                     MerchantId = domainEvent.MerchantId,
                                                     Name = domainEvent.MerchantName,
                                                     CreatedDateTime = domainEvent.DateCreated,
                                                     LastStatementGenerated = DateTime.MinValue,
                                                     SettlementSchedule = 0
                                                 };

            await context.Merchants.AddAsync(merchant, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant address.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchantAddress(AddressAddedEvent domainEvent,
                                             CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            MerchantAddress merchantAddress = new MerchantAddress
            {
                MerchantId = domainEvent.MerchantId,
                AddressId = domainEvent.AddressId,
                AddressLine1 = domainEvent.AddressLine1,
                AddressLine2 = domainEvent.AddressLine2,
                AddressLine3 = domainEvent.AddressLine3,
                AddressLine4 = domainEvent.AddressLine4,
                Country = domainEvent.Country,
                PostalCode = domainEvent.PostalCode,
                Region = domainEvent.Region,
                Town = domainEvent.Town
            };

            await context.MerchantAddresses.AddAsync(merchantAddress, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant contact.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchantContact(ContactAddedEvent domainEvent,
                                             CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            MerchantContact merchantContact = new MerchantContact
            {
                MerchantId = domainEvent.MerchantId,
                Name = domainEvent.ContactName,
                ContactId = domainEvent.ContactId,
                EmailAddress = domainEvent.ContactEmailAddress,
                PhoneNumber = domainEvent.ContactPhoneNumber
            };

            await context.MerchantContacts.AddAsync(merchantContact, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant device.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                                            CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            MerchantDevice merchantDevice = new MerchantDevice
            {
                MerchantId = domainEvent.MerchantId,
                DeviceId = domainEvent.DeviceId,
                DeviceIdentifier = domainEvent.DeviceIdentifier
            };

            await context.MerchantDevices.AddAsync(merchantDevice, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant operator.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchantOperator(OperatorAssignedToMerchantEvent domainEvent,
                                              CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            MerchantOperator merchantOperator = new MerchantOperator
            {
                Name = domainEvent.Name,
                MerchantId = domainEvent.MerchantId,
                MerchantNumber = domainEvent.MerchantNumber,
                OperatorId = domainEvent.OperatorId,
                TerminalNumber = domainEvent.TerminalNumber
            };

            await context.MerchantOperators.AddAsync(merchantOperator, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the merchant security user.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddMerchantSecurityUser(SecurityUserAddedToMerchantEvent domainEvent,
                                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            MerchantSecurityUser merchantSecurityUser = new MerchantSecurityUser
            {
                MerchantId = domainEvent.MerchantId,
                EmailAddress = domainEvent.EmailAddress,
                SecurityUserId = domainEvent.SecurityUserId
            };

            await context.MerchantSecurityUsers.AddAsync(merchantSecurityUser, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the pending merchant fee to settlement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddPendingMerchantFeeToSettlement(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
            {
                SettlementId = domainEvent.SettlementId,
                CalculatedValue = domainEvent.CalculatedValue,
                FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
                FeeId = domainEvent.FeeId,
                FeeValue = domainEvent.FeeValue,
                IsSettled = false,
                MerchantId = domainEvent.MerchantId,
                TransactionId = domainEvent.TransactionId
            };
            await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the product details to transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.ContractId = domainEvent.ContractId;
            transaction.ProductId = domainEvent.ProductId;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the settled fee to statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id {domainEvent.TransactionId} not found for estate Id {estateId}</exception>
        public async Task AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                                   CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            // Find the corresponding transaction
            TransactionsView transaction = await context.TransactionsView.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id {domainEvent.TransactionId} not found");
            }

            StatementLine line = new StatementLine
            {
                MerchantId = domainEvent.MerchantId,
                StatementId = domainEvent.MerchantStatementId,
                ActivityDateTime = domainEvent.SettledDateTime,
                ActivityDescription = $"{transaction.OperatorIdentifier} Transaction Fee",
                ActivityType = 2, // Transaction Fee
                TransactionId = domainEvent.TransactionId,
                InAmount = domainEvent.SettledValue
            };

            await context.StatementLines.AddAsync(line, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the settled merchant fee to settlement.
        /// </summary>
        /// <param name="settlementId">The settlement identifier.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AddSettledMerchantFeeToSettlement(Guid settlementId,
                                                            MerchantFeeAddedToTransactionEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
            {
                SettlementId = settlementId,
                CalculatedValue = domainEvent.CalculatedValue,
                FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
                FeeId = domainEvent.FeeId,
                FeeValue = domainEvent.FeeValue,
                IsSettled = true,
                MerchantId = domainEvent.MerchantId,
                TransactionId = domainEvent.TransactionId
            };
            await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.TransactionSource = domainEvent.TransactionSource;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the transaction to statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id {domainEvent.TransactionId} not found for estate Id {estateId}</exception>
        public async Task AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                                    CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            // Find the corresponding transaction
            TransactionsView transaction = await context.TransactionsView.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id {domainEvent.TransactionId} not found");
            }

            StatementLine line = new StatementLine
            {
                MerchantId = domainEvent.MerchantId,
                StatementId = domainEvent.MerchantStatementId,
                ActivityDateTime = domainEvent.TransactionDateTime,
                ActivityDescription = $"{transaction.OperatorIdentifier} Transaction",
                ActivityType = 1, // Transaction
                TransactionId = domainEvent.TransactionId,
                OutAmount = domainEvent.TransactionValue
            };

            await context.StatementLines.AddAsync(line, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Completes the reconciliation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                                 CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Reconciliation reconciliation =
                await context.Reconciliations.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (reconciliation == null)
            {
                throw new NotFoundException($"Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            reconciliation.IsCompleted = true;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Completes the transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                              CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.IsCompleted = true;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Creates the read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CreateReadModel(EstateCreatedEvent domainEvent,
                                          CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Logger.LogInformation($"About to run migrations on Read Model database for estate [{domainEvent.EstateId}]");

            // Ensure the db is at the latest version
            await context.MigrateAsync(cancellationToken);

            Logger.LogInformation($"Read Model database for estate [{domainEvent.EstateId}] migrated to latest version");
        }

        /// <summary>
        /// Creates the settlement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CreateSettlement(SettlementCreatedForDateEvent domainEvent,
                                           CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);

            Settlement settlement = new Settlement
            {
                EstateReportingId = estate.EstateReportingId,
                IsCompleted = false,
                SettlementDate = domainEvent.SettlementDate.Date,
                SettlementId = domainEvent.SettlementId
            };

            await context.Settlements.AddAsync(settlement, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Creates the statement.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CreateStatement(StatementCreatedEvent domainEvent,
                                          CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            StatementHeader header = new StatementHeader
            {
                MerchantId = domainEvent.MerchantId,
                StatementCreatedDate = domainEvent.DateCreated,
                StatementId = domainEvent.MerchantStatementId
            };

            await context.StatementHeaders.AddAsync(header, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Disables the contract product transaction fee.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction Fee with Id [{domainEvent.TransactionFeeId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction Fee with Id [{domainEvent.TransactionFeeId}] not found in the Read Model</exception>
        public async Task DisableContractProductTransactionFee(TransactionFeeForProductDisabledEvent domainEvent,
                                                               CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            ContractProductTransactionFee transactionFee =
                await context.ContractProductTransactionFees.SingleOrDefaultAsync(t => t.TransactionFeeId == domainEvent.TransactionFeeId, cancellationToken:cancellationToken);

            if (transactionFee == null)
            {
                throw new NotFoundException($"Transaction Fee with Id [{domainEvent.TransactionFeeId}] not found in the Read Model");
            }

            transactionFee.IsEnabled = false;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Marks the merchant fee as settled.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Merchant Fee not found to update as settled</exception>
        public async Task MarkMerchantFeeAsSettled(MerchantFeeSettledEvent domainEvent,
                                                   CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            MerchantSettlementFee merchantFee = await context.MerchantSettlementFees.Where(m =>
                                                                                               m.MerchantId == domainEvent.MerchantId && m.TransactionId == domainEvent.TransactionId &&
                                                                                               m.SettlementId == domainEvent.SettlementId && m.FeeId == domainEvent.FeeId)
                                                             .SingleOrDefaultAsync(cancellationToken);
            if (merchantFee == null)
            {
                throw new NotFoundException("Merchant Fee not found to update as settled");
            }

            merchantFee.IsSettled = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Marks the settlement as completed.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">No settlement with Id {domainEvent.SettlementId} found to mark as completed</exception>
        public async Task MarkSettlementAsCompleted(SettlementCompletedEvent domainEvent,
                                                    CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Settlement settlement = await context.Settlements.SingleOrDefaultAsync(s => s.SettlementId == domainEvent.SettlementId, cancellationToken);

            if (settlement == null)
            {
                throw new NotFoundException($"No settlement with Id {domainEvent.SettlementId} found to mark as completed");
            }

            settlement.IsCompleted = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Marks the statement as generated.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">No statement with Id {domainEvent.MerchantStatementId} found to mark as generated</exception>
        public async Task MarkStatementAsGenerated(StatementGeneratedEvent domainEvent,
                                                   CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            StatementHeader statementHeader = await context.StatementHeaders.SingleOrDefaultAsync(s => s.StatementId == domainEvent.MerchantStatementId, cancellationToken);

            if (statementHeader == null)
            {
                throw new NotFoundException($"No statement with Id {domainEvent.MerchantStatementId} found to mark as generated");
            }

            statementHeader.StatementGeneratedDate = domainEvent.DateGenerated;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Records the transaction additional request data.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                                 CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            TransactionAdditionalRequestData additionalRequestData = new TransactionAdditionalRequestData
            {
                MerchantId = domainEvent.MerchantId,
                TransactionId = domainEvent.TransactionId
            };

            foreach (String additionalRequestField in this.AdditionalRequestFields)
            {
                Logger.LogInformation($"Field to look for [{additionalRequestField}]");
            }

            foreach (KeyValuePair<String, String> additionalRequestField in domainEvent.AdditionalTransactionRequestMetadata)
            {
                Logger.LogInformation($"Key: [{additionalRequestField.Key}] Value: [{additionalRequestField.Value}]");
            }

            foreach (String additionalRequestField in this.AdditionalRequestFields)
            {
                if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower()))
                {
                    Type dbTableType = additionalRequestData.GetType();
                    PropertyInfo propertyInfo = dbTableType.GetProperty(additionalRequestField);

                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(additionalRequestData,
                                              domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value);
                    }
                    else
                    {
                        Logger.LogInformation("propertyInfo == null");
                    }
                }
            }

            await context.TransactionsAdditionalRequestData.AddAsync(additionalRequestData, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Records the transaction additional response data.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            TransactionAdditionalResponseData additionalResponseData = new TransactionAdditionalResponseData
            {
                MerchantId = domainEvent.MerchantId,
                TransactionId = domainEvent.TransactionId
            };

            foreach (String additionalResponseField in this.AdditionalResponseFields)
            {
                if (domainEvent.AdditionalTransactionResponseMetadata.Any(m => m.Key.ToLower() == additionalResponseField.ToLower()))
                {
                    Type dbTableType = additionalResponseData.GetType();
                    PropertyInfo propertyInfo = dbTableType.GetProperty(additionalResponseField);

                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(additionalResponseData,
                                              domainEvent.AdditionalTransactionResponseMetadata.Single(m => m.Key.ToLower() == additionalResponseField.ToLower()).Value);
                    }
                }
            }

            await context.TransactionsAdditionalResponseData.AddAsync(additionalResponseData, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Starts the reconciliation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                              CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Reconciliation reconciliation = new Reconciliation
            {
                MerchantId = domainEvent.MerchantId,
                TransactionDate = domainEvent.TransactionDateTime.Date,
                TransactionDateTime = domainEvent.TransactionDateTime,
                TransactionTime = domainEvent.TransactionDateTime.TimeOfDay,
                TransactionId = domainEvent.TransactionId,
            };

            await context.Reconciliations.AddAsync(reconciliation, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Starts the transaction.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task StartTransaction(TransactionHasStartedEvent domainEvent,
                                           CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction = new Transaction
            {
                MerchantId = domainEvent.MerchantId,
                TransactionDate = domainEvent.TransactionDateTime.Date,
                TransactionDateTime = domainEvent.TransactionDateTime,
                TransactionTime = domainEvent.TransactionDateTime.TimeOfDay,
                TransactionId = domainEvent.TransactionId,
                TransactionNumber = domainEvent.TransactionNumber,
                TransactionReference = domainEvent.TransactionReference,
                TransactionType = domainEvent.TransactionType,
                DeviceIdentifier = domainEvent.DeviceIdentifier
            };

            await context.Transactions.AddAsync(transaction, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the estate.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Estate not found with Id {domainEvent.EstateId}</exception>
        public async Task UpdateEstate(EstateReferenceAllocatedEvent domainEvent,
                                       CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Estate estate = await LoadEstate(context, domainEvent);
            
            if (estate == null)
            {
                throw new NotFoundException($"Estate not found with Id {domainEvent.EstateId}");
            }

            estate.Reference = domainEvent.EstateReference;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the file as complete.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">File Id {domainEvent.FileId} not found for estate Id {domainEvent.EstateId}</exception>
        public async Task UpdateFileAsComplete(FileProcessingCompletedEvent domainEvent,
                                               CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            File file = await context.Files.SingleOrDefaultAsync(f => f.FileId == domainEvent.FileId);

            if (file == null)
            {
                throw new NotFoundException($"File Id {domainEvent.FileId} not found for estate Id {domainEvent.EstateId}");
            }

            file.IsCompleted = true;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            await this.UpdateFileLineStatus(context, domainEvent.FileId, domainEvent.LineNumber,
                                            domainEvent.TransactionId, "S", cancellationToken);
        }

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            await this.UpdateFileLineStatus(context, domainEvent.FileId, domainEvent.LineNumber,
                                            domainEvent.TransactionId, "F", cancellationToken);
        }

        /// <summary>
        /// Updates the file line.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            await this.UpdateFileLineStatus(context, domainEvent.FileId, domainEvent.LineNumber,
                                            Guid.Empty, "I", cancellationToken);
        }

        /// <summary>
        /// Updates the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Merchant not found with Id {domainEvent.MerchantId}</exception>
        public async Task UpdateMerchant(StatementGeneratedEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Merchant merchant = await context.Merchants.SingleOrDefaultAsync(m => m.MerchantId == domainEvent.MerchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with Id {domainEvent.MerchantId}");
            }

            if (merchant.LastStatementGenerated > domainEvent.DateGenerated)
                return;

            merchant.LastStatementGenerated = domainEvent.DateGenerated;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Merchant not found with Id {domainEvent.MerchantId}</exception>
        public async Task UpdateMerchant(SettlementScheduleChangedEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Merchant merchant = await context.Merchants.SingleOrDefaultAsync(m => m.MerchantId == domainEvent.MerchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with Id {domainEvent.MerchantId}");
            }

            merchant.SettlementSchedule = domainEvent.SettlementSchedule;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the merchant.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Merchant not found with Id {domainEvent.MerchantId}</exception>
        public async Task UpdateMerchant(MerchantReferenceAllocatedEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Merchant merchant = await context.Merchants.SingleOrDefaultAsync(m => m.MerchantId == domainEvent.MerchantId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with Id {domainEvent.MerchantId}");
            }

            merchant.Reference = domainEvent.MerchantReference;
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the reconciliation overall totals.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateReconciliationOverallTotals(OverallTotalsRecordedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guid estateId = domainEvent.EstateId;

            EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId, ConnectionStringIdentifier, cancellationToken);

            Reconciliation reconciliation =
                await context.Reconciliations.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (reconciliation == null)
            {
                throw new NotFoundException($"Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            reconciliation.TransactionCount = domainEvent.TransactionCount;
            reconciliation.TransactionValue = domainEvent.TransactionValue;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the reconciliation status.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Reconciliation reconciliation =
                await context.Reconciliations.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (reconciliation == null)
            {
                throw new NotFoundException($"Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            reconciliation.IsAuthorised = true;
            reconciliation.ResponseCode = domainEvent.ResponseCode;
            reconciliation.ResponseMessage = domainEvent.ResponseMessage;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the reconciliation status.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Reconciliation reconciliation =
                await context.Reconciliations.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (reconciliation == null)
            {
                throw new NotFoundException($"Reconciliation with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            reconciliation.IsAuthorised = false;
            reconciliation.ResponseCode = domainEvent.ResponseCode;
            reconciliation.ResponseMessage = domainEvent.ResponseMessage;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.IsAuthorised = true;
            transaction.ResponseCode = domainEvent.ResponseCode;
            transaction.AuthorisationCode = domainEvent.AuthorisationCode;
            transaction.ResponseMessage = domainEvent.ResponseMessage;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.IsAuthorised = false;
            transaction.ResponseCode = domainEvent.ResponseCode;
            transaction.ResponseMessage = domainEvent.ResponseMessage;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.IsAuthorised = true;
            transaction.ResponseCode = domainEvent.ResponseCode;
            transaction.AuthorisationCode = domainEvent.AuthorisationCode;
            transaction.ResponseMessage = domainEvent.ResponseMessage;
            transaction.OperatorIdentifier = domainEvent.OperatorIdentifier;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the transaction authorisation.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model</exception>
        public async Task UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Transaction transaction =
                await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == domainEvent.TransactionId, cancellationToken: cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.IsAuthorised = false;
            transaction.ResponseCode = domainEvent.ResponseCode;
            transaction.ResponseMessage = domainEvent.ResponseMessage;
            transaction.OperatorIdentifier = domainEvent.OperatorIdentifier;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the voucher issue details.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model</exception>
        public async Task UpdateVoucherIssueDetails(VoucherIssuedEvent domainEvent,
                                                    CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(v => v.VoucherId == domainEvent.VoucherId);

            if (voucher == null)
            {
                throw new NotFoundException($"Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model");
            }

            voucher.IsIssued = true;
            voucher.RecipientEmail = domainEvent.RecipientEmail;
            voucher.RecipientMobile = domainEvent.RecipientMobile;
            voucher.IssuedDateTime = domainEvent.IssuedDateTime;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the voucher redemption details.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model</exception>
        /// <exception cref="NotFoundException">Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model</exception>
        public async Task UpdateVoucherRedemptionDetails(VoucherFullyRedeemedEvent domainEvent,
                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);
            
            Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(v => v.VoucherId == domainEvent.VoucherId);

            if (voucher == null)
            {
                throw new NotFoundException($"Voucher with Id [{domainEvent.VoucherId}] not found in the Read Model");
            }

            voucher.IsRedeemed = true;
            voucher.RedeemedDateTime = domainEvent.RedeemedDateTime;

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the file line status.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="newStatus">The new status.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Shared.Exceptions.NotFoundException">FileLine number {lineNumber} in File Id {fileId} not found for estate Id {estateId}</exception>
        /// <exception cref="NotFoundException">FileLine number {lineNumber} in File Id {fileId} not found for estate Id {estateId}</exception>
        private async Task UpdateFileLineStatus(EstateManagementGenericContext context,
                                                Guid fileId,
                                                Int32 lineNumber,
                                                Guid transactionId,
                                                String newStatus,
                                                CancellationToken cancellationToken)
        {
            FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(f => f.FileId == fileId && f.LineNumber == lineNumber);

            if (fileLine == null)
            {
                throw new NotFoundException($"FileLine number {lineNumber} in File Id {fileId} not found");
            }

            fileLine.Status = newStatus;
            fileLine.TransactionId = transactionId;

            await context.SaveChangesAsync(cancellationToken);
        }

        private const String ConnectionStringIdentifier = "EstateReportingReadModel";

        #endregion
    }

    public static class DomainEventHelper
    {
        public static Boolean HasProperty(IDomainEvent domainEvent,
                                          String propertyName)
        {
            PropertyInfo propertyInfo = domainEvent.GetType()
                                                   .GetProperties()
                                                   .SingleOrDefault(p => p.Name == propertyName);

            return propertyInfo != null;
        }

        public static T GetPropertyIgnoreCase<T>(IDomainEvent domainEvent, String propertyName)
        {
            try
            {
                var f = domainEvent.GetType()
                                   .GetProperties()
                                   .SingleOrDefault(p => String.Compare(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) == 0);

                if (f != null)
                {
                    return (T)f.GetValue(domainEvent);
                }
            }
            catch
            {
                // ignored
            }

            return default(T);
        }

        public static T GetProperty<T>(IDomainEvent domainEvent, String propertyName)
        {
            try
            {
                var f = domainEvent.GetType()
                                   .GetProperties()
                                   .SingleOrDefault(p => p.Name == propertyName);

                if (f != null)
                {
                    return (T)f.GetValue(domainEvent);
                }
            }
            catch
            {
                // ignored
            }

            return default(T);
        }

        public static Guid GetEstateId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "EstateId");
    }
}

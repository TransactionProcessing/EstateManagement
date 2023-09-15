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
        
        Task AddFile(FileCreatedEvent domainEvent,
                     CancellationToken cancellationToken);

        Task AddFileImportLog(ImportLogCreatedEvent domainEvent,
                              CancellationToken cancellationToken);

        Task AddFileLineToFile(FileLineAddedEvent domainEvent,
                               CancellationToken cancellationToken);

        Task AddFileToImportLog(FileAddedToImportLogEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddGeneratedVoucher(VoucherGeneratedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task AddMerchant(MerchantCreatedEvent domainEvent,
                         CancellationToken cancellationToken);

        Task AddMerchantAddress(AddressAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddMerchantContact(ContactAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                               CancellationToken cancellationToken);

        Task AddMerchantOperator(OperatorAssignedToMerchantEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task AddMerchantSecurityUser(SecurityUserAddedToMerchantEvent domainEvent,
                                     CancellationToken cancellationToken);

        Task AddPendingMerchantFeeToSettlement(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task AddSettledMerchantFeeToSettlement(Guid settlementId,
                                               MerchantFeeAddedToTransactionEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                           CancellationToken cancellationToken);

        Task AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task CreateReadModel(EstateCreatedEvent domainEvent,
                             CancellationToken cancellationToken);

        Task CreateSettlement(SettlementCreatedForDateEvent domainEvent,
                              CancellationToken cancellationToken);

        Task CreateStatement(StatementCreatedEvent domainEvent,
                             CancellationToken cancellationToken);

        Task DisableContractProductTransactionFee(TransactionFeeForProductDisabledEvent domainEvent,
                                                  CancellationToken cancellationToken);

        Task MarkMerchantFeeAsSettled(MerchantFeeSettledEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task MarkSettlementAsCompleted(SettlementCompletedEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task MarkSettlementAsProcessingStarted(SettlementProcessingStartedEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task MarkStatementAsGenerated(StatementGeneratedEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken);

        Task StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task StartTransaction(TransactionHasStartedEvent domainEvent,
                              CancellationToken cancellationToken);

        Task UpdateEstate(EstateReferenceAllocatedEvent domainEvent,
                          CancellationToken cancellationToken);

        Task UpdateFileAsComplete(FileProcessingCompletedEvent domainEvent,
                                  CancellationToken cancellationToken);

        Task UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateMerchant(MerchantReferenceAllocatedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateMerchant(StatementGeneratedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateMerchant(SettlementScheduleChangedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateMerchant(TransactionHasBeenCompletedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateReconciliationOverallTotals(OverallTotalsRecordedEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task UpdateReconciliationStatus(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task UpdateVoucherIssueDetails(VoucherIssuedEvent domainEvent,
                                       CancellationToken cancellationToken);

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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Contract contract = await LoadContract(context, domainEvent);

            ContractProduct contractProduct = new ContractProduct
            {
                ContractReportingId = contract.ContractReportingId,
                ProductId = domainEvent.ProductId,
                DisplayText = domainEvent.DisplayText,
                ProductName = domainEvent.ProductName,
                Value = null,
                ProductType = domainEvent.ProductType
            };

            await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Contract contract = await LoadContract(context, domainEvent);

            ContractProduct contractProduct = new ContractProduct
            {
                ContractReportingId = contract.ContractReportingId,
                ProductId = domainEvent.ProductId,
                DisplayText = domainEvent.DisplayText,
                ProductName = domainEvent.ProductName,
                Value = domainEvent.Value,
                ProductType = domainEvent.ProductType
            };

            await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Contract contract = await LoadContract(context, domainEvent);
            ContractProduct contractProduct = await LoadContractProduct(context, domainEvent);

            ContractProductTransactionFee contractProductTransactionFee = new ContractProductTransactionFee
            {
                ContractProductReportingId = contractProduct.ContractProductReportingId,
                Description = domainEvent.Description,
                Value = domainEvent.Value,
                TransactionFeeId = domainEvent.TransactionFeeId,
                CalculationType = domainEvent.CalculationType,
                IsEnabled = true,
                FeeType = domainEvent.FeeType
            };

            await context.ContractProductTransactionFees.AddAsync(contractProductTransactionFee, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
        }

        private async Task<Estate> LoadEstate(EstateManagementGenericContext context, IDomainEvent domainEvent){
            Guid estateId = DomainEventHelper.GetEstateId(domainEvent);
            return await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId);
        }

        private async Task<Merchant> LoadMerchant(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid merchantId = DomainEventHelper.GetMerchantId(domainEvent);
            return await context.Merchants.SingleOrDefaultAsync(e => e.MerchantId == merchantId);
        }

        private async Task<Contract> LoadContract(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid contractId = DomainEventHelper.GetContractId(domainEvent);
            return await context.Contracts.SingleOrDefaultAsync(e => e.ContractId== contractId);
        }

        private async Task<ContractProduct> LoadContractProduct(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid contractProductId = DomainEventHelper.GetContractProductId(domainEvent);
            return await context.ContractProducts.SingleOrDefaultAsync(e => e.ProductId == contractProductId);
        }

        private async Task<FileImportLog> LoadFileImportLog(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid fileImportLogId = DomainEventHelper.GetFileImportLogId(domainEvent);
            return await context.FileImportLogs.SingleOrDefaultAsync(e => e.FileImportLogId == fileImportLogId);
        }

        private async Task<File> LoadFile(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid fileId = DomainEventHelper.GetFileId(domainEvent);
            return await context.Files.SingleOrDefaultAsync(e => e.FileId == fileId);
        }

        private async Task<ContractProductTransactionFee> LoadContractProductTransactionFee(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid contractProductTransactionFeeId = DomainEventHelper.GetContractProductTransactionFeeId(domainEvent);
            return await context.ContractProductTransactionFees.SingleOrDefaultAsync(e => e.TransactionFeeId == contractProductTransactionFeeId);
        }

        private async Task<Transaction> LoadTransaction(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid transactionId = DomainEventHelper.GetTransactionId(domainEvent);
            return await context.Transactions.SingleOrDefaultAsync(e => e.TransactionId == transactionId);
        }

        private async Task<Settlement> LoadSettlement(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid settlementId = DomainEventHelper.GetSettlementId(domainEvent);
            return await context.Settlements.SingleOrDefaultAsync(e => e.SettlementId == settlementId);
        }

        private async Task<StatementHeader> LoadStatementHeader(EstateManagementGenericContext context, IDomainEvent domainEvent)
        {
            Guid statementHeaderId = DomainEventHelper.GetStatementHeaderId(domainEvent);
            return await context.StatementHeaders.SingleOrDefaultAsync(e => e.StatementId == statementHeaderId);
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

            Merchant merchant = await this.LoadMerchant(context, domainEvent);

            FileImportLog fileImportLog = await this.LoadFileImportLog(context, domainEvent);

            File file = new File
            {
                EstateReportingId = estate.EstateReportingId,
                MerchantReportingId = merchant.MerchantReportingId,
                FileImportLogReportingId = fileImportLog.FileImportLogReportingId,
                UserId = domainEvent.UserId,
                FileId = domainEvent.FileId,
                FileProfileId = domainEvent.FileProfileId,
                FileLocation = domainEvent.FileLocation,
                FileReceivedDateTime = domainEvent.FileReceivedDateTime,
                FileReceivedDate = domainEvent.FileReceivedDateTime.Date
            };

            await context.Files.AddAsync(file, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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
                ImportLogDateTime = domainEvent.ImportLogDateTime,
                ImportLogDate = domainEvent.ImportLogDateTime.Date
            };

            await context.FileImportLogs.AddAsync(fileImportLog, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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
                FileReportingId = file.FileReportingId,
                LineNumber = domainEvent.LineNumber,
                FileLineData = domainEvent.FileLine,
                Status = "P" // Pending
            };

            await context.FileLines.AddAsync(fileLine, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            FileImportLog fileImportLog = await this.LoadFileImportLog(context, domainEvent);

            File file = await this.LoadFile(context, domainEvent);

            if (fileImportLog == null)
            {
                throw new NotFoundException($"Import log with Id {domainEvent.FileImportLogId} not found");
            }

            FileImportLogFile fileImportLogFile = new FileImportLogFile
            {
                MerchantReportingId = merchant.MerchantReportingId,
                FileImportLogReportingId = fileImportLog.FileImportLogReportingId,
                FileReportingId = file.FileReportingId,
                FilePath = domainEvent.FilePath,
                FileProfileId = domainEvent.FileProfileId,
                FileUploadedDateTime = domainEvent.FileUploadedDateTime,
                FileUploadedDate = domainEvent.FileUploadedDateTime.Date,
                OriginalFileName = domainEvent.OriginalFileName,
                UserId = domainEvent.UserId
            };

            await context.FileImportLogFiles.AddAsync(fileImportLogFile, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            Voucher voucher = new Voucher
            {
                ExpiryDateTime = domainEvent.ExpiryDateTime,
                ExpiryDate = domainEvent.ExpiryDateTime.Date,
                IsGenerated = true,
                IsIssued = false,
                OperatorIdentifier = domainEvent.OperatorIdentifier,
                Value = domainEvent.Value,
                VoucherCode = domainEvent.VoucherCode,
                VoucherId = domainEvent.VoucherId,
                TransactionReportingId = transaction.TransactionReportingId,
                GenerateDateTime = domainEvent.GeneratedDateTime,
                GenerateDate = domainEvent.GeneratedDateTime.Date
            };

            await context.Vouchers.AddAsync(voucher, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            MerchantAddress merchantAddress = new MerchantAddress
            {
                MerchantReportingId = merchant.MerchantReportingId,
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            MerchantContact merchantContact = new MerchantContact
            {
                MerchantReportingId = merchant.MerchantReportingId,
                Name = domainEvent.ContactName,
                ContactId = domainEvent.ContactId,
                EmailAddress = domainEvent.ContactEmailAddress,
                PhoneNumber = domainEvent.ContactPhoneNumber
            };

            await context.MerchantContacts.AddAsync(merchantContact, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            MerchantDevice merchantDevice = new MerchantDevice
            {
                MerchantReportingId = merchant.MerchantReportingId,
                DeviceId = domainEvent.DeviceId,
                DeviceIdentifier = domainEvent.DeviceIdentifier
            };

            await context.MerchantDevices.AddAsync(merchantDevice, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            MerchantOperator merchantOperator = new MerchantOperator
            {
                Name = domainEvent.Name,
                MerchantReportingId = merchant.MerchantReportingId,
                MerchantNumber = domainEvent.MerchantNumber,
                OperatorId = domainEvent.OperatorId,
                TerminalNumber = domainEvent.TerminalNumber
            };

            await context.MerchantOperators.AddAsync(merchantOperator, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            MerchantSecurityUser merchantSecurityUser = new MerchantSecurityUser
            {
                MerchantReportingId = merchant.MerchantReportingId,
                EmailAddress = domainEvent.EmailAddress,
                SecurityUserId = domainEvent.SecurityUserId
            };

            await context.MerchantSecurityUsers.AddAsync(merchantSecurityUser, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent);

            Settlement settlement = await this.LoadSettlement(context, domainEvent);

            MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
            {
                SettlementReportingId = settlement.SettlementReportingId,
                CalculatedValue = domainEvent.CalculatedValue,
                FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
                TransactionFeeReportingId = contractProductTransactionFee.TransactionFeeReportingId,
                FeeValue = domainEvent.FeeValue,
                IsSettled = false,
                MerchantReportingId = merchant.MerchantReportingId,
                TransactionReportingId = transaction.TransactionReportingId
            };

            await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Contract contract = await LoadContract(context, domainEvent);
            ContractProduct contractProduct = await this.LoadContractProduct(context, domainEvent);
            EstateOperator estateOperator = await LoadEstateOperator(context, contract.OperatorId);
            

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id [{domainEvent.TransactionId}] not found in the Read Model");
            }

            transaction.ContractReportingId = contract.ContractReportingId;
            transaction.ContractProductReportingId = contractProduct.ContractProductReportingId;
            transaction.OperatorIdentifier = estateOperator.Name;

            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task<EstateOperator> LoadEstateOperator(EstateManagementGenericContext context, Guid operatorId){
            return await context.EstateOperators.SingleOrDefaultAsync(e => e.OperatorId == operatorId);
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
            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id {domainEvent.TransactionId} not found");
            }

            StatementHeader statementHeader = await this.LoadStatementHeader(context, domainEvent); 

            StatementLine line = new StatementLine
            {
                StatementReportingId = statementHeader.StatementReportingId,
                ActivityDateTime = domainEvent.SettledDateTime,
                ActivityDate= domainEvent.SettledDateTime.Date,
                ActivityDescription = $"{transaction.OperatorIdentifier} Transaction Fee",
                ActivityType = 2, // Transaction Fee
                TransactionReportingId = transaction.TransactionReportingId,
                InAmount = domainEvent.SettledValue
            };

            await context.StatementLines.AddAsync(line, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent);

            Settlement settlement = await this.LoadSettlement(context, domainEvent);

            MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
            {
                SettlementReportingId = settlement.SettlementReportingId,
                CalculatedValue = domainEvent.CalculatedValue,
                FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
                TransactionFeeReportingId = contractProductTransactionFee.TransactionFeeReportingId,
                FeeValue = domainEvent.FeeValue,
                IsSettled = true,
                MerchantReportingId = merchant.MerchantReportingId,
                TransactionReportingId = transaction.TransactionReportingId
            };
            await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            // Find the corresponding transaction
            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            if (transaction == null)
            {
                throw new NotFoundException($"Transaction with Id {domainEvent.TransactionId} not found");
            }

            StatementHeader statementHeader = await this.LoadStatementHeader(context, domainEvent);

            StatementLine line = new StatementLine
                                 {
                                     StatementReportingId = statementHeader.StatementReportingId,
                                     ActivityDateTime = domainEvent.TransactionDateTime,
                                     ActivityDate = domainEvent.TransactionDateTime.Date,
                ActivityDescription = $"{transaction.OperatorIdentifier} Transaction",
                                     ActivityType = 1, // Transaction
                                     TransactionReportingId = transaction.TransactionReportingId,
                                     OutAmount = domainEvent.TransactionValue
                                 };

            await context.StatementLines.AddAsync(line, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            Settlement settlement = new Settlement
            {
                EstateReportingId = merchant.EstateReportingId,
                MerchantReportingId = merchant.MerchantReportingId,
                IsCompleted = false,
                SettlementDate = domainEvent.SettlementDate.Date,
                SettlementId = domainEvent.SettlementId
            };

            await context.Settlements.AddAsync(settlement, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            StatementHeader header = new StatementHeader
            {
                MerchantReportingId = merchant.MerchantReportingId,
                StatementCreatedDateTime = domainEvent.DateCreated,
                StatementCreatedDate = domainEvent.DateCreated.Date,
                StatementId = domainEvent.MerchantStatementId
            };

            await context.StatementHeaders.AddAsync(header, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent);

            Settlement settlement = await this.LoadSettlement(context, domainEvent);

            MerchantSettlementFee merchantFee = await context.MerchantSettlementFees.Where(m =>
                                                                                               m.MerchantReportingId == merchant.MerchantReportingId &&
                                                                                               m.TransactionReportingId == transaction.TransactionReportingId &&
                                                                                               m.SettlementReportingId == settlement.SettlementReportingId && m.TransactionFeeReportingId == contractProductTransactionFee.TransactionFeeReportingId)
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

        public async Task MarkSettlementAsProcessingStarted(SettlementProcessingStartedEvent domainEvent, CancellationToken cancellationToken){
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Settlement settlement = await context.Settlements.SingleOrDefaultAsync(s => s.SettlementId == domainEvent.SettlementId, cancellationToken);

            if (settlement == null)
            {
                throw new NotFoundException($"No settlement with Id {domainEvent.SettlementId} found to mark as completed");
            }

            settlement.ProcessingStarted = true;
            settlement.ProcessingStartedDateTIme = domainEvent.ProcessingStartedDateTime;
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

        public async Task SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
                                               CancellationToken cancellationToken){

            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            foreach (String additionalRequestField in this.AdditionalRequestFields)
            {
                if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower()))
                {
                    //Type dbTableType = additionalRequestData.GetType();
                    //PropertyInfo propertyInfo = dbTableType.GetProperty(additionalRequestField);

                    //if (propertyInfo != null)
                    //{
                    //    propertyInfo.SetValue(additionalRequestData, value);

                        if (additionalRequestField == "Amount")
                        {
                            String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                        // Load this value to the transaction as well
                        transaction.TransactionAmount = Decimal.Parse(value);
                        break;
                        }
                    //}
                    //else
                    //{
                    //    Logger.LogInformation("propertyInfo == null");
                    //}
                }
            }

            context.Transactions.Entry(transaction).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                                 CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            TransactionAdditionalRequestData additionalRequestData = new TransactionAdditionalRequestData
            {
                TransactionReportingId = transaction.TransactionReportingId
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

                    if (propertyInfo != null){
                        String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                        propertyInfo.SetValue(additionalRequestData,value);

                        if (additionalRequestField == "Amount"){
                            // Load this value to the transaction as well
                            transaction.TransactionAmount = Decimal.Parse(value);
                        }
                    }
                    else
                    {
                        Logger.LogInformation("propertyInfo == null");
                    }
                }
            }

            await context.TransactionsAdditionalRequestData.AddAsync(additionalRequestData, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            TransactionAdditionalResponseData additionalResponseData = new TransactionAdditionalResponseData
            {
                TransactionReportingId = transaction.TransactionReportingId
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

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            var merchant = await this.LoadMerchant(context, domainEvent);

            Reconciliation reconciliation = new Reconciliation
            {
                MerchantReportingId = merchant.MerchantReportingId,
                TransactionDate = domainEvent.TransactionDateTime.Date,
                TransactionDateTime = domainEvent.TransactionDateTime,
                TransactionTime = domainEvent.TransactionDateTime.TimeOfDay,
                TransactionId = domainEvent.TransactionId,
            };

            await context.Reconciliations.AddAsync(reconciliation, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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

            var merchant = await this.LoadMerchant(context, domainEvent);

            Transaction transaction = new Transaction
            {
                MerchantReportingId = merchant.MerchantReportingId,
                TransactionDate = domainEvent.TransactionDateTime.Date,
                TransactionDateTime = domainEvent.TransactionDateTime,
                TransactionTime = domainEvent.TransactionDateTime.TimeOfDay,
                TransactionId = domainEvent.TransactionId,
                TransactionNumber = domainEvent.TransactionNumber,
                TransactionReference = domainEvent.TransactionReference,
                TransactionType = domainEvent.TransactionType,
                DeviceIdentifier = domainEvent.DeviceIdentifier,
            };

            if (domainEvent.TransactionAmount.HasValue){
                transaction.TransactionAmount = domainEvent.TransactionAmount.Value;
            }

            await context.Transactions.AddAsync(transaction, cancellationToken);

            await context.SaveChangesWithDuplicateHandling(cancellationToken);
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
            
            File file = await context.Files.SingleOrDefaultAsync(f => f.FileId == domainEvent.FileId, cancellationToken:cancellationToken);

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

            File file = await this.LoadFile(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            await this.UpdateFileLineStatus(context, file.FileReportingId, domainEvent.LineNumber,
                                            transaction.TransactionReportingId, "S", cancellationToken);
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

            File file = await this.LoadFile(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            await this.UpdateFileLineStatus(context, file.FileReportingId, domainEvent.LineNumber,
                                            transaction.TransactionReportingId, "F", cancellationToken);
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

            File file = await this.LoadFile(context, domainEvent);

            Transaction transaction = await this.LoadTransaction(context, domainEvent);

            await this.UpdateFileLineStatus(context, file.FileReportingId, domainEvent.LineNumber,
                                            0, "I", cancellationToken);
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

            Merchant merchant = await LoadMerchant(context, domainEvent);

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

            Merchant merchant = await LoadMerchant(context, domainEvent);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with Id {domainEvent.MerchantId}");
            }

            merchant.SettlementSchedule = domainEvent.SettlementSchedule;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMerchant(TransactionHasBeenCompletedEvent domainEvent, CancellationToken cancellationToken){
            EstateManagementGenericContext context = await GetContextFromDomainEvent(domainEvent, cancellationToken);

            Merchant merchant = await LoadMerchant(context, domainEvent);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant not found with Id {domainEvent.MerchantId}");
            }

            if (domainEvent.CompletedDateTime > merchant.LastSaleDateTime){
                merchant.LastSaleDate = domainEvent.CompletedDateTime.Date;
                merchant.LastSaleDateTime = domainEvent.CompletedDateTime;
            }

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

            Merchant merchant = await LoadMerchant(context, domainEvent);

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
            voucher.IssuedDate = domainEvent.IssuedDateTime.Date;

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
            voucher.RedeemedDate = domainEvent.RedeemedDateTime.Date;

            await context.SaveChangesAsync(cancellationToken);
        }
        
        private async Task UpdateFileLineStatus(EstateManagementGenericContext context,
                                                Int32 fileReportingId,
                                                Int32 lineNumber,
                                                Int32 transactionReportingId,
                                                String newStatus,
                                                CancellationToken cancellationToken)
        {
            FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(f => f.FileReportingId == fileReportingId && f.LineNumber == lineNumber);

            if (fileLine == null)
            {
                throw new NotFoundException($"FileLine number {lineNumber} in File Reporting Id {fileReportingId} not found");
            }

            fileLine.Status = newStatus;
            fileLine.TransactionReportingId = transactionReportingId;

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

        public static Guid GetMerchantId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "MerchantId");

        public static Guid GetContractId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "ContractId");

        public static Guid GetContractProductId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "ProductId");

        public static Guid GetContractProductTransactionFeeId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FeeId");

        public static Guid GetFileImportLogId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FileImportLogId");

        public static Guid GetFileId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "FileId");

        public static Guid GetTransactionId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "TransactionId");

        public static Guid GetSettlementId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "SettlementId");

        public static Guid GetStatementHeaderId(IDomainEvent domainEvent) => DomainEventHelper.GetProperty<Guid>(domainEvent, "StatementId");
    }
}

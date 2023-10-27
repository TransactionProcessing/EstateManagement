﻿namespace EstateManagement.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Contract.DomainEvents;
using Database.Contexts;
using Database.Entities;
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

public class EstateReportingRepository : IEstateReportingRepository{
    #region Fields

    /// <summary>
    /// The additional request fields
    /// </summary>
    private readonly List<String> AdditionalRequestFields = new List<String>{
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
    public EstateReportingRepository(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> dbContextFactory){
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
                                  CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        Contract contract = new Contract{
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
                                         CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Contract contract = await this.LoadContract(context, domainEvent, cancellationToken);

        ContractProduct contractProduct = new ContractProduct{
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
                                         CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Contract contract = await this.LoadContract(context, domainEvent, cancellationToken);

        ContractProduct contractProduct = new ContractProduct{
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
                                                       CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        ContractProduct contractProduct = await this.LoadContractProduct(context, domainEvent, cancellationToken);

        ContractProductTransactionFee contractProductTransactionFee = new ContractProductTransactionFee{
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
                                CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Add the estate to the read model
        Estate estate = new Estate{
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
                                        CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        EstateOperator estateOperator = new EstateOperator{
                                                              EstateReportingId = estate.EstateReportingId,
                                                              Name = domainEvent.Name,
                                                              OperatorId = domainEvent.OperatorId,
                                                              RequireCustomMerchantNumber = domainEvent.RequireCustomMerchantNumber,
                                                              RequireCustomTerminalNumber = domainEvent.RequireCustomTerminalNumber
                                                          };

        await context.EstateOperators.AddAsync(estateOperator, cancellationToken);

        await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    /// <summary>
    /// Adds the estate security user.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task AddEstateSecurityUser(SecurityUserAddedToEstateEvent domainEvent,
                                            CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        EstateSecurityUser estateSecurityUser = new EstateSecurityUser{
                                                                          EstateReportingId = estate.EstateReportingId,
                                                                          EmailAddress = domainEvent.EmailAddress,
                                                                          SecurityUserId = domainEvent.SecurityUserId,
                                                                          CreatedDateTime = domainEvent.EventTimestamp.DateTime
                                                                      };

        await context.EstateSecurityUsers.AddAsync(estateSecurityUser, cancellationToken);

        await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    /// <summary>
    /// Adds the file.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task AddFile(FileCreatedEvent domainEvent,
                              CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        FileImportLog fileImportLog = await this.LoadFileImportLog(context, domainEvent, cancellationToken);

        File file = new File{
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
                                       CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        FileImportLog fileImportLog = new FileImportLog{
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
                                        CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        FileLine fileLine = new FileLine{
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
                                         CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        FileImportLog fileImportLog = await this.LoadFileImportLog(context, domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        FileImportLogFile fileImportLogFile = new FileImportLogFile{
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
                                          CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        Voucher voucher = new Voucher{
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
                                  CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

        Merchant merchant = new Merchant{
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
                                         CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        MerchantAddress merchantAddress = new MerchantAddress{
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
                                         CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        MerchantContact merchantContact = new MerchantContact{
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
                                        CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        MerchantDevice merchantDevice = new MerchantDevice{
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
                                          CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        MerchantOperator merchantOperator = new MerchantOperator{
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
                                              CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        MerchantSecurityUser merchantSecurityUser = new MerchantSecurityUser{
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
                                                        CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent, cancellationToken);

        Settlement settlement = await this.LoadSettlement(context, domainEvent, cancellationToken);

        MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee{
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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);
        Contract contract = await this.LoadContract(context, domainEvent, cancellationToken);
        ContractProduct contractProduct = await this.LoadContractProduct(context, domainEvent, cancellationToken);
        EstateOperator estateOperator = await this.LoadEstateOperator(context, contract.OperatorId, cancellationToken);

        transaction.ContractReportingId = contract.ContractReportingId;
        transaction.ContractProductReportingId = contractProduct.ContractProductReportingId;
        transaction.OperatorIdentifier = estateOperator.Name;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Adds the settled fee to statement.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="Shared.Exceptions.NotFoundException">Transaction with Id {domainEvent.TransactionId} not found for estate Id {estateId}</exception>
    public async Task AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                               CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Find the corresponding transaction
        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        StatementHeader statementHeader = await this.LoadStatementHeader(context, domainEvent, cancellationToken);

        StatementLine line = new StatementLine{
                                                  StatementReportingId = statementHeader.StatementReportingId,
                                                  ActivityDateTime = domainEvent.SettledDateTime,
                                                  ActivityDate = domainEvent.SettledDateTime.Date,
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
    public async Task AddSettledMerchantFeeToSettlement(SettledMerchantFeeAddedToTransactionEvent domainEvent,
                                                        CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent, cancellationToken);

        Settlement settlement = await this.LoadSettlement(context, domainEvent, cancellationToken);

        MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee{
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
                                                    CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

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
                                                CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Find the corresponding transaction
        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        StatementHeader statementHeader = await this.LoadStatementHeader(context, domainEvent, cancellationToken);

        StatementLine line = new StatementLine{
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
                                             CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Reconciliation reconciliation = await this.LoadReconcilation(context, domainEvent, cancellationToken);

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
                                          CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        transaction.IsCompleted = true;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Creates the read model.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task CreateReadModel(EstateCreatedEvent domainEvent,
                                      CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

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
                                       CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Settlement settlement = new Settlement{
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
                                      CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        StatementHeader header = new StatementHeader{
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
                                                           CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        ContractProductTransactionFee transactionFee = await this.LoadContractProductTransactionFee(context, domainEvent, cancellationToken);

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
                                               CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        ContractProductTransactionFee contractProductTransactionFee = await this.LoadContractProductTransactionFee(context, domainEvent, cancellationToken);

        Settlement settlement = await this.LoadSettlement(context, domainEvent, cancellationToken);

        MerchantSettlementFee merchantFee = await context.MerchantSettlementFees.Where(m =>
                                                                                           m.MerchantReportingId == merchant.MerchantReportingId &&
                                                                                           m.TransactionReportingId == transaction.TransactionReportingId &&
                                                                                           m.SettlementReportingId == settlement.SettlementReportingId && m.TransactionFeeReportingId == contractProductTransactionFee.TransactionFeeReportingId)
                                                         .SingleOrDefaultAsync(cancellationToken);
        if (merchantFee == null){
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
                                                CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Settlement settlement = await this.LoadSettlement(context, domainEvent, cancellationToken);

        settlement.IsCompleted = true;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkSettlementAsProcessingStarted(SettlementProcessingStartedEvent domainEvent, CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Settlement settlement = await this.LoadSettlement(context, domainEvent, cancellationToken);

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
                                               CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        StatementHeader statementHeader = await this.LoadStatementHeader(context, domainEvent, cancellationToken);

        statementHeader.StatementGeneratedDate = domainEvent.DateGenerated;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        TransactionAdditionalRequestData additionalRequestData = new TransactionAdditionalRequestData{
                                                                                                         TransactionReportingId = transaction.TransactionReportingId
                                                                                                     };

        foreach (String additionalRequestField in this.AdditionalRequestFields){
            Logger.LogInformation($"Field to look for [{additionalRequestField}]");
        }

        foreach (KeyValuePair<String, String> additionalRequestField in domainEvent.AdditionalTransactionRequestMetadata){
            Logger.LogInformation($"Key: [{additionalRequestField.Key}] Value: [{additionalRequestField.Value}]");
        }

        foreach (String additionalRequestField in this.AdditionalRequestFields){
            if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower())){
                Type dbTableType = additionalRequestData.GetType();
                PropertyInfo propertyInfo = dbTableType.GetProperty(additionalRequestField);

                if (propertyInfo != null){
                    String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                    propertyInfo.SetValue(additionalRequestData, value);

                    if (additionalRequestField == "Amount"){
                        // Load this value to the transaction as well
                        transaction.TransactionAmount = Decimal.Parse(value);
                    }
                }
                else{
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
                                                              CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        TransactionAdditionalResponseData additionalResponseData = new TransactionAdditionalResponseData{
                                                                                                            TransactionReportingId = transaction.TransactionReportingId
                                                                                                        };

        foreach (String additionalResponseField in this.AdditionalResponseFields){
            if (domainEvent.AdditionalTransactionResponseMetadata.Any(m => m.Key.ToLower() == additionalResponseField.ToLower())){
                Type dbTableType = additionalResponseData.GetType();
                PropertyInfo propertyInfo = dbTableType.GetProperty(additionalResponseField);

                if (propertyInfo != null){
                    propertyInfo.SetValue(additionalResponseData,
                                          domainEvent.AdditionalTransactionResponseMetadata.Single(m => m.Key.ToLower() == additionalResponseField.ToLower()).Value);
                }
            }
        }

        await context.TransactionsAdditionalResponseData.AddAsync(additionalResponseData, cancellationToken);

        await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    public async Task SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
                                           CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        foreach (String additionalRequestField in this.AdditionalRequestFields){
            if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower())){
                if (additionalRequestField == "Amount"){
                    String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                    // Load this value to the transaction as well
                    transaction.TransactionAmount = Decimal.Parse(value);
                    break;
                }
            }
        }

        context.Transactions.Entry(transaction).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Starts the reconciliation.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                          CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Reconciliation reconciliation = new Reconciliation{
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
                                       CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        Transaction transaction = new Transaction{
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
                                   CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Estate estate = await this.LoadEstate(context, domainEvent, cancellationToken);

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
                                           CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        file.IsCompleted = true;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the file line.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        await this.UpdateFileLineStatus(context,
                                        file.FileReportingId,
                                        domainEvent.LineNumber,
                                        transaction.TransactionReportingId,
                                        "S",
                                        cancellationToken);
    }

    /// <summary>
    /// Updates the file line.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

        await this.UpdateFileLineStatus(context,
                                        file.FileReportingId,
                                        domainEvent.LineNumber,
                                        transaction.TransactionReportingId,
                                        "F",
                                        cancellationToken);
    }

    /// <summary>
    /// Updates the file line.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        File file = await this.LoadFile(context, domainEvent, cancellationToken);

        await this.UpdateFileLineStatus(context,
                                        file.FileReportingId,
                                        domainEvent.LineNumber,
                                        0,
                                        "I",
                                        cancellationToken);
    }

    /// <summary>
    /// Updates the merchant.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="Shared.Exceptions.NotFoundException">Merchant not found with Id {domainEvent.MerchantId}</exception>
    public async Task UpdateMerchant(StatementGeneratedEvent domainEvent,
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

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
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

        merchant.SettlementSchedule = domainEvent.SettlementSchedule;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateMerchant(TransactionHasBeenCompletedEvent domainEvent, CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

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
                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = await this.LoadMerchant(context, domainEvent, cancellationToken);

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
                                                        CancellationToken cancellationToken){
        Guid estateId = domainEvent.EstateId;

        EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId, EstateReportingRepository.ConnectionStringIdentifier, cancellationToken);

        Reconciliation reconciliation = await this.LoadReconcilation(context, domainEvent, cancellationToken);

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
                                                 CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Reconciliation reconciliation = await this.LoadReconcilation(context, domainEvent, cancellationToken);

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
                                                 CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Reconciliation reconciliation = await this.LoadReconcilation(context, domainEvent, cancellationToken);

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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Transaction transaction = await this.LoadTransaction(context, domainEvent, cancellationToken);

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
                                                CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Voucher voucher = await this.LoadVoucher(context, domainEvent, cancellationToken);

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
                                                     CancellationToken cancellationToken){
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Voucher voucher = await this.LoadVoucher(context, domainEvent, cancellationToken);

        voucher.IsRedeemed = true;
        voucher.RedeemedDateTime = domainEvent.RedeemedDateTime;
        voucher.RedeemedDate = domainEvent.RedeemedDateTime.Date;

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<EstateManagementGenericContext> GetContextFromDomainEvent(IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid estateId = DomainEventHelper.GetEstateId(domainEvent);
        if (estateId == Guid.Empty){
            throw new Exception($"Unable to resolve context for Domain Event {domainEvent.GetType()}");
        }

        return await this.DbContextFactory.GetContext(estateId, EstateReportingRepository.ConnectionStringIdentifier, cancellationToken);
    }

    private async Task<Contract> LoadContract(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid contractId = DomainEventHelper.GetContractId(domainEvent);
        Contract contract = await context.Contracts.SingleOrDefaultAsync(e => e.ContractId == contractId, cancellationToken:cancellationToken);
        if (contract == null){
            throw new NotFoundException($"Contract not found with Id {contractId}");
        }

        return contract;
    }

    private async Task<ContractProduct> LoadContractProduct(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid contractProductId = DomainEventHelper.GetContractProductId(domainEvent);
        ContractProduct contractProduct = await context.ContractProducts.SingleOrDefaultAsync(e => e.ProductId == contractProductId, cancellationToken:cancellationToken);

        if (contractProduct == null){
            throw new NotFoundException($"Contract Product not found with Id {contractProductId}");
        }

        return contractProduct;
    }

    private async Task<ContractProductTransactionFee> LoadContractProductTransactionFee(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid contractProductTransactionFeeId = DomainEventHelper.GetContractProductTransactionFeeId(domainEvent);
        ContractProductTransactionFee contractProductTransactionFee = await context.ContractProductTransactionFees.SingleOrDefaultAsync(e => e.TransactionFeeId == contractProductTransactionFeeId, cancellationToken:cancellationToken);

        if (contractProductTransactionFee == null){
            throw new NotFoundException($"Contract Product Transaction Fee not found with Id {contractProductTransactionFeeId}");
        }

        return contractProductTransactionFee;
    }

    private async Task<Estate> LoadEstate(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid estateId = DomainEventHelper.GetEstateId(domainEvent);
        Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken);
        if (estate == null){
            throw new NotFoundException($"Estate not found with Id {estateId}");
        }

        return estate;
    }

    private async Task<EstateOperator> LoadEstateOperator(EstateManagementGenericContext context, Guid operatorId, CancellationToken cancellationToken){
        EstateOperator estateOperator = await context.EstateOperators.SingleOrDefaultAsync(e => e.OperatorId == operatorId, cancellationToken:cancellationToken);
        if (estateOperator == null){
            throw new NotFoundException($"Estate Operator not found with Id {operatorId}");
        }

        return estateOperator;
    }

    private async Task<File> LoadFile(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid fileId = DomainEventHelper.GetFileId(domainEvent);
        File file = await context.Files.SingleOrDefaultAsync(e => e.FileId == fileId, cancellationToken:cancellationToken);
        if (file == null){
            throw new NotFoundException($"File not found with Id {fileId}");
        }

        return file;
    }

    private async Task<FileImportLog> LoadFileImportLog(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid fileImportLogId = DomainEventHelper.GetFileImportLogId(domainEvent);
        FileImportLog fileImportLog = await context.FileImportLogs.SingleOrDefaultAsync(e => e.FileImportLogId == fileImportLogId, cancellationToken:cancellationToken);
        if (fileImportLog == null){
            throw new NotFoundException($"File Import Log not found with Id {fileImportLogId}");
        }

        return fileImportLog;
    }

    private async Task<Merchant> LoadMerchant(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid merchantId = DomainEventHelper.GetMerchantId(domainEvent);
        Merchant merchant = await context.Merchants.SingleOrDefaultAsync(e => e.MerchantId == merchantId, cancellationToken:cancellationToken);
        if (merchant == null){
            throw new NotFoundException($"Merchant not found with Id {merchantId}");
        }

        return merchant;
    }

    private async Task<Reconciliation> LoadReconcilation(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid transactionId = DomainEventHelper.GetTransactionId(domainEvent);
        Reconciliation reconciliation =
            await context.Reconciliations.SingleOrDefaultAsync(t => t.TransactionId == transactionId, cancellationToken:cancellationToken);

        if (reconciliation == null){
            throw new NotFoundException($"Reconciliation not found with Id {transactionId}");
        }

        return reconciliation;
    }

    private async Task<Settlement> LoadSettlement(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid settlementId = DomainEventHelper.GetSettlementId(domainEvent);
        Settlement settlement = await context.Settlements.SingleOrDefaultAsync(e => e.SettlementId == settlementId, cancellationToken:cancellationToken);
        if (settlement == null){
            throw new NotFoundException($"Settlement not found with Id {settlementId}");
        }

        return settlement;
    }

    private async Task<StatementHeader> LoadStatementHeader(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid statementHeaderId = DomainEventHelper.GetStatementHeaderId(domainEvent);
        StatementHeader statementHeader = await context.StatementHeaders.SingleOrDefaultAsync(e => e.StatementId == statementHeaderId, cancellationToken:cancellationToken);
        if (statementHeader == null){
            throw new NotFoundException($"Statement Header not found with Id {statementHeaderId}");
        }

        return statementHeader;
    }

    private async Task<Transaction> LoadTransaction(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid transactionId = DomainEventHelper.GetTransactionId(domainEvent);
        Transaction transaction = await context.Transactions.SingleOrDefaultAsync(e => e.TransactionId == transactionId, cancellationToken:cancellationToken);
        if (transaction == null){
            throw new NotFoundException($"Transaction not found with Id {transactionId}");
        }

        return transaction;
    }

    private async Task<Voucher> LoadVoucher(EstateManagementGenericContext context, IDomainEvent domainEvent, CancellationToken cancellationToken){
        Guid voucherId = DomainEventHelper.GetVoucherId(domainEvent);
        Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(v => v.VoucherId == voucherId, cancellationToken);

        if (voucher == null){
            throw new NotFoundException($"Voucher not found with Id {voucherId}");
        }

        return voucher;
    }

    private async Task UpdateFileLineStatus(EstateManagementGenericContext context,
                                            Int32 fileReportingId,
                                            Int32 lineNumber,
                                            Int32 transactionReportingId,
                                            String newStatus,
                                            CancellationToken cancellationToken){
        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(f => f.FileReportingId == fileReportingId && f.LineNumber == lineNumber, cancellationToken:cancellationToken);

        if (fileLine == null){
            throw new NotFoundException($"FileLine number {lineNumber} in File Reporting Id {fileReportingId} not found");
        }

        fileLine.Status = newStatus;
        fileLine.TransactionReportingId = transactionReportingId;

        await context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Others

    private const String ConnectionStringIdentifier = "EstateReportingReadModel";

    #endregion
}
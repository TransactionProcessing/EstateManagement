using Shared.EventStore.Aggregate;
using Shared.Results;
using SimpleResults;

namespace EstateManagement.Repository;

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
using Operator.DomainEvents;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.Exceptions;
using Shared.Logger;
using TransactionProcessor.Float.DomainEvents;
using TransactionProcessor.Reconciliation.DomainEvents;
using TransactionProcessor.Settlement.DomainEvents;
using TransactionProcessor.Transaction.DomainEvents;
using TransactionProcessor.Voucher.DomainEvents;

public class EstateReportingRepository : IEstateReportingRepository
{
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
    public EstateReportingRepository(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> dbContextFactory)
    {
        this.DbContextFactory = dbContextFactory;
    }

    #endregion

    #region Methods

    public async Task<Result> UpdateOperator(OperatorNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Operator> operatorResult = await context.LoadOperator(domainEvent, cancellationToken);
        if (operatorResult.IsFailed)
            return ResultHelpers.CreateFailure(operatorResult);
        Operator @operator = operatorResult.Data;
        @operator.Name = domainEvent.Name;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateOperator(OperatorRequireCustomMerchantNumberChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Operator> operatorResult = await context.LoadOperator(domainEvent, cancellationToken);
        if (operatorResult.IsFailed)
            return ResultHelpers.CreateFailure(operatorResult);
        Operator @operator = operatorResult.Data;

        @operator.RequireCustomMerchantNumber = domainEvent.RequireCustomMerchantNumber;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateOperator(OperatorRequireCustomTerminalNumberChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Operator> operatorResult = await context.LoadOperator(domainEvent, cancellationToken);
        if (operatorResult.IsFailed)
            return ResultHelpers.CreateFailure(operatorResult);
        Operator @operator = operatorResult.Data;

        @operator.RequireCustomTerminalNumber = domainEvent.RequireCustomTerminalNumber;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddOperator(OperatorCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Operator @operator = new Operator
        {
            RequireCustomTerminalNumber = domainEvent.RequireCustomTerminalNumber,
            OperatorId = domainEvent.OperatorId,
            Name = domainEvent.Name,
            RequireCustomMerchantNumber = domainEvent.RequireCustomMerchantNumber,
            EstateId = domainEvent.EstateId
        };

        await context.Operators.AddAsync(@operator, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddContract(ContractCreatedEvent domainEvent,
                                  CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Contract contract = new Contract
        {
            EstateId = domainEvent.EstateId,
            OperatorId = domainEvent.OperatorId,
            ContractId = domainEvent.ContractId,
            Description = domainEvent.Description
        };

        await context.Contracts.AddAsync(contract, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddContractProduct(VariableValueProductAddedToContractEvent domainEvent,
                                         CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        ContractProduct contractProduct = new ContractProduct
        {
            ContractId = domainEvent.ContractId,
            ContractProductId = domainEvent.ProductId,
            DisplayText = domainEvent.DisplayText,
            ProductName = domainEvent.ProductName,
            Value = null,
            ProductType = domainEvent.ProductType
        };

        await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddContractProduct(FixedValueProductAddedToContractEvent domainEvent,
                                         CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        ContractProduct contractProduct = new ContractProduct
        {
            ContractId = domainEvent.ContractId,
            ContractProductId = domainEvent.ProductId,
            DisplayText = domainEvent.DisplayText,
            ProductName = domainEvent.ProductName,
            Value = domainEvent.Value,
            ProductType = domainEvent.ProductType
        };

        await context.ContractProducts.AddAsync(contractProduct, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddContractProductTransactionFee(TransactionFeeForProductAddedToContractEvent domainEvent,
                                                       CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        ContractProductTransactionFee contractProductTransactionFee = new ContractProductTransactionFee
        {
            ContractProductId = domainEvent.ProductId,
            Description = domainEvent.Description,
            Value = domainEvent.Value,
            ContractProductTransactionFeeId = domainEvent.TransactionFeeId,
            CalculationType = domainEvent.CalculationType,
            IsEnabled = true,
            FeeType = domainEvent.FeeType
        };

        await context.ContractProductTransactionFees.AddAsync(contractProductTransactionFee, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddEstate(EstateCreatedEvent domainEvent,
                                CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Add the estate to the read model
        Estate estate = new Estate
        {
            EstateId = domainEvent.EstateId,
            Name = domainEvent.EstateName,
            Reference = String.Empty
        };
        await context.Estates.AddAsync(estate, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Result> AddEstateSecurityUser(SecurityUserAddedToEstateEvent domainEvent,
                                            CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        EstateSecurityUser estateSecurityUser = new EstateSecurityUser
        {
            EstateId = domainEvent.EstateId,
            EmailAddress = domainEvent.EmailAddress,
            SecurityUserId = domainEvent.SecurityUserId,
            CreatedDateTime = domainEvent.EventTimestamp.DateTime
        };

        await context.EstateSecurityUsers.AddAsync(estateSecurityUser, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddFile(FileCreatedEvent domainEvent,
                              CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        File file = new File
        {
            EstateId = domainEvent.EstateId,
            MerchantId = domainEvent.MerchantId,
            FileImportLogId = domainEvent.FileImportLogId,
            UserId = domainEvent.UserId,
            FileId = domainEvent.FileId,
            FileProfileId = domainEvent.FileProfileId,
            FileLocation = domainEvent.FileLocation,
            FileReceivedDateTime = domainEvent.FileReceivedDateTime,
            FileReceivedDate = domainEvent.FileReceivedDateTime.Date
        };

        await context.Files.AddAsync(file, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddFileImportLog(ImportLogCreatedEvent domainEvent,
                                       CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        FileImportLog fileImportLog = new FileImportLog
        {
            EstateId = domainEvent.EstateId,
            FileImportLogId = domainEvent.FileImportLogId,
            ImportLogDateTime = domainEvent.ImportLogDateTime,
            ImportLogDate = domainEvent.ImportLogDateTime.Date
        };

        await context.FileImportLogs.AddAsync(fileImportLog, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddFileLineToFile(FileLineAddedEvent domainEvent,
                                        CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        FileLine fileLine = new FileLine
        {
            FileId = domainEvent.FileId,
            LineNumber = domainEvent.LineNumber,
            FileLineData = domainEvent.FileLine,
            Status = "P" // Pending
        };

        await context.FileLines.AddAsync(fileLine, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddFileToImportLog(FileAddedToImportLogEvent domainEvent,
                                         CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        FileImportLogFile fileImportLogFile = new FileImportLogFile
        {
            MerchantId = domainEvent.MerchantId,
            FileImportLogId = domainEvent.FileImportLogId,
            FileId = domainEvent.FileId,
            FilePath = domainEvent.FilePath,
            FileProfileId = domainEvent.FileProfileId,
            FileUploadedDateTime = domainEvent.FileUploadedDateTime,
            FileUploadedDate = domainEvent.FileUploadedDateTime.Date,
            OriginalFileName = domainEvent.OriginalFileName,
            UserId = domainEvent.UserId
        };

        await context.FileImportLogFiles.AddAsync(fileImportLogFile, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddGeneratedVoucher(VoucherGeneratedEvent domainEvent,
                                          CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        Voucher voucher = new Voucher
        {
            ExpiryDateTime = domainEvent.ExpiryDateTime,
            ExpiryDate = domainEvent.ExpiryDateTime.Date,
            IsGenerated = true,
            IsIssued = false,
            OperatorIdentifier = domainEvent.OperatorId.ToString(),
            Value = domainEvent.Value,
            VoucherCode = domainEvent.VoucherCode,
            VoucherId = domainEvent.VoucherId,
            TransactionId = domainEvent.TransactionId,
            GenerateDateTime = domainEvent.GeneratedDateTime,
            GenerateDate = domainEvent.GeneratedDateTime.Date
        };

        await context.Vouchers.AddAsync(voucher, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchant(MerchantCreatedEvent domainEvent,
                                  CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Merchant merchant = new Merchant
        {
            EstateId = domainEvent.EstateId,
            MerchantId = domainEvent.MerchantId,
            Name = domainEvent.MerchantName,
            CreatedDateTime = domainEvent.DateCreated,
            LastStatementGenerated = DateTime.MinValue,
            SettlementSchedule = 0
        };

        await context.Merchants.AddAsync(merchant, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchant(MerchantNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Merchant> merchantResult = await context.LoadMerchant(domainEvent, cancellationToken);
        if (merchantResult.IsFailed)
            return ResultHelpers.CreateFailure(merchantResult);
        var merchant = merchantResult.Data;
        merchant.Name = domainEvent.MerchantName;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchantAddress(AddressAddedEvent domainEvent,
                                         CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
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

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchantContact(ContactAddedEvent domainEvent,
                                         CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        MerchantContact merchantContact = new MerchantContact
        {
            MerchantId = domainEvent.MerchantId,
            Name = domainEvent.ContactName,
            ContactId = domainEvent.ContactId,
            EmailAddress = domainEvent.ContactEmailAddress,
            PhoneNumber = domainEvent.ContactPhoneNumber
        };

        await context.MerchantContacts.AddAsync(merchantContact, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                                        CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantDevice merchantDevice = new MerchantDevice
        {
            MerchantId = domainEvent.MerchantId,
            DeviceId = domainEvent.DeviceId,
            DeviceIdentifier = domainEvent.DeviceIdentifier
        };

        await context.MerchantDevices.AddAsync(merchantDevice, cancellationToken);
        
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> SwapMerchantDevice(DeviceSwappedForMerchantEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        var getDeviceResult = await context.LoadMerchantDevice(domainEvent, cancellationToken);
        if (getDeviceResult.IsFailed)
            return ResultHelpers.CreateFailure(getDeviceResult);
        var device = getDeviceResult.Data;

        device.DeviceIdentifier = domainEvent.NewDeviceIdentifier;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchantOperator(OperatorAssignedToMerchantEvent domainEvent,
                                          CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        String operatorName = domainEvent.Name;
        if (String.IsNullOrEmpty(operatorName)) {
            // Lookup the operator
            Operator @operator = await context.Operators.SingleOrDefaultAsync(o => o.OperatorId == domainEvent.OperatorId,cancellationToken);
            operatorName = @operator.Name;
        }

        if (String.IsNullOrEmpty(operatorName)) {
            return Result.Failure("Unable to get operator name and this can't be null");
        }

        MerchantOperator merchantOperator = new MerchantOperator
        {
            Name = operatorName,
            MerchantId = domainEvent.MerchantId,
            MerchantNumber = domainEvent.MerchantNumber,
            OperatorId = domainEvent.OperatorId,
            TerminalNumber = domainEvent.TerminalNumber
        };

        await context.MerchantOperators.AddAsync(merchantOperator, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddMerchantSecurityUser(SecurityUserAddedToMerchantEvent domainEvent,
                                              CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantSecurityUser merchantSecurityUser = new MerchantSecurityUser
        {
            MerchantId = domainEvent.MerchantId,
            EmailAddress = domainEvent.EmailAddress,
            SecurityUserId = domainEvent.SecurityUserId
        };

        await context.MerchantSecurityUsers.AddAsync(merchantSecurityUser, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddPendingMerchantFeeToSettlement(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                                        CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
        {
            SettlementId = domainEvent.SettlementId,
            CalculatedValue = domainEvent.CalculatedValue,
            FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
            ContractProductTransactionFeeId = domainEvent.FeeId,
            FeeValue = domainEvent.FeeValue,
            IsSettled = false,
            MerchantId = domainEvent.MerchantId,
            TransactionId = domainEvent.TransactionId
        };

        await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

        return await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    public async Task<Result> AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var getContractResult= await context.LoadContract(domainEvent, cancellationToken);
        if (getContractResult.IsFailed)
            return ResultHelpers.CreateFailure(getContractResult);

        var transaction = getTransactionResult.Data;
        var contract = getContractResult.Data;

        transaction.ContractId = domainEvent.ContractId;
        transaction.ContractProductId = domainEvent.ProductId;
        transaction.OperatorId = contract.OperatorId;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                               CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Find the corresponding transaction
        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        Result<Operator> operatorResult = await context.LoadOperator(domainEvent, cancellationToken);
        if (operatorResult.IsFailed)
            return ResultHelpers.CreateFailure(operatorResult);
        var @operator = operatorResult.Data;

        StatementLine line = new StatementLine
        {
            StatementId = domainEvent.MerchantStatementId,
            ActivityDateTime = domainEvent.SettledDateTime,
            ActivityDate = domainEvent.SettledDateTime.Date,
            ActivityDescription = $"{@operator.Name} Transaction Fee",
            ActivityType = 2, // Transaction Fee
            TransactionId = domainEvent.TransactionId,
            InAmount = domainEvent.SettledValue
        };

        await context.StatementLines.AddAsync(line, cancellationToken);

        return await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    public async Task<Result> AddSettledMerchantFeeToSettlement(SettledMerchantFeeAddedToTransactionEvent domainEvent,
                                                        CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantSettlementFee merchantSettlementFee = new MerchantSettlementFee
        {
            SettlementId = domainEvent.SettlementId,
            CalculatedValue = domainEvent.CalculatedValue,
            FeeCalculatedDateTime = domainEvent.FeeCalculatedDateTime,
            ContractProductTransactionFeeId = domainEvent.FeeId,
            FeeValue = domainEvent.FeeValue,
            IsSettled = true,
            MerchantId = domainEvent.MerchantId,
            TransactionId = domainEvent.TransactionId
        };
        await context.MerchantSettlementFees.AddAsync(merchantSettlementFee, cancellationToken);

        return await context.SaveChangesWithDuplicateHandling(cancellationToken);
    }

    public async Task<Result> AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                                    CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.TransactionSource = domainEvent.TransactionSource;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                                CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // Find the corresponding transaction
        Result<Operator> operatorResult = await context.LoadOperator(domainEvent, cancellationToken);
        if (operatorResult.IsFailed)
            return ResultHelpers.CreateFailure(operatorResult);
        var @operator = operatorResult.Data;
        
        StatementLine line = new StatementLine
        {
            StatementId = domainEvent.MerchantStatementId,
            ActivityDateTime = domainEvent.TransactionDateTime,
            ActivityDate = domainEvent.TransactionDateTime.Date,
            ActivityDescription = $"{@operator.Name} Transaction",
            ActivityType = 1, // Transaction
            TransactionId = domainEvent.TransactionId,
            OutAmount = domainEvent.TransactionValue
        };

        await context.StatementLines.AddAsync(line, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                             CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getReconcilationResult = await context.LoadReconcilation(domainEvent, cancellationToken);
        if (getReconcilationResult.IsFailed)
            return ResultHelpers.CreateFailure(getReconcilationResult);
        var reconciliation = getReconcilationResult.Data;

        reconciliation.IsCompleted = true;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                          CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.IsCompleted = true;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CreateFloat(FloatCreatedForContractProductEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Float floatRecord = new Float
        {
            CreatedDate = domainEvent.CreatedDateTime.Date,
            CreatedDateTime = domainEvent.CreatedDateTime,
            ContractId = domainEvent.ContractId,
            EstateId = domainEvent.EstateId,
            FloatId = domainEvent.FloatId,
            ProductId = domainEvent.ProductId
        };
        await context.Floats.AddAsync(floatRecord, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CreateFloatActivity(FloatCreditPurchasedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        FloatActivity floatActivity = new FloatActivity
        {
            ActivityDate = domainEvent.CreditPurchasedDateTime.Date,
            ActivityDateTime = domainEvent.CreditPurchasedDateTime,
            Amount = domainEvent.Amount,
            CostPrice = domainEvent.CostPrice,
            CreditOrDebit = "C",
            EventId = domainEvent.EventId,
            FloatId = domainEvent.FloatId
        };
        await context.FloatActivity.AddAsync(floatActivity, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);

    }

    public async Task<Result> CreateFloatActivity(FloatDecreasedByTransactionEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        FloatActivity floatActivity = new FloatActivity
        {
            ActivityDate = transaction.TransactionDate,
            ActivityDateTime = transaction.TransactionDateTime,
            Amount = domainEvent.Amount,
            CostPrice = 0,
            CreditOrDebit = "D",
            EventId = domainEvent.EventId,
            FloatId = domainEvent.FloatId
        };
        await context.FloatActivity.AddAsync(floatActivity, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CreateReadModel(EstateCreatedEvent domainEvent,
                                      CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Logger.LogInformation($"About to run migrations on Read Model database for estate [{domainEvent.EstateId}]");

        // Ensure the db is at the latest version
        await context.MigrateAsync(cancellationToken);

        Logger.LogWarning($"Read Model database for estate [{domainEvent.EstateId}] migrated to latest version");
        return Result.Success();
    }

    public async Task<Result> CreateSettlement(SettlementCreatedForDateEvent domainEvent,
                                       CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Settlement settlement = new Settlement
        {
            EstateId = domainEvent.EstateId,
            MerchantId = domainEvent.MerchantId,
            IsCompleted = false,
            SettlementDate = domainEvent.SettlementDate.Date,
            SettlementId = domainEvent.SettlementId
        };

        await context.Settlements.AddAsync(settlement, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> CreateStatement(StatementCreatedEvent domainEvent,
                                      CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        StatementHeader header = new StatementHeader
        {
            MerchantId = domainEvent.MerchantId,
            StatementCreatedDateTime = domainEvent.DateCreated,
            StatementCreatedDate = domainEvent.DateCreated.Date,
            StatementId = domainEvent.MerchantStatementId
        };

        await context.StatementHeaders.AddAsync(header, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> DisableContractProductTransactionFee(TransactionFeeForProductDisabledEvent domainEvent,
                                                           CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        ContractProductTransactionFee transactionFee = await context.LoadContractProductTransactionFee(domainEvent, cancellationToken);

        transactionFee.IsEnabled = false;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> MarkMerchantFeeAsSettled(MerchantFeeSettledEvent domainEvent,
                                               CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        // TODO: LoadMerchantSettlementFee
        MerchantSettlementFee merchantFee = await context.MerchantSettlementFees.Where(m =>
                m.MerchantId == domainEvent.MerchantId &&
                m.TransactionId == domainEvent.TransactionId &&
                m.SettlementId == domainEvent.SettlementId &&
                m.ContractProductTransactionFeeId == domainEvent.FeeId)
            .SingleOrDefaultAsync(cancellationToken);

        if (merchantFee == null)
        {
            return Result.NotFound("Merchant Fee not found to update as settled");
        }

        merchantFee.IsSettled = true;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> MarkSettlementAsCompleted(SettlementCompletedEvent domainEvent,
                                                CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getSettlementResult = await context.LoadSettlement(domainEvent, cancellationToken);
        if (getSettlementResult.IsFailed)
            return ResultHelpers.CreateFailure(getSettlementResult);
        var settlement = getSettlementResult.Data;

        settlement.IsCompleted = true;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> MarkSettlementAsProcessingStarted(SettlementProcessingStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getSettlementResult = await context.LoadSettlement(domainEvent, cancellationToken);
        if (getSettlementResult.IsFailed)
            return ResultHelpers.CreateFailure(getSettlementResult);
        var settlement = getSettlementResult.Data;
        settlement.ProcessingStarted = true;
        settlement.ProcessingStartedDateTIme = domainEvent.ProcessingStartedDateTime;
        
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> MarkStatementAsGenerated(StatementGeneratedEvent domainEvent,
                                               CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getLoadStatementHeaderResult = await context.LoadStatementHeader(domainEvent, cancellationToken);
        if (getLoadStatementHeaderResult.IsFailed)
            return ResultHelpers.CreateFailure(getLoadStatementHeaderResult);
        var statementHeader = getLoadStatementHeaderResult.Data;

        statementHeader.StatementGeneratedDate = domainEvent.DateGenerated;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        TransactionAdditionalRequestData additionalRequestData = new TransactionAdditionalRequestData
        {
            TransactionId = domainEvent.TransactionId
        };

        foreach (String additionalRequestField in this.AdditionalRequestFields)
        {
            Logger.LogDebug($"Field to look for [{additionalRequestField}]");
        }

        foreach (KeyValuePair<String, String> additionalRequestField in domainEvent.AdditionalTransactionRequestMetadata)
        {
            Logger.LogDebug($"Key: [{additionalRequestField.Key}] Value: [{additionalRequestField.Value}]");
        }

        foreach (String additionalRequestField in this.AdditionalRequestFields)
        {
            if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower()))
            {
                Type dbTableType = additionalRequestData.GetType();
                PropertyInfo propertyInfo = dbTableType.GetProperty(additionalRequestField);

                if (propertyInfo != null)
                {
                    String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                    propertyInfo.SetValue(additionalRequestData, value);
                }
                else
                {
                    Logger.LogInformation("propertyInfo == null");
                }
            }
        }

        await context.TransactionsAdditionalRequestData.AddAsync(additionalRequestData, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                              CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        TransactionAdditionalResponseData additionalResponseData = new TransactionAdditionalResponseData
        {
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

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
                                           CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        foreach (String additionalRequestField in this.AdditionalRequestFields)
        {
            if (domainEvent.AdditionalTransactionRequestMetadata.Any(m => m.Key.ToLower() == additionalRequestField.ToLower()))
            {
                if (additionalRequestField == "Amount")
                {
                    String value = domainEvent.AdditionalTransactionRequestMetadata.Single(m => m.Key.ToLower() == additionalRequestField.ToLower()).Value;
                    // Load this value to the transaction as well
                    transaction.TransactionAmount = Decimal.Parse(value);
                    break;
                }
            }
        }

        context.Transactions.Entry(transaction).State = EntityState.Modified;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                          CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Reconciliation reconciliation = new Reconciliation
        {
            MerchantId = domainEvent.MerchantId,
            TransactionDate = domainEvent.TransactionDateTime.Date,
            TransactionDateTime = domainEvent.TransactionDateTime,
            TransactionTime = domainEvent.TransactionDateTime.TimeOfDay,
            TransactionId = domainEvent.TransactionId,
        };

        await context.Reconciliations.AddAsync(reconciliation, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> StartTransaction(TransactionHasStartedEvent domainEvent,
                                       CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        Transaction t = new Transaction
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

        if (domainEvent.TransactionAmount.HasValue)
        {
            t.TransactionAmount = domainEvent.TransactionAmount.Value;
        }

        await context.AddAsync(t, cancellationToken);
        
        Logger.LogDebug($"Transaction Loaded with Id [{domainEvent.TransactionId}]");
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateEstate(EstateReferenceAllocatedEvent domainEvent,
                                   CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getEstateResult= await context.LoadEstate(domainEvent, cancellationToken);
        if (getEstateResult.IsFailed)
            return ResultHelpers.CreateFailure(getEstateResult);
            
        var estate = getEstateResult.Data;
        estate.Reference = domainEvent.EstateReference;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateFileAsComplete(FileProcessingCompletedEvent domainEvent,
                                           CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getFileResult = await context.LoadFile(domainEvent, cancellationToken);
        if (getFileResult.IsFailed)
            return ResultHelpers.CreateFailure(getFileResult);

        var file = getFileResult.Data;
        file.IsCompleted = true;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        return await this.UpdateFileLineStatus(context,
                                        domainEvent.FileId,
                                        domainEvent.LineNumber,
                                        domainEvent.TransactionId,
                                        "S",
                                        cancellationToken);
    }

    public async Task<Result> UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        return await this.UpdateFileLineStatus(context,
            domainEvent.FileId,
            domainEvent.LineNumber,
            domainEvent.TransactionId,
                                        "F",
                                        cancellationToken);
    }

    public async Task<Result> UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        return await this.UpdateFileLineStatus(context,
                                        domainEvent.FileId,
                                        domainEvent.LineNumber,
                                        Guid.Empty, 
                                        "I",
                                        cancellationToken);
    }

    public async Task<Result> UpdateMerchant(StatementGeneratedEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Merchant> merchantResult = await context.LoadMerchant(domainEvent, cancellationToken);
        if (merchantResult.IsFailed)
            return ResultHelpers.CreateFailure(merchantResult);
        var merchant = merchantResult.Data;

        if (merchant.LastStatementGenerated > domainEvent.DateGenerated)
            return Result.Success();

        merchant.LastStatementGenerated = domainEvent.DateGenerated;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchant(SettlementScheduleChangedEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Merchant> merchantResult = await context.LoadMerchant(domainEvent, cancellationToken);
        if (merchantResult.IsFailed)
            return ResultHelpers.CreateFailure(merchantResult);
        var merchant = merchantResult.Data;

        merchant.SettlementSchedule = domainEvent.SettlementSchedule;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchant(TransactionHasBeenCompletedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Merchant> merchantResult = await context.LoadMerchant(domainEvent, cancellationToken);
        if (merchantResult.IsFailed)
            return ResultHelpers.CreateFailure(merchantResult);
        var merchant = merchantResult.Data;

        if (domainEvent.CompletedDateTime > merchant.LastSaleDateTime)
        {
            merchant.LastSaleDate = domainEvent.CompletedDateTime.Date;
            merchant.LastSaleDateTime = domainEvent.CompletedDateTime;
        }

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchant(MerchantReferenceAllocatedEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        Result<Merchant> merchantResult = await context.LoadMerchant(domainEvent, cancellationToken);
        if (merchantResult.IsFailed)
            return ResultHelpers.CreateFailure(merchantResult);
        var merchant = merchantResult.Data;

        merchant.Reference = domainEvent.MerchantReference;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateReconciliationOverallTotals(OverallTotalsRecordedEvent domainEvent,
                                                        CancellationToken cancellationToken)
    {
        Guid estateId = domainEvent.EstateId;

        EstateManagementGenericContext context = await this.DbContextFactory.GetContext(estateId, EstateReportingRepository.ConnectionStringIdentifier, cancellationToken);

        var getReconcilationResult = await context.LoadReconcilation(domainEvent, cancellationToken);
        if (getReconcilationResult.IsFailed)
            return ResultHelpers.CreateFailure(getReconcilationResult);
        var reconciliation = getReconcilationResult.Data;

        reconciliation.TransactionCount = domainEvent.TransactionCount;
        reconciliation.TransactionValue = domainEvent.TransactionValue;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateReconciliationStatus(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getReconcilationResult = await context.LoadReconcilation(domainEvent, cancellationToken);
        if (getReconcilationResult.IsFailed)
            return ResultHelpers.CreateFailure(getReconcilationResult);
        var reconciliation = getReconcilationResult.Data;

        reconciliation.IsAuthorised = true;
        reconciliation.ResponseCode = domainEvent.ResponseCode;
        reconciliation.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateReconciliationStatus(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getReconcilationResult= await context.LoadReconcilation(domainEvent, cancellationToken);
        if(getReconcilationResult.IsFailed)
            return ResultHelpers.CreateFailure(getReconcilationResult);
        var reconciliation = getReconcilationResult.Data;
        
        reconciliation.IsAuthorised = false;
        reconciliation.ResponseCode = domainEvent.ResponseCode;
        reconciliation.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.IsAuthorised = true;
        transaction.ResponseCode = domainEvent.ResponseCode;
        transaction.AuthorisationCode = domainEvent.AuthorisationCode;
        transaction.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.IsAuthorised = false;
        transaction.ResponseCode = domainEvent.ResponseCode;
        transaction.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.IsAuthorised = true;
        transaction.ResponseCode = domainEvent.ResponseCode;
        transaction.AuthorisationCode = domainEvent.AuthorisationCode;
        transaction.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getTransactionResult = await context.LoadTransaction(domainEvent, cancellationToken);
        if (getTransactionResult.IsFailed)
            return ResultHelpers.CreateFailure(getTransactionResult);
        var transaction = getTransactionResult.Data;

        transaction.IsAuthorised = false;
        transaction.ResponseCode = domainEvent.ResponseCode;
        transaction.ResponseMessage = domainEvent.ResponseMessage;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateVoucherIssueDetails(VoucherIssuedEvent domainEvent,
                                                CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getVoucherResult = await context.LoadVoucher(domainEvent, cancellationToken);
        if (getVoucherResult.IsFailed)
            return ResultHelpers.CreateFailure(getVoucherResult);
        var voucher = getVoucherResult.Data;
        voucher.IsIssued = true;
        voucher.RecipientEmail = domainEvent.RecipientEmail;
        voucher.RecipientMobile = domainEvent.RecipientMobile;
        voucher.IssuedDateTime = domainEvent.IssuedDateTime;
        voucher.IssuedDate = domainEvent.IssuedDateTime.Date;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateVoucherRedemptionDetails(VoucherFullyRedeemedEvent domainEvent,
                                                     CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getVoucherResult = await context.LoadVoucher(domainEvent, cancellationToken);
        if (getVoucherResult.IsFailed)
            return ResultHelpers.CreateFailure(getVoucherResult);
        var voucher = getVoucherResult.Data;

        voucher.IsRedeemed = true;
        voucher.RedeemedDateTime = domainEvent.RedeemedDateTime;
        voucher.RedeemedDate = domainEvent.RedeemedDateTime.Date;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> RemoveOperatorFromMerchant(OperatorRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantOperator merchantOperator = await context.MerchantOperators.SingleOrDefaultAsync(o => o.OperatorId == domainEvent.OperatorId &&
                                                                                                      o.MerchantId == domainEvent.MerchantId,
                                                                                                 cancellationToken: cancellationToken);
        if (merchantOperator == null) {
            return Result.NotFound($"No operator {domainEvent.OperatorId} found for merchant {domainEvent.MerchantId}");
        }

        merchantOperator.IsDeleted = true;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> RemoveContractFromMerchant(ContractRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);
        
        MerchantContract merchantContract = await context.MerchantContracts.SingleOrDefaultAsync(o => o.ContractId == domainEvent.ContractId &&
                                                                                                      o.MerchantId == domainEvent.MerchantId,
                                                                                                 cancellationToken: cancellationToken);
        if (merchantContract == null)
        {
            return Result.NotFound($"No contract {domainEvent.ContractId} found for merchant {domainEvent.MerchantId}");
        }


        merchantContract.IsDeleted = true;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantAddressLine1UpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.AddressLine1 = domainEvent.AddressLine1;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantAddressLine2UpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.AddressLine2 = domainEvent.AddressLine2;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantAddressLine3UpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.AddressLine3 = domainEvent.AddressLine3;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantAddressLine4UpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.AddressLine4 = domainEvent.AddressLine4;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantCountyUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.Country = domainEvent.Country;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantRegionUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.Region = domainEvent.Region;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantTownUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.Town = domainEvent.Town;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(MerchantPostalCodeUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantAddressResult = await context.LoadMerchantAddress(domainEvent, cancellationToken);
        if (getMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantAddressResult);
        var merchantAddress = getMerchantAddressResult.Data;

        merchantAddress.PostalCode = domainEvent.PostalCode;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantContact(MerchantContactNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantContactResult = await context.LoadMerchantContact(domainEvent, cancellationToken);
        if (getMerchantContactResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantContactResult);
        var merchantContact = getMerchantContactResult.Data;

        merchantContact.Name = domainEvent.ContactName;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantContact(MerchantContactEmailAddressUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantContactResult = await context.LoadMerchantContact(domainEvent, cancellationToken);
        if (getMerchantContactResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantContactResult);
        var merchantContact = getMerchantContactResult.Data;

        merchantContact.EmailAddress = domainEvent.ContactEmailAddress;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateMerchantContact(MerchantContactPhoneNumberUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        var getMerchantContactResult = await context.LoadMerchantContact(domainEvent, cancellationToken);
        if (getMerchantContactResult.IsFailed)
            return ResultHelpers.CreateFailure(getMerchantContactResult);
        var merchantContact = getMerchantContactResult.Data;

        merchantContact.PhoneNumber = domainEvent.ContactPhoneNumber;

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> AddContractToMerchant(ContractAddedToMerchantEvent domainEvent, CancellationToken cancellationToken)
    {
        EstateManagementGenericContext context = await this.GetContextFromDomainEvent(domainEvent, cancellationToken);

        MerchantContract merchantContract = new MerchantContract
        {
            MerchantId = domainEvent.MerchantId,
            ContractId = domainEvent.ContractId
        };

        await context.MerchantContracts.AddAsync(merchantContract, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<EstateManagementGenericContext> GetContextFromDomainEvent(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Guid estateId = Database.Contexts.DomainEventHelper.GetEstateId(domainEvent);
        if (estateId == Guid.Empty)
        {
            throw new Exception($"Unable to resolve context for Domain Event {domainEvent.GetType()}");
        }

        return await this.DbContextFactory.GetContext(estateId, EstateReportingRepository.ConnectionStringIdentifier, cancellationToken);
    }
    
    private async Task<Result> UpdateFileLineStatus(EstateManagementGenericContext context,
                                            Guid fileId,
                                            Int32 lineNumber,
                                            Guid transactionId,
                                            String newStatus,
                                            CancellationToken cancellationToken)
    {
        FileLine fileLine = await context.FileLines.SingleOrDefaultAsync(f => f.FileId == fileId && f.LineNumber == lineNumber, cancellationToken: cancellationToken);

        if (fileLine == null)
        {
            return Result.NotFound($"FileLine number {lineNumber} in File Id {fileId} not found");
        }

        fileLine.Status = newStatus;
        fileLine.TransactionId = transactionId;

        return await context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Others

    private const String ConnectionStringIdentifier = "EstateReportingReadModel";

    #endregion
}
using SimpleResults;

namespace EstateManagement.Repository{
    using System.Threading;
    using System.Threading.Tasks;
    using Contract.DomainEvents;
    using Estate.DomainEvents;
    using FileProcessor.File.DomainEvents;
    using FileProcessor.FileImportLog.DomainEvents;
    using Merchant.DomainEvents;
    using MerchantStatement.DomainEvents;
    using Operator.DomainEvents;
    using TransactionProcessor.Float.DomainEvents;
    using TransactionProcessor.Reconciliation.DomainEvents;
    using TransactionProcessor.Settlement.DomainEvents;
    using TransactionProcessor.Transaction.DomainEvents;
    using TransactionProcessor.Voucher.DomainEvents;

    public interface IEstateReportingRepository{
        #region Methods

        Task<Result> UpdateOperator(OperatorNameUpdatedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task<Result> UpdateOperator(OperatorRequireCustomMerchantNumberChangedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateOperator(OperatorRequireCustomTerminalNumberChangedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> AddOperator(OperatorCreatedEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> AddContract(ContractCreatedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task<Result> AddContractProduct(VariableValueProductAddedToContractEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddContractProduct(FixedValueProductAddedToContractEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddContractProductTransactionFee(TransactionFeeForProductAddedToContractEvent domainEvent,
                                                      CancellationToken cancellationToken);

        Task<Result> AddContractToMerchant(ContractAddedToMerchantEvent domainEvent,
                                           CancellationToken cancellationToken);

        Task<Result> AddEstate(EstateCreatedEvent domainEvent,
                               CancellationToken cancellationToken);

        Task<Result> AddEstateSecurityUser(SecurityUserAddedToEstateEvent domainEvent,
                                           CancellationToken cancellationToken);

        Task<Result> AddFile(FileCreatedEvent domainEvent,
                             CancellationToken cancellationToken);

        Task<Result> AddFileImportLog(ImportLogCreatedEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task<Result> AddFileLineToFile(FileLineAddedEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task<Result> AddFileToImportLog(FileAddedToImportLogEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddGeneratedVoucher(VoucherGeneratedEvent domainEvent,
                                         CancellationToken cancellationToken);

        Task<Result> AddMerchant(MerchantCreatedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(MerchantNameUpdatedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> AddMerchantAddress(AddressAddedEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddMerchantContact(ContactAddedEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task<Result> SwapMerchantDevice(DeviceSwappedForMerchantEvent domainEvent,
                                        CancellationToken cancellationToken);

        Task<Result> AddMerchantOperator(OperatorAssignedToMerchantEvent domainEvent,
                                         CancellationToken cancellationToken);

        Task<Result> AddMerchantSecurityUser(SecurityUserAddedToMerchantEvent domainEvent,
                                             CancellationToken cancellationToken);

        Task<Result> AddPendingMerchantFeeToSettlement(MerchantFeeAddedPendingSettlementEvent domainEvent,
                                                       CancellationToken cancellationToken);

        Task<Result> AddProductDetailsToTransaction(ProductDetailsAddedToTransactionEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> AddSettledFeeToStatement(SettledFeeAddedToStatementEvent domainEvent,
                                              CancellationToken cancellationToken);

        Task<Result> AddSettledMerchantFeeToSettlement(SettledMerchantFeeAddedToTransactionEvent domainEvent,
                                                       CancellationToken cancellationToken);

        Task<Result> AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                                   CancellationToken cancellationToken);

        Task<Result> AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task<Result> CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                            CancellationToken cancellationToken);

        Task<Result> CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                         CancellationToken cancellationToken);

        Task<Result> CreateFloat(FloatCreatedForContractProductEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> CreateFloatActivity(FloatCreditPurchasedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> CreateFloatActivity(FloatDecreasedByTransactionEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> CreateReadModel(EstateCreatedEvent domainEvent,
                                     CancellationToken cancellationToken);

        Task<Result> CreateSettlement(SettlementCreatedForDateEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task<Result> CreateStatement(StatementCreatedEvent domainEvent,
                                     CancellationToken cancellationToken);

        Task<Result> DisableContractProductTransactionFee(TransactionFeeForProductDisabledEvent domainEvent,
                                                          CancellationToken cancellationToken);

        Task<Result> MarkMerchantFeeAsSettled(MerchantFeeSettledEvent domainEvent,
                                              CancellationToken cancellationToken);

        Task<Result> MarkSettlementAsCompleted(SettlementCompletedEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task<Result> MarkSettlementAsProcessingStarted(SettlementProcessingStartedEvent domainEvent,
                                                       CancellationToken cancellationToken);

        Task<Result> MarkStatementAsGenerated(StatementGeneratedEvent domainEvent,
                                              CancellationToken cancellationToken);

        Task<Result> RecordTransactionAdditionalRequestData(AdditionalRequestDataRecordedEvent domainEvent,
                                                            CancellationToken cancellationToken);

        Task<Result> RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                             CancellationToken cancellationToken);

        Task<Result> SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
                                          CancellationToken cancellationToken);

        Task<Result> StartReconciliation(ReconciliationHasStartedEvent domainEvent,
                                         CancellationToken cancellationToken);

        Task<Result> StartTransaction(TransactionHasStartedEvent domainEvent,
                                      CancellationToken cancellationToken);

        Task<Result> UpdateEstate(EstateReferenceAllocatedEvent domainEvent,
                                  CancellationToken cancellationToken);

        Task<Result> UpdateFileAsComplete(FileProcessingCompletedEvent domainEvent,
                                          CancellationToken cancellationToken);

        Task<Result> UpdateFileLine(FileLineProcessingSuccessfulEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateFileLine(FileLineProcessingFailedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateFileLine(FileLineProcessingIgnoredEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(MerchantReferenceAllocatedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(StatementGeneratedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(SettlementScheduleChangedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(TransactionHasBeenCompletedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateReconciliationOverallTotals(OverallTotalsRecordedEvent domainEvent,
                                                       CancellationToken cancellationToken);

        Task<Result> UpdateReconciliationStatus(ReconciliationHasBeenLocallyAuthorisedEvent domainEvent,
                                                CancellationToken cancellationToken);

        Task<Result> UpdateReconciliationStatus(ReconciliationHasBeenLocallyDeclinedEvent domainEvent,
                                                CancellationToken cancellationToken);

        Task<Result> UpdateTransactionAuthorisation(TransactionHasBeenLocallyAuthorisedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> UpdateTransactionAuthorisation(TransactionHasBeenLocallyDeclinedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> UpdateTransactionAuthorisation(TransactionAuthorisedByOperatorEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> UpdateTransactionAuthorisation(TransactionDeclinedByOperatorEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> UpdateVoucherIssueDetails(VoucherIssuedEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task<Result> UpdateVoucherRedemptionDetails(VoucherFullyRedeemedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        Task<Result> RemoveOperatorFromMerchant(OperatorRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> RemoveContractFromMerchant(ContractRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> UpdateMerchantAddress(MerchantAddressLine1UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantAddressLine2UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantAddressLine3UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantAddressLine4UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantCountyUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantRegionUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantTownUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantPostalCodeUpdatedEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> UpdateMerchantContact(MerchantContactNameUpdatedEvent domainEvent, CancellationToken cancellationToken);

        Task<Result> UpdateMerchantContact(MerchantContactEmailAddressUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantContact(MerchantContactPhoneNumberUpdatedEvent domainEvent, CancellationToken cancellationToken);



        #endregion
    }
}
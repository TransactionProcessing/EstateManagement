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

        Task UpdateOperator(OperatorNameUpdatedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateOperator(OperatorRequireCustomMerchantNumberChangedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task UpdateOperator(OperatorRequireCustomTerminalNumberChangedEvent domainEvent,
                            CancellationToken cancellationToken);

        Task AddOperator(OperatorCreatedEvent domainEvent, CancellationToken cancellationToken);

        Task AddContract(ContractCreatedEvent domainEvent,
                         CancellationToken cancellationToken);

        Task AddContractProduct(VariableValueProductAddedToContractEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddContractProduct(FixedValueProductAddedToContractEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddContractProductTransactionFee(TransactionFeeForProductAddedToContractEvent domainEvent,
                                              CancellationToken cancellationToken);

        Task AddContractToMerchant(ContractAddedToMerchantEvent domainEvent,
                                   CancellationToken cancellationToken);

        Task AddEstate(EstateCreatedEvent domainEvent,
                       CancellationToken cancellationToken);

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

        Task UpdateMerchant(MerchantNameUpdatedEvent domainEvent,
                         CancellationToken cancellationToken);

        Task AddMerchantAddress(AddressAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddMerchantContact(ContactAddedEvent domainEvent,
                                CancellationToken cancellationToken);

        Task AddMerchantDevice(DeviceAddedToMerchantEvent domainEvent,
                               CancellationToken cancellationToken);

        Task SwapMerchantDevice(DeviceSwappedForMerchantEvent domainEvent,
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

        Task AddSettledMerchantFeeToSettlement(SettledMerchantFeeAddedToTransactionEvent domainEvent,
                                               CancellationToken cancellationToken);

        Task AddSourceDetailsToTransaction(TransactionSourceAddedToTransactionEvent domainEvent,
                                           CancellationToken cancellationToken);

        Task AddTransactionToStatement(TransactionAddedToStatementEvent domainEvent,
                                       CancellationToken cancellationToken);

        Task CompleteReconciliation(ReconciliationHasCompletedEvent domainEvent,
                                    CancellationToken cancellationToken);

        Task CompleteTransaction(TransactionHasBeenCompletedEvent domainEvent,
                                 CancellationToken cancellationToken);

        Task CreateFloat(FloatCreatedForContractProductEvent domainEvent, CancellationToken cancellationToken);
        Task CreateFloatActivity(FloatCreditPurchasedEvent domainEvent, CancellationToken cancellationToken);
        Task CreateFloatActivity(FloatDecreasedByTransactionEvent domainEvent, CancellationToken cancellationToken);

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

        Task RecordTransactionAdditionalResponseData(AdditionalResponseDataRecordedEvent domainEvent,
                                                     CancellationToken cancellationToken);

        Task SetTransactionAmount(AdditionalRequestDataRecordedEvent domainEvent,
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

        Task RemoveOperatorFromMerchant(OperatorRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken);

        Task RemoveContractFromMerchant(ContractRemovedFromMerchantEvent domainEvent, CancellationToken cancellationToken);

        Task UpdateMerchantAddress(MerchantAddressLine1UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantAddressLine2UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantAddressLine3UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantAddressLine4UpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantCountyUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantRegionUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantTownUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantAddress(MerchantPostalCodeUpdatedEvent domainEvent, CancellationToken cancellationToken);

        Task UpdateMerchantContact(MerchantContactNameUpdatedEvent domainEvent, CancellationToken cancellationToken);

        Task UpdateMerchantContact(MerchantContactEmailAddressUpdatedEvent domainEvent, CancellationToken cancellationToken);
        Task UpdateMerchantContact(MerchantContactPhoneNumberUpdatedEvent domainEvent, CancellationToken cancellationToken);



        #endregion
    }
}
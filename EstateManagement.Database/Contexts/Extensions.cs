namespace EstateManagement.Database.Contexts;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViewEntities;

public static class Extensions{
    #region Methods

    /// <summary>
    /// Decimals the precision.
    /// </summary>
    /// <param name="propertyBuilder">The property builder.</param>
    /// <param name="precision">The precision.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    public static PropertyBuilder DecimalPrecision(this PropertyBuilder propertyBuilder,
                                                   Int32 precision,
                                                   Int32 scale){
        return propertyBuilder.HasColumnType($"decimal({precision},{scale})");
    }

    public static PropertyBuilder IsDateOnly(this PropertyBuilder propertyBuilder){
        return propertyBuilder.HasColumnType("date");
    }

    public static ModelBuilder SetupContract(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Contract>().HasKey(c => new {
                                                            c.EstateReportingId,
                                                            c.OperatorId,
                                                            c.ContractId
                                                        });

        return modelBuilder;
    }

    public static ModelBuilder SetupContractProduct(this ModelBuilder modelBuilder){
        modelBuilder.Entity<ContractProduct>().HasKey(c => new {
                                                                   c.ContractReportingId,
                                                                   c.ProductId
                                                               });

        return modelBuilder;
    }

    public static ModelBuilder SetupContractProductTransactionFee(this ModelBuilder modelBuilder){
        modelBuilder.Entity<ContractProductTransactionFee>().HasKey(c => new {
                                                                                 c.ContractProductReportingId,
                                                                                 c.TransactionFeeId
                                                                             });

        modelBuilder.Entity<ContractProductTransactionFee>().Property(p => p.Value).DecimalPrecision(18, 4);

        return modelBuilder;
    }

    public static ModelBuilder SetupEstate(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Estate>().HasKey(t => new{
                                                         t.EstateReportingId
                                                     });

        modelBuilder.Entity<Estate>().HasIndex(t => new{
                                                           t.EstateId
                                                       }).IsUnique();

        return modelBuilder;
    }

    public static ModelBuilder SetupOperator(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>().HasKey(t => new {
                                                          t.OperatorReportingId
                                                      });

        modelBuilder.Entity<Operator>().HasIndex(t => new {
                                                              t.OperatorId
                                                          }).IsUnique();

        return modelBuilder;
    }

    public static ModelBuilder SetupEstateOperator(this ModelBuilder modelBuilder){
        modelBuilder.Entity<EstateOperator>().HasKey(t => new{
                                                                 t.EstateReportingId,
                                                                 t.OperatorReportingId
                                                             });
        return modelBuilder;
    }

    public static ModelBuilder SetupEstateSecurityUser(this ModelBuilder modelBuilder){
        modelBuilder.Entity<EstateSecurityUser>().HasKey(t => new{
                                                                     t.SecurityUserId,
                                                                     t.EstateReportingId
                                                                 });
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchant(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Merchant>().HasKey(t => new {
                                                            t.EstateReportingId,
                                                            t.MerchantReportingId
        });

        modelBuilder.Entity<Merchant>().HasIndex(t => new {
                                                              t.EstateReportingId,
                                                              t.MerchantId
                                                          }).IsUnique();

        modelBuilder.Entity<Merchant>(e => { e.Property(p => p.LastSaleDate).IsDateOnly(); });
        
        return modelBuilder;
    }
    
    public static ModelBuilder SetupMerchantAddress(this ModelBuilder modelBuilder){
        modelBuilder.Entity<MerchantAddress>().HasKey(t => new {
                                                                   t.MerchantReportingId,
                                                                   t.AddressId
                                                               });
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantContact(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MerchantContact>().HasKey(t => new {
                                                                   t.MerchantReportingId,
                                                                   t.ContactId
                                                               });
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantDevice(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MerchantDevice>().HasKey(t => new {
                                                                  t.MerchantReportingId,
                                                                  t.DeviceId
                                                              });
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantSecurityUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MerchantSecurityUser>().HasKey(t => new {
                                                                        t.MerchantReportingId,
                                                                        t.SecurityUserId
                                                                    });
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantOperator(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MerchantOperator>().HasKey(t => new {
                                                                    t.MerchantReportingId,
                                                                    t.OperatorId
                                                                });
        return modelBuilder;
    }
    
    public static ModelBuilder SetupResponseCodes(this ModelBuilder modelBuilder){
        modelBuilder.Entity<ResponseCodes>().HasKey(r => new{
                                                                r.ResponseCode
                                                            });
        return modelBuilder;
    }

    public static ModelBuilder SetupSettlement(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Settlement>().HasKey(s => new {
                                                              s.SettlementReportingId
                                                          }).IsClustered(false);

        modelBuilder.Entity<Settlement>().HasIndex(s => new {
                                                                s.EstateReportingId,
                                                                s.SettlementId
                                                            }).IsClustered(false).IsUnique(true);

        modelBuilder.Entity<Settlement>().HasIndex(s => new {
                                                                s.SettlementDate,
                                                                s.EstateReportingId,
                                                            }).IsClustered(true);

        modelBuilder.Entity<Settlement>(e => { e.Property(p => p.SettlementDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantSettlementFee(this ModelBuilder modelBuilder){
        

        modelBuilder.Entity<MerchantSettlementFee>().HasKey(s => new{
                                                                        s.SettlementReportingId,
                                                                        s.TransactionReportingId,
                                                                        s.TransactionFeeReportingId
                                                                    });

        modelBuilder.Entity<MerchantSettlementFee>().HasIndex(s => new{
                                                                          s.TransactionReportingId
                                                                      }).IsUnique(false);

        return modelBuilder;
    }

    public static ModelBuilder SetupTransaction(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Transaction>().HasKey(t => new {
                                                               t.TransactionReportingId,
                                                           }).IsClustered(false);

        modelBuilder.Entity<Transaction>().HasIndex(t => new {
                                                                 t.TransactionId
                                                             }).IsClustered(false).IsUnique(true);

        modelBuilder.Entity<Transaction>().HasIndex(t => new {
                                                                 t.TransactionId,
                                                                 t.MerchantReportingId,
                                                             }).IsClustered(false).IsUnique(true);

        modelBuilder.Entity<Transaction>().HasIndex(t => new {
                                                                 t.TransactionDate,
                                                                 t.MerchantReportingId,
                                                                 }).IsClustered(true);

        modelBuilder.Entity<Transaction>(e => { e.Property(p => p.TransactionDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupTransactionAdditionalRequestData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.TransactionReportingId
                                                                                });


        return modelBuilder;
    }

    public static ModelBuilder SetupTransactionAdditionalResponseData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionAdditionalResponseData>().HasKey(t => new {
                                                                                     t.TransactionReportingId
                                                                                 });


        return modelBuilder;
    }

    public static ModelBuilder SetupVoucher(this ModelBuilder modelBuilder){
        modelBuilder.Entity<Voucher>().HasKey(t => new{
                                                          t.VoucherId
                                                      });

        modelBuilder.Entity<Voucher>().HasIndex(t => new {
                                                           t.VoucherCode
                                                       });

        modelBuilder.Entity<Voucher>().HasIndex(t => new {
                                                             t.TransactionReportingId
                                                         });

        modelBuilder.Entity<Voucher>(e => { e.Property(p => p.IssuedDate).IsDateOnly(); });
        modelBuilder.Entity<Voucher>(e => { e.Property(p => p.GenerateDate).IsDateOnly(); });
        modelBuilder.Entity<Voucher>(e => { e.Property(p => p.ExpiryDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupReconciliation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reconciliation>().HasKey(t => new {
                                                               t.TransactionReportingId,
                                                           }).IsClustered(false);

        modelBuilder.Entity<Reconciliation>().HasIndex(t => new {
                                                                    t.TransactionId,
                                                                    t.MerchantReportingId,
                                                                }).IsClustered(false).IsUnique(true);

        modelBuilder.Entity<Reconciliation>().HasIndex(t => new {
                                                                    t.TransactionDate,
                                                                    t.MerchantReportingId,
                                                                }).IsClustered(true);

        modelBuilder.Entity<Reconciliation>(e => { e.Property(p => p.TransactionDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupStatementHeader(this ModelBuilder modelBuilder){
        modelBuilder.Entity<StatementHeader>().HasKey(s => new {
                                                                   s.MerchantReportingId,
                                                                   s.StatementId
                                                               }).IsClustered(false);

        modelBuilder.Entity<StatementHeader>().HasIndex(s => new {
                                                                     s.MerchantReportingId,
                                                                     s.StatementGeneratedDate,
                                                                 }).IsClustered();

        modelBuilder.Entity<StatementHeader>(e => { e.Property(p => p.StatementGeneratedDate).IsDateOnly(); });
        modelBuilder.Entity<StatementHeader>(e => { e.Property(p => p.StatementCreatedDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupStatementLine(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatementLine>().HasKey(t => new {
                                                                 t.StatementReportingId,
                                                                 t.TransactionReportingId,
                                                                 t.ActivityDateTime,
                                                                 t.ActivityType
                                                             });
        
        modelBuilder.Entity<StatementLine>(e => { e.Property(p => p.ActivityDate).IsDateOnly(); });
        

        return modelBuilder;
    }

    public static ModelBuilder SetupViewEntities(this ModelBuilder modelBuilder){
        modelBuilder.Entity<SettlementView>().HasNoKey().ToView("uvwSettlements");

        return modelBuilder;
    }

    public static ModelBuilder SetupFileImportLog(this ModelBuilder modelBuilder){
        modelBuilder.Entity<FileImportLog>().HasKey(f => new{
                                                                f.EstateReportingId,
                                                                f.FileImportLogReportingId
                                                            });

        modelBuilder.Entity<FileImportLog>().HasIndex(f => new {
                                                                   f.EstateReportingId,
                                                                   f.FileImportLogId
                                                               }).IsUnique();

        modelBuilder.Entity<FileImportLog>(e => { e.Property(p => p.ImportLogDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupFileImportLogFile(this ModelBuilder modelBuilder){
        modelBuilder.Entity<FileImportLogFile>().HasKey(f => new {
                                                                     f.FileImportLogReportingId,
                                                                     f.FileReportingId,
                                                                 });

        modelBuilder.Entity<FileImportLogFile>(e => { e.Property(p => p.FileUploadedDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupFile(this ModelBuilder modelBuilder){
        modelBuilder.Entity<File>().HasKey(f => new {
                                                        f.FileReportingId
                                                    });

        modelBuilder.Entity<File>().HasIndex(f => new {
                                                          f.FileId
                                                      }).IsUnique();

        modelBuilder.Entity<File>(e => { e.Property(p => p.FileReceivedDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupFileLine(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileLine>().HasKey(f => new {
            f.FileReportingId,
            f.LineNumber
        }).IsClustered();

        modelBuilder.Entity<FileLine>().HasIndex(f => new {
            f.TransactionReportingId
        }).IsUnique(false);
        
        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantContract(this ModelBuilder modelBuilder){
        modelBuilder.Entity<MerchantContract>().HasKey(mc => new{
                                                                    mc.MerchantReportingId,
                                                                    mc.ContractReportingId
                                                                });

        return modelBuilder;
    }

    public static ModelBuilder SetupFloat(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Float>().HasKey(t => new {
                                                           t.FloatId
                                                       }).IsClustered(false);

        modelBuilder.Entity<Float>().HasIndex(t => new {
                                                           t.CreatedDate
                                                       }).IsClustered(true);
        
        modelBuilder.Entity<Float>(e => { e.Property(p => p.CreatedDate).IsDateOnly(); });

        return modelBuilder;
    }

    public static ModelBuilder SetupFloatActivity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FloatActivity>().HasKey(t => new {
                                                         t.EventId
                                                     }).IsClustered(false);

        modelBuilder.Entity<FloatActivity>().HasIndex(t => new {
                                                                   t.ActivityDate
                                                               }).IsClustered(true);

        modelBuilder.Entity<FloatActivity>(e => { e.Property(p => p.ActivityDate).IsDateOnly(); });

        return modelBuilder;
    }

    #endregion
}
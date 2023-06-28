namespace EstateManagement.Database.Contexts;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViewEntities;

public static class Extensions
{
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
                                                   Int32 scale)
    {
        return propertyBuilder.HasColumnType($"decimal({precision},{scale})");
    }

    public static ModelBuilder SetupResponseCodesTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResponseCodes>().HasKey(r => new {
                                                                 r.ResponseCode
                                                             });
        return modelBuilder;
    }

    public static ModelBuilder SetupEstateTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estate>().HasKey(t => new {
                                                          t.EstateId
                                                      }).IsClustered(false);

        modelBuilder.Entity<Estate>().HasIndex(t => new {
                                                          t.EstateReportingId
                                                      }).IsClustered(true).IsUnique(true);

        modelBuilder.Entity<EstateSecurityUser>().HasKey(t => new {
                                                                      t.SecurityUserId,
                                                                      t.EstateReportingId
                                                                  });

        modelBuilder.Entity<EstateOperator>().HasKey(t => new {
                                                                  t.EstateReportingId,
                                                                  t.OperatorId
                                                              });


        return modelBuilder;
    }

    public static ModelBuilder SetupFileTables(this ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<FileImportLog>().HasKey(f => new {
                                                                 f.EstateReportingId,
                                                                 f.FileImportLogId
                                                             }).IsClustered(false);

        modelBuilder.Entity<FileImportLog>().HasIndex(f => new{
                                                                  f.EstateReportingId,
                                                                  f.FileImportLogReportingId,
                                                                  f.ImportLogDateTime
                                                              }).IsClustered(true).IsUnique(true);


        modelBuilder.Entity<FileImportLogFile>().HasKey(f => new{
                                                                    f.FileImportLogReportingId,
                                                                    f.FileReportingId,
                                                                });

        modelBuilder.Entity<File>().HasKey(f => new{
                                                       f.EstateReportingId,
                                                       f.FileImportLogReportingId,
                                                       f.FileId
                                                   }).IsClustered(false);

        modelBuilder.Entity<File>().HasIndex(f => new {
                                                        f.EstateReportingId,
                                                        f.FileImportLogReportingId,
                                                        f.FileReportingId
                                                    }).IsClustered(true).IsUnique(true);

        modelBuilder.Entity<FileLine>().HasKey(f => new {
                                                            f.FileReportingId,
                                                            f.LineNumber
                                                        }).IsClustered(true);

        modelBuilder.Entity<FileLine>().HasIndex(f => new {
                                                            f.TransactionReportingId
                                                        }).IsUnique(false);

        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Merchant>().HasKey(t => new {
                                                            t.EstateReportingId,
                                                            t.MerchantId
                                                        }).IsClustered(false);

        modelBuilder.Entity<Merchant>().HasIndex(t => new{
                                                             t.EstateReportingId,
                                                             t.MerchantReportingId
                                                         }).IsClustered(true).IsUnique(true);

        modelBuilder.Entity<MerchantAddress>().HasKey(t => new {
                                                                   t.MerchantReportingId,
                                                                   t.AddressId
                                                               });

        modelBuilder.Entity<MerchantContact>().HasKey(t => new {
                                                                   t.MerchantReportingId,
                                                                   t.ContactId
                                                               });

        modelBuilder.Entity<MerchantDevice>().HasKey(t => new {
                                                                  t.MerchantReportingId,
                                                                  t.DeviceId
                                                              });

        modelBuilder.Entity<MerchantSecurityUser>().HasKey(t => new {
                                                                        t.MerchantReportingId,
                                                                        t.SecurityUserId
                                                                    });
        
        modelBuilder.Entity<MerchantOperator>().HasKey(t => new {
                                                                    t.MerchantReportingId,
                                                                    t.OperatorId
                                                                });
        
        return modelBuilder;
    }

    public static ModelBuilder SetupTransactionTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasKey(t => new {
                                                               t.MerchantReportingId,
                                                               t.TransactionId,
                                                           }).IsClustered(false);

        modelBuilder.Entity<Transaction>().HasIndex(t => new {
                                                               t.TransactionDate,
                                                               t.MerchantReportingId
                                                           }).IsClustered(true);

        modelBuilder.Entity<TransactionFee>().HasKey(t => new {
                                                                  t.TransactionReportingId,
                                                                  t.TransactionFeeReportingId
                                                              });

        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.TransactionReportingId
                                                                                });

        modelBuilder.Entity<TransactionAdditionalResponseData>().HasKey(t => new {
                                                                                    t.TransactionReportingId
                                                                                });

        modelBuilder.Entity<Reconciliation>().HasKey(t => new {
                                                               t.TransactionId,
                                                           }).IsClustered(false);

        modelBuilder.Entity<Reconciliation>().HasIndex(t => new {
                                                                    t.TransactionDate,
                                                                    t.MerchantReportingId
                                                                }).IsClustered(true);

        return modelBuilder;
    }

    public static ModelBuilder SetupContractTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>().HasKey(c => new {
                                                            c.EstateReportingId,
                                                            c.OperatorId,
                                                            c.ContractId
                                                        });

        modelBuilder.Entity<ContractProduct>().HasKey(c => new {
                                                                   c.ContractReportingId,
                                                                   c.ContractProductReportingId
                                                               });

        modelBuilder.Entity<ContractProductTransactionFee>().HasKey(c => new{
                                                                                c.ContractProductReportingId,
                                                                                c.TransactionFeeReportingId
                                                                            });

        modelBuilder.Entity<ContractProductTransactionFee>().Property(p => p.Value).DecimalPrecision(18, 4);

        return modelBuilder;
    }

    public static ModelBuilder SetupSettlementTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Settlement>().HasKey(s => new {
                                                              s.EstateReportingId,
                                                              s.SettlementId
                                                          }).IsClustered(false);

        modelBuilder.Entity<Settlement>().HasIndex(s => new {
                                                              s.SettlementDate,
                                                              s.EstateReportingId,
                                                          }).IsClustered(true).IsUnique(true);

        modelBuilder.Entity<MerchantSettlementFee>().HasKey(s => new {
                                                                         s.SettlementReportingId,
                                                                         s.TransactionReportingId,
                                                                         s.TransactionFeeReportingId
                                                                     });
        return modelBuilder;
    }

    public static ModelBuilder SetupStatementTables(this ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<StatementHeader>().HasKey(t => new {
        //                                                           t.StatementId
        //                                                       });

        modelBuilder.Entity<StatementHeader>().HasKey(s => new {
                                                                   s.MerchantReportingId,
                                                                   s.StatementId
                                                               }).IsClustered(false);

        modelBuilder.Entity<StatementHeader>().HasIndex(s => new {
                                                                     s.MerchantReportingId,
                                                                     s.StatementGeneratedDate,
                                                                 }).IsClustered(true).IsUnique(true);
        
        modelBuilder.Entity<StatementLine>().HasKey(t => new {
                                                                 t.StatementReportingId,
                                                                 t.TransactionReportingId,
                                                                 t.ActivityDateTime,
                                                                 t.ActivityType
                                                             });
        return modelBuilder;
    }

    public static ModelBuilder SetupViewEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SettlementView>().HasNoKey().ToView("uvwSettlements");

        return modelBuilder;
    }

    #endregion
}
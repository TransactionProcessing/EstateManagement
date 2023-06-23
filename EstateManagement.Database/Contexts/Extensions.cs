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
                                                          t.EstateReportingId
                                                      }).IsClustered(true);

        modelBuilder.Entity<Estate>().HasIndex(t => new {
                                                          t.EstateId
                                                      }).IsClustered(false).IsUnique(true);

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
                                                             });

        modelBuilder.Entity<FileImportLogFile>().HasKey(f => new {
                                                                     f.FileImportLogId,
                                                                     f.FileId
                                                                 });

        modelBuilder.Entity<File>().HasKey(f => new {
                                                        f.EstateReportingId,
                                                        f.FileImportLogId,
                                                        f.FileId
                                                    });

        modelBuilder.Entity<FileLine>().HasKey(f => new {
                                                            f.FileId,
                                                            f.LineNumber
                                                        });

        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Merchant>().HasKey(t => new {
                                                            t.EstateReportingId,
                                                            t.MerchantId
                                                        });

        modelBuilder.Entity<MerchantAddress>().HasKey(t => new {
                                                                   t.MerchantId,
                                                                   t.AddressId
                                                               });

        modelBuilder.Entity<MerchantContact>().HasKey(t => new {
                                                                   t.MerchantId,
                                                                   t.ContactId
                                                               });

        modelBuilder.Entity<MerchantDevice>().HasKey(t => new {
                                                                  t.MerchantId,
                                                                  t.DeviceId
                                                              });

        modelBuilder.Entity<MerchantSecurityUser>().HasKey(t => new {
                                                                        t.MerchantId,
                                                                        t.SecurityUserId
                                                                    });


        modelBuilder.Entity<MerchantOperator>().HasKey(t => new {
                                                                    t.MerchantId,
                                                                    t.OperatorId
                                                                });
        
        return modelBuilder;
    }

    public static ModelBuilder SetupTransactionTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasKey(t => new {
                                                               t.MerchantId,
                                                               t.TransactionId
                                                           });

        modelBuilder.Entity<TransactionFee>().HasKey(t => new {
                                                                  t.TransactionId,
                                                                  t.FeeId
                                                              });

        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.MerchantId,
                                                                                    t.TransactionId
                                                                                });

        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.MerchantId,
                                                                                    t.TransactionId
                                                                                });



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
                                                                   c.ContractId,
                                                                   c.ProductId
                                                               });

        modelBuilder.Entity<ContractProductTransactionFee>().HasKey(c => new {
                                                                   c.ContractId,
                                                                                 c.ProductId,
                                                                                 c.TransactionFeeId
                                                                             });

        modelBuilder.Entity<ContractProductTransactionFee>().Property(p => p.Value).DecimalPrecision(18, 4);

        return modelBuilder;
    }

    public static ModelBuilder SetupSettlementTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Settlement>().HasKey(s => new {
                                                              s.EstateReportingId,
                                                              s.SettlementId
                                                          });

        modelBuilder.Entity<MerchantSettlementFee>().HasKey(s => new {
                                                                         s.SettlementId,
                                                                         s.TransactionId,
                                                                         s.FeeId
                                                                     });
        return modelBuilder;
    }

    public static ModelBuilder SetupStatementTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatementHeader>().HasKey(t => new {
                                                                   t.StatementId
                                                               });

        modelBuilder.Entity<StatementLine>().HasKey(t => new {
                                                                 t.StatementId,
                                                                 t.TransactionId,
                                                                 t.ActivityDateTime,
                                                                 t.ActivityType
                                                             });
        return modelBuilder;
    }

    public static ModelBuilder SetupViewEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionsView>().HasNoKey().ToView("uvwTransactions");
        modelBuilder.Entity<FileImportLogView>().HasNoKey().ToView("uvwFileImportLog");
        modelBuilder.Entity<FileView>().HasNoKey().ToView("uvwFile");
        modelBuilder.Entity<SettlementView>().HasNoKey().ToView("uvwSettlements");

        return modelBuilder;
    }

    #endregion
}
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
                                                      });

        modelBuilder.Entity<EstateSecurityUser>().HasKey(t => new {
                                                                      t.SecurityUserId,
                                                                      t.EstateId
                                                                  });

        modelBuilder.Entity<EstateOperator>().HasKey(t => new {
                                                                  t.EstateId,
                                                                  t.OperatorId
                                                              });


        return modelBuilder;
    }

    public static ModelBuilder SetupFileTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileImportLog>().HasKey(f => new {
                                                                 f.EstateId,
                                                                 f.FileImportLogId
                                                             });

        modelBuilder.Entity<FileImportLogFile>().HasKey(f => new {
                                                                     f.EstateId,
                                                                     f.FileImportLogId,
                                                                     f.FileId
                                                                 });

        modelBuilder.Entity<File>().HasKey(f => new {
                                                        f.EstateId,
                                                        f.FileImportLogId,
                                                        f.FileId
                                                    });

        modelBuilder.Entity<FileLine>().HasKey(f => new {
                                                            f.EstateId,
                                                            f.FileId,
                                                            f.LineNumber
                                                        });

        return modelBuilder;
    }

    public static ModelBuilder SetupMerchantTables(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Merchant>().HasKey(t => new {
                                                            t.EstateId,
                                                            t.MerchantId
                                                        });

        modelBuilder.Entity<MerchantAddress>().HasKey(t => new {
                                                                   t.EstateId,
                                                                   t.MerchantId,
                                                                   t.AddressId
                                                               });

        modelBuilder.Entity<MerchantContact>().HasKey(t => new {
                                                                   t.EstateId,
                                                                   t.MerchantId,
                                                                   t.ContactId
                                                               });

        modelBuilder.Entity<MerchantDevice>().HasKey(t => new {
                                                                  t.EstateId,
                                                                  t.MerchantId,
                                                                  t.DeviceId
                                                              });

        modelBuilder.Entity<MerchantSecurityUser>().HasKey(t => new {
                                                                        t.EstateId,
                                                                        t.MerchantId,
                                                                        t.SecurityUserId
                                                                    });


        modelBuilder.Entity<MerchantOperator>().HasKey(t => new {
                                                                    t.EstateId,
                                                                    t.MerchantId,
                                                                    t.OperatorId
                                                                });
        
        return modelBuilder;
    }

    public static ModelBuilder SetupTransactionTables(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Transaction>().HasKey(t => new {
                                                               t.EstateId,
                                                               t.MerchantId,
                                                               t.TransactionId
                                                           });

        modelBuilder.Entity<TransactionFee>().HasKey(t => new {
                                                                  t.TransactionId,
                                                                  t.FeeId
                                                              });

        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.EstateId,
                                                                                    t.MerchantId,
                                                                                    t.TransactionId
                                                                                });

        modelBuilder.Entity<TransactionAdditionalRequestData>().HasKey(t => new {
                                                                                    t.EstateId,
                                                                                    t.MerchantId,
                                                                                    t.TransactionId
                                                                                });



        return modelBuilder;
    }

    public static ModelBuilder SetupContractTables(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Contract>().HasKey(c => new {
                                                            c.EstateId,
                                                            c.OperatorId,
                                                            c.ContractId
                                                        });

        modelBuilder.Entity<ContractProduct>().HasKey(c => new {
                                                                   c.EstateId,
                                                                   c.ContractId,
                                                                   c.ProductId
                                                               });

        modelBuilder.Entity<ContractProductTransactionFee>().HasKey(c => new {
                                                                                 c.EstateId,
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
                                                              s.EstateId,
                                                              s.SettlementId
                                                          });

        modelBuilder.Entity<MerchantSettlementFee>().HasKey(s => new {
                                                                         s.EstateId,
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
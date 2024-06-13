using EstateManagement.Database.Entities.Summary;

namespace EstateManagement.Database.Contexts;

using System.Reflection;
using Entities;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Shared.Logger;
using ViewEntities;
public abstract class EstateManagementGenericContext : DbContext
{
    #region Fields

    protected readonly String ConnectionString;

    protected readonly String DatabaseEngine;

    protected static List<String> TablesToIgnoreDuplicates = new List<String>();

    #endregion

    #region Constructors

    protected EstateManagementGenericContext(String databaseEngine,
                                            String connectionString)
    {
        this.DatabaseEngine = databaseEngine;
        this.ConnectionString = connectionString;
    }

    public EstateManagementGenericContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    #endregion

    #region Properties

    public DbSet<Calendar> Calendar { get; set; }

    public DbSet<ContractProduct> ContractProducts { get; set; }

    public DbSet<ContractProductTransactionFee> ContractProductTransactionFees { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Operator> Operators { get; set; }

    public DbSet<Estate> Estates { get; set; }
    
    public DbSet<EstateSecurityUser> EstateSecurityUsers { get; set; }
    
    public virtual DbSet<FileImportLogFile> FileImportLogFiles { get; set; }
    
    public virtual DbSet<FileImportLog> FileImportLogs { get; set; }
    
    public virtual DbSet<FileLine> FileLines { get; set; }

    public virtual DbSet<File> Files { get; set; }
    public DbSet<Float> Floats { get; set; }
    public DbSet<FloatActivity> FloatActivity { get; set; }

    public DbSet<MerchantAddress> MerchantAddresses { get; set; }

    public DbSet<MerchantContact> MerchantContacts { get; set; }

    public DbSet<MerchantDevice> MerchantDevices { get; set; }

    public DbSet<MerchantOperator> MerchantOperators { get; set; }
    
    public DbSet<Merchant> Merchants { get; set; }

    public DbSet<MerchantSecurityUser> MerchantSecurityUsers { get; set; }

    public DbSet<MerchantSettlementFee> MerchantSettlementFees { get; set; }
    
    public DbSet<Reconciliation> Reconciliations { get; set; }

    public DbSet<ResponseCodes> ResponseCodes { get; set; }

    public DbSet<Settlement> Settlements { get; set; }

    public virtual DbSet<SettlementView> SettlementsView { get; set; }

    public DbSet<StatementHeader> StatementHeaders { get; set; }

    public DbSet<StatementLine> StatementLines { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<TransactionAdditionalRequestData> TransactionsAdditionalRequestData { get; set; }

    public DbSet<TransactionAdditionalResponseData> TransactionsAdditionalResponseData { get; set; }

    public DbSet<Voucher> Vouchers { get; set; }

    public DbSet<MerchantContract> MerchantContracts { get; set; }

    public DbSet<SettlementSummary> SettlementSummary { get; set; }
    public DbSet<TodayTransaction> TodayTransactions { get; set; }
    public DbSet<TransactionHistory> TransactionHistory { get; set; }

    #endregion

    #region Methods

    private async Task CreateStoredProcedures(CancellationToken cancellationToken)
    {
        String executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
        String executingAssemblyFolder = Path.GetDirectoryName(executingAssemblyLocation);

        String scriptsFolder = $@"{executingAssemblyFolder}/StoredProcedures/{this.DatabaseEngine}";

        String[] directories = Directory.GetDirectories(scriptsFolder);
        if (directories.Length == 0)
        {
            var list = new List<string> { scriptsFolder };
            directories = list.ToArray();
        }
        directories = directories.OrderBy(d => d).ToArray();

        foreach (String directiory in directories)
        {
            String[] sqlFiles = Directory.GetFiles(directiory, "*.sql");
            foreach (String sqlFile in sqlFiles.OrderBy(x => x))
            {
                Logger.LogInformation($"About to create Stored Procedure [{sqlFile}]");
                String sql = System.IO.File.ReadAllText(sqlFile);

                // Check here is we need to replace a Database Name
                if (sql.Contains("{DatabaseName}"))
                {
                    sql = sql.Replace("{DatabaseName}", this.Database.GetDbConnection().Database);
                }

                // Create the new view using the original sql from file
                await this.Database.ExecuteSqlRawAsync(sql, cancellationToken);

                Logger.LogInformation($"Created Stored Procedure [{sqlFile}] successfully.");
            }
        }
    }

    private async Task CreateViews(CancellationToken cancellationToken)
    {
        String executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
        String executingAssemblyFolder = Path.GetDirectoryName(executingAssemblyLocation);

        String scriptsFolder = $@"{executingAssemblyFolder}/Views/{this.DatabaseEngine}";

        String[] directiories = Directory.GetDirectories(scriptsFolder);
        directiories = directiories.OrderBy(d => d).ToArray();

        foreach (String directiory in directiories)
        {
            String[] sqlFiles = Directory.GetFiles(directiory, "*View.sql");
            foreach (String sqlFile in sqlFiles.OrderBy(x => x))
            {
                Logger.LogInformation($"About to create View [{sqlFile}]");
                String sql = System.IO.File.ReadAllText(sqlFile);

                // Check here is we need to replace a Database Name
                if (sql.Contains("{DatabaseName}"))
                {
                    sql = sql.Replace("{DatabaseName}", this.Database.GetDbConnection().Database);
                }

                // Create the new view using the original sql from file
                await this.Database.ExecuteSqlRawAsync(sql, cancellationToken);

                Logger.LogInformation($"Created View [{sqlFile}] successfully.");
            }
        }
    }

    private async Task SeedStandingData(CancellationToken cancellationToken)
    {
        String executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
        String executingAssemblyFolder = Path.GetDirectoryName(executingAssemblyLocation);

        String scriptsFolder = $@"{executingAssemblyFolder}/SeedingScripts"; ///{this.DatabaseEngine}";

        String[] sqlFiles = Directory.GetFiles(scriptsFolder, "*.sql");
        foreach (String sqlFile in sqlFiles.OrderBy(x => x))
        {
            Logger.LogDebug($"About to create View [{sqlFile}]");
            String sql = System.IO.File.ReadAllText(sqlFile);

            // Check here is we need to replace a Database Name
            if (sql.Contains("{DatabaseName}"))
            {
                sql = sql.Replace("{DatabaseName}", this.Database.GetDbConnection().Database);
            }

            // Create the new view using the original sql from file
            await this.Database.ExecuteSqlRawAsync(sql, cancellationToken);

            Logger.LogInformation($"Run Seeding Script [{sqlFile}] successfully.");
        }
    }

    public static Boolean IsDuplicateInsertsIgnored(String tableName) =>
        EstateManagementGenericContext.TablesToIgnoreDuplicates.Contains(tableName.Trim(), StringComparer.InvariantCultureIgnoreCase);

    public virtual async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if (this.Database.IsSqlServer() || this.Database.IsMySql())
        {
            await this.Database.MigrateAsync(cancellationToken);
            await this.SetIgnoreDuplicates(cancellationToken);
            await this.CreateViews(cancellationToken);
            await this.SeedStandingData(cancellationToken);
        }

        if (this.Database.IsSqlServer())
        {
            await this.CreateStoredProcedures(cancellationToken);
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetupResponseCodes()
                    .SetupEstate()
                    .SetupEstateSecurityUser()
                    .SetupMerchant()
                    .SetupMerchantAddress()
                    .SetupMerchantContact()
                    .SetupMerchantDevice()
                    .SetupMerchantDevice()
                    .SetupMerchantOperator()
                    .SetupMerchantSecurityUser()
                    .SetupContract()
                    .SetupContractProduct()
                    .SetupContractProductTransactionFee()
                    .SetupTransaction()
                    .SetupTransactionAdditionalResponseData()
                    .SetupTransactionAdditionalRequestData()
                    .SetupSettlement()
                    .SetupMerchantSettlementFee()
                    .SetupFile()
                    .SetupFileImportLog()
                    .SetupFileImportLogFile()
                    .SetupFileLine()
                    .SetupStatementHeader()
                    .SetupStatementLine()
                    .SetupReconciliation()
                    .SetupVoucher()
                    .SetupMerchantContract()
                    .SetupFloat()
                    .SetupFloatActivity()
                    .SetupOperator()
                    .SetupSettlementSummary()
                    .SetupTransactionHistory()
                    .SetupTodaysTransactions();
        
        modelBuilder.SetupViewEntities();

        base.OnModelCreating(modelBuilder);
    }

    protected virtual async Task SetIgnoreDuplicates(CancellationToken cancellationToken)
    {
        EstateManagementGenericContext.TablesToIgnoreDuplicates = new List<String> {
                                                                                      nameof(this.ResponseCodes)
                                                                                  };
    }

    public virtual async Task SaveChangesWithDuplicateHandling(CancellationToken cancellationToken)
    {
        try
        {
            await this.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException uex)
        {
            // Swallow the error
            // TODO: handle PK exceptions uex.ConstraintName and uex.ConstraintProperties are both null
            //Logger.LogWarning($"Unique Constraint Exception. Constraint [{uex.ConstraintName}]. Properties [{String.Join(",", uex.ConstraintProperties)}]  Message [{uex.Message}]");
        }
    }


    #endregion
}
namespace EstateManagement.Database.Contexts;

using System.Reflection;
using Entities;
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

    /// <summary>
    /// Gets or sets the contract products.
    /// </summary>
    /// <value>
    /// The contract products.
    /// </value>
    public DbSet<ContractProduct> ContractProducts { get; set; }

    /// <summary>
    /// Gets or sets the contract product transaction fees.
    /// </summary>
    /// <value>
    /// The contract product transaction fees.
    /// </value>
    public DbSet<ContractProductTransactionFee> ContractProductTransactionFees { get; set; }

    /// <summary>
    /// Gets or sets the contracts.
    /// </summary>
    /// <value>
    /// The contracts.
    /// </value>
    public DbSet<Contract> Contracts { get; set; }

    /// <summary>
    /// Gets or sets the estate operators.
    /// </summary>
    /// <value>
    /// The estate operators.
    /// </value>
    public DbSet<EstateOperator> EstateOperators { get; set; }

    /// <summary>
    /// Gets or sets the estates.
    /// </summary>
    /// <value>
    /// The estates.
    /// </value>
    public DbSet<Estate> Estates { get; set; }

    /// <summary>
    /// Gets or sets the estate security users.
    /// </summary>
    /// <value>
    /// The estate security users.
    /// </value>
    public DbSet<EstateSecurityUser> EstateSecurityUsers { get; set; }

    /// <summary>
    /// Gets or sets the file import log files.
    /// </summary>
    /// <value>
    /// The file import log files.
    /// </value>
    public virtual DbSet<FileImportLogFile> FileImportLogFiles { get; set; }

    /// <summary>
    /// Gets or sets the file import logs.
    /// </summary>
    /// <value>
    /// The file import logs.
    /// </value>
    public virtual DbSet<FileImportLog> FileImportLogs { get; set; }

    /// <summary>
    /// Gets or sets the file import log view.
    /// </summary>
    /// <value>
    /// The file import log view.
    /// </value>
    public virtual DbSet<FileImportLogView> FileImportLogView { get; set; }

    /// <summary>
    /// Gets or sets the file lines.
    /// </summary>
    /// <value>
    /// The file lines.
    /// </value>
    public virtual DbSet<FileLine> FileLines { get; set; }

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>
    /// The files.
    /// </value>
    public virtual DbSet<File> Files { get; set; }

    /// <summary>
    /// Gets or sets the file view.
    /// </summary>
    /// <value>
    /// The file view.
    /// </value>
    public virtual DbSet<FileView> FileView { get; set; }

    /// <summary>
    /// Gets or sets the merchant addresses.
    /// </summary>
    /// <value>
    /// The merchant addresses.
    /// </value>
    public DbSet<MerchantAddress> MerchantAddresses { get; set; }

    /// <summary>
    /// Gets or sets the merchant contacts.
    /// </summary>
    /// <value>
    /// The merchant contacts.
    /// </value>
    public DbSet<MerchantContact> MerchantContacts { get; set; }

    /// <summary>
    /// Gets or sets the merchant devices.
    /// </summary>
    /// <value>
    /// The merchant devices.
    /// </value>
    public DbSet<MerchantDevice> MerchantDevices { get; set; }

    /// <summary>
    /// Gets or sets the merchant operators.
    /// </summary>
    /// <value>
    /// The merchant operators.
    /// </value>
    public DbSet<MerchantOperator> MerchantOperators { get; set; }

    /// <summary>
    /// Gets or sets the estate security users.
    /// </summary>
    /// <value>
    /// The estate security users.
    /// </value>
    public DbSet<Merchant> Merchants { get; set; }

    /// <summary>
    /// Gets or sets the merchant security users.
    /// </summary>
    /// <value>
    /// The merchant security users.
    /// </value>
    public DbSet<MerchantSecurityUser> MerchantSecurityUsers { get; set; }

    public DbSet<MerchantSettlementFee> MerchantSettlementFees { get; set; }

    /// <summary>
    /// Gets or sets the reconciliations.
    /// </summary>
    /// <value>
    /// The reconciliations.
    /// </value>
    public DbSet<Reconciliation> Reconciliations { get; set; }

    public DbSet<ResponseCodes> ResponseCodes { get; set; }

    public DbSet<Settlement> Settlements { get; set; }

    public virtual DbSet<SettlementView> SettlementsView { get; set; }

    public DbSet<StatementHeader> StatementHeaders { get; set; }

    public DbSet<StatementLine> StatementLines { get; set; }

    /// <summary>
    /// Gets or sets the transaction fees.
    /// </summary>
    /// <value>
    /// The transaction fees.
    /// </value>
    public DbSet<TransactionFee> TransactionFees { get; set; }

    /// <summary>
    /// Gets or sets the transactions.
    /// </summary>
    /// <value>
    /// The transactions.
    /// </value>
    public DbSet<Transaction> Transactions { get; set; }

    /// <summary>
    /// Gets or sets the transaction additional request data.
    /// </summary>
    /// <value>
    /// The transaction additional request data.
    /// </value>
    public DbSet<TransactionAdditionalRequestData> TransactionsAdditionalRequestData { get; set; }

    /// <summary>
    /// Gets or sets the transaction additional response data.
    /// </summary>
    /// <value>
    /// The transaction additional response data.
    /// </value>
    public DbSet<TransactionAdditionalResponseData> TransactionsAdditionalResponseData { get; set; }

    /// <summary>
    /// Gets or sets the transactions view.
    /// </summary>
    /// <value>
    /// The transactions view.
    /// </value>
    public virtual DbSet<TransactionsView> TransactionsView { get; set; }

    /// <summary>
    /// Gets or sets the vouchers.
    /// </summary>
    /// <value>
    /// The vouchers.
    /// </value>
    public DbSet<Voucher> Vouchers { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the views.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
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
                Logger.LogDebug($"About to create View [{sqlFile}]");
                String sql = System.IO.File.ReadAllText(sqlFile);

                // Check here is we need to replace a Database Name
                if (sql.Contains("{DatabaseName}"))
                {
                    sql = sql.Replace("{DatabaseName}", this.Database.GetDbConnection().Database);
                }

                // Create the new view using the original sql from file
                await this.Database.ExecuteSqlRawAsync(sql, cancellationToken);

                Logger.LogDebug($"Created View [{sqlFile}] successfully.");
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

            Logger.LogDebug($"Run Seeding Script [{sqlFile}] successfully.");
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
            //await this.CreateViews(cancellationToken);
            await this.SeedStandingData(cancellationToken);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetupResponseCodesTable()
                    .SetupEstateTables()
                    .SetupMerchantTables()
                    .SetupTransactionTables()
                    .SetupContractTables()
                    .SetupFileTables()
                    .SetupSettlementTables()
                    .SetupStatementTables();

        modelBuilder.SetupViewEntities();

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Sets the ignore duplicates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    protected virtual async Task SetIgnoreDuplicates(CancellationToken cancellationToken)
    {
        EstateManagementGenericContext.TablesToIgnoreDuplicates = new List<String> {
                                                                                      nameof(Estate),
                                                                                      nameof(EstateSecurityUser),
                                                                                      nameof(EstateOperator),
                                                                                      nameof(Merchant),
                                                                                      nameof(MerchantAddress),
                                                                                      nameof(MerchantContact),
                                                                                      nameof(MerchantDevice),
                                                                                      nameof(MerchantSecurityUser),
                                                                                      nameof(MerchantOperator),
                                                                                      nameof(Transaction),
                                                                                      nameof(TransactionFee),
                                                                                      nameof(TransactionAdditionalRequestData),
                                                                                      nameof(TransactionAdditionalResponseData),
                                                                                      nameof(Contract),
                                                                                      nameof(ContractProduct),
                                                                                      nameof(ContractProductTransactionFee),
                                                                                      nameof(Reconciliation),
                                                                                      nameof(Voucher),
                                                                                      nameof(FileImportLog),
                                                                                      nameof(FileImportLogFile),
                                                                                      nameof(File),
                                                                                      nameof(FileLine),
                                                                                      nameof(Settlement),
                                                                                      nameof(MerchantSettlementFee),
                                                                                      nameof(StatementHeader),
                                                                                      nameof(StatementLine),
                                                                                      nameof(this.ResponseCodes)
                                                                                  };
    }

    #endregion
}
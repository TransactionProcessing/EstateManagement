namespace EstateManagement.Database.Contexts;

using Microsoft.EntityFrameworkCore;
using Shared.General;

public class EstateManagementSqlServerContext : EstateManagementGenericContext
{
    public EstateManagementSqlServerContext() : base("SqlServer", ConfigurationReader.GetConnectionString("EstateReportingReadModel"))
    {
    }

    public EstateManagementSqlServerContext(String connectionString) : base("SqlServer", connectionString)
    {
    }

    public EstateManagementSqlServerContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!string.IsNullOrWhiteSpace(this.ConnectionString))
        {
            options.UseSqlServer(this.ConnectionString);
        }
    }

    protected override async Task SetIgnoreDuplicates(CancellationToken cancellationToken)
    {
        base.SetIgnoreDuplicates(cancellationToken);

        EstateManagementGenericContext.TablesToIgnoreDuplicates = EstateManagementGenericContext.TablesToIgnoreDuplicates.Select(x => $"ALTER TABLE [{x}]  REBUILD WITH (IGNORE_DUP_KEY = ON)").ToList();

        String sql = string.Join(";", EstateManagementGenericContext.TablesToIgnoreDuplicates);

        await this.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}
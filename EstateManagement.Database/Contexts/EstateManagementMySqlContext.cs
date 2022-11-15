namespace EstateManagement.Database.Contexts;

using Microsoft.EntityFrameworkCore;
using Shared.General;

public class EstateManagementMySqlContext : EstateManagementGenericContext
{
    public EstateManagementMySqlContext() : base("MySql", ConfigurationReader.GetConnectionString("EstateReportingReadModel"))
    {
    }

    public EstateManagementMySqlContext(String connectionString) : base("MySql", connectionString)
    {
    }

    public EstateManagementMySqlContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!string.IsNullOrWhiteSpace(this.ConnectionString))
        {
            options.UseMySql(this.ConnectionString, ServerVersion.Parse("8.0.27")).AddInterceptors(new MySqlIgnoreDuplicatesOnInsertInterceptor());
        }
    }
}
namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.Common;
    using ContractAggregate;
    using EstateAggregate;
    using EstateReporting.Database;
    using Lamar;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Microsoft.Extensions.DependencyInjection;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EntityFramework;
    using Shared.EntityFramework.ConnectionStringConfiguration;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventStore;
    using Shared.EventStore.Extensions;
    using Shared.General;
    using Shared.Repositories;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class RepositoryRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryRegistry"/> class.
        /// </summary>
        public RepositoryRegistry()
        {
            Boolean useConnectionStringConfig = bool.Parse(ConfigurationReader.GetValue("AppSettings", "UseConnectionStringConfig"));

            if (useConnectionStringConfig)
            {
                String connectionStringConfigurationConnString = ConfigurationReader.GetConnectionString("ConnectionStringConfiguration");
                this.AddSingleton<IConnectionStringConfigurationRepository, ConnectionStringConfigurationRepository>();
                this.AddTransient(c => { return new ConnectionStringConfigurationContext(connectionStringConfigurationConnString); });

                // TODO: Read this from a the database and set
            }
            else
            {
                this.AddEventStoreClient(Startup.ConfigureEventStoreSettings);
                this.AddEventStoreProjectionManagerClient(Startup.ConfigureEventStoreSettings);
                this.AddEventStorePersistentSubscriptionsClient(Startup.ConfigureEventStoreSettings);
                this.AddSingleton<IConnectionStringConfigurationRepository, ConfigurationReaderConnectionStringRepository>();
            }

            this.AddTransient<IEventStoreContext, EventStoreContext>();
            this.AddSingleton<IEstateManagementRepository, EstateManagementRepository>();
            this.AddSingleton<IDbContextFactory<EstateReportingGenericContext>, DbContextFactory<EstateReportingGenericContext>>();

            this.AddSingleton<Func<String, EstateReportingGenericContext>>(cont => connectionString =>
                                                                                   {
                                                                                       String databaseEngine =
                                                                                           ConfigurationReader.GetValue("AppSettings", "DatabaseEngine");

                                                                                       return databaseEngine switch
                                                                                       {
                                                                                           "MySql" => new EstateReportingMySqlContext(connectionString),
                                                                                           "SqlServer" => new EstateReportingSqlServerContext(connectionString),
                                                                                           _ => throw new
                                                                                               NotSupportedException($"Unsupported Database Engine {databaseEngine}")
                                                                                       };
                                                                                   });

            this.AddSingleton<IAggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent>,
                AggregateRepository<EstateAggregate, DomainEventRecord.DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>,
                AggregateRepository<MerchantAggregate, DomainEventRecord.DomainEvent>>();
            this.AddSingleton<IAggregateRepository<ContractAggregate, DomainEventRecord.DomainEvent>,
                AggregateRepository<ContractAggregate, DomainEventRecord.DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>,
                AggregateRepository<MerchantStatementAggregate, DomainEventRecord.DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantDepositListAggregate, DomainEventRecord.DomainEvent>,
                AggregateRepository<MerchantDepositListAggregate, DomainEventRecord.DomainEvent>>();
        }

        #endregion
    }
}
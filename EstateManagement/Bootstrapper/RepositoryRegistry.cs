﻿namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.Common;
    using ContractAggregate;
    using Database.Contexts;
    using EstateAggregate;
    using Lamar;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OperatorAggregate;
    using Repository;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EntityFramework;
    using Shared.EntityFramework.ConnectionStringConfiguration;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventStore;
    using Shared.EventStore.SubscriptionWorker;
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
            else{

                String connectionString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");

                this.AddEventStoreProjectionManagementClient(connectionString);
                this.AddEventStorePersistentSubscriptionsClient(connectionString);

                this.AddEventStoreClient(connectionString);
                this.AddSingleton<IConnectionStringConfigurationRepository, ConfigurationReaderConnectionStringRepository>();
            }

            this.AddTransient<IEventStoreContext, EventStoreContext>();
            this.AddSingleton<IEstateManagementRepository, EstateManagementRepository>();
            this.AddSingleton<IEstateReportingRepository, EstateReportingRepository>();
            this.AddSingleton<IEstateReportingRepositoryForReports, EstateReportingRepositoryForReports>();
            this.AddSingleton<IDbContextFactory<EstateManagementGenericContext>, DbContextFactory<EstateManagementGenericContext>>();

            this.AddSingleton<Func<String, EstateManagementGenericContext>>(cont => connectionString =>
                                                                                   {
                                                                                       String databaseEngine =
                                                                                           ConfigurationReader.GetValue("AppSettings", "DatabaseEngine");

                                                                                       return databaseEngine switch
                                                                                       {
                                                                                           "MySql" => new EstateManagementMySqlContext(connectionString),
                                                                                           "SqlServer" => new EstateManagementSqlServerContext(connectionString),
                                                                                           _ => throw new
                                                                                               NotSupportedException($"Unsupported Database Engine {databaseEngine}")
                                                                                       };
                                                                                   });

            this.AddSingleton<IAggregateRepository<EstateAggregate, DomainEvent>,
                AggregateRepository<EstateAggregate, DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantAggregate, DomainEvent>,
                AggregateRepository<MerchantAggregate, DomainEvent>>();
            this.AddSingleton<IAggregateRepository<ContractAggregate, DomainEvent>,
                AggregateRepository<ContractAggregate, DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantStatementAggregate, DomainEvent>,
                AggregateRepository<MerchantStatementAggregate, DomainEvent>>();
            this.AddSingleton<IAggregateRepository<MerchantDepositListAggregate, DomainEvent>,
                AggregateRepository<MerchantDepositListAggregate, DomainEvent>>();
            this.AddSingleton<IAggregateRepository<OperatorAggregate, DomainEvent>,
                AggregateRepository<OperatorAggregate, DomainEvent>>();

            this.AddSingleton<Func<String, Int32, ISubscriptionRepository>>(cont => (esConnString, cacheDuration) => {
                                                                                        return SubscriptionRepository.Create(esConnString, cacheDuration);
                                                                                    });
        }

        #endregion
    }


}
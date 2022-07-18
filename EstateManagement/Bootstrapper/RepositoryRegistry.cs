namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Security;
    using BusinessLogic.Common;
    using ContractAggregate;
    using EstateAggregate;
    using EstateReporting.Database;
    using Lamar;
    using MerchantAggregate;
    using MerchantStatementAggregate;
    using Microsoft.Extensions.Configuration;
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
            else {
                Boolean insecureES = Startup.Configuration.GetValue<Boolean>("EventStoreSettings:Insecure");

                Func<SocketsHttpHandler> CreateHttpMessageHandler = () => new SocketsHttpHandler
                                                                          {

                                                                              SslOptions = new SslClientAuthenticationOptions
                                                                                           {
                                                                                               RemoteCertificateValidationCallback = (sender,
                                                                                                   certificate,
                                                                                                   chain,
                                                                                                   errors) => {

                                                                                                   return true;
                                                                                               }
                                                                                           }
                                                                          };

                if (insecureES) {
                    this.AddInSecureEventStoreClient(Startup.EventStoreClientSettings.ConnectivitySettings.Address, CreateHttpMessageHandler);
                }
                else {
                    this.AddEventStoreClient(Startup.EventStoreClientSettings.ConnectivitySettings.Address, CreateHttpMessageHandler);
                }
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
        }

        #endregion
    }


}
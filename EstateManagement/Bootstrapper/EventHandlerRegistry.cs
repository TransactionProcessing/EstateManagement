namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.EventHandling;
    using Lamar;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared.EventStore.EventHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class EventHandlerRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerRegistry"/> class.
        /// </summary>
        public EventHandlerRegistry()
        {
            Dictionary<String, String[]> eventHandlersConfiguration = new Dictionary<String, String[]>();
            Dictionary<String, String[]> eventHandlersConfigurationOrdered = new Dictionary<String, String[]>();

            IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration");

            if (section != null)
            {
                Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration").Bind(eventHandlersConfiguration);
            }

            //this.AddSingleton(eventHandlersConfiguration);
            this.Use(eventHandlersConfiguration).Named("Concurrent");

            section = Startup.Configuration.GetSection("AppSettings:EventHandlerConfigurationOrdered");

            if (section != null)
            {
                Startup.Configuration.GetSection("AppSettings:EventHandlerConfigurationOrdered").Bind(eventHandlersConfigurationOrdered);
            }

            this.Use(eventHandlersConfigurationOrdered).Named("Ordered");

            this.AddSingleton<Func<Type, IDomainEventHandler>>(container => type =>
                                                                            {
                                                                                IDomainEventHandler handler = container.GetService(type) as IDomainEventHandler;
                                                                                return handler;
                                                                            });

            this.AddSingleton<StatementDomainEventHandler>();
            this.AddSingleton<EstateDomainEventHandler>();
            this.AddSingleton<MerchantDomainEventHandler>();
            this.AddSingleton<TransactionDomainEventHandler>();
            this.AddSingleton<ContractDomainEventHandler>();
            this.AddSingleton<SettlementDomainEventHandler>();
            this.AddSingleton<FileProcessorDomainEventHandler>();
            this.AddSingleton<MerchantStatementDomainEventHandler>();

            this.For<IDomainEventHandlerResolver>().Use<DomainEventHandlerResolver>().Named("Concurrent")
                .Ctor<Dictionary<String, String[]>>().Is(eventHandlersConfiguration).Singleton();
            this.For<IDomainEventHandlerResolver>().Use<DomainEventHandlerResolver>().Named("Ordered")
                .Ctor<Dictionary<String, String[]>>().Is(eventHandlersConfigurationOrdered).Singleton();
        }

        #endregion
    }
}
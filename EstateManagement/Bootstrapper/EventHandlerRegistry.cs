namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using BusinessLogic.EventHandling;
    using Lamar;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared.EventStore.EventHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    public class EventHandlerRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerRegistry"/> class.
        /// </summary>
        public EventHandlerRegistry()
        {
            Dictionary<String, String[]> eventHandlersConfiguration = new Dictionary<String, String[]>();

            if (Startup.Configuration != null)
            {
                IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration");

                if (section != null)
                {
                    Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration").Bind(eventHandlersConfiguration);
                }
            }

            this.AddSingleton(eventHandlersConfiguration);

            this.AddSingleton<Func<Type, IDomainEventHandler>>(container => type =>
                                                                            {
                                                                                IDomainEventHandler handler = container.GetService(type) as IDomainEventHandler;
                                                                                return handler;
                                                                            });

            this.AddSingleton<MerchantDomainEventHandler>();
            this.AddSingleton<TransactionDomainEventHandler>();
            this.AddSingleton<SettlementDomainEventHandler>();
            this.AddSingleton<StatementDomainEventHandler>();
            this.AddSingleton<IDomainEventHandlerResolver, DomainEventHandlerResolver>();
        }

        #endregion
    }
}
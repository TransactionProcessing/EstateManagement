﻿namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using Lamar;
    using MessagingService.Client;
    using Microsoft.Extensions.DependencyInjection;
    using SecurityService.Client;
    using Shared.General;
    using TransactionProcessor.Client;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class ClientsRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsRegistry" /> class.
        /// </summary>
        public ClientsRegistry()
        {
            this.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            this.AddSingleton<IMessagingServiceClient, MessagingServiceClient>();
            this.AddSingleton<ITransactionProcessorClient, TransactionProcessorClient>();

            this.AddSingleton<Func<String, String>>(container => serviceName => { return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString; });

            HttpClientHandler httpClientHandler = new HttpClientHandler
                                                  {
                                                      ServerCertificateCustomValidationCallback = (message,
                                                                                                   certificate2,
                                                                                                   arg3,
                                                                                                   arg4) =>
                                                                                                  {
                                                                                                      return true;
                                                                                                  }
                                                  };
            HttpClient httpClient = new HttpClient(httpClientHandler);
            this.AddSingleton(httpClient);
        }

        #endregion
    }
}
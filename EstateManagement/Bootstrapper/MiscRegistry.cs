﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shared.General;

namespace EstateManagement.Bootstrapper
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Abstractions;
    using BusinessLogic.Manger;
    using BusinessLogic.Services;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Factories;
    using Shared.Middleware;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class MiscRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MiscRegistry"/> class.
        /// </summary>
        public MiscRegistry()
        {
            this.AddSingleton<IReportingManager, ReportingManager>();
            this.AddSingleton<IEstateManagementManager, EstateManagementManager>();
            this.AddSingleton<IModelFactory, ModelFactory>();
            this.AddSingleton<IStatementBuilder, StatementBuilder>();
            this.AddSingleton<IFileSystem, FileSystem>();
            this.AddSingleton<IPDFGenerator, PDFGenerator>();

            bool logRequests = ConfigurationReaderExtensions.GetValueOrDefault<Boolean>("MiddlewareLogging", "LogRequests", true);
            bool logResponses = ConfigurationReaderExtensions.GetValueOrDefault<Boolean>("MiddlewareLogging", "LogResponses", true);
            LogLevel middlewareLogLevel = ConfigurationReaderExtensions.GetValueOrDefault<LogLevel>("MiddlewareLogging", "MiddlewareLogLevel", LogLevel.Warning);

            RequestResponseMiddlewareLoggingConfig config =
                new RequestResponseMiddlewareLoggingConfig(middlewareLogLevel, logRequests, logResponses);

            this.AddSingleton(config);
        }
        
        #endregion
    }

    public static class ConfigurationReaderExtensions
    {
        public static T GetValueOrDefault<T>(String sectionName, String keyName, T defaultValue)
        {
            try
            {
                var value = ConfigurationReader.GetValue(sectionName, keyName);

                if (String.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (KeyNotFoundException kex)
            {
                return defaultValue;
            }
        }
    }
}
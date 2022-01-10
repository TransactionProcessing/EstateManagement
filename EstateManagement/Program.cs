namespace EstateManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Events;
    using EventStore.Client;
    using Lamar.Microsoft.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using Shared.EventStore.Subscriptions;
    using Shared.Logger;

    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            Console.Title = "Estate Management";

            //At this stage, we only need our hosting file for ip and ports
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                  .AddJsonFile("hosting.json", optional: true)
                                                                  .AddJsonFile("hosting.development.json", optional: true)
                                                                  .AddEnvironmentVariables().Build();

            //IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            //hostBuilder.ConfigureLogging(logging =>
            //                             {
            //                                 logging.AddConsole(); 

            //                             });
            //hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            //                                     {
            //                                         webBuilder.UseLamar().UseStartup<Startup>().UseConfiguration(config).UseKestrel();
            //                                     });
            //return hostBuilder;

            IWebHostBuilder builder = new WebHostBuilder();
            builder
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .UseLamar()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>();

            return builder;
        }

        /// <summary>
        /// Workers the trace generated.
        /// </summary>
        /// <param name="trace">The trace.</param>
        /// <param name="logLevel">The log level.</param>
        private static void Worker_TraceGenerated(string trace, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Logger.LogTrace(trace);
                    break;
                case LogLevel.Debug:
                    Logger.LogDebug(trace);
                    break;
                case LogLevel.Information:
                    Logger.LogInformation(trace);
                    break;
                case LogLevel.Warning:
                    Logger.LogWarning(trace);
                    break;
                case LogLevel.Error:
                    Logger.LogError(new Exception(trace));
                    break;
                case LogLevel.Critical:
                    Logger.LogCritical(new Exception(trace));
                    break;
            }
        }
    }
}

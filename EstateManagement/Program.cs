namespace EstateManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Lamar.Microsoft.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Shared.Logger;

    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {


            //At this stage, we only need our hosting file for ip and ports
            FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(fi.Directory.FullName)
                                                                  .AddJsonFile("hosting.json", optional: true)
                                                                  .AddJsonFile("hosting.development.json", optional: true)
                                                                  .AddEnvironmentVariables().Build();

            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.UseWindowsService();
            hostBuilder.UseLamar();
            hostBuilder.ConfigureLogging(logging =>
                                         {
                                             logging.AddConsole();
                                         });
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
                                                 {
                                                     webBuilder.UseStartup<Startup>();
                                                     webBuilder.UseConfiguration(config);
                                                     webBuilder.UseKestrel();
                                                 });
            return hostBuilder;
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

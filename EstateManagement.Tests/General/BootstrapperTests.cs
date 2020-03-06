namespace EstateManagement.Tests.General
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Common;
    using Controllers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared.DomainDrivenDesign.EventStore;
    using Xunit;

    [Collection("TestCollection")]
    public class BootstrapperTests
    {
        [Fact]//(Skip="Change required after moving from Autofac")]
        public void VerifyBootstrapperIsValid()
        {
            Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");
            hostingEnvironment.Setup(he => he.ContentRootPath).Returns("/home");
            hostingEnvironment.Setup(he => he.ApplicationName).Returns("Test Application");

            IServiceCollection services = new ServiceCollection();
            Startup s = new Startup(hostingEnvironment.Object);
            Startup.Configuration = this.SetupMemoryConfiguration();

            s.ConfigureServices(services);

            this.AddTestRegistrations(services, hostingEnvironment.Object);

            services.AssertConfigurationIsValid();
        }

        private IConfigurationRoot SetupMemoryConfiguration()
        {
            Dictionary<String, String> configuration = new Dictionary<String, String>();

            IConfigurationBuilder builder = new ConfigurationBuilder();

            configuration.Add("EventStoreSettings:ConnectionString", "ConnectTo=tcp://admin:changeit@127.0.0.1:1112;VerboseLogging=true;");
            configuration.Add("EventStoreSettings:ConnectionName", "UnitTestConnection");
            configuration.Add("EventStoreSettings:HttpPort", "2113");
            configuration.Add("AppSettings:UseConnectionStringConfig", "false");
            configuration.Add("AppSettings:SecurityService", "http://127.0.0.1");


            builder.AddInMemoryCollection(configuration);

            return builder.Build();
        }

        private void AddTestRegistrations(IServiceCollection services,
                                          IWebHostEnvironment hostingEnvironment)
        {
            services.AddLogging();
            DiagnosticListener diagnosticSource = new DiagnosticListener(hostingEnvironment.ApplicationName);
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddSingleton<DiagnosticListener>(diagnosticSource);
            services.AddSingleton<IWebHostEnvironment>(hostingEnvironment);
        }
    }

    //public static class ScopeExtensions
    //{
    //    public static IList<IServiceWithType> Filter(this IEnumerable<IServiceWithType> services,
    //                                                 IEnumerable<string> ignoredAssemblies)
    //    {
    //        return services.Where(serviceWithType => ignoredAssemblies
    //                                  .All(ignored => ignored != serviceWithType.ServiceType.FullName)).ToList();
    //    }

    //    public static IList<object> ResolveAll(this ILifetimeScope scope, IEnumerable<string> ignoredAssemblies)
    //    {
    //        var services = scope.ComponentRegistry.Registrations.SelectMany(x => x.Services)
    //                            .OfType<IServiceWithType>().Filter(ignoredAssemblies).ToList();

    //        foreach (var serviceWithType in services)
    //        {
    //            try
    //            {
    //                scope.Resolve(serviceWithType.ServiceType);
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine(e);
    //                throw;
    //            }
    //        }

    //        return services.Select(x => x.ServiceType).Select(scope.Resolve).ToList();
    //    }
    //}
}

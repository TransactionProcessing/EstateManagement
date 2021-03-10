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
    using Xunit;

    [Collection("TestCollection")]
    public class BootstrapperTests
    {
        [Fact(Skip="issue with typemap needs investigated")]
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

            configuration.Add("ConnectionStrings:HealthCheck", "HeathCheckConnString");
            configuration.Add("SecurityConfiguration:Authority", "https://127.0.0.1");
            configuration.Add("EventStoreSettings:ConnectionString", "https://127.0.0.1:2113");
            configuration.Add("EventStoreSettings:ConnectionName", "UnitTestConnection");
            configuration.Add("EventStoreSettings:UserName", "admin");
            configuration.Add("EventStoreSettings:Password", "changeit");
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

namespace EstateManagement.Tests.General
{
    using System;
    using System.Collections.Generic;
    //using Lamar;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared.DomainDrivenDesign.EventStore;
    using Xunit;

    [Collection("TestCollection")]
    public class BootstrapperTests
    {
        //[Fact]
        //public void VerifyBootstrapperIsValid_Development()
        //{
        //    ServiceRegistry serviceRegistry = new ServiceRegistry();
        //    Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
        //    hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");

        //    Startup.Configuration = this.SetupMemoryConfiguration();
        //    Startup.WebHostEnvironment = hostingEnvironment.Object;

        //    IContainer container = Startup.GetConfiguredContainer(serviceRegistry, hostingEnvironment.Object);

        //    this.AddTestRegistrations(container);

        //    container.AssertConfigurationIsValid();
        //}

        //[Fact]
        //public void VerifyBootstrapperIsValid_Production()
        //{
        //    ServiceRegistry serviceRegistry = new ServiceRegistry();
        //    Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
        //    hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Production");

        //    Startup.Configuration = this.SetupMemoryConfiguration();
        //    Startup.WebHostEnvironment = hostingEnvironment.Object;

        //    IContainer container = Startup.GetConfiguredContainer(serviceRegistry, hostingEnvironment.Object);

        //    this.AddTestRegistrations(container);

        //    container.AssertConfigurationIsValid();
        //}

        //private IConfigurationRoot SetupMemoryConfiguration()
        //{
        //    Dictionary<String, String> configuration = new Dictionary<String, String>();

        //    IConfigurationBuilder builder = new ConfigurationBuilder();

        //    configuration.Add("EventStoreSettings:ConnectionString", "ConnectTo=tcp://admin:changeit@127.0.0.1:1112;VerboseLogging=true;");
        //    configuration.Add("EventStoreSettings:ConnectionName", "UnitTestConnection");
        //    configuration.Add("EventStoreSettings:HttpPort", "2113");
        //    //configuration.Add("AppSettings:HandlerEventTypesToSilentlyHandle", "\"GolfClubDomainEventHandler\": []");


        //    builder.AddInMemoryCollection(configuration);

        //    return builder.Build();
        //}

        //private void AddTestRegistrations(IContainer container)
        //{
        //    ServiceRegistry testServiceRegistry = new ServiceRegistry();

        //    testServiceRegistry.For<IOptions<EventStoreConnectionSettings>>().Use<OptionsManager<EventStoreConnectionSettings>>();
        //    testServiceRegistry.For<IOptionsFactory<EventStoreConnectionSettings>>().Use<OptionsFactory<EventStoreConnectionSettings>>();

        //    container.Configure(testServiceRegistry);
        //}
    }
}

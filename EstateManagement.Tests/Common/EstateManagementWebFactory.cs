namespace EstateManagement.Tests.Common
{
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Xunit;

    public class EstateManagementWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Setup my mocks in here
            Mock<ICommandRouter> commandRouterMock = this.CreateCommandRouterMock();

            builder.ConfigureServices((builderContext, services) =>
            {
                if (commandRouterMock != null)
                {
                    services.AddSingleton<ICommandRouter>(commandRouterMock.Object);
                }

                services.AddMvc(options =>
                {
                    options.Filters.Add(new AllowAnonymousFilter());
                })
                        .AddApplicationPart(typeof(Startup).Assembly);
            });
            ;
        }

        private Mock<ICommandRouter> CreateCommandRouterMock()
        {
            Mock<ICommandRouter> commandRouterMock = new Mock<ICommandRouter>(MockBehavior.Strict);

            commandRouterMock.Setup(c => c.Route(It.IsAny<ICommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            return commandRouterMock;
        }

    }

    /// <summary>
    /// </summary>
    /// <seealso cref="Startup" />
    [CollectionDefinition("TestCollection")]
    public class TestCollection : ICollectionFixture<EstateManagementWebFactory<Startup>>
    {
        // A class with no code, only used to define the collection
    }

    public static class Helpers
    {
        #region Methods

        /// <summary>
        /// Creates the content of the string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        public static StringContent CreateStringContent<T>(T requestObject)
        {
            return new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
        }

        #endregion
    }
}

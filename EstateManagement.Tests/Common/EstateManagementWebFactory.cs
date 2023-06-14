namespace EstateManagement.Tests.Common
{
    using System;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class EstateManagementWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Setup my mocks in here
            Mock<IMediator> mediatorMock = this.CreateMediatorMock();

            builder.ConfigureServices((builderContext, services) =>
            {
                if (mediatorMock != null)
                {
                    services.AddSingleton<IMediator>(mediatorMock.Object);
                }

                services.AddMvcCore(options =>
                                    {
                                        options.Filters.Add(new AllowAnonymousFilter());
                                    })
                        .AddApplicationPart(typeof(Startup).Assembly);
            });
            ;
        }

        private Mock<IMediator> CreateMediatorMock()
        {
            Mock<IMediator> mediatorMock = new Mock<IMediator>(MockBehavior.Strict);

            mediatorMock.Setup(c => c.Send(It.IsAny<IRequest<String>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult("Hello"));

            return mediatorMock;
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

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                               ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    
}

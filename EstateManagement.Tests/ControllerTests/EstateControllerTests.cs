namespace EstateManagement.Tests.ControllerTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using EstateManagement.Testing;
    //using Lamar;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class EstateControllerTests : IClassFixture<EstateManagementWebFactory<Startup>>
    {
        #region Fields

        /// <summary>
        /// The web application factory
        /// </summary>
        private readonly EstateManagementWebFactory<Startup> WebApplicationFactory;

        #endregion

        #region Constructors

        public EstateControllerTests(EstateManagementWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task EstateController_POST_CreateEstate_CreateEstateResponseIsReturned()
        {
            HttpClient client = this.WebApplicationFactory.CreateClient();

            CreateEstateRequest createEstateRequest = TestData.CreateEstateRequest;
            String uri = "api/estates/";
            StringContent content = Helpers.CreateStringContent(createEstateRequest);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();
            responseAsJson.ShouldNotBeNullOrEmpty();

            CreateEstateResponse responseObject = JsonConvert.DeserializeObject<CreateEstateResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.EstateId.ShouldNotBe(Guid.Empty);
        }

        #endregion
    }
}

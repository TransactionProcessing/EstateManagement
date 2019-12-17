namespace EstateManagement.Tests.ControllerTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using EstateManagement.Testing;
    using Microsoft.AspNetCore.Mvc.Testing;
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

        [Fact(Skip = "Authentication Issues...")]
        public async Task EstateController_POST_CreateEstate_CreateEstateResponseIsReturned()
        {
            HttpClient client = this.WebApplicationFactory.CreateClient();


            CreateEstateRequest createEstateRequest = TestData.CreateEstateRequestDTO;
            String uri = "api/estates/";
            StringContent content = Helpers.CreateStringContent(createEstateRequest);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6Il9JbGpYLXU5M0FnV3FrUjl2eEZIU0EiLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1NzYzNTIwMTQsImV4cCI6MTU3NjM1NTYxNCwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzM6NTAwMSIsImF1ZCI6WyJlc3RhdGVNYW5hZ2VtZW50IiwidHJhbnNhY3Rpb25Qcm9jZXNzb3IiLCJ0cmFuc2FjdGlvblByb2Nlc3NvckFDTCJdLCJjbGllbnRfaWQiOiJzZXJ2aWNlQ2xpZW50Iiwic2NvcGUiOlsiZXN0YXRlTWFuYWdlbWVudCIsInRyYW5zYWN0aW9uUHJvY2Vzc29yIiwidHJhbnNhY3Rpb25Qcm9jZXNzb3JBQ0wiXX0.dqwKXApOl5DTEp7wz-PAWoEy_43gquzuA1o7H7MF-tBgdTmlmbzfHD5Sez6y1F0tJHTPN0ZIbGJa1aiLs0m4YVfdvpLuIE8pEI5KUpTPuXr-olJkuRxZ7lAkstMFYvxsjd1d_Q-Y1fAIQFg_zq1So9c3eAbfdYC0-KF13Iwpp8fJpNN8Z5OVqrs3aUYtldJUkBt5o6toXFxdsJx-qS4ewnkLfDTLv1VJD1mg5lMJgTFLzvWQesB3Emia2XiwY-vq2CrXdu83Ez9guKvrmAbWDQa-WZoGeIMFjVC3ug8CVyxYYi6rZtmpbN9D8u-PRuo7jALiPef27QwzY9MLTIcPJg");

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

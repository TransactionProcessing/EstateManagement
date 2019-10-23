namespace EstateManagement.Client
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="EstateManagment.Client.IEstateClient" />
    public class EstateClient : ClientProxyBase, IEstateClient
    {
        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public EstateClient(Func<String, String> baseAddressResolver, HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("EstateManagementApi");

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="createEstateRequest">The create estate request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateEstateResponse> CreateEstate(String accessToken ,
                                                             CreateEstateRequest createEstateRequest,
                                       CancellationToken cancellationToken)
        {
            CreateEstateResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createEstateRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                Console.Out.WriteLine($"Request Uri [{requestUri}]");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                Console.Out.WriteLine($"Response Status [{httpResponse.StatusCode}]");

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateEstateResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new estate {createEstateRequest.EstateName}.", ex);

                throw exception;
            }

            return response;
        }
    }
}

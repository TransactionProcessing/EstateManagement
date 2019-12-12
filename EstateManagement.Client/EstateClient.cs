﻿namespace EstateManagement.Client
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
    /// <seealso cref="EstateManagement.Client.IEstateClient" />
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="EstateManagment.Client.IEstateClient" />
    public class EstateClient : ClientProxyBase, IEstateClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public EstateClient(Func<String, String> baseAddressResolver,
                            HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("EstateManagementApi");

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Assigns the operator to merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="assignOperatorRequest">The assign operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<AssignOperatorResponse> AssignOperatorToMerchant(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           AssignOperatorRequest assignOperatorRequest,
                                                                           CancellationToken cancellationToken)
        {
            AssignOperatorResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/merchants/{merchantId}/operators";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(assignOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<AssignOperatorResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error assigning operator Id {assignOperatorRequest.OperatorId} to merchant Id {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="createEstateRequest">The create estate request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateEstateResponse> CreateEstate(String accessToken,
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

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateEstateResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new estate {createEstateRequest.EstateName}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the estate user.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createEstateUserRequest">The create estate user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateEstateUserResponse> CreateEstateUser(String accessToken,
                                                                     Guid estateId,
                                                                     CreateEstateUserRequest createEstateUserRequest,
                                                                     CancellationToken cancellationToken)
        {
            CreateEstateUserResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/users";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createEstateUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateEstateUserResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new estate user Estate Id {estateId} Email Address {createEstateUserRequest.EmailAddress}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createMerchantRequest">The create merchant request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateMerchantResponse> CreateMerchant(String accessToken,
                                                                 Guid estateId,
                                                                 CreateMerchantRequest createMerchantRequest,
                                                                 CancellationToken cancellationToken)
        {
            CreateMerchantResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/merchants";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createMerchantRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateMerchantResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new merchant {createMerchantRequest.Name} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the merchant user.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="createMerchantUserRequest">The create merchant user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateMerchantUserResponse> CreateMerchantUser(String accessToken,
                                                                        Guid estateId,
                                                                        Guid merchantId,
                                                                        CreateMerchantUserRequest createMerchantUserRequest,
                                                                        CancellationToken cancellationToken)
        {
            CreateMerchantUserResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/merchants/{merchantId}/users";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createMerchantUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateMerchantUserResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new mercant user Merchant Id {estateId} Email Address {createMerchantUserRequest.EmailAddress}.",
                                                    ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Adds the operator.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createOperatorRequest">The create operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateOperatorResponse> CreateOperator(String accessToken,
                                                                 Guid estateId,
                                                                 CreateOperatorRequest createOperatorRequest,
                                                                 CancellationToken cancellationToken)
        {
            CreateOperatorResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/operators";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateOperatorResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new operator {createOperatorRequest.Name} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<EstateResponse> GetEstate(String accessToken,
                                                    Guid estateId,
                                                    CancellationToken cancellationToken)
        {
            EstateResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}";

            try
            {
                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<EstateResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<MerchantResponse> GetMerchant(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken)
        {
            MerchantResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/estates/{estateId}/merchants/{merchantId}";

            try
            {
                // Add the access token to the client headers
                //this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<MerchantResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        #endregion
    }
}
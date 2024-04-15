namespace EstateManagement.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects.Requests.Contract;
    using DataTransferObjects.Requests.Estate;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Estate;
    using DataTransferObjects.Responses.Merchant;
    using DataTransferObjects.Responses.Operator;
    using DataTransferObjects.Responses.Settlement;
    using Newtonsoft.Json;
    using MerchantResponse = DataTransferObjects.Responses.Merchant.MerchantResponse;

    public class EstateClient : ClientProxyBase, IEstateClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        /// <summary>
        /// The base address resolver
        /// </summary>
        private readonly Func<String, String> BaseAddressResolver;

        #endregion

        #region Constructors

        public EstateClient(Func<String, String> baseAddressResolver,
                            HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        #endregion

        #region Methods

        public async Task<AddMerchantDeviceResponse> AddDeviceToMerchant(String accessToken,
                                                                         Guid estateId,
                                                                         Guid merchantId,
                                                                         AddMerchantDeviceRequest addMerchantDeviceRequest,
                                                                         CancellationToken cancellationToken)
        {
            AddMerchantDeviceResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/devices");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(addMerchantDeviceRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<AddMerchantDeviceResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding device to merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<SwapMerchantDeviceResponse> SwapDeviceForMerchant(String accessToken,
                                                                            Guid estateId,
                                                                            Guid merchantId,
                                                                            SwapMerchantDeviceRequest swapMerchantDeviceRequest,
                                                                            CancellationToken cancellationToken)
        {
            SwapMerchantDeviceResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/devices");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(swapMerchantDeviceRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<SwapMerchantDeviceResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error swapping device for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task AddContractToMerchant(String accessToken, Guid estateId, Guid merchantId, AddMerchantContractRequest addMerchantContractRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(addMerchantContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding contract {addMerchantContractRequest.ContractId} to merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task UpdateMerchant(String accessToken, Guid estateId, Guid merchantId, UpdateMerchantRequest updateMerchantRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");
            
            try
            {
                String requestSerialised = JsonConvert.SerializeObject(updateMerchantRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);

            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task UpdateMerchantAddress(String accessToken, Guid estateId, Guid merchantId, Guid addressId, Address updatedAddressRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/addresses/{addressId}");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(updatedAddressRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);

            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating address {addressId} for merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<AddProductToContractResponse> AddProductToContract(String accessToken,
                                                                             Guid estateId,
                                                                             Guid contractId,
                                                                             AddProductToContractRequest addProductToContractRequest,
                                                                             CancellationToken cancellationToken)
        {
            AddProductToContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(addProductToContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<AddProductToContractResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding product [{addProductToContractRequest.ProductName}] to contract [{contractId}] for estate {estateId}.",
                                                    ex);

                throw exception;
            }

            return response;
        }

        public async Task<AddTransactionFeeForProductToContractResponse> AddTransactionFeeForProductToContract(String accessToken,
                                                                                                               Guid estateId,
                                                                                                               Guid contractId,
                                                                                                               Guid productId,
                                                                                                               AddTransactionFeeForProductToContractRequest
                                                                                                                   addTransactionFeeForProductToContractRequest,
                                                                                                               CancellationToken cancellationToken)
        {
            AddTransactionFeeForProductToContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products/{productId}/transactionFees");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(addTransactionFeeForProductToContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<AddTransactionFeeForProductToContractResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new
                        Exception($"Error adding transaction fee [{addTransactionFeeForProductToContractRequest.Description}] for product [{productId}] to contract [{contractId}] for estate {estateId}.",
                                  ex);

                throw exception;
            }

            return response;
        }

        public async Task<AssignOperatorResponse> AssignOperatorToMerchant(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           AssignOperatorRequest assignOperatorRequest,
                                                                           CancellationToken cancellationToken)
        {
            AssignOperatorResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/operators");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(assignOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<CreateContractResponse> CreateContract(String accessToken,
                                                                 Guid estateId,
                                                                 CreateContractRequest createContractRequest,
                                                                 CancellationToken cancellationToken)
        {
            CreateContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateContractResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating contract [{createContractRequest.Description}] for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<CreateEstateResponse> CreateEstate(String accessToken,
                                                             CreateEstateRequest createEstateRequest,
                                                             CancellationToken cancellationToken)
        {
            CreateEstateResponse response = null;

            String requestUri = this.BuildRequestUrl("/api/estates/");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createEstateRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<CreateEstateUserResponse> CreateEstateUser(String accessToken,
                                                                     Guid estateId,
                                                                     CreateEstateUserRequest createEstateUserRequest,
                                                                     CancellationToken cancellationToken)
        {
            CreateEstateUserResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/users");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createEstateUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<CreateMerchantResponse> CreateMerchant(String accessToken,
                                                                 Guid estateId,
                                                                 CreateMerchantRequest createMerchantRequest,
                                                                 CancellationToken cancellationToken)
        {
            CreateMerchantResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createMerchantRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<CreateMerchantUserResponse> CreateMerchantUser(String accessToken,
                                                                         Guid estateId,
                                                                         Guid merchantId,
                                                                         CreateMerchantUserRequest createMerchantUserRequest,
                                                                         CancellationToken cancellationToken)
        {
            CreateMerchantUserResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/users");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createMerchantUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<CreateOperatorResponse> CreateOperator(String accessToken,
                                                                 Guid estateId,
                                                                 CreateOperatorRequest createOperatorRequest,
                                                                 CancellationToken cancellationToken)
        {
            CreateOperatorResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(createOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task DisableTransactionFeeForProduct(String accessToken,
                                                          Guid estateId,
                                                          Guid contractId,
                                                          Guid productId,
                                                          Guid transactionFeeId,
                                                          CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products/{productId}/transactionFees/{transactionFeeId}");

            try
            {
                String requestSerialised = string.Empty;

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error disabling transaction fee Id [{transactionFeeId}] for product [{productId}] on contract [{contractId}] for estate {estateId}.",
                                  ex);

                throw exception;
            }
        }

        public async Task<List<ContractResponse>> GetContracts(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken)
        {
            List<ContractResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<ContractResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contracts for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<ContractResponse> GetContract(String accessToken,
                                                               Guid estateId,
                                                               Guid contractId,
                                                               CancellationToken cancellationToken)
        {
            ContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<ContractResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contract {contractId} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<EstateResponse> GetEstate(String accessToken,
                                                    Guid estateId,
                                                    CancellationToken cancellationToken)
        {
            EstateResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<List<EstateResponse>> GetEstates(String accessToken,
                                                    Guid estateId,
                                                    CancellationToken cancellationToken)
        {
            List<EstateResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/all");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<EstateResponse>>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting all estates for estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<MerchantResponse> GetMerchant(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken)
        {
            MerchantResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

        public async Task<List<ContractResponse>> GetMerchantContracts(String accessToken,
                                                                       Guid estateId,
                                                                       Guid merchantId,
                                                                       CancellationToken cancellationToken)
        {
            List<ContractResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<ContractResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant contracts for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<List<MerchantResponse>> GetMerchants(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken)
        {
            List<MerchantResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<MerchantResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant list for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<List<ContractProductTransactionFee>> GetTransactionFeesForProduct(String accessToken,
                                                                                            Guid estateId,
                                                                                            Guid merchantId,
                                                                                            Guid contractId,
                                                                                            Guid productId,
                                                                                            CancellationToken cancellationToken)
        {
            List<ContractProductTransactionFee> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts/{contractId}/products/{productId}/transactionFees");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<ContractProductTransactionFee>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error transaction fees for product {productId} on contract {contractId} for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<MakeMerchantDepositResponse> MakeMerchantDeposit(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           MakeMerchantDepositRequest makeMerchantDepositRequest,
                                                                           CancellationToken cancellationToken)
        {
            MakeMerchantDepositResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/deposits");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(makeMerchantDepositRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<MakeMerchantDepositResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error making merchant deposit for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<MakeMerchantWithdrawalResponse> MakeMerchantWithdrawal(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest,
                                                                           CancellationToken cancellationToken) {
            MakeMerchantWithdrawalResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/withdrawals");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(makeMerchantWithdrawalRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<MakeMerchantWithdrawalResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error making merchant withdrawal for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task SetMerchantSettlementSchedule(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        SetSettlementScheduleRequest setSettlementScheduleRequest,
                                                        CancellationToken cancellationToken)
        {
            MakeMerchantDepositResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(setSettlementScheduleRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = httpContent;

                HttpResponseMessage httpResponse = await this.HttpClient.SendAsync(requestMessage, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error setting settlement interval for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Builds the request URL.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("EstateManagementApi");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }

        public async Task<SettlementResponse> GetSettlement(String accessToken,
                                                            Guid estateId,
                                                            Guid? merchantId,
                                                            Guid settlementId,
                                                            CancellationToken cancellationToken)
        {
            SettlementResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/settlements/{settlementId}?merchantId={merchantId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<SettlementResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting settlement id {settlementId} for estate [{estateId}]");

                throw exception;
            }

            return response;
        }

        public async Task<List<SettlementResponse>> GetSettlements(String accessToken,
                                                                   Guid estateId,
                                                                   Guid? merchantId,
                                                                   String startDate,
                                                                   String endDate,
                                                                   CancellationToken cancellationToken)
        {
            List<SettlementResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/settlements/?merchantId={merchantId}&start_date={startDate}&end_date={endDate}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<SettlementResponse>>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting settlements for estate [{estateId}]");

                throw exception;
            }

            return response;
        }

        #endregion
    }
}
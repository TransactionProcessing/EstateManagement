using Shared.Results;
using SimpleResults;

namespace EstateManagement.Client{
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
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Estate;
    using DataTransferObjects.Responses.Merchant;
    using DataTransferObjects.Responses.Operator;
    using DataTransferObjects.Responses.Settlement;
    using Newtonsoft.Json;
    using AssignOperatorRequest = DataTransferObjects.Requests.Merchant.AssignOperatorRequest;

    public class EstateClient : ClientProxyBase, IEstateClient{
        #region Fields
        
        /// <summary>
        /// The base address resolver
        /// </summary>
        private readonly Func<String, String> BaseAddressResolver;

        private readonly Int32 ApiVersion;
        #endregion

        #region Constructors

        public EstateClient(Func<String, String> baseAddressResolver,
                            HttpClient httpClient, Int32 apiVersion = 0) : base(httpClient){
            this.BaseAddressResolver = baseAddressResolver;

            // Add the API version header
            
            this.HttpClient.DefaultRequestHeaders.Add("api-version", apiVersion.ToString("0.0"));
            this.ApiVersion = apiVersion;

        }

        #endregion

        #region Methods

        public async Task<Result> AddContractToMerchant(String accessToken, Guid estateId, Guid merchantId, AddMerchantContractRequest addMerchantContractRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts");

            try{
                String requestSerialised = JsonConvert.SerializeObject(addMerchantContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding contract {addMerchantContractRequest.ContractId} to merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> AddDeviceToMerchant(String accessToken,
                                                      Guid estateId,
                                                      Guid merchantId,
                                                      AddMerchantDeviceRequest addMerchantDeviceRequest,
                                                      CancellationToken cancellationToken){

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/devices");

            try{
                String requestSerialised = JsonConvert.SerializeObject(addMerchantDeviceRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();

            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding device to merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> AddMerchantAddress(String accessToken, Guid estateId, Guid merchantId, Address newAddressRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/addresses");

            try{
                String requestSerialised = JsonConvert.SerializeObject(newAddressRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding address for merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> AddMerchantContact(String accessToken, Guid estateId, Guid merchantId, Contact newContactRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contacts");

            try{
                String requestSerialised = JsonConvert.SerializeObject(newContactRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding contact for merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> AddProductToContract(String accessToken,
                                                       Guid estateId,
                                                       Guid contractId,
                                                       AddProductToContractRequest addProductToContractRequest,
                                                       CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products");

            try{
                String requestSerialised = JsonConvert.SerializeObject(addProductToContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding product [{addProductToContractRequest.ProductName}] to contract [{contractId}] for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> AddTransactionFeeForProductToContract(String accessToken,
                                                                        Guid estateId,
                                                                        Guid contractId,
                                                                        Guid productId,
                                                                        AddTransactionFeeForProductToContractRequest
                                                                            addTransactionFeeForProductToContractRequest,
                                                                        CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products/{productId}/transactionFees");

            try{
                String requestSerialised = JsonConvert.SerializeObject(addTransactionFeeForProductToContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new
                        Exception($"Error adding transaction fee [{addTransactionFeeForProductToContractRequest.Description}] for product [{productId}] to contract [{contractId}] for estate {estateId}.",
                                  ex);

                throw exception;
            }
        }

        public async Task<Result> AssignOperatorToMerchant(String accessToken,
                                                           Guid estateId,
                                                           Guid merchantId,
                                                           AssignOperatorRequest assignOperatorRequest,
                                                           CancellationToken cancellationToken){

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/operators");

            try{
                String requestSerialised = JsonConvert.SerializeObject(assignOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error assigning operator Id {assignOperatorRequest.OperatorId} to merchant Id {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> RemoveOperatorFromMerchant(String accessToken, Guid estateId, Guid merchantId, Guid operatorId, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/operators/{operatorId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.DeleteAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();

            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error removing operator Id {operatorId} from merchant Id {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> RemoveOperatorFromEstate(String accessToken, Guid estateId, Guid operatorId, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators/{operatorId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.DeleteAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();

            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error removing operator Id {operatorId} from estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> RemoveContractFromMerchant(String accessToken, Guid estateId, Guid merchantId, Guid contractId, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts/{contractId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.DeleteAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();

            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error removing contract Id {contractId} from merchant Id {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> CreateContract(String accessToken,
                                                 Guid estateId,
                                                 CreateContractRequest createContractRequest,
                                                 CancellationToken cancellationToken){
            CreateContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/");

            try{
                String requestSerialised = JsonConvert.SerializeObject(createContractRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating contract [{createContractRequest.Description}] for estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> CreateEstate(String accessToken,
                                               CreateEstateRequest createEstateRequest,
                                               CancellationToken cancellationToken){
            CreateEstateResponse response = null;

            String requestUri = this.BuildRequestUrl("/api/estates/");

            try{
                String requestSerialised = JsonConvert.SerializeObject(createEstateRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new estate {createEstateRequest.EstateName}.", ex);

                throw exception;
            }
        }

        public async Task<Result> CreateOperator(String accessToken, Guid estateId,  CreateOperatorRequest createOperatorRequest, CancellationToken cancellationToken){
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
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new operator {createOperatorRequest.Name}.", ex);

                throw exception;
            }
        }

        public async Task<Result> CreateEstateUser(String accessToken,
                                                   Guid estateId,
                                                   CreateEstateUserRequest createEstateUserRequest,
                                                   CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/users");

            try{
                String requestSerialised = JsonConvert.SerializeObject(createEstateUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success(); ;
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new estate user Estate Id {estateId} Email Address {createEstateUserRequest.EmailAddress}.", ex);

                throw exception;
            }
        }

        public async Task<Result> CreateMerchant(String accessToken,
                                                 Guid estateId,
                                                 CreateMerchantRequest createMerchantRequest,
                                                 CancellationToken cancellationToken){
            CreateMerchantResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants");

            try{
                String requestSerialised = JsonConvert.SerializeObject(createMerchantRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new merchant {createMerchantRequest.Name} for estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> CreateMerchantUser(String accessToken,
                                                     Guid estateId,
                                                     Guid merchantId,
                                                     CreateMerchantUserRequest createMerchantUserRequest,
                                                     CancellationToken cancellationToken){

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/users");

            try{
                String requestSerialised = JsonConvert.SerializeObject(createMerchantUserRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating new merchant user Merchant Id {estateId} Email Address {createMerchantUserRequest.EmailAddress}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> AssignOperatorToEstate(String accessToken,
                                                         Guid estateId,
                                                         DataTransferObjects.Requests.Estate.AssignOperatorRequest assignOperatorRequest,
                                                         CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators");

            try{
                String requestSerialised = JsonConvert.SerializeObject(assignOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error adding new operator {assignOperatorRequest.OperatorId} to estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> DisableTransactionFeeForProduct(String accessToken,
                                                                  Guid estateId,
                                                                  Guid contractId,
                                                                  Guid productId,
                                                                  Guid transactionFeeId,
                                                                  CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}/products/{productId}/transactionFees/{transactionFeeId}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.DeleteAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error disabling transaction fee Id [{transactionFeeId}] for product [{productId}] on contract [{contractId}] for estate {estateId}.",
                                  ex);

                throw exception;
            }
        }

        public async Task<Result<ContractResponse>> GetContract(String accessToken,
                                                                Guid estateId,
                                                                Guid contractId,
                                                                CancellationToken cancellationToken){
            ContractResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts/{contractId}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                // call was successful so now deserialise the body to the response object
                ResponseData<ContractResponse> responseData = this.HandleResponseContent<ContractResponse>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contract {contractId} for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<ContractResponse>>> GetContracts(String accessToken,
                                                                       Guid estateId,
                                                                       CancellationToken cancellationToken){
            List<ContractResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/contracts");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<ContractResponse>> responseData =
                    HandleResponseContent<List<ContractResponse>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contracts for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<EstateResponse>> GetEstate(String accessToken,
                                                            Guid estateId,
                                                            CancellationToken cancellationToken){
            EstateResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                // call was successful so now deserialise the body to the response object
                ResponseData<EstateResponse> responseData =
                    HandleResponseContent<EstateResponse>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<EstateResponse>>> GetEstates(String accessToken,
                                                                   Guid estateId,
                                                                   CancellationToken cancellationToken){
            List<EstateResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/all");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<EstateResponse>> responseData =
                    HandleResponseContent<List<EstateResponse>>(result.Data);
                
                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting all estates for estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<MerchantResponse>> GetMerchant(String accessToken,
                                                                Guid estateId,
                                                                Guid merchantId,
                                                                CancellationToken cancellationToken){
            MerchantResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<MerchantResponse> responseData =
                    HandleResponseContent<MerchantResponse>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<ContractResponse>>> GetMerchantContracts(String accessToken,
                                                                               Guid estateId,
                                                                               Guid merchantId,
                                                                               CancellationToken cancellationToken){
            List<ContractResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<ContractResponse>> responseData =
                    HandleResponseContent<List<ContractResponse>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant contracts for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<MerchantResponse>>> GetMerchants(String accessToken,
                                                                       Guid estateId,
                                                                       CancellationToken cancellationToken){
            List<MerchantResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<MerchantResponse>> responseData =
                    HandleResponseContent<List<MerchantResponse>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant list for estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<SettlementResponse>> GetSettlement(String accessToken,
                                                                    Guid estateId,
                                                                    Guid? merchantId,
                                                                    Guid settlementId,
                                                                    CancellationToken cancellationToken){
            SettlementResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/settlements/{settlementId}?merchantId={merchantId}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<SettlementResponse> responseData =
                    HandleResponseContent<SettlementResponse>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting settlement id {settlementId} for estate [{estateId}]");

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<SettlementResponse>>> GetSettlements(String accessToken,
                                                                           Guid estateId,
                                                                           Guid? merchantId,
                                                                           String startDate,
                                                                           String endDate,
                                                                           CancellationToken cancellationToken){
            List<SettlementResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/settlements/?merchantId={merchantId}&start_date={startDate}&end_date={endDate}");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<SettlementResponse>> responseData =
                    HandleResponseContent<List<SettlementResponse>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting settlements for estate [{estateId}]");

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<ContractProductTransactionFee>>> GetTransactionFeesForProduct(String accessToken,
                                                                                                    Guid estateId,
                                                                                                    Guid merchantId,
                                                                                                    Guid contractId,
                                                                                                    Guid productId,
                                                                                                    CancellationToken cancellationToken){
            List<ContractProductTransactionFee> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contracts/{contractId}/products/{productId}/transactionFees");

            try{
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                // call was successful so now deserialise the body to the response object
                ResponseData<List<ContractProductTransactionFee>> responseData =
                    HandleResponseContent<List<ContractProductTransactionFee>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error transaction fees for product {productId} on contract {contractId} for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result> MakeMerchantDeposit(String accessToken,
                                                      Guid estateId,
                                                      Guid merchantId,
                                                      MakeMerchantDepositRequest makeMerchantDepositRequest,
                                                      CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/deposits");

            try{
                String requestSerialised = JsonConvert.SerializeObject(makeMerchantDepositRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);
                
                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error making merchant deposit for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> MakeMerchantWithdrawal(String accessToken,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest,
                                                         CancellationToken cancellationToken){
            MakeMerchantWithdrawalResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/withdrawals");

            try{
                String requestSerialised = JsonConvert.SerializeObject(makeMerchantWithdrawalRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error making merchant withdrawal for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> SetMerchantSettlementSchedule(String accessToken,
                                                                Guid estateId,
                                                                Guid merchantId,
                                                                SetSettlementScheduleRequest setSettlementScheduleRequest,
                                                                CancellationToken cancellationToken){
            MakeMerchantDepositResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");

            try{
                String requestSerialised = JsonConvert.SerializeObject(setSettlementScheduleRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = httpContent;

                HttpResponseMessage httpResponse = await this.HttpClient.SendAsync(requestMessage, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error setting settlement interval for merchant {merchantId} for estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> SwapDeviceForMerchant(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        String deviceIdentifier,
                                                        SwapMerchantDeviceRequest swapMerchantDeviceRequest,
                                                        CancellationToken cancellationToken){

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/devices/{deviceIdentifier}");

            try{
                String requestSerialised = JsonConvert.SerializeObject(swapMerchantDeviceRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error swapping device for merchant Id {merchantId} in estate {estateId}.", ex);

                throw exception;
            }
        }

        public async Task<Result> UpdateMerchant(String accessToken, Guid estateId, Guid merchantId, UpdateMerchantRequest updateMerchantRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}");

            try{
                String requestSerialised = JsonConvert.SerializeObject(updateMerchantRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> UpdateMerchantAddress(String accessToken, Guid estateId, Guid merchantId, Guid addressId, Address updatedAddressRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/addresses/{addressId}");

            try{
                String requestSerialised = JsonConvert.SerializeObject(updatedAddressRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating address {addressId} for merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> UpdateMerchantContact(String accessToken, Guid estateId, Guid merchantId, Guid contactId, Contact updatedContactRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/merchants/{merchantId}/contacts/{contactId}");

            try{
                String requestSerialised = JsonConvert.SerializeObject(updatedContactRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PatchAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch(Exception ex){
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating contact {contactId} for merchant {merchantId} for estate {estateId}.",
                                                    ex);

                throw exception;
            }
        }

        public async Task<Result> UpdateOperator(String accessToken, Guid estateId, Guid operatorId, UpdateOperatorRequest updateOperatorRequest, CancellationToken cancellationToken){
            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators/{operatorId}");

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(updateOperatorRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return Result.Success();
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error updating operator id {operatorId}.", ex);

                throw exception;
            }
        }

        public async Task<Result<OperatorResponse>> GetOperator(String accessToken, Guid estateId, Guid operatorId, CancellationToken cancellationToken){
            OperatorResponse response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators/{operatorId}");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<OperatorResponse> responseData =
                    HandleResponseContent<OperatorResponse>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting operator id {operatorId} for estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<Result<List<OperatorResponse>>> GetOperators(String accessToken, Guid estateId, CancellationToken cancellationToken){
            List<OperatorResponse> response = null;

            String requestUri = this.BuildRequestUrl($"/api/estates/{estateId}/operators");

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                Result<String> result = await this.HandleResponseX(httpResponse, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                ResponseData<List<OperatorResponse>> responseData =
                    HandleResponseContent<List<OperatorResponse>>(result.Data);

                return Result.Success(responseData.Data);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting all operators for estate Id {estateId}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Builds the request URL.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private String BuildRequestUrl(String route){
            String baseAddress = this.BaseAddressResolver("EstateManagementApi");

            String requestUri = this.ApiVersion switch {
                0 => $"{baseAddress}{route}",
                _ => $"{baseAddress}{route.Replace("api", $"api/v{this.ApiVersion}")}"
            };
            
            return requestUri;
        }

        #endregion
    }
}
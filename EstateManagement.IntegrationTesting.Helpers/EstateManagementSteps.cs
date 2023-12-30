namespace EstateManagement.IntegrationTesting.Helpers;

using System.Text;
using Client;
using DataTransferObjects;
using DataTransferObjects.Requests;
using DataTransferObjects.Responses;
using Shared.IntegrationTesting;
using Shouldly;

public class EstateManagementSteps{
    private readonly IEstateClient EstateClient;

    private readonly HttpClient TestHostClient;

    public EstateManagementSteps(IEstateClient estateClient, HttpClient testHostClient){
        this.EstateClient = estateClient;
        this.TestHostClient = testHostClient;
    }

    public async Task WhenIGetTheEstateTheEstateDetailsAreReturnedAsFollows(String accessToken, String estateName, List<EstateDetails> estateDetailsList, List<String> expectedEstateDetails){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        EstateResponse estate = await this.EstateClient.GetEstate(token, estateId, CancellationToken.None).ConfigureAwait(false);
        estate.EstateName.ShouldBe(expectedEstateDetails.Single());
    }

    public async Task WhenIMakeTheFollowingMerchantWithdrawals(String accessToken, List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> requests){
        foreach ((EstateDetails, Guid, MakeMerchantWithdrawalRequest) makeMerchantWithdrawalRequest in requests){

            MakeMerchantWithdrawalResponse makeMerchantWithdrawalResponse = await this.EstateClient
                                                                                      .MakeMerchantWithdrawal(accessToken,
                                                                                                              makeMerchantWithdrawalRequest.Item1.EstateId,
                                                                                                              makeMerchantWithdrawalRequest.Item2,
                                                                                                              makeMerchantWithdrawalRequest.Item3,
                                                                                                              CancellationToken.None).ConfigureAwait(false);

            makeMerchantWithdrawalResponse.EstateId.ShouldBe(makeMerchantWithdrawalRequest.Item1.EstateId);
            makeMerchantWithdrawalResponse.MerchantId.ShouldBe(makeMerchantWithdrawalRequest.Item2);
            makeMerchantWithdrawalResponse.WithdrawalId.ShouldNotBe(Guid.Empty);
        }
    }

    public async Task WhenICreateTheFollowingSecurityUsers(String accessToken, List<CreateNewUserRequest> requests, List<EstateDetails> estateDetailsList){
        foreach (CreateNewUserRequest createNewUserRequest in requests){
            var estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateId == createNewUserRequest.EstateId.GetValueOrDefault());
            estateDetails.ShouldNotBeNull();


            if (createNewUserRequest.UserType == 1){
                CreateEstateUserRequest request = new CreateEstateUserRequest{
                                                                                 EmailAddress = createNewUserRequest.EmailAddress,
                                                                                 FamilyName = createNewUserRequest.FamilyName,
                                                                                 GivenName = createNewUserRequest.GivenName,
                                                                                 MiddleName = createNewUserRequest.MiddleName,
                                                                                 Password = createNewUserRequest.Password
                                                                             };

                CreateEstateUserResponse createEstateUserResponse =
                    await this.EstateClient.CreateEstateUser(accessToken,
                                                             estateDetails.EstateId,
                                                             request,
                                                             CancellationToken.None);

                createEstateUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                createEstateUserResponse.UserId.ShouldNotBe(Guid.Empty);
                estateDetails.SetEstateUser(request.EmailAddress, request.Password);

                //        this.TestingContext.Logger.LogInformation($"Security user {createEstateUserRequest.EmailAddress} assigned to Estate {estateDetails.EstateName}");
            }
            else{
                // Creating a merchant user
                String token = accessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                    token = estateDetails.AccessToken;
                }

                CreateMerchantUserRequest createMerchantUserRequest = new CreateMerchantUserRequest{
                                                                                                       EmailAddress = createNewUserRequest.EmailAddress,
                                                                                                       FamilyName = createNewUserRequest.FamilyName,
                                                                                                       GivenName = createNewUserRequest.GivenName,
                                                                                                       MiddleName = createNewUserRequest.MiddleName,
                                                                                                       Password = createNewUserRequest.Password
                                                                                                   };

                CreateMerchantUserResponse createMerchantUserResponse =
                    await this.EstateClient.CreateMerchantUser(token,
                                                               estateDetails.EstateId,
                                                               createNewUserRequest.MerchantId.Value,
                                                               createMerchantUserRequest,
                                                               CancellationToken.None);

                createMerchantUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                createMerchantUserResponse.MerchantId.ShouldBe(createNewUserRequest.MerchantId.Value);
                createMerchantUserResponse.UserId.ShouldNotBe(Guid.Empty);

                estateDetails.AddMerchantUser(createNewUserRequest.MerchantName, createMerchantUserRequest.EmailAddress, createMerchantUserRequest.Password);

                //        this.TestingContext.Logger.LogInformation($"Security user {createMerchantUserRequest.EmailAddress} assigned to Merchant {merchantName}");
            }
        }
    }

    public async Task<List<EstateResponse>> WhenICreateTheFollowingEstates(String accessToken, List<CreateEstateRequest> requests){
        foreach (CreateEstateRequest createEstateRequest in requests){
            CreateEstateResponse response = await this.EstateClient
                                                      .CreateEstate(accessToken, createEstateRequest, CancellationToken.None)
                                                      .ConfigureAwait(false);

            response.ShouldNotBeNull();
            response.EstateId.ShouldNotBe(Guid.Empty);
        }

        List<EstateResponse> results = new List<EstateResponse>();
        foreach (CreateEstateRequest createEstateRequest in requests){
            EstateResponse estate = null;
            await Retry.For(async () => {
                                estate = await this.EstateClient
                                                   .GetEstate(accessToken, createEstateRequest.EstateId, CancellationToken.None).ConfigureAwait(false);
                                estate.ShouldNotBeNull();
                            },
                            retryFor:TimeSpan.FromSeconds(180)).ConfigureAwait(false);

            estate.EstateName.ShouldBe(createEstateRequest.EstateName);
            results.Add(estate);
        }

        return results;
    }

    public async Task<List<(Guid, EstateOperatorResponse)>> WhenICreateTheFollowingOperators(String accessToken, List<(EstateDetails estate, CreateOperatorRequest request)> requests){
        List<(Guid, EstateOperatorResponse)> results = new List<(Guid, EstateOperatorResponse)>();
        foreach ((EstateDetails estate, CreateOperatorRequest request) request in requests){
            CreateOperatorResponse response = await this.EstateClient
                                                        .CreateOperator(accessToken,
                                                                        request.estate.EstateId,
                                                                        request.request,
                                                                        CancellationToken.None).ConfigureAwait(false);

            response.ShouldNotBeNull();
            response.EstateId.ShouldNotBe(Guid.Empty);
            response.OperatorId.ShouldNotBe(Guid.Empty);
        }

        // verify at the read model
        foreach ((EstateDetails estate, CreateOperatorRequest request) request in requests){
            await Retry.For(async () => {
                                EstateResponse e = await this.EstateClient.GetEstate(accessToken,
                                                                                     request.estate.EstateId,
                                                                                     CancellationToken.None);
                                EstateOperatorResponse operatorResponse = e.Operators.SingleOrDefault(o => o.Name == request.request.Name);
                                operatorResponse.ShouldNotBeNull();
                                results.Add((request.estate.EstateId, operatorResponse));

                                request.estate.AddOperator(operatorResponse.OperatorId, request.request.Name);
                            },
                            retryFor:TimeSpan.FromSeconds(180)).ConfigureAwait(false);
        }

        return results;
    }

    public async Task<List<MerchantResponse>> WhenICreateTheFollowingMerchants(string accessToken, List<(EstateDetails estate, CreateMerchantRequest request)> requests){
        List<MerchantResponse> responses = new List<MerchantResponse>();

        List<(Guid, Guid, String)> merchants = new List<(Guid, Guid, String)>();

        foreach ((EstateDetails estate, CreateMerchantRequest request) request in requests){

            CreateMerchantResponse response = await this.EstateClient
                                                        .CreateMerchant(accessToken, request.estate.EstateId, request.request, CancellationToken.None)
                                                        .ConfigureAwait(false);

            response.ShouldNotBeNull();
            response.EstateId.ShouldBe(request.estate.EstateId);
            response.MerchantId.ShouldNotBe(Guid.Empty);

            merchants.Add((response.EstateId, response.MerchantId, request.request.Name));
        }

        foreach ((Guid, Guid, String) m in merchants){
            await Retry.For(async () => {
                                MerchantResponse merchant = await this.EstateClient
                                                                      .GetMerchant(accessToken, m.Item1, m.Item2, CancellationToken.None)
                                                                      .ConfigureAwait(false);
                                responses.Add(merchant);
                            });
        }

        return responses;
    }

    public async Task<List<(EstateDetails, MerchantOperatorResponse)>> WhenIAssignTheFollowingOperatorToTheMerchants(string accessToken, List<(EstateDetails estate, Guid merchantId, AssignOperatorRequest request)> requests){

        List<(EstateDetails, MerchantOperatorResponse)> result = new();

        List<(EstateDetails estate, Guid merchantId, Guid operatorId)> merchantOperators = new();
        foreach ((EstateDetails estate, Guid merchantId, AssignOperatorRequest request) request in requests){
            AssignOperatorResponse assignOperatorResponse = await this.EstateClient
                                                                      .AssignOperatorToMerchant(accessToken,
                                                                                                request.estate.EstateId,
                                                                                                request.merchantId,
                                                                                                request.request,
                                                                                                CancellationToken.None).ConfigureAwait(false);

            assignOperatorResponse.EstateId.ShouldBe(request.estate.EstateId);
            assignOperatorResponse.MerchantId.ShouldBe(request.merchantId);
            assignOperatorResponse.OperatorId.ShouldBe(request.request.OperatorId);

            merchantOperators.Add((request.estate, assignOperatorResponse.MerchantId, assignOperatorResponse.OperatorId));
        }

        foreach (var m in merchantOperators){
            await Retry.For(async () => {
                                MerchantResponse merchant = await this.EstateClient
                                                                      .GetMerchant(accessToken, m.estate.EstateId, m.merchantId, CancellationToken.None)
                                                                      .ConfigureAwait(false);
                                MerchantOperatorResponse op = merchant.Operators.SingleOrDefault(o => o.OperatorId == m.operatorId);
                                op.ShouldNotBeNull();
                                result.Add((m.estate, op));
                            });
        }

        return result;
    }

    public async Task<List<(EstateDetails, MerchantResponse, String)>> GivenIHaveAssignedTheFollowingDevicesToTheMerchants(string accessToken, List<(EstateDetails, Guid, AddMerchantDeviceRequest)> requests){
        List<(EstateDetails, MerchantResponse, String)> result = new();
        List<(EstateDetails estate, Guid merchantId, Guid deviceId)> merchantDevices = new();
        foreach ((EstateDetails, Guid, AddMerchantDeviceRequest) request in requests){
            AddMerchantDeviceResponse addMerchantDeviceResponse = await this.EstateClient.AddDeviceToMerchant(accessToken, request.Item1.EstateId, request.Item2, request.Item3, CancellationToken.None).ConfigureAwait(false);

            addMerchantDeviceResponse.EstateId.ShouldBe(request.Item1.EstateId);
            addMerchantDeviceResponse.MerchantId.ShouldBe(request.Item2);
            addMerchantDeviceResponse.DeviceId.ShouldNotBe(Guid.Empty);
        }

        foreach (var m in merchantDevices){
            await Retry.For(async () => {
                                MerchantResponse merchant = await this.EstateClient
                                                                      .GetMerchant(accessToken, m.estate.EstateId, m.merchantId, CancellationToken.None)
                                                                      .ConfigureAwait(false);
                                var device = merchant.Devices.SingleOrDefault(o => o.Key == m.deviceId);
                                device.Value.ShouldNotBeNull();
                                result.Add((m.estate, merchant, device.Value));
                            });
        }

        return result;
    }

    public async Task<List<ContractResponse>> GivenICreateAContractWithTheFollowingValues(string accessToken, List<(EstateDetails, CreateContractRequest)> requests){
        List<ContractResponse> result = new List<ContractResponse>();

        List<(EstateDetails, Guid)> estateContracts = new List<(EstateDetails, Guid)>();

        foreach ((EstateDetails, CreateContractRequest) request in requests){
            CreateContractResponse contractResponse =
                await this.EstateClient.CreateContract(accessToken, request.Item1.EstateId, request.Item2, CancellationToken.None);
            estateContracts.Add((request.Item1, contractResponse.ContractId));
        }

        foreach ((EstateDetails, Guid) estateContract in estateContracts){

            await Retry.For(async () => {
                                ContractResponse contract = await this.EstateClient.GetContract(accessToken, estateContract.Item1.EstateId, estateContract.Item2, CancellationToken.None).ConfigureAwait(false);
                                contract.ShouldNotBeNull();
                                result.Add(contract);
                                estateContract.Item1.AddContract(contract.ContractId, contract.Description, contract.OperatorId);
                            });
        }

        return result;
    }

    public async Task WhenICreateTheFollowingProducts(String accessToken, List<(EstateDetails, Contract, AddProductToContractRequest)> requests){
        List<(EstateDetails, Contract, AddProductToContractRequest, AddProductToContractResponse)> estateContractProducts = new();
        foreach ((EstateDetails, Contract, AddProductToContractRequest) request in requests){
            AddProductToContractResponse addProductToContractResponse =
                await this.EstateClient.AddProductToContract(accessToken,
                                                             request.Item1.EstateId,
                                                             request.Item2.ContractId,
                                                             request.Item3,
                                                             CancellationToken.None);
            estateContractProducts.Add((request.Item1, request.Item2, request.Item3, addProductToContractResponse));
        }

        foreach ((EstateDetails, Contract, AddProductToContractRequest, AddProductToContractResponse) estateContractProduct in estateContractProducts){

            await Retry.For(async () => {
                                ContractResponse contract = await this.EstateClient.GetContract(accessToken, estateContractProduct.Item1.EstateId, estateContractProduct.Item2.ContractId, CancellationToken.None).ConfigureAwait(false);
                                contract.ShouldNotBeNull();

                                ContractProduct product = contract.Products.SingleOrDefault(c => c.ProductId == estateContractProduct.Item4.ProductId);
                                product.ShouldNotBeNull();

                                estateContractProduct.Item2.AddProduct(estateContractProduct.Item4.ProductId,
                                                                       estateContractProduct.Item3.ProductName,
                                                                       estateContractProduct.Item3.DisplayText,
                                                                       estateContractProduct.Item3.Value);
                            });
        }
    }

    public async Task WhenIAddTheFollowingContractsToTheFollowingMerchants(String accessToken, List<(EstateDetails, Guid, Guid)> requests)
    {
        foreach ((EstateDetails, Guid, Guid) request in requests){
            AddMerchantContractRequest addMerchantContractRequest = new AddMerchantContractRequest{
                                                                                                      ContractId = request.Item3
                                                                                                  };
            await this.EstateClient.AddContractToMerchant(accessToken, request.Item1.EstateId, request.Item2, addMerchantContractRequest, CancellationToken.None);
        }
    }

    public async Task WhenIAddTheFollowingTransactionFees(String accessToken, List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> requests){
        List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest, AddTransactionFeeForProductToContractResponse)> estateContractProductsFees = new();
        foreach ((EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest) request in requests){
            AddTransactionFeeForProductToContractResponse addTransactionFeeForProductToContractResponse =
                await this.EstateClient.AddTransactionFeeForProductToContract(accessToken,
                                                                              request.Item1.EstateId,
                                                                              request.Item2.ContractId,
                                                                              request.Item3.ProductId,
                                                                              request.Item4,
                                                                              CancellationToken.None);
        }

        foreach ((EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest, AddTransactionFeeForProductToContractResponse) estateContractProductsFee in estateContractProductsFees){
            await Retry.For(async () => {
                                ContractResponse contract = await this.EstateClient.GetContract(accessToken, estateContractProductsFee.Item1.EstateId, estateContractProductsFee.Item2.ContractId, CancellationToken.None).ConfigureAwait(false);
                                contract.ShouldNotBeNull();

                                ContractProduct product = contract.Products.SingleOrDefault(c => c.ProductId == estateContractProductsFee.Item3.ProductId);
                                product.ShouldNotBeNull();

                                var transactionFee = product.TransactionFees.SingleOrDefault(f => f.TransactionFeeId == estateContractProductsFee.Item5.TransactionFeeId);
                                transactionFee.ShouldNotBeNull();

                                estateContractProductsFee.Item3.AddTransactionFee(estateContractProductsFee.Item5.TransactionFeeId,
                                                                                  estateContractProductsFee.Item4.CalculationType,
                                                                                  estateContractProductsFee.Item4.FeeType,
                                                                                  estateContractProductsFee.Item4.Description,
                                                                                  estateContractProductsFee.Item4.Value);
                            });
        }
    }

    public async Task GivenIMakeTheFollowingManualMerchantDeposits(String accessToken, (EstateDetails, Guid, MakeMerchantDepositRequest) request){
        MakeMerchantDepositResponse makeMerchantDepositResponse = await this.EstateClient.MakeMerchantDeposit(accessToken, request.Item1.EstateId, request.Item2, request.Item3, CancellationToken.None).ConfigureAwait(false);

        makeMerchantDepositResponse.EstateId.ShouldBe(request.Item1.EstateId);
        makeMerchantDepositResponse.MerchantId.ShouldBe(request.Item2);
        makeMerchantDepositResponse.DepositId.ShouldNotBe(Guid.Empty);

    }

    public async Task WhenIGetTheEstateAnErrorIsReturned(String accessToken, String estateName, List<EstateDetails> estateDetailsList){
        Guid estateId = Guid.NewGuid();
        //EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        //estateDetails.ShouldNotBeNull();

        //String token = accessToken;
        //if (estateDetails != null){
        //    estateId = estateDetails.EstateId;
        //    if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
        //        token = estateDetails.AccessToken;
        //    }
        //}

        Exception exception = Should.Throw<Exception>(async () => { await this.EstateClient.GetEstate(accessToken, estateId, CancellationToken.None).ConfigureAwait(false); });
        exception.InnerException.ShouldBeOfType<KeyNotFoundException>();
    }

    public async Task WhenIGetTheEstateTheEstateSecurityUserDetailsAreReturnedAsFollows(String accessToken, String estateName, List<EstateDetails> estateDetailsList, List<String> expectedSecurityUsers){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        EstateResponse? estate = await this.EstateClient.GetEstate(token, estateId, CancellationToken.None).ConfigureAwait(false);
        estate.ShouldNotBeNull();
        foreach (String expectedSecurityUser in expectedSecurityUsers){
            var user = estate.SecurityUsers.SingleOrDefault(o => o.EmailAddress == expectedSecurityUser);
            user.ShouldNotBeNull();
        }

    }

    public async Task WhenIGetTheEstateTheEstateOperatorDetailsAreReturnedAsFollows(String accessToken, String estateName, List<EstateDetails> estateDetailsList, List<String> expectedOperators){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        EstateResponse? estate = await this.EstateClient.GetEstate(token, estateId, CancellationToken.None).ConfigureAwait(false);
        estate.ShouldNotBeNull();
        foreach (String expectedOperator in expectedOperators){
            EstateOperatorResponse? op = estate.Operators.SingleOrDefault(o => o.Name == expectedOperator);
            op.ShouldNotBeNull();
        }

    }

    public async Task WhenIGetTheMerchantsForThenMerchantsWillBeReturned(String accessToken, String estateName, List<EstateDetails> estateDetailsList, Int32 expectedMerchantCount){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        await Retry.For(async () => {
                            List<MerchantResponse> merchantList = await this.EstateClient
                                                                            .GetMerchants(token, estateDetails.EstateId, CancellationToken.None)
                                                                            .ConfigureAwait(false);

                            merchantList.ShouldNotBeNull();
                            merchantList.ShouldNotBeEmpty();
                            merchantList.Count.ShouldBe(expectedMerchantCount);
                        });
    }

    public async Task ThenIGetTheContractsForTheFollowingContractDetailsAreReturned(String accessToken, String estateName, List<EstateDetails> estateDetailsList, List<(String, String)> contractDetails){

        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        await Retry.For(async () => {
                            List<ContractResponse> contracts =
                                await this.EstateClient.GetContracts(token, estateDetails.EstateId, CancellationToken.None);

                            contracts.ShouldNotBeNull();
                            contracts.ShouldHaveSingleItem();
                            ContractResponse contract = contracts.Single();
                            contract.Products.ShouldNotBeNull();
                            foreach ((String, String) contractDetail in contractDetails){
                                contract.Description.ShouldBe(contractDetail.Item1);
                                contract.Products.Any(p => p.Name == contractDetail.Item2).ShouldBeTrue();
                            }
                        });

    }

    public async Task ThenIGetTheMerchantContractsForForTheFollowingContractDetailsAreReturned(String accessToken, String estateName, String merchantName, List<EstateDetails> estateDetailsList, List<(String, String)> contractDetails){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

        await Retry.For(async () => {
                            List<ContractResponse> contracts =
                                await this.EstateClient.GetMerchantContracts(token, estateDetails.EstateId, merchantId, CancellationToken.None);

                            contracts.ShouldNotBeNull();
                            contracts.ShouldHaveSingleItem();
                            ContractResponse contractResponse = contracts.Single();

                            foreach (var contract in contractDetails){
                                contractResponse.Description.ShouldBe(contract.Item1);
                                contractResponse.Products.Any(p => p.Name == contract.Item2).ShouldBeTrue();
                            }
                        });
    }

    public async Task ThenIGetTheTransactionFeesForOnTheContractForTheFollowingFeesAreReturned(String accessToken, String estateName, String contractName, String productName, List<EstateDetails> estateDetailsList, List<(CalculationType, String, Decimal?, FeeType)> transactionFees){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        Contract contract = estateDetails.GetContract(contractName);

        Product product = contract.GetProduct(productName);

        await Retry.For(async () => {
                            List<ContractProductTransactionFee> transactionFeesResults =
                                await this.EstateClient.GetTransactionFeesForProduct(token,
                                                                                     estateDetails.EstateId,
                                                                                     Guid.Empty,
                                                                                     contract.ContractId,
                                                                                     product.ProductId,
                                                                                     CancellationToken.None);
                            foreach ((CalculationType, String, Decimal?, FeeType) transactionFee in transactionFees){
                                Boolean feeFound = transactionFeesResults.Any(f => f.CalculationType == transactionFee.Item1 && f.Description == transactionFee.Item2 &&
                                                                                   f.Value == transactionFee.Item3 && f.FeeType == transactionFee.Item4);

                                feeFound.ShouldBeTrue();
                            }
                        });

    }

    public async Task WhenIMakeTheFollowingAutomaticMerchantDeposits(List<String> makeDepositRequests){
        foreach (String makeDepositRequest in makeDepositRequests){
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/testbank");
            requestMessage.Content = new StringContent(makeDepositRequest, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await this.TestHostClient.SendAsync(requestMessage);

            responseMessage.IsSuccessStatusCode.ShouldBeTrue();
        }
    }

    public async Task WhenIMakeTheFollowingMerchantDepositsTheDepositIsRejected(String accessToken, List<(EstateDetails, Guid, MakeMerchantDepositRequest)> makeDepositRequests)
    {
        foreach ((EstateDetails, Guid, MakeMerchantDepositRequest) makeDepositRequest in makeDepositRequests){
            Exception exception = Should.Throw<Exception>(async () =>
                                                          {
                                                              await this.EstateClient
                                                                        .MakeMerchantDeposit(accessToken,
                                                                                             makeDepositRequest.Item1.EstateId,
                                                                                             makeDepositRequest.Item2,
                                                                                             makeDepositRequest.Item3,
                                                                                             CancellationToken.None).ConfigureAwait(false);
                                                          });
            exception.InnerException.GetType().ShouldBe(typeof(InvalidOperationException));
        }
    }

    public async Task WhenIGetTheMerchantForEstateAnErrorIsReturned(String accessToken, String estateName, String merchantName, List<EstateDetails> estateDetailsList){
        Guid estateId = Guid.NewGuid();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        String token = accessToken;
        if (estateDetails != null){
            estateId = estateDetails.EstateId;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false){
                token = estateDetails.AccessToken;
            }
        }

        Guid merchantId = Guid.NewGuid();
            
        Exception exception = Should.Throw<Exception>(async () => {
                                                          await this.EstateClient
                                                                    .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                                    .ConfigureAwait(false);
                                                      });
        exception.InnerException.ShouldBeOfType<KeyNotFoundException>();
    }

    public async Task WhenISwapTheMerchantDeviceTheDeviceIsSwapped(String accessToken, List<(EstateDetails, Guid, SwapMerchantDeviceRequest)> requests){
        foreach ((EstateDetails, Guid, SwapMerchantDeviceRequest) request in requests){
            SwapMerchantDeviceResponse swapMerchantDeviceResponse = await this.EstateClient
                                                                              .SwapDeviceForMerchant(accessToken,
                                                                                                     request.Item1.EstateId,
                                                                                                     request.Item2,
                                                                                                     request.Item3,
                                                                                                     CancellationToken.None).ConfigureAwait(false);

            swapMerchantDeviceResponse.EstateId.ShouldBe(request.Item1.EstateId);
            swapMerchantDeviceResponse.MerchantId.ShouldBe(request.Item2);
            swapMerchantDeviceResponse.DeviceId.ShouldNotBe(Guid.Empty);

            //this.TestingContext.Logger.LogInformation($"Device {newDeviceIdentifier} assigned to Merchant {merchantName}");

            await Retry.For(async () => {
                                MerchantResponse? merchantResponse = await this.EstateClient
                                                                               .GetMerchant(accessToken, request.Item1.EstateId, request.Item2, CancellationToken.None)
                                                                               .ConfigureAwait(false);

                                merchantResponse.Devices.ContainsValue(request.Item3.NewDeviceIdentifier);
                            });
        }
    }

    public async Task WhenISetTheMerchantsSettlementSchedule(String accessToken, List<(EstateDetails, Guid, SetSettlementScheduleRequest)> requests){
        foreach (var request in requests){
            Should.NotThrow(async () => {
                                await this.EstateClient.SetMerchantSettlementSchedule(accessToken,
                                                                                      request.Item1.EstateId,
                                                                                      request.Item2,
                                                                                      request.Item3,
                                                                                      CancellationToken.None);
                            });

        }
    }

    public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(String accessToken, DateTime stateDate, DateTime endDate, SpecflowExtensions.SettlementDetails expectedSettlementDetails){
        await Retry.For(async () => {
                            List<DataTransferObjects.Responses.SettlementResponse> settlementList =
                                await this.EstateClient.GetSettlements(accessToken,
                                                                       expectedSettlementDetails.EstateId,
                                                                       expectedSettlementDetails.MerchantId,
                                                                       stateDate.ToString("yyyyMMdd"),
                                                                       endDate.ToString("yyyyMMdd"),
                                                                       CancellationToken.None);

                            settlementList.ShouldNotBeNull();
                            settlementList.ShouldNotBeEmpty();

                            DataTransferObjects.Responses.SettlementResponse settlement =
                                settlementList.SingleOrDefault(s => s.SettlementDate == expectedSettlementDetails.SettlementDate &&
                                                                    s.NumberOfFeesSettled == expectedSettlementDetails.NumberOfFeesSettled &&
                                                                    s.ValueOfFeesSettled == expectedSettlementDetails.ValueOfFeesSettled && s.IsCompleted == expectedSettlementDetails.IsCompleted);

                            settlement.ShouldNotBeNull();
                        },
                        TimeSpan.FromMinutes(2));
    }

    public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheDateTheFollowingFeesAreSettled(String accessToken, List<SpecflowExtensions.SettlementFeeDetails> settlementFeeDetailsList){
        var settlements = settlementFeeDetailsList.DistinctBy(d => new{
                                                                          d.EstateId,
                                                                          d.MerchantId,
                                                                          d.SettlementId
                                                                      }).Select(s => new {
                                                                                             s.EstateId,
                                                                                             s.MerchantId,
                                                                                             s.SettlementId
                                                                                         });

        foreach (var settlementFeeDetails in settlements){
            await Retry.For(async () => {
                                SettlementResponse settlement =
                                    await this.EstateClient.GetSettlement(accessToken,
                                                                          settlementFeeDetails.EstateId,
                                                                          settlementFeeDetails.MerchantId,
                                                                          settlementFeeDetails.SettlementId,
                                                                          CancellationToken.None);

                                settlement.ShouldNotBeNull();

                                settlement.SettlementFees.ShouldNotBeNull();
                                settlement.SettlementFees.ShouldNotBeEmpty();

                                var settlementFees = settlementFeeDetailsList.Where(s => s.EstateId == settlementFeeDetails.EstateId &&
                                                                                         s.MerchantId == settlementFeeDetails.MerchantId &&
                                                                                         s.SettlementId == settlementFeeDetails.SettlementId).ToList();

                                foreach (SpecflowExtensions.SettlementFeeDetails feeDetails in settlementFees){
                                    SettlementFeeResponse settlementFee =
                                        settlement.SettlementFees.SingleOrDefault(sf => sf.FeeDescription == feeDetails.FeeDescription && sf.IsSettled == feeDetails.IsSettled &&
                                                                                        sf.OperatorIdentifier == feeDetails.Operator &&
                                                                                        sf.CalculatedValue == feeDetails.CalculatedValue);

                                    settlementFee.ShouldNotBeNull();
                                }
                            },
                            TimeSpan.FromMinutes(3));
        }
    }
}
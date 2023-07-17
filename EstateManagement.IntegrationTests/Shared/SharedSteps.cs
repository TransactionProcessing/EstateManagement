using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Shared
{
    using System.ComponentModel.Design;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using global::Shared.Logger;
    using Newtonsoft.Json;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using Shouldly;
    using TechTalk.SpecFlow;
    using TransactionProcessor.DataTransferObjects;
    using ClientDetails = Common.ClientDetails;
    using SettlementResponse = TransactionProcessor.DataTransferObjects.SettlementResponse;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public SharedSteps(ScenarioContext scenarioContext,
                           TestingContext testingContext) {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                // Setup the subscriptions for the estate
                await Retry.For(async () => { this.TestingContext.DockerHelper.CreateEstateSubscriptions(estateName).ConfigureAwait(false); },
                                retryFor:TimeSpan.FromMinutes(2),
                                retryInterval:TimeSpan.FromSeconds(30));
            }

            foreach (TableRow tableRow in table.Rows) {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                CreateEstateRequest createEstateRequest = new CreateEstateRequest {
                                                                                      EstateId = Guid.NewGuid(),
                                                                                      EstateName = estateName
                                                                                  };

                CreateEstateResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                          .CreateEstate(this.TestingContext.AccessToken, createEstateRequest, CancellationToken.None)
                                                          .ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Logger.LogInformation($"Estate {estateName} created with Id {response.EstateId}");

                EstateResponse estate = null;
                await Retry.For(async () => {
                                    estate = await this.TestingContext.DockerHelper.EstateClient
                                                       .GetEstate(this.TestingContext.AccessToken, response.EstateId, CancellationToken.None).ConfigureAwait(false);
                                    estate.ShouldNotBeNull();

                                    // Cache the estate id
                                    this.TestingContext.AddEstateDetails(estate.EstateId, estate.EstateName, estate.EstateReference);
                                },
                                TimeSpan.FromMinutes(5),
                                TimeSpan.FromSeconds(60)).ConfigureAwait(false);

                estate.EstateName.ShouldBe(estateName);
            }
        }

        [Given(@"I have created the following operators")]
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
                Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

                CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest {
                                                                                            Name = operatorName,
                                                                                            RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                                                            RequireCustomTerminalNumber = requireCustomTerminalNumber
                                                                                        };

                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                CreateOperatorResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateOperator(this.TestingContext.AccessToken,
                                                                            estateDetails.EstateId,
                                                                            createOperatorRequest,
                                                                            CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);
                response.OperatorId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                estateDetails.AddOperator(response.OperatorId, operatorName);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} created with Id {response.OperatorId} for Estate {estateDetails.EstateName}");
            }
        }

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(Table table) {
            Dictionary<String, Guid> merchantCache = new Dictionary<String, Guid>();

            foreach (TableRow tableRow in table.Rows) {
                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);
                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                var settlementSchedule = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

                SettlementSchedule schedule = SettlementSchedule.Immediate;
                if (String.IsNullOrEmpty(settlementSchedule) == false) {
                    schedule = Enum.Parse<SettlementSchedule>(settlementSchedule);
                }

                CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest {
                                                                                            Name = merchantName,
                                                                                            Contact = new Contact {
                                                                                                          ContactName =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                  "ContactName"),
                                                                                                          EmailAddress =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                  "EmailAddress")
                                                                                                      },
                                                                                            Address = new Address {
                                                                                                          AddressLine1 =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                  "AddressLine1"),
                                                                                                          Town =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                                                          Region =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                  "Region"),
                                                                                                          Country =
                                                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                  "Country")
                                                                                                      },
                                                                                            SettlementSchedule = schedule
                                                                                        };

                CreateMerchantResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateMerchant(token, estateDetails.EstateId, createMerchantRequest, CancellationToken.None)
                                                            .ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldBe(estateDetails.EstateId);
                response.MerchantId.ShouldNotBe(Guid.Empty);

                // Cache the merchant
                merchantCache.Add(merchantName, response.MerchantId);


                this.TestingContext.Logger.LogInformation($"Merchant {merchantName} created with Id {response.MerchantId} for Estate {estateDetails.EstateName}");
            }

            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                var merchantId = merchantCache[merchantName];
                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                var settlementSchedule = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

                SettlementSchedule schedule = SettlementSchedule.Immediate;
                if (String.IsNullOrEmpty(settlementSchedule) == false) {
                    schedule = Enum.Parse<SettlementSchedule>(settlementSchedule);
                }

                await Retry.For(async () => {
                                    MerchantResponse merchant = await this.TestingContext.DockerHelper.EstateClient
                                                                          .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                                          .ConfigureAwait(false);

                                    merchant.MerchantName.ShouldBe(merchantName);
                                    merchant.SettlementSchedule.ShouldBe(schedule);
                                    merchant.MerchantReference.ShouldNotBeNullOrEmpty();
                                    estateDetails.AddMerchant(merchant);
                                });
            }
        }

        [Given(@"I have assigned the following operator to the merchants")]
        [When(@"I assign the following operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                // Lookup the operator id
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest {
                                                                                            OperatorId = operatorId,
                                                                                            MerchantNumber =
                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                                                                                            TerminalNumber =
                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                                                                                        };

                AssignOperatorResponse assignOperatorResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                          .AssignOperatorToMerchant(token,
                                                                                                    estateDetails.EstateId,
                                                                                                    merchantId,
                                                                                                    assignOperatorRequest,
                                                                                                    CancellationToken.None).ConfigureAwait(false);

                assignOperatorResponse.EstateId.ShouldBe(estateDetails.EstateId);
                assignOperatorResponse.MerchantId.ShouldBe(merchantId);
                assignOperatorResponse.OperatorId.ShouldBe(operatorId);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} assigned to Estate {estateDetails.EstateName}");
            }
        }

        [When(@"I create the following security users")]
        [Given("I have created the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                if (tableRow.ContainsKey("EstateName") && tableRow.ContainsKey("MerchantName") == false) {
                    // Creating an Estate User
                    CreateEstateUserRequest createEstateUserRequest = new CreateEstateUserRequest {
                                                                                                      EmailAddress =
                                                                                                          SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                                                                      FamilyName =
                                                                                                          SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                                                                      GivenName =
                                                                                                          SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                                                                      MiddleName =
                                                                                                          SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                                                                      Password =
                                                                                                          SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
                                                                                                  };

                    CreateEstateUserResponse createEstateUserResponse =
                        await this.TestingContext.DockerHelper.EstateClient.CreateEstateUser(this.TestingContext.AccessToken,
                                                                                             estateDetails.EstateId,
                                                                                             createEstateUserRequest,
                                                                                             CancellationToken.None);

                    createEstateUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                    createEstateUserResponse.UserId.ShouldNotBe(Guid.Empty);

                    estateDetails.SetEstateUser(createEstateUserRequest.EmailAddress, createEstateUserRequest.Password);

                    this.TestingContext.Logger.LogInformation($"Security user {createEstateUserRequest.EmailAddress} assigned to Estate {estateDetails.EstateName}");
                }
                else if (tableRow.ContainsKey("MerchantName")) {
                    // Creating a merchant user
                    String token = this.TestingContext.AccessToken;
                    if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                        token = estateDetails.AccessToken;
                    }

                    // lookup the merchant id based on the name in the table
                    String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                    Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                    CreateMerchantUserRequest createMerchantUserRequest = new CreateMerchantUserRequest {
                                                                                                            EmailAddress =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "EmailAddress"),
                                                                                                            FamilyName =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "FamilyName"),
                                                                                                            GivenName =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "GivenName"),
                                                                                                            MiddleName =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "MiddleName"),
                                                                                                            Password =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "Password")
                                                                                                        };

                    CreateMerchantUserResponse createMerchantUserResponse =
                        await this.TestingContext.DockerHelper.EstateClient.CreateMerchantUser(token,
                                                                                               estateDetails.EstateId,
                                                                                               merchantId,
                                                                                               createMerchantUserRequest,
                                                                                               CancellationToken.None);

                    createMerchantUserResponse.EstateId.ShouldBe(estateDetails.EstateId);
                    createMerchantUserResponse.MerchantId.ShouldBe(merchantId);
                    createMerchantUserResponse.UserId.ShouldNotBe(Guid.Empty);

                    estateDetails.AddMerchantUser(merchantName, createMerchantUserRequest.EmailAddress, createMerchantUserRequest.Password);

                    this.TestingContext.Logger.LogInformation($"Security user {createMerchantUserRequest.EmailAddress} assigned to Merchant {merchantName}");
                }
            }
        }

        [Given(@"the following security roles exist")]
        public async Task GivenTheFollowingSecurityRolesExist(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String roleName = SpecflowTableHelper.GetStringRowValue(tableRow, "RoleName");

                CreateRoleRequest createRoleRequest = new CreateRoleRequest {
                                                                                RoleName = roleName
                                                                            };

                CreateRoleResponse createRoleResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateRole(createRoleRequest, CancellationToken.None)
                                                                  .ConfigureAwait(false);

                createRoleResponse.RoleId.ShouldNotBe(Guid.Empty);
            }
        }

        [Given(@"I make the following manual merchant deposits")]
        [When(@"I make the following manual merchant deposits")]
        public async Task WhenIMakeTheFollowingManualMerchantDeposits(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                MerchantBalanceResponse balanceBeforeDeposit = await this.TestingContext.DockerHelper.TransactionProcessorClient.GetMerchantBalance(token, estateDetails.EstateId, merchantId, CancellationToken.None);

                MakeMerchantDepositRequest makeMerchantDepositRequest = new MakeMerchantDepositRequest {
                                                                                                           DepositDateTime =
                                                                                                               SpecflowTableHelper
                                                                                                                   .GetDateTimeForDateString(SpecflowTableHelper
                                                                                                                           .GetStringRowValue(tableRow, "DateTime"),
                                                                                                                       DateTime.Now),
                                                                                                           Reference =
                                                                                                               SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                   "Reference"),
                                                                                                           Amount =
                                                                                                               SpecflowTableHelper.GetDecimalValue(tableRow, "Amount")
                                                                                                       };

                MakeMerchantDepositResponse makeMerchantDepositResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                                    .MakeMerchantDeposit(token,
                                                                                                         estateDetails.EstateId,
                                                                                                         merchantId,
                                                                                                         makeMerchantDepositRequest,
                                                                                                         CancellationToken.None).ConfigureAwait(false);

                makeMerchantDepositResponse.EstateId.ShouldBe(estateDetails.EstateId);
                makeMerchantDepositResponse.MerchantId.ShouldBe(merchantId);
                makeMerchantDepositResponse.DepositId.ShouldNotBe(Guid.Empty);

                await Retry.For(async () => {
                                    MerchantBalanceResponse balanceAfterDeposit =
                                        await this.TestingContext.DockerHelper.TransactionProcessorClient.GetMerchantBalance(token,
                                            estateDetails.EstateId,
                                            merchantId,
                                            CancellationToken.None);
                                    balanceAfterDeposit.AvailableBalance.ShouldBe(balanceBeforeDeposit.AvailableBalance + makeMerchantDepositRequest.Amount);
                                });

                this.TestingContext.Logger.LogInformation($"Deposit Reference {makeMerchantDepositRequest.Reference} made for Merchant {merchantName}");
            }
        }

        [When(@"I make the following merchant withdrawals")]
        public async Task WhenIMakeTheFollowingMerchantWithdrawals(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest = new MakeMerchantWithdrawalRequest {
                                                                                                                    WithdrawalDateTime =
                                                                                                                        SpecflowTableHelper
                                                                                                                            .GetDateTimeForDateString(SpecflowTableHelper
                                                                                                                                    .GetStringRowValue(tableRow,
                                                                                                                                        "DateTime"),
                                                                                                                                DateTime.Now),
                                                                                                                    Amount =
                                                                                                                        SpecflowTableHelper.GetDecimalValue(tableRow,
                                                                                                                            "Amount")
                                                                                                                };

                MakeMerchantWithdrawalResponse makeMerchantWithdrawalResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                                    .MakeMerchantWithdrawal(token,
                                                                                                            estateDetails.EstateId,
                                                                                                            merchantId,
                                                                                                            makeMerchantWithdrawalRequest,
                                                                                                            CancellationToken.None).ConfigureAwait(false);

                makeMerchantWithdrawalResponse.EstateId.ShouldBe(estateDetails.EstateId);
                makeMerchantWithdrawalResponse.MerchantId.ShouldBe(merchantId);
                makeMerchantWithdrawalResponse.WithdrawalId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Logger.LogInformation($"Withdrawal made for Merchant {merchantName}");
            }
        }

        [Given(@"I create the following api scopes")]
        public async Task GivenICreateTheFollowingApiScopes(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                CreateApiScopeRequest createApiScopeRequest = new CreateApiScopeRequest {
                                                                                            Name = SpecflowTableHelper.GetStringRowValue(tableRow, "Name"),
                                                                                            Description = SpecflowTableHelper.GetStringRowValue(tableRow, "Description"),
                                                                                            DisplayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName")
                                                                                        };
                var createApiScopeResponse = await this.CreateApiScope(createApiScopeRequest, CancellationToken.None).ConfigureAwait(false);

                createApiScopeResponse.ShouldNotBeNull();
                createApiScopeResponse.ApiScopeName.ShouldNotBeNullOrEmpty();
            }
        }

        private async Task<CreateApiScopeResponse> CreateApiScope(CreateApiScopeRequest createApiScopeRequest,
                                                                  CancellationToken cancellationToken) {
            CreateApiScopeResponse createApiScopeResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                                      .CreateApiScope(createApiScopeRequest, cancellationToken).ConfigureAwait(false);
            return createApiScopeResponse;
        }

        [Given(@"the following api resources exist")]
        public async Task GivenTheFollowingApiResourcesExist(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String resourceName = SpecflowTableHelper.GetStringRowValue(tableRow, "ResourceName");
                String displayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String scopes = SpecflowTableHelper.GetStringRowValue(tableRow, "Scopes");
                String userClaims = SpecflowTableHelper.GetStringRowValue(tableRow, "UserClaims");

                List<String> splitScopes = scopes.Split(",").ToList();
                List<String> splitUserClaims = userClaims.Split(",").ToList();

                CreateApiResourceRequest createApiResourceRequest = new CreateApiResourceRequest {
                                                                                                     Description = String.Empty,
                                                                                                     DisplayName = displayName,
                                                                                                     Name = resourceName,
                                                                                                     Scopes = new List<String>(),
                                                                                                     Secret = secret,
                                                                                                     UserClaims = new List<String>()
                                                                                                 };
                splitScopes.ForEach(a => { createApiResourceRequest.Scopes.Add(a.Trim()); });
                splitUserClaims.ForEach(a => { createApiResourceRequest.UserClaims.Add(a.Trim()); });

                CreateApiResourceResponse createApiResourceResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                                                .CreateApiResource(createApiResourceRequest, CancellationToken.None)
                                                                                .ConfigureAwait(false);

                createApiResourceResponse.ApiResourceName.ShouldBe(resourceName);
            }
        }

        [Given(@"I create the following identity resources")]
        public async Task GivenICreateTheFollowingIdentityResources(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                // Get the scopes
                String userClaims = SpecflowTableHelper.GetStringRowValue(tableRow, "UserClaims");

                CreateIdentityResourceRequest createIdentityResourceRequest = new CreateIdentityResourceRequest {
                                                                                                                    Name =
                                                                                                                        SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                            "Name"),
                                                                                                                    Claims = string.IsNullOrEmpty(userClaims)
                                                                                                                        ? null
                                                                                                                        : userClaims.Split(",").ToList(),
                                                                                                                    Description =
                                                                                                                        SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                            "Description"),
                                                                                                                    DisplayName =
                                                                                                                        SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                            "DisplayName")
                                                                                                                };

                await this.CreateIdentityResource(createIdentityResourceRequest, CancellationToken.None).ConfigureAwait(false);
            }
        }

        private async Task CreateIdentityResource(CreateIdentityResourceRequest createIdentityResourceRequest,
                                                  CancellationToken cancellationToken) {
            CreateIdentityResourceResponse createIdentityResourceResponse = null;

            List<IdentityResourceDetails> identityResourceList = await this.TestingContext.DockerHelper.SecurityServiceClient.GetIdentityResources(cancellationToken);

            if (identityResourceList == null || identityResourceList.Any() == false) {
                createIdentityResourceResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                           .CreateIdentityResource(createIdentityResourceRequest, cancellationToken).ConfigureAwait(false);
                createIdentityResourceResponse.ShouldNotBeNull();
                createIdentityResourceResponse.IdentityResourceName.ShouldNotBeNullOrEmpty();

                this.TestingContext.IdentityResources.Add(createIdentityResourceResponse.IdentityResourceName);
            }
            else {
                if (identityResourceList.Where(i => i.Name == createIdentityResourceRequest.Name).Any()) {
                    return;
                }

                createIdentityResourceResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                           .CreateIdentityResource(createIdentityResourceRequest, cancellationToken).ConfigureAwait(false);
                createIdentityResourceResponse.ShouldNotBeNull();
                createIdentityResourceResponse.IdentityResourceName.ShouldNotBeNullOrEmpty();

                this.TestingContext.IdentityResources.Add(createIdentityResourceResponse.IdentityResourceName);
            }
        }

        [Given(@"the following clients exist")]
        public async Task GivenTheFollowingClientsExist(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");
                String clientName = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String allowedScopes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedScopes");
                String allowedGrantTypes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedGrantTypes");

                List<String> splitAllowedScopes = allowedScopes.Split(",").ToList();
                List<String> splitAllowedGrantTypes = allowedGrantTypes.Split(",").ToList();

                CreateClientRequest createClientRequest = new CreateClientRequest {
                                                                                      Secret = secret,
                                                                                      AllowedGrantTypes = new List<String>(),
                                                                                      AllowedScopes = new List<String>(),
                                                                                      ClientDescription = String.Empty,
                                                                                      ClientId = clientId,
                                                                                      ClientName = clientName
                                                                                  };

                splitAllowedScopes.ForEach(a => { createClientRequest.AllowedScopes.Add(a.Trim()); });
                splitAllowedGrantTypes.ForEach(a => { createClientRequest.AllowedGrantTypes.Add(a.Trim()); });

                CreateClientResponse createClientResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                                      .CreateClient(createClientRequest, CancellationToken.None).ConfigureAwait(false);

                createClientResponse.ClientId.ShouldBe(clientId);

                this.TestingContext.AddClientDetails(clientId, secret, allowedGrantTypes);
            }
        }

        [Given(@"I have a token to access the estate management resource")]
        [Given(@"I have a token to access the estate management and transaction processor resources")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementResource(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");

                ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

                if (clientDetails.GrantType == "client_credentials") {
                    TokenResponse tokenResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                            .GetToken(clientId, clientDetails.ClientSecret, CancellationToken.None).ConfigureAwait(false);

                    this.TestingContext.AccessToken = tokenResponse.AccessToken;
                }
            }

        }

        [Given(@"I am logged in as ""(.*)"" with password ""(.*)"" for Estate ""(.*)"" with client ""(.*)""")]
        public async Task GivenIAmLoggedInAsWithPasswordForEstate(String username,
                                                                  String password,
                                                                  String estateName,
                                                                  String clientId) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            TokenResponse tokenResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                    .GetToken(username, password, clientId, clientDetails.ClientSecret, CancellationToken.None).ConfigureAwait(false);

            estateDetails.SetEstateUserToken(tokenResponse.AccessToken);
        }


        [When(@"I get the estate ""(.*)"" the estate details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateDetailsAreReturnedAsFollows(String estateName,
                                                                                Table table) {
            EstateResponse estate = await this.GetEstate(estateName);

            foreach (TableRow tableRow in table.Rows) {
                String estateNameFromRow = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                estate.EstateName.ShouldBe(estateNameFromRow);
            }
        }

        [When(@"I get the estate ""(.*)"" the estate operator details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateOperatorDetailsAreReturnedAsFollows(String estateName,
                                                                                        Table table) {
            EstateResponse estate = await this.GetEstate(estateName);

            foreach (TableRow tableRow in table.Rows) {
                String operatorNameFromRow = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                EstateOperatorResponse operatorResponse = estate.Operators.SingleOrDefault(o => o.Name == operatorNameFromRow);
                operatorResponse.ShouldNotBeNull();
            }
        }

        [When(@"I get the estate ""(.*)"" the estate security user details are returned as follows")]
        public async Task WhenIGetTheEstateTheEstateSecurityUserDetailsAreReturnedAsFollows(String estateName,
                                                                                            Table table) {
            //EstateResponse estate = await this.GetEstate(estateName);

            //foreach (TableRow tableRow in table.Rows)
            //{
            //    String emailAddressFromRow = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress");
            //    SecurityUserResponse securityUserResponse = estate.SecurityUsers.SingleOrDefault(o => o.EmailAddress == emailAddressFromRow);
            //    securityUserResponse.ShouldNotBeNull();
            //}
        }

        private async Task<EstateResponse> GetEstate(String estateName) {
            Guid estateId = Guid.NewGuid();
            String token = this.TestingContext.AccessToken;
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            if (estateDetails != null) {
                estateId = estateDetails.EstateId;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }
            }

            EstateResponse estate = await this.TestingContext.DockerHelper.EstateClient.GetEstate(token, estateId, CancellationToken.None).ConfigureAwait(false);
            return estate;
        }


        [Given(@"I have assigned the following devices to the merchants")]
        [When(@"I add the following devices to the merchant")]
        public async Task WhenIAddTheFollowingDevicesToTheMerchant(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                String deviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "DeviceIdentifier");

                AddMerchantDeviceRequest addMerchantDeviceRequest = new AddMerchantDeviceRequest {
                                                                                                     DeviceIdentifier = deviceIdentifier
                                                                                                 };

                AddMerchantDeviceResponse addMerchantDeviceResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                                .AddDeviceToMerchant(token,
                                                                                                     estateDetails.EstateId,
                                                                                                     merchantId,
                                                                                                     addMerchantDeviceRequest,
                                                                                                     CancellationToken.None).ConfigureAwait(false);

                addMerchantDeviceResponse.EstateId.ShouldBe(estateDetails.EstateId);
                addMerchantDeviceResponse.MerchantId.ShouldBe(merchantId);
                addMerchantDeviceResponse.DeviceId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Logger.LogInformation($"Device {deviceIdentifier} assigned to Merchant {merchantName}");

                await Retry.For(async () => {
                                    var merchantResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                     .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                                     .ConfigureAwait(false);

                                    merchantResponse.Devices.ContainsValue(addMerchantDeviceRequest.DeviceIdentifier);
                                });
            }
        }

        [When(@"I swap the merchant device the device is swapped")]
        public async Task WhenISwapTheMerchantDeviceTheDeviceIsSwapped(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                String originalDeviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OriginalDeviceIdentifier");
                String newDeviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "NewDeviceIdentifier");

                SwapMerchantDeviceRequest swapMerchantDeviceRequest = new SwapMerchantDeviceRequest {
                                                                                                        OriginalDeviceIdentifier = originalDeviceIdentifier,
                                                                                                        NewDeviceIdentifier = newDeviceIdentifier
                                                                                                    };

                SwapMerchantDeviceResponse swapMerchantDeviceResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                                  .SwapDeviceForMerchant(token,
                                                                                                         estateDetails.EstateId,
                                                                                                         merchantId,
                                                                                                         swapMerchantDeviceRequest,
                                                                                                         CancellationToken.None).ConfigureAwait(false);

                swapMerchantDeviceResponse.EstateId.ShouldBe(estateDetails.EstateId);
                swapMerchantDeviceResponse.MerchantId.ShouldBe(merchantId);
                swapMerchantDeviceResponse.DeviceId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Logger.LogInformation($"Device {newDeviceIdentifier} assigned to Merchant {merchantName}");

                await Retry.For(async () => {
                                    var merchantResponse = await this.TestingContext.DockerHelper.EstateClient
                                                                     .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                                     .ConfigureAwait(false);

                                    merchantResponse.Devices.ContainsValue(swapMerchantDeviceRequest.NewDeviceIdentifier);
                                });
            }
        }


        [When(@"I get the merchants for '(.*)' then (.*) merchants will be returned")]
        public async Task WhenIGetTheMerchantsForThenMerchantsWillBeReturned(String estateName,
                                                                             Int32 expectedMerchantCount) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            String token = this.TestingContext.AccessToken;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                token = estateDetails.AccessToken;
            }

            await Retry.For(async () => {
                                List<MerchantResponse> merchantList = await this.TestingContext.DockerHelper.EstateClient
                                                                                .GetMerchants(token, estateDetails.EstateId, CancellationToken.None)
                                                                                .ConfigureAwait(false);

                                merchantList.ShouldNotBeNull();
                                merchantList.ShouldNotBeEmpty();
                                merchantList.Count.ShouldBe(expectedMerchantCount);
                            });

        }

        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                CreateContractRequest createContractRequest = new CreateContractRequest {
                                                                                            OperatorId = operatorId,
                                                                                            Description =
                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription")
                                                                                        };

                CreateContractResponse contractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.CreateContract(token, estateDetails.EstateId, createContractRequest, CancellationToken.None);

                estateDetails.AddContract(contractResponse.ContractId, createContractRequest.Description, operatorId);
            }
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                Contract contract = estateDetails.GetContract(contractName);
                String productValue = SpecflowTableHelper.GetStringRowValue(tableRow, "Value");

                AddProductToContractRequest addProductToContractRequest = new AddProductToContractRequest {
                                                                                                              ProductName =
                                                                                                                  SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "ProductName"),
                                                                                                              DisplayText =
                                                                                                                  SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "DisplayText"),
                                                                                                              ProductType = SpecflowTableHelper.GetEnumValue<ProductType>(tableRow,
                                                                                                                   "ProductType"),
                                                                                                              Value = null
                                                                                                          };
                if (String.IsNullOrEmpty(productValue) == false) {
                    addProductToContractRequest.Value = Decimal.Parse(productValue);
                }

                AddProductToContractResponse addProductToContractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.AddProductToContract(token,
                                                                                             estateDetails.EstateId,
                                                                                             contract.ContractId,
                                                                                             addProductToContractRequest,
                                                                                             CancellationToken.None);

                contract.AddProduct(addProductToContractResponse.ProductId,
                                    addProductToContractRequest.ProductName,
                                    addProductToContractRequest.DisplayText,
                                    addProductToContractRequest.Value);
            }
        }

        [When(@"I add the following Transaction Fees")]
        public async Task WhenIAddTheFollowingTransactionFees(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
                Contract contract = estateDetails.GetContract(contractName);

                Product product = contract.GetProduct(productName);

                AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest = new AddTransactionFeeForProductToContractRequest {
                                                                                                                Value =
                                                                                                                    SpecflowTableHelper.GetDecimalValue(tableRow,
                                                                                                                        "Value"),
                                                                                                                Description =
                                                                                                                    SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                        "FeeDescription"),
                                                                                                                CalculationType =
                                                                                                                    SpecflowTableHelper
                                                                                                                        .GetEnumValue<CalculationType>(tableRow,
                                                                                                                            "CalculationType"),
                                                                                                                FeeType =
                                                                                                                    SpecflowTableHelper
                                                                                                                        .GetEnumValue<FeeType>(tableRow, "FeeType")

                                                                                                            };

                AddTransactionFeeForProductToContractResponse addTransactionFeeForProductToContractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.AddTransactionFeeForProductToContract(token,
                                                                                                              estateDetails.EstateId,
                                                                                                              contract.ContractId,
                                                                                                              product.ProductId,
                                                                                                              addTransactionFeeForProductToContractRequest,
                                                                                                              CancellationToken.None);

                product.AddTransactionFee(addTransactionFeeForProductToContractResponse.TransactionFeeId,
                                          addTransactionFeeForProductToContractRequest.CalculationType,
                                          addTransactionFeeForProductToContractRequest.FeeType,
                                          addTransactionFeeForProductToContractRequest.Description,
                                          addTransactionFeeForProductToContractRequest.Value);
            }
        }

        [Then(@"I get the Contracts for '(.*)' the following contract details are returned")]
        public async Task ThenIGetTheContractsForTheFollowingContractDetailsAreReturned(String estateName,
                                                                                        Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            String token = this.TestingContext.AccessToken;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                token = estateDetails.AccessToken;
            }

            await Retry.For(async () => {
                                List<ContractResponse> contracts =
                                    await this.TestingContext.DockerHelper.EstateClient.GetContracts(token, estateDetails.EstateId, CancellationToken.None);

                                contracts.ShouldNotBeNull();
                                contracts.ShouldHaveSingleItem();
                                ContractResponse contract = contracts.Single();
                                contract.Products.ShouldNotBeNull();
                                foreach (TableRow tableRow in table.Rows) {
                                    String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                                    String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");

                                    contract.Description.ShouldBe(contractDescription);
                                    contract.Products.Any(p => p.Name == productName).ShouldBeTrue();
                                }
                            });
        }


        [Then(@"I get the Merchant Contracts for '(.*)' for '(.*)' the following contract details are returned")]
        public async Task ThenIGetTheMerchantContractsForForTheFollowingContractDetailsAreReturned(String merchantName,
                                                                                                   String estateName,
                                                                                                   Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

            String token = this.TestingContext.AccessToken;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                token = estateDetails.AccessToken;
            }

            List<ContractResponse> contracts =
                await this.TestingContext.DockerHelper.EstateClient.GetMerchantContracts(token, estateDetails.EstateId, merchantId, CancellationToken.None);

            contracts.ShouldNotBeNull();
            contracts.ShouldHaveSingleItem();
            ContractResponse contract = contracts.Single();

            foreach (TableRow tableRow in table.Rows) {
                String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");

                contract.Description.ShouldBe(contractDescription);
                contract.Products.Any(p => p.Name == productName).ShouldBeTrue();

            }
        }


        [Then(@"I get the Transaction Fees for '(.*)' on the '(.*)' contract for '(.*)' the following fees are returned")]
        public async Task ThenIGetTheTransactionFeesForOnTheContractForTheFollowingFeesAreReturned(String productName,
                                                                                                   String contractName,
                                                                                                   String estateName,
                                                                                                   Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            String token = this.TestingContext.AccessToken;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                token = estateDetails.AccessToken;
            }

            Contract contract = estateDetails.GetContract(contractName);

            Product product = contract.GetProduct(productName);

            await Retry.For(async () => {


                                List<ContractProductTransactionFee> transactionFees =
                                    await this.TestingContext.DockerHelper.EstateClient.GetTransactionFeesForProduct(token,
                                        estateDetails.EstateId,
                                        Guid.Empty,
                                        contract.ContractId,
                                        product.ProductId,
                                        CancellationToken.None);
                                foreach (TableRow tableRow in table.Rows) {
                                    CalculationType calculationType = SpecflowTableHelper.GetEnumValue<CalculationType>(tableRow, "CalculationType");
                                    FeeType feeType = SpecflowTableHelper.GetEnumValue<FeeType>(tableRow, "FeeType");
                                    String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
                                    Decimal feeValue = SpecflowTableHelper.GetDecimalValue(tableRow, "Value");

                                    Boolean feeFound = transactionFees.Any(f => f.CalculationType == calculationType && f.Description == feeDescription &&
                                                                                f.Value == feeValue && f.FeeType == feeType);

                                    feeFound.ShouldBeTrue();
                                }
                            });

        }

        [When(@"I get the estate ""(.*)"" an error is returned")]
        public void WhenIGetTheEstateAnErrorIsReturned(String estateName) {
            Exception exception = Should.Throw<Exception>(async () => { await this.GetEstate(estateName).ConfigureAwait(false); });
            exception.InnerException.ShouldBeOfType<KeyNotFoundException>();
        }

        [When(@"I get the merchant ""(.*)"" for estate ""(.*)"" an error is returned")]
        public void WhenIGetTheMerchantForEstateAnErrorIsReturned(String merchantName,
                                                                  String estateName) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            Guid merchantId = Guid.NewGuid();

            String token = this.TestingContext.AccessToken;
            if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                token = estateDetails.AccessToken;
            }

            Exception exception = Should.Throw<Exception>(async () => {
                                                              await this.TestingContext.DockerHelper.EstateClient
                                                                        .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                                        .ConfigureAwait(false);
                                                          });
            exception.InnerException.ShouldBeOfType<KeyNotFoundException>();
        }

        [When(@"I set the merchants settlement schedule")]
        public void WhenISetTheMerchantsSettlementSchedule(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                SettlementSchedule schedule = Enum.Parse<SettlementSchedule>(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementSchedule"));

                SetSettlementScheduleRequest setSettlementScheduleRequest = new SetSettlementScheduleRequest {
                                                                                                                 SettlementSchedule = schedule
                                                                                                             };

                Should.NotThrow(async () => {
                                    await this.TestingContext.DockerHelper.EstateClient.SetMerchantSettlementSchedule(token,
                                        estateDetails.EstateId,
                                        merchantId,
                                        setSettlementScheduleRequest,
                                        CancellationToken.None);
                                });

            }
        }

        [When(@"I make the following automatic merchant deposits")]
        public async Task WhenIMakeTheFollowingAutomaticMerchantDeposits(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                Decimal amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount");
                DateTime depositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.UtcNow);
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
                var merchant = estateDetails.GetMerchant(merchantName);

                var depositReference = $"{estateDetails.EstateReference}-{merchant.MerchantReference}";

                // This will send a request to the Test Host (test bank)
                var makeDepositRequest = new {
                                                 date_time = depositDateTime,
                                                 from_sort_code = "665544",
                                                 from_account_number = "12312312",
                                                 to_sort_code = DockerHelper.TestBankSortCode,
                                                 to_account_number = DockerHelper.TestBankAccountNumber,
                                                 deposit_reference = depositReference,
                                                 amount = amount
                                             };
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/testbank");
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(makeDepositRequest), Encoding.UTF8, "application/json");
                var responseMessage = await this.TestingContext.DockerHelper.TestHostClient.SendAsync(requestMessage);

                responseMessage.IsSuccessStatusCode.ShouldBeTrue();
            }
        }

        [When(@"I make the following manual merchant deposits the deposit is rejected")]
        [When(@"I make the following automatic merchant deposits the deposit is rejected")]
        public async Task WhenIMakeTheFollowingMerchantDepositsTheDepositIsRejected(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false) {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

                MakeMerchantDepositRequest makeMerchantDepositRequest = new MakeMerchantDepositRequest {
                                                                                                           DepositDateTime =
                                                                                                               SpecflowTableHelper
                                                                                                                   .GetDateTimeForDateString(SpecflowTableHelper
                                                                                                                           .GetStringRowValue(tableRow, "DateTime"),
                                                                                                                       DateTime.Now),
                                                                                                           Reference =
                                                                                                               SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                   "Reference"),
                                                                                                           Amount =
                                                                                                               SpecflowTableHelper.GetDecimalValue(tableRow, "Amount")
                                                                                                       };

                var exception = Should.Throw<Exception>(async () => {
                                                            await this.TestingContext.DockerHelper.EstateClient
                                                                      .MakeMerchantDeposit(token,
                                                                                           estateDetails.EstateId,
                                                                                           merchantId,
                                                                                           makeMerchantDepositRequest,
                                                                                           CancellationToken.None).ConfigureAwait(false);
                                                        });
                exception.InnerException.GetType().ShouldBe(typeof(InvalidOperationException));
            }
        }

        [When(@"I perform the following transactions")]
        public async Task WhenIPerformTheFollowingTransactions(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                String dateString = SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime");
                DateTime transactionDateTime = SpecflowTableHelper.GetDateForDateString(dateString, this.TestingContext.DateToUseForToday);
                // hack for UTC :|
                transactionDateTime = transactionDateTime.AddHours(1);
                String transactionNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionNumber");
                String transactionType = SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionType");
                String deviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "DeviceIdentifier");

                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                // Lookup the merchant id
                Guid merchantId = estateDetails.GetMerchantId(merchantName);
                SerialisedMessage transactionResponse = null;
                switch(transactionType) {
                    case "Logon":
                        transactionResponse = await this.PerformLogonTransaction(estateDetails.EstateId,
                                                                                 merchantId,
                                                                                 transactionDateTime,
                                                                                 transactionType,
                                                                                 transactionNumber,
                                                                                 deviceIdentifier,
                                                                                 CancellationToken.None);
                        break;
                    case "Sale":

                        // Get specific sale fields
                        String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                        Decimal transactionAmount = SpecflowTableHelper.GetDecimalValue(tableRow, "TransactionAmount");
                        String customerAccountNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "CustomerAccountNumber");
                        String customerEmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "CustomerEmailAddress");
                        String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                        String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
                        String recipientMobile = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientMobile");

                        Guid contractId = Guid.Empty;
                        Guid productId = Guid.Empty;
                        Contract contract = estateDetails.GetContract(contractDescription);
                        if (contract != null) {
                            contractId = contract.ContractId;
                            Product product = contract.GetProduct(productName);
                            productId = product.ProductId;
                        }

                        transactionResponse = await this.PerformSaleTransaction(estateDetails.EstateId,
                                                                                merchantId,
                                                                                transactionDateTime,
                                                                                transactionType,
                                                                                transactionNumber,
                                                                                deviceIdentifier,
                                                                                operatorName,
                                                                                transactionAmount,
                                                                                customerAccountNumber,
                                                                                customerEmailAddress,
                                                                                contractId,
                                                                                productId,
                                                                                recipientMobile,
                                                                                CancellationToken.None);
                        break;
                }

                estateDetails.AddTransactionResponse(merchantId, transactionNumber, transactionResponse);
            }
        }

        [Then(@"transaction response should contain the following information")]
        public void ThenTransactionResponseShouldContainTheFollowingInformation(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                // Get the merchant name
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                String transactionNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionNumber");
                SerialisedMessage serialisedMessage = estateDetails.GetTransactionResponse(merchantId, transactionNumber);
                Object transactionResponse = JsonConvert.DeserializeObject(serialisedMessage.SerialisedData,
                                                                           new JsonSerializerSettings {
                                                                                                          TypeNameHandling = TypeNameHandling.All
                                                                                                      });
                this.ValidateTransactionResponse(transactionNumber, (dynamic)transactionResponse, tableRow);
            }
        }

        private void ValidateTransactionResponse(String transactionNumber,
                                                 LogonTransactionResponse logonTransactionResponse,
                                                 TableRow tableRow) {
            String expectedResponseCode = SpecflowTableHelper.GetStringRowValue(tableRow, "ResponseCode");
            String expectedResponseMessage = SpecflowTableHelper.GetStringRowValue(tableRow, "ResponseMessage");

            logonTransactionResponse.ResponseCode.ShouldBe(expectedResponseCode, $"Transaction Number [{transactionNumber}]");
            logonTransactionResponse.ResponseMessage.ShouldBe(expectedResponseMessage, $"Transaction Number [{transactionNumber}]");
        }

        private void ValidateTransactionResponse(String transactionNumber,
                                                 SaleTransactionResponse saleTransactionResponse,
                                                 TableRow tableRow) {
            String expectedResponseCode = SpecflowTableHelper.GetStringRowValue(tableRow, "ResponseCode");
            String expectedResponseMessage = SpecflowTableHelper.GetStringRowValue(tableRow, "ResponseMessage");

            saleTransactionResponse.ResponseCode.ShouldBe(expectedResponseCode, $"Transaction Number [{transactionNumber}]");
            saleTransactionResponse.ResponseMessage.ShouldBe(expectedResponseMessage, $"Transaction Number [{transactionNumber}]");
        }

        [When(@"I get the pending settlements the following information should be returned")]
        public async Task WhenIGetThePendingSettlementsTheFollowingInformationShouldBeReturned(Table table) {
            foreach (TableRow tableRow in table.Rows) {
                // Get the merchant name
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);
                Guid merchantId = estateDetails.GetMerchantId(tableRow["MerchantName"]);
                String settlementDateString = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate");
                Int32 numberOfFees = SpecflowTableHelper.GetIntValue(tableRow, "NumberOfFees");
                DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.UtcNow.Date);

                await Retry.For(async () => {
                                    SettlementResponse settlements =
                                        await this.TestingContext.DockerHelper.TransactionProcessorClient.GetSettlementByDate(this.TestingContext.AccessToken,
                                            settlementDate,
                                            estateDetails.EstateId,
                                            merchantId,
                                            CancellationToken.None);

                                    settlements.NumberOfFeesPendingSettlement.ShouldBe(numberOfFees, $"Settlement date {settlementDate}");
                                },
                                TimeSpan.FromMinutes(3));
            }
        }

        [When(@"I process the settlement for '([^']*)' on Estate '([^']*)' for Merchant '([^']*)' then (.*) fees are marked as settled and the settlement is completed")]
        public async Task WhenIProcessTheSettlementForOnEstateForMerchantThenFeesAreMarkedAsSettledAndTheSettlementIsCompleted(String dateString,
                                                                                                                               String estateName,
                                                                                                                               String merchantName,
                                                                                                                               Int32 numberOfFeesSettled) {
            DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(dateString, DateTime.UtcNow.Date);

            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            Guid merchantId = estateDetails.GetMerchantId(merchantName);
            await this.TestingContext.DockerHelper.TransactionProcessorClient.ProcessSettlement(this.TestingContext.AccessToken,
                                                                                                settlementDate,
                                                                                                estateDetails.EstateId,
                                                                                                merchantId,
                                                                                                CancellationToken.None);

            await Retry.For(async () => {
                                SettlementResponse settlement =
                                    await this.TestingContext.DockerHelper.TransactionProcessorClient.GetSettlementByDate(this.TestingContext.AccessToken,
                                        settlementDate,
                                        estateDetails.EstateId,
                                        merchantId,
                                        CancellationToken.None);

                                settlement.NumberOfFeesPendingSettlement.ShouldBe(0);
                                settlement.NumberOfFeesSettled.ShouldBe(numberOfFeesSettled);
                                settlement.SettlementCompleted.ShouldBeTrue();
                            },
                            TimeSpan.FromMinutes(2));
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' with the Start Date '([^']*)' and the End Date '([^']*)' the following data is returned")]
        public async Task WhenIGetTheEstateSettlementReportForEstateWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(string estateName,
            string startDateString,
            string endDateString,
            Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            DateTime stateDate = SpecflowTableHelper.GetDateForDateString(startDateString, DateTime.UtcNow.Date);
            DateTime endDate = SpecflowTableHelper.GetDateForDateString(endDateString, DateTime.UtcNow.Date);

            foreach (TableRow tableRow in table.Rows) {
                DateTime settlementDate =
                    SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
                Int32 numberOfFeesSettled = SpecflowTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
                Decimal valueOfFeesSettled = SpecflowTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
                Boolean isCompleted = SpecflowTableHelper.GetBooleanValue(tableRow, "IsCompleted");

                await Retry.For(async () => {

                                    List<DataTransferObjects.Responses.SettlementResponse> settlementList =
                                        await this.TestingContext.DockerHelper.EstateClient.GetSettlements(this.TestingContext.AccessToken,
                                                                                                           estateDetails.EstateId,
                                                                                                           null,
                                                                                                           stateDate.ToString("yyyyMMdd"),
                                                                                                           endDate.ToString("yyyyMMdd"),
                                                                                                           CancellationToken.None);

                                    settlementList.ShouldNotBeNull();
                                    settlementList.ShouldNotBeEmpty();

                                    DataTransferObjects.Responses.SettlementResponse settlement =
                                        settlementList.SingleOrDefault(s => s.SettlementDate == settlementDate && s.NumberOfFeesSettled == numberOfFeesSettled &&
                                                                            s.ValueOfFeesSettled == valueOfFeesSettled && s.IsCompleted == isCompleted);

                                    settlement.ShouldNotBeNull();
                                },
                                TimeSpan.FromMinutes(2));
            }
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' with the Date '([^']*)' the following fees are settled")]
        public void WhenIGetTheEstateSettlementReportForEstateWithTheDateTheFollowingFeesAreSettled(string p0,
                                                                                                    string p1) {
            throw new PendingStepException();
        }


        [When(@"I get the Estate Settlement Report for Estate '([^']*)' with the Date '([^']*)' the following fees are settled")]
        public async Task WhenIGetTheEstateSettlementReportForEstateWithTheDateTheFollowingFeesAreSettled(string estateName,
                                                                                                          string settlementDateString,
                                                                                                          Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.UtcNow.Date);

            foreach (TableRow tableRow in table.Rows) {
                Guid settlementId = Helpers.CalculateSettlementAggregateId(settlementDate, estateDetails.EstateId);
                DataTransferObjects.Responses.SettlementResponse settlement =
                    await this.TestingContext.DockerHelper.EstateClient.GetSettlement(this.TestingContext.AccessToken,
                                                                                               estateDetails.EstateId,
                                                                                               null,
                                                                                               settlementId,
                                                                                               CancellationToken.None);

                settlement.ShouldNotBeNull();

                settlement.SettlementFees.ShouldNotBeNull();
                settlement.SettlementFees.ShouldNotBeEmpty();

                String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
                Boolean isSettled = SpecflowTableHelper.GetBooleanValue(tableRow, "IsSettled");
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "Operator");
                Decimal calculatedValue = SpecflowTableHelper.GetDecimalValue(tableRow, "CalculatedValue");

                SettlementFeeResponse settlementFee = settlement.SettlementFees.SingleOrDefault(sf => sf.FeeDescription == feeDescription && sf.IsSettled == isSettled &&
                                                                                                      sf.MerchantName == merchantName &&
                                                                                                      sf.OperatorIdentifier == operatorName &&
                                                                                                      sf.CalculatedValue == calculatedValue);

                settlementFee.ShouldNotBeNull();
            }
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' for Merchant '([^']*)' with the Start Date '([^']*)' and the End Date '([^']*)' the following data is returned")]
        public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheStartDateAndTheEndDateTheFollowingDataIsReturned(string estateName,
            string merchantName,
            string startDateString,
            string endDateString,
            Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            Guid merchantId = estateDetails.GetMerchantId(merchantName);
            DateTime stateDate = SpecflowTableHelper.GetDateForDateString(startDateString, DateTime.UtcNow.Date);
            DateTime endDate = SpecflowTableHelper.GetDateForDateString(endDateString, DateTime.UtcNow.Date);

            foreach (TableRow tableRow in table.Rows) {
                DateTime settlementDate =
                    SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
                Int32 numberOfFeesSettled = SpecflowTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
                Decimal valueOfFeesSettled = SpecflowTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
                Boolean isCompleted = SpecflowTableHelper.GetBooleanValue(tableRow, "IsCompleted");

                await Retry.For(async () => {
                                    List<DataTransferObjects.Responses.SettlementResponse> settlementList =
                                        await this.TestingContext.DockerHelper.EstateClient.GetSettlements(this.TestingContext.AccessToken,
                                            estateDetails.EstateId,
                                            merchantId,
                                            stateDate.ToString("yyyyMMdd"),
                                            endDate.ToString("yyyyMMdd"),
                                            CancellationToken.None);

                                    settlementList.ShouldNotBeNull();
                                    settlementList.ShouldNotBeEmpty();

                                    DataTransferObjects.Responses.SettlementResponse settlement =
                                        settlementList.SingleOrDefault(s => s.SettlementDate == settlementDate && s.NumberOfFeesSettled == numberOfFeesSettled &&
                                                                            s.ValueOfFeesSettled == valueOfFeesSettled && s.IsCompleted == isCompleted);

                                    settlement.ShouldNotBeNull();
                                },
                                TimeSpan.FromMinutes(2));
            }
        }

        [When(@"I get the Estate Settlement Report for Estate '([^']*)' for Merchant '([^']*)' with the Date '([^']*)' the following fees are settled")]
        public async Task WhenIGetTheEstateSettlementReportForEstateForMerchantWithTheDateTheFollowingFeesAreSettled(string estateName,
            string merchantName,
            string settlementDateString,
            Table table) {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);
            Guid merchantId = estateDetails.GetMerchantId(merchantName);
            DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.UtcNow.Date);

            foreach (TableRow tableRow in table.Rows) {
                await Retry.For(async () => {
                                    Guid settlementId = Helpers.CalculateSettlementAggregateId(settlementDate, estateDetails.EstateId);
                                    DataTransferObjects.Responses.SettlementResponse settlement =
                                        await this.TestingContext.DockerHelper.EstateClient.GetSettlement(this.TestingContext.AccessToken,
                                            estateDetails.EstateId,
                                            merchantId,
                                            settlementId,
                                            CancellationToken.None);

                                    settlement.ShouldNotBeNull();

                                    settlement.SettlementFees.ShouldNotBeNull();
                                    settlement.SettlementFees.ShouldNotBeEmpty();

                                    String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
                                    Boolean isSettled = SpecflowTableHelper.GetBooleanValue(tableRow, "IsSettled");
                                    String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "Operator");
                                    Decimal calculatedValue = SpecflowTableHelper.GetDecimalValue(tableRow, "CalculatedValue");

                                    SettlementFeeResponse settlementFee =
                                        settlement.SettlementFees.SingleOrDefault(sf => sf.FeeDescription == feeDescription && sf.IsSettled == isSettled &&
                                                                                        sf.MerchantName == merchantName && sf.OperatorIdentifier == operatorName &&
                                                                                        sf.CalculatedValue == calculatedValue);

                                    settlementFee.ShouldNotBeNull();
                                },
                                TimeSpan.FromMinutes(3));
            }
        }



        private DateTime GetSettlementDate(DateTime now,
                                           String nextSettlementDate) {
            if (nextSettlementDate == "Yesterday") {
                return now.AddDays(-1).Date;
            }

            if (nextSettlementDate == "NextWeek") {
                return now.AddDays(6).Date;
            }

            if (nextSettlementDate == "NextMonth") {
                return now.AddMonths(1).Date.AddDays(-1).Date;
            }

            return now.Date;
        }

        private async Task<SerialisedMessage> PerformLogonTransaction(Guid estateId,
                                                                      Guid merchantId,
                                                                      DateTime transactionDateTime,
                                                                      String transactionType,
                                                                      String transactionNumber,
                                                                      String deviceIdentifier,
                                                                      CancellationToken cancellationToken) {
            LogonTransactionRequest logonTransactionRequest = new LogonTransactionRequest {
                                                                                              MerchantId = merchantId,
                                                                                              EstateId = estateId,
                                                                                              TransactionDateTime = transactionDateTime,
                                                                                              TransactionNumber = transactionNumber,
                                                                                              DeviceIdentifier = deviceIdentifier,
                                                                                              TransactionType = transactionType
                                                                                          };

            SerialisedMessage serialisedMessage = new SerialisedMessage();
            serialisedMessage.Metadata.Add(MetadataContants.KeyNameEstateId, estateId.ToString());
            serialisedMessage.Metadata.Add(MetadataContants.KeyNameMerchantId, merchantId.ToString());
            serialisedMessage.SerialisedData = JsonConvert.SerializeObject(logonTransactionRequest,
                                                                           new JsonSerializerSettings {
                                                                                                          TypeNameHandling = TypeNameHandling.All
                                                                                                      });

            SerialisedMessage responseSerialisedMessage =
                await this.TestingContext.DockerHelper.TransactionProcessorClient.PerformTransaction(this.TestingContext.AccessToken,
                                                                                                     serialisedMessage,
                                                                                                     cancellationToken);

            return responseSerialisedMessage;
        }

        private async Task<SerialisedMessage> PerformSaleTransaction(Guid estateId,
                                                                     Guid merchantId,
                                                                     DateTime transactionDateTime,
                                                                     String transactionType,
                                                                     String transactionNumber,
                                                                     String deviceIdentifier,
                                                                     String operatorIdentifier,
                                                                     Decimal transactionAmount,
                                                                     String customerAccountNumber,
                                                                     String customerEmailAddress,
                                                                     Guid contractId,
                                                                     Guid productId,
                                                                     String recipientMobile,
                                                                     CancellationToken cancellationToken) {
            Dictionary<String, String> additionalTransactionMetadata = new Dictionary<String, String>();

            if (operatorIdentifier == "Voucher") {
                additionalTransactionMetadata = new Dictionary<String, String> {
                                                                                   {"Amount", transactionAmount.ToString()},
                                                                                   {"RecipientMobile", recipientMobile}
                                                                               };
            }
            else {
                additionalTransactionMetadata = new Dictionary<String, String> {
                                                                                   {"Amount", transactionAmount.ToString()},
                                                                                   {"CustomerAccountNumber", customerAccountNumber}
                                                                               };
            }

            SaleTransactionRequest saleTransactionRequest = new SaleTransactionRequest {
                                                                                           MerchantId = merchantId,
                                                                                           EstateId = estateId,
                                                                                           TransactionDateTime = transactionDateTime,
                                                                                           TransactionNumber = transactionNumber,
                                                                                           DeviceIdentifier = deviceIdentifier,
                                                                                           TransactionType = transactionType,
                                                                                           OperatorIdentifier = operatorIdentifier,
                                                                                           AdditionalTransactionMetadata = additionalTransactionMetadata,
                                                                                           CustomerEmailAddress = customerEmailAddress,
                                                                                           ProductId = productId,
                                                                                           ContractId = contractId
                                                                                       };

            SerialisedMessage serialisedMessage = new SerialisedMessage();
            serialisedMessage.Metadata.Add(MetadataContants.KeyNameEstateId, estateId.ToString());
            serialisedMessage.Metadata.Add(MetadataContants.KeyNameMerchantId, merchantId.ToString());
            serialisedMessage.SerialisedData = JsonConvert.SerializeObject(saleTransactionRequest,
                                                                           new JsonSerializerSettings {
                                                                                                          TypeNameHandling = TypeNameHandling.All
                                                                                                      });

            SerialisedMessage responseSerialisedMessage =
                await this.TestingContext.DockerHelper.TransactionProcessorClient.PerformTransaction(this.TestingContext.AccessToken,
                                                                                                     serialisedMessage,
                                                                                                     cancellationToken);

            return responseSerialisedMessage;
        }
    }

    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Converts to datetime.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Guid guid) {
            var bytes = guid.ToByteArray();

            Array.Resize(ref bytes, 8);

            return new DateTime(BitConverter.ToInt64(bytes));
        }

        /// <summary>
        /// Converts to guid.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static Guid ToGuid(this DateTime dt) {
            var bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }

        #endregion
    }

    public static class Helpers
    {
        public static Guid CalculateSettlementAggregateId(DateTime settlementDate,
                                                          Guid estateId) {
            Guid aggregateId = GuidCalculator.Combine(estateId, settlementDate.ToGuid());
            return aggregateId;
        }
    }

    public static class GuidCalculator
    {
        #region Methods

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Byte offset) {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++) {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified GUIDs into a new GUID.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid) {
            return GuidCalculator.Combine(firstGuid, secondGuid, 0);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid,
                                   Byte offset) {
            Byte[] firstAsBytes = firstGuid.ToByteArray();
            Byte[] secondAsBytes = secondGuid.ToByteArray();
            Byte[] thirdAsBytes = thirdGuid.ToByteArray();

            Byte[] newBytes = new Byte[16];

            for (Int32 i = 0; i < 16; i++) {
                // Add and truncate any overflow
                newBytes[i] = (Byte)(firstAsBytes[i] + secondAsBytes[i] + thirdAsBytes[i] + offset);
            }

            return new Guid(newBytes);
        }

        /// <summary>
        /// Combines the specified first unique identifier.
        /// </summary>
        /// <param name="firstGuid">The first unique identifier.</param>
        /// <param name="secondGuid">The second unique identifier.</param>
        /// <param name="thirdGuid">The third unique identifier.</param>
        /// <returns>Guid.</returns>
        public static Guid Combine(Guid firstGuid,
                                   Guid secondGuid,
                                   Guid thirdGuid) {
            return GuidCalculator.Combine(firstGuid, secondGuid, thirdGuid, 0);
        }

        #endregion
    }
}

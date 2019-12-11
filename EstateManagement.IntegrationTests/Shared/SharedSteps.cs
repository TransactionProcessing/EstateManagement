using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Shared
{
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using EstateManagement.DataTransferObjects.Requests;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public SharedSteps(ScenarioContext scenarioContext,
                         TestingContext testingContext)
        {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                CreateEstateRequest createEstateRequest = new CreateEstateRequest
                                                          {
                                                              EstateId = Guid.NewGuid(),
                                                              EstateName = estateName
                                                          };

                CreateEstateResponse response = await this.TestingContext.DockerHelper.EstateClient.CreateEstate(String.Empty, createEstateRequest, CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);
                
                // Cache the estate id
                this.TestingContext.Estates.Add(estateName, response.EstateId);

                this.TestingContext.Logger.LogInformation($"Estate {estateName} created with Id {response.EstateId}");
            }

            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                KeyValuePair<String, Guid> estateItem= this.TestingContext.Estates.SingleOrDefault(e => e.Key == estateName);

                estateItem.Key.ShouldNotBeNullOrEmpty();
                estateItem.Value.ShouldNotBe(Guid.Empty);

                EstateResponse estate = await this.TestingContext.DockerHelper.EstateClient.GetEstate(String.Empty, estateItem.Value, CancellationToken.None).ConfigureAwait(false);

                estate.EstateName.ShouldBe(estateName);
            }
        }

        [Given(@"I have created the following operators")]
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
                Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

                CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest
                                                              {
                                                                  Name = operatorName,
                                                                  RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                                  RequireCustomTerminalNumber = requireCustomTerminalNumber
                                                              };

                // lookup the estate id based on the name in the table
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                Guid estateId = this.TestingContext.Estates.Single(e => e.Key == estateName).Value;

                CreateOperatorResponse response = await this.TestingContext.DockerHelper.EstateClient.CreateOperator(String.Empty, estateId, createOperatorRequest, CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);
                response.OperatorId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                this.TestingContext.Operators.Add(operatorName, response.OperatorId);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} created with Id {response.OperatorId} for Estate {response.EstateId}");
            }
        }

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest
                                                              {
                                                                  Name = merchantName,
                                                                  Contact = new Contact
                                                                            {
                                                                                ContactName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContactName"),
                                                                                EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress")
                                                                            },
                                                                  Address = new Address
                                                                            {
                                                                                AddressLine1 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine1"),
                                                                                Town = SpecflowTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                                Region = SpecflowTableHelper.GetStringRowValue(tableRow, "Region"),
                                                                                Country = SpecflowTableHelper.GetStringRowValue(tableRow, "Country")
                                                                            }
                                                              };

                // lookup the estate id based on the name in the table
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                Guid estateId = this.TestingContext.Estates.Single(e => e.Key == estateName).Value;
                
                CreateMerchantResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateMerchant(String.Empty, estateId, createMerchantRequest, CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldBe(estateId);
                response.MerchantId.ShouldNotBe(Guid.Empty);

                // Cache the merchant id
                this.TestingContext.Merchants.Add(merchantName, response.MerchantId);
                if (this.TestingContext.EstateMerchants.ContainsKey(estateId))
                {
                    List<Guid> merchantIdList = this.TestingContext.EstateMerchants[estateId];
                    merchantIdList.Add(response.MerchantId);
                }
                else
                {
                    this.TestingContext.EstateMerchants.Add(estateId, new List<Guid> {response.MerchantId});
                }

                this.TestingContext.Logger.LogInformation($"Merchant {merchantName} created with Id {response.MerchantId} for Estate {response.EstateId}");
            }

            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                KeyValuePair<String, Guid> estateItem = this.TestingContext.Estates.SingleOrDefault(e => e.Key == estateName);

                estateItem.Key.ShouldNotBeNullOrEmpty();
                estateItem.Value.ShouldNotBe(Guid.Empty);

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");

                KeyValuePair<String, Guid> merchantItem = this.TestingContext.Merchants.SingleOrDefault(m => m.Key == merchantName);

                merchantItem.Key.ShouldNotBeNullOrEmpty();
                merchantItem.Value.ShouldNotBe(Guid.Empty);

                MerchantResponse merchant = await this.TestingContext.DockerHelper.EstateClient.GetMerchant(String.Empty, estateItem.Value, merchantItem.Value, CancellationToken.None).ConfigureAwait(false);

                merchant.MerchantName.ShouldBe(merchantName);
            }
        }

        [When(@"I assign the following  operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = this.TestingContext.Merchants[merchantName];

                // Lookup the operator id
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = this.TestingContext.Operators[operatorName];

                // Now find the estate Id
                Guid estateId = this.TestingContext.EstateMerchants.Where(e => e.Value.Contains(merchantId)).Single().Key;

                AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
                                                              {
                                                                  OperatorId = operatorId,
                                                                  MerchantNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                                                                  TerminalNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                                                              };

                AssignOperatorResponse assignOperatorResponse = await this.TestingContext.DockerHelper.EstateClient.AssignOperatorToMerchant(String.Empty, estateId, merchantId, assignOperatorRequest, CancellationToken.None).ConfigureAwait(false);
                
                assignOperatorResponse.EstateId.ShouldBe(estateId);
                assignOperatorResponse.MerchantId.ShouldBe(merchantId);
                assignOperatorResponse.OperatorId.ShouldBe(operatorId);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} assigned to Estate {estateId}");
            }
        }

        [When(@"I create the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // lookup the estate id based on the name in the table
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
                Guid estateId = this.TestingContext.Estates.Single(e => e.Key == estateName).Value;
                
                CreateEstateUserRequest createEstateUserRequest = new CreateEstateUserRequest
                                                                  {
                                                                      EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                                      FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                                      GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                                      MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                                      Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
                                                                  };

                CreateEstateUserResponse createEstateUserResponse = await this.TestingContext.DockerHelper.EstateClient.CreateEstateUser(String.Empty, estateId, createEstateUserRequest, CancellationToken.None);

                createEstateUserResponse.EstateId.ShouldBe(estateId);
                createEstateUserResponse.UserId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Logger.LogInformation($"Security user {createEstateUserRequest.EmailAddress} assigned to Estate {estateName}");
            }
        }

    }
}

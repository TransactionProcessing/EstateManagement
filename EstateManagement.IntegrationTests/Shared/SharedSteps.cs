using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Shared
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
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


    }
}

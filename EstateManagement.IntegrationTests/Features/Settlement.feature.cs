﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:1.0.0.0
//      Reqnroll Generator Version:1.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EstateManagement.IntegrationTests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "1.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "shared")]
    public partial class SettlementFeature : object, Xunit.IClassFixture<SettlementFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private static Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Settlement.feature"
#line hidden
        
        public SettlementFeature(SettlementFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }
        
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.GetWorkerId());
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Settlement", null, ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            string testWorkerId = testRunner.TestWorkerId;
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
            Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.ReleaseWorker(testWorkerId);
        }
        
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 4
#line hidden
            Reqnroll.Table table72 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table72.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table72.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
#line 6
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table72, "Given ");
#line hidden
            Reqnroll.Table table73 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table73.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
            table73.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
#line 11
 await testRunner.GivenAsync("the following api resources exist", ((string)(null)), table73, "Given ");
#line hidden
            Reqnroll.Table table74 = new Reqnroll.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "Scopes",
                        "GrantTypes"});
            table74.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor",
                        "client_credentials"});
#line 16
 await testRunner.GivenAsync("the following clients exist", ((string)(null)), table74, "Given ");
#line hidden
            Reqnroll.Table table75 = new Reqnroll.Table(new string[] {
                        "ClientId"});
            table75.AddRow(new string[] {
                        "serviceClient"});
#line 20
 await testRunner.GivenAsync("I have a token to access the estate management and transaction processor resource" +
                    "s", ((string)(null)), table75, "Given ");
#line hidden
            Reqnroll.Table table76 = new Reqnroll.Table(new string[] {
                        "EstateName"});
            table76.AddRow(new string[] {
                        "Test Estate 1"});
            table76.AddRow(new string[] {
                        "Test Estate 2"});
#line 24
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table76, "Given ");
#line hidden
            Reqnroll.Table table77 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table77.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "True",
                        "True"});
            table77.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "True",
                        "True"});
#line 29
 await testRunner.GivenAsync("I have created the following operators", ((string)(null)), table77, "Given ");
#line hidden
            Reqnroll.Table table78 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table78.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract"});
            table78.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract"});
#line 34
 await testRunner.GivenAsync("I create a contract with the following values", ((string)(null)), table78, "Given ");
#line hidden
            Reqnroll.Table table79 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value",
                        "ProductType"});
            table79.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
            table79.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
#line 39
 await testRunner.WhenAsync("I create the following Products", ((string)(null)), table79, "When ");
#line hidden
            Reqnroll.Table table80 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value",
                        "FeeType"});
            table80.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.50",
                        "Merchant"});
            table80.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.85",
                        "Merchant"});
#line 44
 await testRunner.WhenAsync("I add the following Transaction Fees", ((string)(null)), table80, "When ");
#line hidden
            Reqnroll.Table table81 = new Reqnroll.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName",
                        "SettlementSchedule"});
            table81.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table81.AddRow(new string[] {
                        "Test Merchant 2",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 2",
                        "testcontact2@merchant2.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table81.AddRow(new string[] {
                        "Test Merchant 3",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 3",
                        "testcontact3@merchant2.co.uk",
                        "Test Estate 2",
                        "Monthly"});
#line 49
 await testRunner.GivenAsync("I create the following merchants", ((string)(null)), table81, "Given ");
#line hidden
            Reqnroll.Table table82 = new Reqnroll.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table82.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table82.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 2",
                        "00000002",
                        "10000002",
                        "Test Estate 1"});
            table82.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 3",
                        "00000003",
                        "10000003",
                        "Test Estate 2"});
#line 55
 await testRunner.GivenAsync("I have assigned the following operator to the merchants", ((string)(null)), table82, "Given ");
#line hidden
            Reqnroll.Table table83 = new Reqnroll.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table83.AddRow(new string[] {
                        "123456780",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table83.AddRow(new string[] {
                        "123456781",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table83.AddRow(new string[] {
                        "123456782",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 61
 await testRunner.GivenAsync("I have assigned the following devices to the merchants", ((string)(null)), table83, "Given ");
#line hidden
            Reqnroll.Table table84 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "ContractDescription"});
            table84.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "Safaricom Contract"});
            table84.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "Safaricom Contract"});
            table84.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "Safaricom Contract"});
#line 67
 await testRunner.WhenAsync("I add the following contracts to the following merchants", ((string)(null)), table84, "When ");
#line hidden
            Reqnroll.Table table85 = new Reqnroll.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table85.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table85.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table85.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 73
 await testRunner.GivenAsync("I make the following manual merchant deposits", ((string)(null)), table85, "Given ");
#line hidden
            Reqnroll.Table table86 = new Reqnroll.Table(new string[] {
                        "DateTime",
                        "TransactionNumber",
                        "TransactionType",
                        "TransactionSource",
                        "MerchantName",
                        "DeviceIdentifier",
                        "EstateName",
                        "OperatorName",
                        "TransactionAmount",
                        "CustomerAccountNumber",
                        "CustomerEmailAddress",
                        "ContractDescription",
                        "ProductName"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "2",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "5.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "3",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "25.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "4",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "150.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "5",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "3.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "6",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "40.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "7",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "60.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "8",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Safaricom",
                        "101.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "1",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate 1",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "2",
                        "Sale",
                        "1",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate 1",
                        "Safaricom",
                        "5.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "3",
                        "Sale",
                        "1",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate 1",
                        "Safaricom",
                        "25.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "4",
                        "Sale",
                        "1",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate 1",
                        "Safaricom",
                        "15.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table86.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "1",
                        "Test Merchant 3",
                        "123456782",
                        "Test Estate 2",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
#line 79
 await testRunner.WhenAsync("I perform the following transactions", ((string)(null)), table86, "When ");
#line hidden
            Reqnroll.Table table87 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "TransactionNumber",
                        "ResponseCode",
                        "ResponseMessage"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "1",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "3",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "4",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "5",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "7",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "8",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "1",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "4",
                        "0000",
                        "SUCCESS"});
            table87.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1",
                        "0000",
                        "SUCCESS"});
#line 97
 await testRunner.ThenAsync("transaction response should contain the following information", ((string)(null)), table87, "Then ");
#line hidden
            Reqnroll.Table table88 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table88.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6"});
            table88.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3"});
#line 115
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table88, "When ");
#line hidden
#line 120
 await testRunner.WhenAsync("I process the settlement for \'2022-01-13\' on Estate \'Test Estate 1\' for Merchant " +
                    "\'Test Merchant 1\' then 6 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 122
 await testRunner.WhenAsync("I process the settlement for \'2022-01-13\' on Estate \'Test Estate 1\' for Merchant " +
                    "\'Test Merchant 2\' then 3 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
            Reqnroll.Table table89 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table89.AddRow(new string[] {
                        "2022-02-06",
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1"});
#line 124
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table89, "When ");
#line hidden
#line 128
 await testRunner.WhenAsync("I process the settlement for \'2022-02-06\' on Estate \'Test Estate 2\' for Merchant " +
                    "\'Test Merchant 3\' then 1 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
        {
            await this.TestInitializeAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
        {
            await this.TestTearDownAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Settlements - Merchant Filter")]
        [Xunit.TraitAttribute("FeatureTitle", "Settlement")]
        [Xunit.TraitAttribute("Description", "Get Settlements - Merchant Filter")]
        [Xunit.TraitAttribute("Category", "settlement")]
        [Xunit.TraitAttribute("Category", "PRTest")]
        public async System.Threading.Tasks.Task GetSettlements_MerchantFilter()
        {
            string[] tagsOfScenario = new string[] {
                    "settlement",
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("Get Settlements - Merchant Filter", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 132
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 4
await this.FeatureBackgroundAsync();
#line hidden
                Reqnroll.Table table90 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table90.AddRow(new string[] {
                            "2022-01-13",
                            "6",
                            "2.39",
                            "True"});
#line 133
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table90, "When ");
#line hidden
                Reqnroll.Table table91 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table91.AddRow(new string[] {
                            "2022-01-13",
                            "3",
                            "0.71",
                            "True"});
#line 137
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table91, "When ");
#line hidden
                Reqnroll.Table table92 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table92.AddRow(new string[] {
                            "2022-02-06",
                            "1",
                            "0.85",
                            "True"});
#line 141
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table92, "When ");
#line hidden
                Reqnroll.Table table93 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.75"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.20"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.30"});
                table93.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.51"});
#line 145
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table93, "When ");
#line hidden
                Reqnroll.Table table94 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table94.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table94.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table94.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.08"});
#line 154
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table94, "When ");
#line hidden
                Reqnroll.Table table95 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table95.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.85"});
#line 160
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Date \'2022-02-06\' the following fees are settled", ((string)(null)), table95, "When ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "1.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : object, Xunit.IAsyncLifetime
        {
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
            {
                await SettlementFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await SettlementFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion

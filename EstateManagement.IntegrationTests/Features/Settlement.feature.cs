﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EstateManagement.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "shared")]
    public partial class SettlementFeature : object, Xunit.IClassFixture<SettlementFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Settlement.feature"
#line hidden
        
        public SettlementFeature(SettlementFeature.FixtureData fixtureData, EstateManagement_IntegrationTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Settlement", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line hidden
            TechTalk.SpecFlow.Table table66 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table66.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table66.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
#line 6
 testRunner.Given("I create the following api scopes", ((string)(null)), table66, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table67 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table67.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
            table67.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
#line 11
 testRunner.Given("the following api resources exist", ((string)(null)), table67, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table68 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table68.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor",
                        "client_credentials"});
#line 16
 testRunner.Given("the following clients exist", ((string)(null)), table68, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table69 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table69.AddRow(new string[] {
                        "serviceClient"});
#line 20
 testRunner.Given("I have a token to access the estate management and transaction processor resource" +
                    "s", ((string)(null)), table69, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table70 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName"});
            table70.AddRow(new string[] {
                        "Test Estate1"});
            table70.AddRow(new string[] {
                        "Test Estate2"});
#line 24
 testRunner.Given("I have created the following estates", ((string)(null)), table70, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table71 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table71.AddRow(new string[] {
                        "Test Estate1",
                        "Safaricom",
                        "True",
                        "True"});
            table71.AddRow(new string[] {
                        "Test Estate2",
                        "Safaricom",
                        "True",
                        "True"});
#line 29
 testRunner.Given("I have created the following operators", ((string)(null)), table71, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table72 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table72.AddRow(new string[] {
                        "Test Estate1",
                        "Safaricom",
                        "Safaricom Contract"});
            table72.AddRow(new string[] {
                        "Test Estate2",
                        "Safaricom",
                        "Safaricom Contract"});
#line 34
 testRunner.Given("I create a contract with the following values", ((string)(null)), table72, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table73 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value",
                        "ProductType"});
            table73.AddRow(new string[] {
                        "Test Estate1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
            table73.AddRow(new string[] {
                        "Test Estate2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
#line 39
 testRunner.When("I create the following Products", ((string)(null)), table73, "When ");
#line hidden
            TechTalk.SpecFlow.Table table74 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value",
                        "FeeType"});
            table74.AddRow(new string[] {
                        "Test Estate1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.50",
                        "Merchant"});
            table74.AddRow(new string[] {
                        "Test Estate2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.85",
                        "Merchant"});
#line 44
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table74, "When ");
#line hidden
            TechTalk.SpecFlow.Table table75 = new TechTalk.SpecFlow.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName",
                        "SettlementSchedule"});
            table75.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate1",
                        "Weekly"});
            table75.AddRow(new string[] {
                        "Test Merchant 2",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 2",
                        "testcontact2@merchant2.co.uk",
                        "Test Estate1",
                        "Weekly"});
            table75.AddRow(new string[] {
                        "Test Merchant 3",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 3",
                        "testcontact3@merchant2.co.uk",
                        "Test Estate2",
                        "Monthly"});
#line 49
 testRunner.Given("I create the following merchants", ((string)(null)), table75, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table76 = new TechTalk.SpecFlow.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table76.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate1"});
            table76.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 2",
                        "00000002",
                        "10000002",
                        "Test Estate1"});
            table76.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 3",
                        "00000003",
                        "10000003",
                        "Test Estate2"});
#line 55
 testRunner.Given("I have assigned the following operator to the merchants", ((string)(null)), table76, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table77 = new TechTalk.SpecFlow.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table77.AddRow(new string[] {
                        "123456780",
                        "Test Merchant 1",
                        "Test Estate1"});
            table77.AddRow(new string[] {
                        "123456781",
                        "Test Merchant 2",
                        "Test Estate1"});
            table77.AddRow(new string[] {
                        "123456782",
                        "Test Merchant 3",
                        "Test Estate2"});
#line 61
 testRunner.Given("I have assigned the following devices to the merchants", ((string)(null)), table77, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table78 = new TechTalk.SpecFlow.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table78.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate1"});
            table78.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 2",
                        "Test Estate1"});
            table78.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 3",
                        "Test Estate2"});
#line 67
 testRunner.Given("I make the following manual merchant deposits", ((string)(null)), table78, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table79 = new TechTalk.SpecFlow.Table(new string[] {
                        "DateTime",
                        "TransactionNumber",
                        "TransactionType",
                        "MerchantName",
                        "DeviceIdentifier",
                        "EstateName",
                        "OperatorName",
                        "TransactionAmount",
                        "CustomerAccountNumber",
                        "CustomerEmailAddress",
                        "ContractDescription",
                        "ProductName"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "2",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "5.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "3",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "25.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "4",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "150.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "5",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "3.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "6",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "40.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "7",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "60.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "8",
                        "Sale",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate1",
                        "Safaricom",
                        "101.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate1",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "2",
                        "Sale",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate1",
                        "Safaricom",
                        "5.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "3",
                        "Sale",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate1",
                        "Safaricom",
                        "25.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "4",
                        "Sale",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate1",
                        "Safaricom",
                        "15.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
            table79.AddRow(new string[] {
                        "2022-01-06",
                        "1",
                        "Sale",
                        "Test Merchant 3",
                        "123456782",
                        "Test Estate2",
                        "Safaricom",
                        "100.00",
                        "123456789",
                        "",
                        "Safaricom Contract",
                        "Variable Topup"});
#line 73
 testRunner.When("I perform the following transactions", ((string)(null)), table79, "When ");
#line hidden
            TechTalk.SpecFlow.Table table80 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "TransactionNumber",
                        "ResponseCode",
                        "ResponseMessage"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "1",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "3",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "4",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "5",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "6",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "7",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 1",
                        "8",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 2",
                        "1",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 2",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 2",
                        "3",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate1",
                        "Test Merchant 2",
                        "4",
                        "0000",
                        "SUCCESS"});
            table80.AddRow(new string[] {
                        "Test Estate2",
                        "Test Merchant 3",
                        "1",
                        "0000",
                        "SUCCESS"});
#line 91
 testRunner.Then("transaction response should contain the following information", ((string)(null)), table80, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table81 = new TechTalk.SpecFlow.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table81.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate1",
                        "Test Merchant 1",
                        "6"});
            table81.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate1",
                        "Test Merchant 2",
                        "3"});
#line 109
 testRunner.When("I get the pending settlements the following information should be returned", ((string)(null)), table81, "When ");
#line hidden
#line 114
 testRunner.When("I process the settlement for \'2022-01-13\' on Estate \'Test Estate1\' for Merchant \'" +
                    "Test Merchant 1\' then 6 fees are marked as settled and the settlement is complet" +
                    "ed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 116
 testRunner.When("I process the settlement for \'2022-01-13\' on Estate \'Test Estate1\' for Merchant \'" +
                    "Test Merchant 2\' then 3 fees are marked as settled and the settlement is complet" +
                    "ed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table82 = new TechTalk.SpecFlow.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table82.AddRow(new string[] {
                        "2022-02-06",
                        "Test Estate2",
                        "Test Merchant 3",
                        "1"});
#line 118
 testRunner.When("I get the pending settlements the following information should be returned", ((string)(null)), table82, "When ");
#line hidden
#line 122
 testRunner.When("I process the settlement for \'2022-02-06\' on Estate \'Test Estate2\' for Merchant \'" +
                    "Test Merchant 3\' then 1 fees are marked as settled and the settlement is complet" +
                    "ed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Settlements - Merchant Filter")]
        [Xunit.TraitAttribute("FeatureTitle", "Settlement")]
        [Xunit.TraitAttribute("Description", "Get Settlements - Merchant Filter")]
        [Xunit.TraitAttribute("Category", "settlement")]
        [Xunit.TraitAttribute("Category", "PRTest")]
        public void GetSettlements_MerchantFilter()
        {
            string[] tagsOfScenario = new string[] {
                    "settlement",
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Settlements - Merchant Filter", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 126
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table83 = new TechTalk.SpecFlow.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table83.AddRow(new string[] {
                            "2022-01-13",
                            "6",
                            "2.39",
                            "True"});
#line 127
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate1\' for Merchant \'Test M" +
                        "erchant 1\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the fo" +
                        "llowing data is returned", ((string)(null)), table83, "When ");
#line hidden
                TechTalk.SpecFlow.Table table84 = new TechTalk.SpecFlow.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table84.AddRow(new string[] {
                            "2022-01-13",
                            "3",
                            "0.71",
                            "True"});
#line 131
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate1\' for Merchant \'Test M" +
                        "erchant 2\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the fo" +
                        "llowing data is returned", ((string)(null)), table84, "When ");
#line hidden
                TechTalk.SpecFlow.Table table85 = new TechTalk.SpecFlow.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table85.AddRow(new string[] {
                            "2022-02-06",
                            "1",
                            "0.85",
                            "True"});
#line 135
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate2\' for Merchant \'Test M" +
                        "erchant 3\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the fo" +
                        "llowing data is returned", ((string)(null)), table85, "When ");
#line hidden
                TechTalk.SpecFlow.Table table86 = new TechTalk.SpecFlow.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.75"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.20"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.30"});
                table86.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.51"});
#line 139
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate1\' for Merchant \'Test M" +
                        "erchant 1\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table86, "When ");
#line hidden
                TechTalk.SpecFlow.Table table87 = new TechTalk.SpecFlow.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table87.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table87.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table87.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.08"});
#line 148
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate1\' for Merchant \'Test M" +
                        "erchant 2\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table87, "When ");
#line hidden
                TechTalk.SpecFlow.Table table88 = new TechTalk.SpecFlow.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table88.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.85"});
#line 154
 testRunner.When("I get the Estate Settlement Report for Estate \'Test Estate2\' for Merchant \'Test M" +
                        "erchant 3\' with the Date \'2022-02-06\' the following fees are settled", ((string)(null)), table88, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                SettlementFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                SettlementFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

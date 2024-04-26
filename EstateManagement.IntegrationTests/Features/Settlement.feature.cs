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
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Settlement")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("shared")]
    public partial class SettlementFeature
    {
        
        private Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
#line 1 "Settlement.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, NUnit.Framework.TestContext.CurrentContext.WorkerId);
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Settlement", null, ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
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
            Reqnroll.Table table80 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table80.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table80.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
#line 6
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table80, "Given ");
#line hidden
            Reqnroll.Table table81 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table81.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
            table81.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
#line 11
 await testRunner.GivenAsync("the following api resources exist", ((string)(null)), table81, "Given ");
#line hidden
            Reqnroll.Table table82 = new Reqnroll.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "Scopes",
                        "GrantTypes"});
            table82.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor",
                        "client_credentials"});
#line 16
 await testRunner.GivenAsync("the following clients exist", ((string)(null)), table82, "Given ");
#line hidden
            Reqnroll.Table table83 = new Reqnroll.Table(new string[] {
                        "ClientId"});
            table83.AddRow(new string[] {
                        "serviceClient"});
#line 20
 await testRunner.GivenAsync("I have a token to access the estate management and transaction processor resource" +
                    "s", ((string)(null)), table83, "Given ");
#line hidden
            Reqnroll.Table table84 = new Reqnroll.Table(new string[] {
                        "EstateName"});
            table84.AddRow(new string[] {
                        "Test Estate 1"});
            table84.AddRow(new string[] {
                        "Test Estate 2"});
#line 24
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table84, "Given ");
#line hidden
            Reqnroll.Table table85 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table85.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "True",
                        "True"});
            table85.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "True",
                        "True"});
#line 29
 await testRunner.GivenAsync("I have created the following operators", ((string)(null)), table85, "Given ");
#line hidden
            Reqnroll.Table table86 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName"});
            table86.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom"});
            table86.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom"});
#line 34
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table86, "And ");
#line hidden
            Reqnroll.Table table87 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table87.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract"});
            table87.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract"});
#line 39
 await testRunner.GivenAsync("I create a contract with the following values", ((string)(null)), table87, "Given ");
#line hidden
            Reqnroll.Table table88 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value",
                        "ProductType"});
            table88.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
            table88.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
#line 44
 await testRunner.WhenAsync("I create the following Products", ((string)(null)), table88, "When ");
#line hidden
            Reqnroll.Table table89 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value",
                        "FeeType"});
            table89.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.50",
                        "Merchant"});
            table89.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.85",
                        "Merchant"});
#line 49
 await testRunner.WhenAsync("I add the following Transaction Fees", ((string)(null)), table89, "When ");
#line hidden
            Reqnroll.Table table90 = new Reqnroll.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName",
                        "SettlementSchedule"});
            table90.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table90.AddRow(new string[] {
                        "Test Merchant 2",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 2",
                        "testcontact2@merchant2.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table90.AddRow(new string[] {
                        "Test Merchant 3",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 3",
                        "testcontact3@merchant2.co.uk",
                        "Test Estate 2",
                        "Monthly"});
#line 54
 await testRunner.GivenAsync("I create the following merchants", ((string)(null)), table90, "Given ");
#line hidden
            Reqnroll.Table table91 = new Reqnroll.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table91.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table91.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 2",
                        "00000002",
                        "10000002",
                        "Test Estate 1"});
            table91.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 3",
                        "00000003",
                        "10000003",
                        "Test Estate 2"});
#line 60
 await testRunner.GivenAsync("I have assigned the following operator to the merchants", ((string)(null)), table91, "Given ");
#line hidden
            Reqnroll.Table table92 = new Reqnroll.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table92.AddRow(new string[] {
                        "123456780",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table92.AddRow(new string[] {
                        "123456781",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table92.AddRow(new string[] {
                        "123456782",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 66
 await testRunner.GivenAsync("I have assigned the following devices to the merchants", ((string)(null)), table92, "Given ");
#line hidden
            Reqnroll.Table table93 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "ContractDescription"});
            table93.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "Safaricom Contract"});
            table93.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "Safaricom Contract"});
            table93.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "Safaricom Contract"});
#line 72
 await testRunner.WhenAsync("I add the following contracts to the following merchants", ((string)(null)), table93, "When ");
#line hidden
            Reqnroll.Table table94 = new Reqnroll.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table94.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table94.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table94.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 78
 await testRunner.GivenAsync("I make the following manual merchant deposits", ((string)(null)), table94, "Given ");
#line hidden
            Reqnroll.Table table95 = new Reqnroll.Table(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
            table95.AddRow(new string[] {
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
#line 84
 await testRunner.WhenAsync("I perform the following transactions", ((string)(null)), table95, "When ");
#line hidden
            Reqnroll.Table table96 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "TransactionNumber",
                        "ResponseCode",
                        "ResponseMessage"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "1",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "3",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "4",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "5",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "7",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "8",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "1",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "4",
                        "0000",
                        "SUCCESS"});
            table96.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1",
                        "0000",
                        "SUCCESS"});
#line 102
 await testRunner.ThenAsync("transaction response should contain the following information", ((string)(null)), table96, "Then ");
#line hidden
            Reqnroll.Table table97 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table97.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6"});
            table97.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3"});
#line 120
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table97, "When ");
#line hidden
#line 125
 await testRunner.WhenAsync("I process the settlement for \'2022-01-13\' on Estate \'Test Estate 1\' for Merchant " +
                    "\'Test Merchant 1\' then 6 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 127
 await testRunner.WhenAsync("I process the settlement for \'2022-01-13\' on Estate \'Test Estate 1\' for Merchant " +
                    "\'Test Merchant 2\' then 3 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
            Reqnroll.Table table98 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table98.AddRow(new string[] {
                        "2022-02-06",
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1"});
#line 129
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table98, "When ");
#line hidden
#line 133
 await testRunner.WhenAsync("I process the settlement for \'2022-02-06\' on Estate \'Test Estate 2\' for Merchant " +
                    "\'Test Merchant 3\' then 1 fees are marked as settled and the settlement is comple" +
                    "ted", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get Settlements - Merchant Filter")]
        [NUnit.Framework.CategoryAttribute("settlement")]
        [NUnit.Framework.CategoryAttribute("PRTest")]
        public async System.Threading.Tasks.Task GetSettlements_MerchantFilter()
        {
            string[] tagsOfScenario = new string[] {
                    "settlement",
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("Get Settlements - Merchant Filter", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 137
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
                Reqnroll.Table table99 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table99.AddRow(new string[] {
                            "2022-01-13",
                            "6",
                            "2.39",
                            "True"});
#line 138
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table99, "When ");
#line hidden
                Reqnroll.Table table100 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table100.AddRow(new string[] {
                            "2022-01-13",
                            "3",
                            "0.71",
                            "True"});
#line 142
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table100, "When ");
#line hidden
                Reqnroll.Table table101 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table101.AddRow(new string[] {
                            "2022-02-06",
                            "1",
                            "0.85",
                            "True"});
#line 146
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table101, "When ");
#line hidden
                Reqnroll.Table table102 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.75"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.20"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.30"});
                table102.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.51"});
#line 150
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table102, "When ");
#line hidden
                Reqnroll.Table table103 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table103.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table103.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table103.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.08"});
#line 159
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table103, "When ");
#line hidden
                Reqnroll.Table table104 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table104.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.85"});
#line 165
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Date \'2022-02-06\' the following fees are settled", ((string)(null)), table104, "When ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion

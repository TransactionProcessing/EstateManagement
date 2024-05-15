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
            Reqnroll.Table table92 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table92.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table92.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST  Scope",
                        "A scope for Transaction Processor REST"});
#line 6
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table92, "Given ");
#line hidden
            Reqnroll.Table table93 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table93.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
            table93.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        ""});
#line 11
 await testRunner.GivenAsync("the following api resources exist", ((string)(null)), table93, "Given ");
#line hidden
            Reqnroll.Table table94 = new Reqnroll.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "Scopes",
                        "GrantTypes"});
            table94.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor",
                        "client_credentials"});
#line 16
 await testRunner.GivenAsync("the following clients exist", ((string)(null)), table94, "Given ");
#line hidden
            Reqnroll.Table table95 = new Reqnroll.Table(new string[] {
                        "ClientId"});
            table95.AddRow(new string[] {
                        "serviceClient"});
#line 20
 await testRunner.GivenAsync("I have a token to access the estate management and transaction processor resource" +
                    "s", ((string)(null)), table95, "Given ");
#line hidden
            Reqnroll.Table table96 = new Reqnroll.Table(new string[] {
                        "EstateName"});
            table96.AddRow(new string[] {
                        "Test Estate 1"});
            table96.AddRow(new string[] {
                        "Test Estate 2"});
#line 24
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table96, "Given ");
#line hidden
            Reqnroll.Table table97 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table97.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "True",
                        "True"});
            table97.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "True",
                        "True"});
#line 29
 await testRunner.GivenAsync("I have created the following operators", ((string)(null)), table97, "Given ");
#line hidden
            Reqnroll.Table table98 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName"});
            table98.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom"});
            table98.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom"});
#line 34
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table98, "And ");
#line hidden
            Reqnroll.Table table99 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table99.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract"});
            table99.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract"});
#line 39
 await testRunner.GivenAsync("I create a contract with the following values", ((string)(null)), table99, "Given ");
#line hidden
            Reqnroll.Table table100 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value",
                        "ProductType"});
            table100.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
            table100.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Custom",
                        "",
                        "MobileTopup"});
#line 44
 await testRunner.WhenAsync("I create the following Products", ((string)(null)), table100, "When ");
#line hidden
            Reqnroll.Table table101 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "CalculationType",
                        "FeeDescription",
                        "Value",
                        "FeeType"});
            table101.AddRow(new string[] {
                        "Test Estate 1",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.50",
                        "Merchant"});
            table101.AddRow(new string[] {
                        "Test Estate 2",
                        "Safaricom",
                        "Safaricom Contract",
                        "Variable Topup",
                        "Percentage",
                        "Merchant Commission",
                        "0.85",
                        "Merchant"});
#line 49
 await testRunner.WhenAsync("I add the following Transaction Fees", ((string)(null)), table101, "When ");
#line hidden
            Reqnroll.Table table102 = new Reqnroll.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName",
                        "SettlementSchedule"});
            table102.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table102.AddRow(new string[] {
                        "Test Merchant 2",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 2",
                        "testcontact2@merchant2.co.uk",
                        "Test Estate 1",
                        "Weekly"});
            table102.AddRow(new string[] {
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
 await testRunner.GivenAsync("I create the following merchants", ((string)(null)), table102, "Given ");
#line hidden
            Reqnroll.Table table103 = new Reqnroll.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table103.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table103.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 2",
                        "00000002",
                        "10000002",
                        "Test Estate 1"});
            table103.AddRow(new string[] {
                        "Safaricom",
                        "Test Merchant 3",
                        "00000003",
                        "10000003",
                        "Test Estate 2"});
#line 60
 await testRunner.GivenAsync("I have assigned the following operator to the merchants", ((string)(null)), table103, "Given ");
#line hidden
            Reqnroll.Table table104 = new Reqnroll.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table104.AddRow(new string[] {
                        "123456780",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table104.AddRow(new string[] {
                        "123456781",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table104.AddRow(new string[] {
                        "123456782",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 66
 await testRunner.GivenAsync("I have assigned the following devices to the merchants", ((string)(null)), table104, "Given ");
#line hidden
            Reqnroll.Table table105 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "ContractDescription"});
            table105.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "Safaricom Contract"});
            table105.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "Safaricom Contract"});
            table105.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "Safaricom Contract"});
#line 72
 await testRunner.WhenAsync("I add the following contracts to the following merchants", ((string)(null)), table105, "When ");
#line hidden
            Reqnroll.Table table106 = new Reqnroll.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table106.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table106.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 2",
                        "Test Estate 1"});
            table106.AddRow(new string[] {
                        "Deposit1",
                        "50000.00",
                        "Today",
                        "Test Merchant 3",
                        "Test Estate 2"});
#line 78
 await testRunner.GivenAsync("I make the following manual merchant deposits", ((string)(null)), table106, "Given ");
#line hidden
            Reqnroll.Table table107 = new Reqnroll.Table(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
            table107.AddRow(new string[] {
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
 await testRunner.WhenAsync("I perform the following transactions", ((string)(null)), table107, "When ");
#line hidden
            Reqnroll.Table table108 = new Reqnroll.Table(new string[] {
                        "EstateName",
                        "MerchantName",
                        "TransactionNumber",
                        "ResponseCode",
                        "ResponseMessage"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "1",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "3",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "4",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "5",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "7",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 1",
                        "8",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "1",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "2",
                        "1008",
                        "DECLINED BY OPERATOR"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Merchant 2",
                        "4",
                        "0000",
                        "SUCCESS"});
            table108.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1",
                        "0000",
                        "SUCCESS"});
#line 102
 await testRunner.ThenAsync("transaction response should contain the following information", ((string)(null)), table108, "Then ");
#line hidden
            Reqnroll.Table table109 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table109.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 1",
                        "6"});
            table109.AddRow(new string[] {
                        "2022-01-13",
                        "Test Estate 1",
                        "Test Merchant 2",
                        "3"});
#line 120
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table109, "When ");
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
            Reqnroll.Table table110 = new Reqnroll.Table(new string[] {
                        "SettlementDate",
                        "EstateName",
                        "MerchantName",
                        "NumberOfFees"});
            table110.AddRow(new string[] {
                        "2022-02-06",
                        "Test Estate 2",
                        "Test Merchant 3",
                        "1"});
#line 129
 await testRunner.WhenAsync("I get the pending settlements the following information should be returned", ((string)(null)), table110, "When ");
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
                Reqnroll.Table table111 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table111.AddRow(new string[] {
                            "2022-01-13",
                            "6",
                            "2.39",
                            "True"});
#line 138
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table111, "When ");
#line hidden
                Reqnroll.Table table112 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table112.AddRow(new string[] {
                            "2022-01-13",
                            "3",
                            "0.71",
                            "True"});
#line 142
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table112, "When ");
#line hidden
                Reqnroll.Table table113 = new Reqnroll.Table(new string[] {
                            "SettlementDate",
                            "NumberOfFeesSettled",
                            "ValueOfFeesSettled",
                            "IsCompleted"});
                table113.AddRow(new string[] {
                            "2022-02-06",
                            "1",
                            "0.85",
                            "True"});
#line 146
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Start Date \'2022-01-13\' and the End Date \'2022-02-06\' the f" +
                        "ollowing data is returned", ((string)(null)), table113, "When ");
#line hidden
                Reqnroll.Table table114 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.75"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.20"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.30"});
                table114.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.51"});
#line 150
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 1\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table114, "When ");
#line hidden
                Reqnroll.Table table115 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table115.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.50"});
                table115.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.13"});
                table115.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.08"});
#line 159
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 1\' for Merchant \'Test " +
                        "Merchant 2\' with the Date \'2022-01-13\' the following fees are settled", ((string)(null)), table115, "When ");
#line hidden
                Reqnroll.Table table116 = new Reqnroll.Table(new string[] {
                            "FeeDescription",
                            "IsSettled",
                            "Operator",
                            "CalculatedValue"});
                table116.AddRow(new string[] {
                            "Merchant Commission",
                            "True",
                            "Safaricom",
                            "0.85"});
#line 165
 await testRunner.WhenAsync("I get the Estate Settlement Report for Estate \'Test Estate 2\' for Merchant \'Test " +
                        "Merchant 3\' with the Date \'2022-02-06\' the following fees are settled", ((string)(null)), table116, "When ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion

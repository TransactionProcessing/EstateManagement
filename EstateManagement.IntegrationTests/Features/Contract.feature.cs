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
    public partial class ContractFeature : object, Xunit.IClassFixture<ContractFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Contract.feature"
#line hidden
        
        public ContractFeature(ContractFeature.FixtureData fixtureData, EstateManagement_IntegrationTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Contract", null, ProgrammingLanguage.CSharp, featureTags);
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
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoleName"});
            table1.AddRow(new string[] {
                        "Estate"});
#line 5
 testRunner.Given("the following security roles exist", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table2.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
#line 9
 testRunner.Given("I create the following api scopes", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table3.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "merchantId, estateId, role"});
#line 13
 testRunner.Given("the following api resources exist", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table4.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement",
                        "client_credentials"});
            table4.AddRow(new string[] {
                        "estateClient",
                        "Estate Client",
                        "Secret1",
                        "estateManagement",
                        "password"});
#line 17
 testRunner.Given("the following clients exist", ((string)(null)), table4, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table5.AddRow(new string[] {
                        "serviceClient"});
#line 22
 testRunner.Given("I have a token to access the estate management resource", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName"});
            table6.AddRow(new string[] {
                        "Test Estate 1"});
            table6.AddRow(new string[] {
                        "Test Estate 2"});
#line 26
 testRunner.Given("I have created the following estates", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table7.AddRow(new string[] {
                        "Test Estate 1",
                        "Test Operator 1",
                        "True",
                        "True"});
            table7.AddRow(new string[] {
                        "Test Estate 2",
                        "Test Operator 1",
                        "True",
                        "True"});
#line 31
 testRunner.Given("I have created the following operators", ((string)(null)), table7, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName"});
            table8.AddRow(new string[] {
                        "estateuser1@testestate1.co.uk",
                        "123456",
                        "TestEstate",
                        "User1",
                        "Test Estate 1"});
            table8.AddRow(new string[] {
                        "estateuser1@testestate2.co.uk",
                        "123456",
                        "TestEstate",
                        "User1",
                        "Test Estate 2"});
#line 36
 testRunner.Given("I have created the following security users", ((string)(null)), table8, "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Create Contract")]
        [Xunit.TraitAttribute("FeatureTitle", "Contract")]
        [Xunit.TraitAttribute("Description", "Create Contract")]
        public void CreateContract()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Contract", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 41
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
                TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription"});
                table9.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract"});
                table9.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract"});
#line 42
 testRunner.Given("I create a contract with the following values", ((string)(null)), table9, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "DisplayText",
                            "Value"});
                table10.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table10.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
                table10.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table10.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
#line 47
 testRunner.When("I create the following Products", ((string)(null)), table10, "When ");
#line hidden
                TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "CalculationType",
                            "FeeType",
                            "FeeDescription",
                            "Value"});
                table11.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Fixed",
                            "Merchant",
                            "Merchant Commission",
                            "2.00"});
                table11.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Fixed",
                            "Merchant",
                            "Merchant Commission",
                            "2.00"});
                table11.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant",
                            "Merchant Commission",
                            "0.75"});
                table11.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Percentage",
                            "Merchant",
                            "Merchant Commission",
                            "0.75"});
#line 54
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table11, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Transaction Fees for a Product")]
        [Xunit.TraitAttribute("FeatureTitle", "Contract")]
        [Xunit.TraitAttribute("Description", "Get Transaction Fees for a Product")]
        public void GetTransactionFeesForAProduct()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Transaction Fees for a Product", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 61
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
                TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription"});
                table12.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract"});
                table12.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract"});
#line 63
 testRunner.Given("I create a contract with the following values", ((string)(null)), table12, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "DisplayText",
                            "Value"});
                table13.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table13.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
                table13.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table13.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
#line 68
 testRunner.When("I create the following Products", ((string)(null)), table13, "When ");
#line hidden
                TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "CalculationType",
                            "FeeType",
                            "FeeDescription",
                            "Value"});
                table14.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Fixed",
                            "Merchant",
                            "Merchant Commission",
                            "2.00"});
                table14.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant",
                            "Merchant Commission",
                            "0.025"});
                table14.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Fixed",
                            "Merchant",
                            "Merchant Commission",
                            "2.50"});
                table14.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant",
                            "Merchant Commission",
                            "0.85"});
                table14.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Percentage",
                            "Merchant",
                            "Merchant Commission",
                            "0.85"});
#line 75
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table14, "When ");
#line hidden
                TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table15.AddRow(new string[] {
                            "Fixed",
                            "Merchant Commission",
                            "2.00",
                            "Merchant"});
                table15.AddRow(new string[] {
                            "Percentage",
                            "Merchant Commission",
                            "0.025",
                            "Merchant"});
#line 83
 testRunner.Then("I get the Transaction Fees for \'100 KES Topup\' on the \'Operator 1 Contract\' contr" +
                        "act for \'Test Estate 1\' the following fees are returned", ((string)(null)), table15, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table16.AddRow(new string[] {
                            "Fixed",
                            "Merchant Commission",
                            "2.50",
                            "Merchant"});
#line 88
 testRunner.Then("I get the Transaction Fees for \'Variable Topup\' on the \'Operator 1 Contract\' cont" +
                        "ract for \'Test Estate 1\' the following fees are returned", ((string)(null)), table16, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table17.AddRow(new string[] {
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
#line 92
 testRunner.Then("I get the Transaction Fees for \'100 KES Topup\' on the \'Operator 1 Contract\' contr" +
                        "act for \'Test Estate 2\' the following fees are returned", ((string)(null)), table17, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table18.AddRow(new string[] {
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
#line 96
 testRunner.Then("I get the Transaction Fees for \'Variable Topup\' on the \'Operator 1 Contract\' cont" +
                        "ract for \'Test Estate 2\' the following fees are returned", ((string)(null)), table18, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Merchant Contracts")]
        [Xunit.TraitAttribute("FeatureTitle", "Contract")]
        [Xunit.TraitAttribute("Description", "Get Merchant Contracts")]
        public void GetMerchantContracts()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Merchant Contracts", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 100
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
                TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                            "MerchantName",
                            "AddressLine1",
                            "Town",
                            "Region",
                            "Country",
                            "ContactName",
                            "EmailAddress",
                            "EstateName"});
                table19.AddRow(new string[] {
                            "Test Merchant 1",
                            "Address Line 1",
                            "TestTown",
                            "Test Region",
                            "United Kingdom",
                            "Test Contact 1",
                            "testcontact1@merchant1.co.uk",
                            "Test Estate 1"});
                table19.AddRow(new string[] {
                            "Test Merchant 2",
                            "Address Line 1",
                            "TestTown",
                            "Test Region",
                            "United Kingdom",
                            "Test Contact 1",
                            "testcontact1@merchant2.co.uk",
                            "Test Estate 2"});
#line 102
 testRunner.Given("I create the following merchants", ((string)(null)), table19, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription"});
                table20.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract"});
                table20.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract"});
#line 107
 testRunner.Given("I create a contract with the following values", ((string)(null)), table20, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "DisplayText",
                            "Value"});
                table21.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table21.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
                table21.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table21.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
#line 112
 testRunner.When("I create the following Products", ((string)(null)), table21, "When ");
#line hidden
                TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table22.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Fixed",
                            "Merchant Commission",
                            "2.00",
                            "Merchant"});
                table22.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.025",
                            "Merchant"});
                table22.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Fixed",
                            "Merchant Commission",
                            "2.50",
                            "Merchant"});
                table22.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
                table22.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
#line 119
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table22, "When ");
#line hidden
                TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                            "ContractDescription",
                            "ProductName"});
                table23.AddRow(new string[] {
                            "Operator 1 Contract",
                            "100 KES Topup"});
                table23.AddRow(new string[] {
                            "Operator 1 Contract",
                            "Variable Topup"});
#line 127
 testRunner.Then("I get the Merchant Contracts for \'Test Merchant 1\' for \'Test Estate 1\' the follow" +
                        "ing contract details are returned", ((string)(null)), table23, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                            "ContractDescription",
                            "ProductName"});
                table24.AddRow(new string[] {
                            "Operator 1 Contract",
                            "100 KES Topup"});
                table24.AddRow(new string[] {
                            "Operator 1 Contract",
                            "Variable Topup"});
#line 132
 testRunner.Then("I get the Merchant Contracts for \'Test Merchant 2\' for \'Test Estate 2\' the follow" +
                        "ing contract details are returned", ((string)(null)), table24, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Estate Contracts")]
        [Xunit.TraitAttribute("FeatureTitle", "Contract")]
        [Xunit.TraitAttribute("Description", "Get Estate Contracts")]
        public void GetEstateContracts()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Estate Contracts", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 137
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
                TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription"});
                table25.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract"});
                table25.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract"});
#line 139
 testRunner.Given("I create a contract with the following values", ((string)(null)), table25, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "DisplayText",
                            "Value"});
                table26.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table26.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
                table26.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "100 KES",
                            "100.00"});
                table26.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Custom",
                            ""});
#line 144
 testRunner.When("I create the following Products", ((string)(null)), table26, "When ");
#line hidden
                TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "ContractDescription",
                            "ProductName",
                            "CalculationType",
                            "FeeDescription",
                            "Value",
                            "FeeType"});
                table27.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Fixed",
                            "Merchant Commission",
                            "2.00",
                            "Merchant"});
                table27.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.025",
                            "Merchant"});
                table27.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Fixed",
                            "Merchant Commission",
                            "2.50",
                            "Merchant"});
                table27.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "100 KES Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
                table27.AddRow(new string[] {
                            "Test Estate 2",
                            "Test Operator 1",
                            "Operator 1 Contract",
                            "Variable Topup",
                            "Percentage",
                            "Merchant Commission",
                            "0.85",
                            "Merchant"});
#line 151
 testRunner.When("I add the following Transaction Fees", ((string)(null)), table27, "When ");
#line hidden
                TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                            "ContractDescription",
                            "ProductName"});
                table28.AddRow(new string[] {
                            "Operator 1 Contract",
                            "100 KES Topup"});
                table28.AddRow(new string[] {
                            "Operator 1 Contract",
                            "Variable Topup"});
#line 159
 testRunner.Then("I get the Contracts for \'Test Estate 1\' the following contract details are return" +
                        "ed", ((string)(null)), table28, "Then ");
#line hidden
                TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                            "ContractDescription",
                            "ProductName"});
                table29.AddRow(new string[] {
                            "Operator 1 Contract",
                            "100 KES Topup"});
                table29.AddRow(new string[] {
                            "Operator 1 Contract",
                            "Variable Topup"});
#line 164
 testRunner.Then("I get the Contracts for \'Test Estate 2\' the following contract details are return" +
                        "ed", ((string)(null)), table29, "Then ");
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
                ContractFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ContractFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

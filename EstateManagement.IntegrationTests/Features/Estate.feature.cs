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
    [NUnit.Framework.DescriptionAttribute("Estate")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("shared")]
    public partial class EstateFeature
    {
        
        private Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
#line 1 "Estate.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, NUnit.Framework.TestContext.CurrentContext.WorkerId);
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Estate", null, ProgrammingLanguage.CSharp, featureTags);
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
            Reqnroll.Table table23 = new Reqnroll.Table(new string[] {
                        "Role Name"});
            table23.AddRow(new string[] {
                        "Estate"});
#line 5
 await testRunner.GivenAsync("the following security roles exist", ((string)(null)), table23, "Given ");
#line hidden
            Reqnroll.Table table24 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table24.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
#line 9
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table24, "Given ");
#line hidden
            Reqnroll.Table table25 = new Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table25.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "merchantId, estateId, role"});
#line 13
 await testRunner.GivenAsync("the following api resources exist", ((string)(null)), table25, "Given ");
#line hidden
            Reqnroll.Table table26 = new Reqnroll.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "Scopes",
                        "GrantTypes"});
            table26.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement",
                        "client_credentials"});
            table26.AddRow(new string[] {
                        "estateClient",
                        "Estate Client",
                        "Secret1",
                        "estateManagement",
                        "password"});
#line 17
 await testRunner.GivenAsync("the following clients exist", ((string)(null)), table26, "Given ");
#line hidden
            Reqnroll.Table table27 = new Reqnroll.Table(new string[] {
                        "ClientId"});
            table27.AddRow(new string[] {
                        "serviceClient"});
#line 22
 await testRunner.GivenAsync("I have a token to access the estate management resource", ((string)(null)), table27, "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get Estate")]
        public async System.Threading.Tasks.Task GetEstate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("Get Estate", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 26
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
                Reqnroll.Table table28 = new Reqnroll.Table(new string[] {
                            "EstateName"});
                table28.AddRow(new string[] {
                            "Test Estate 1"});
#line 27
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table28, "Given ");
#line hidden
                Reqnroll.Table table29 = new Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table29.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "True",
                            "True"});
                table29.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 2",
                            "True",
                            "True"});
#line 30
 await testRunner.AndAsync("I have created the following operators", ((string)(null)), table29, "And ");
#line hidden
                Reqnroll.Table table30 = new Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName"});
                table30.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1"});
                table30.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 2"});
#line 34
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table30, "And ");
#line hidden
                Reqnroll.Table table31 = new Reqnroll.Table(new string[] {
                            "EmailAddress",
                            "Password",
                            "GivenName",
                            "FamilyName",
                            "EstateName"});
                table31.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User1",
                            "Test Estate 1"});
                table31.AddRow(new string[] {
                            "estateuser2@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User2",
                            "Test Estate 1"});
#line 38
 await testRunner.AndAsync("I have created the following security users", ((string)(null)), table31, "And ");
#line hidden
                Reqnroll.Table table32 = new Reqnroll.Table(new string[] {
                            "EstateName"});
                table32.AddRow(new string[] {
                            "Test Estate 1"});
#line 42
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate details are returned as follows", ((string)(null)), table32, "When ");
#line hidden
                Reqnroll.Table table33 = new Reqnroll.Table(new string[] {
                            "OperatorName"});
                table33.AddRow(new string[] {
                            "Test Operator 1"});
                table33.AddRow(new string[] {
                            "Test Operator 2"});
#line 45
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate operator details are returned as foll" +
                        "ows", ((string)(null)), table33, "When ");
#line hidden
                Reqnroll.Table table34 = new Reqnroll.Table(new string[] {
                            "EmailAddress"});
                table34.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk"});
                table34.AddRow(new string[] {
                            "estateuser2@testestate1.co.uk"});
#line 49
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate security user details are returned as" +
                        " follows", ((string)(null)), table34, "When ");
#line hidden
#line 53
 await testRunner.WhenAsync("I get the estate \"Test Estate 2\" an error is returned", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 54
 await testRunner.GivenAsync("I am logged in as \"estateuser1@testestate1.co.uk\" with password \"123456\" for Esta" +
                        "te \"Test Estate 1\" with client \"estateClient\"", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
                Reqnroll.Table table35 = new Reqnroll.Table(new string[] {
                            "EstateName"});
                table35.AddRow(new string[] {
                            "Test Estate 1"});
#line 55
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate details are returned as follows", ((string)(null)), table35, "When ");
#line hidden
                Reqnroll.Table table36 = new Reqnroll.Table(new string[] {
                            "OperatorName"});
                table36.AddRow(new string[] {
                            "Test Operator 1"});
                table36.AddRow(new string[] {
                            "Test Operator 2"});
#line 58
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate operator details are returned as foll" +
                        "ows", ((string)(null)), table36, "When ");
#line hidden
                Reqnroll.Table table37 = new Reqnroll.Table(new string[] {
                            "EmailAddress"});
                table37.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk"});
                table37.AddRow(new string[] {
                            "estateuser2@testestate1.co.uk"});
#line 62
 await testRunner.WhenAsync("I get the estate \"Test Estate 1\" the estate security user details are returned as" +
                        " follows", ((string)(null)), table37, "When ");
#line hidden
#line 66
 await testRunner.WhenAsync("I get the estate \"Test Estate 2\" an error is returned", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update Estate")]
        public async System.Threading.Tasks.Task UpdateEstate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("Update Estate", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 68
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
                Reqnroll.Table table38 = new Reqnroll.Table(new string[] {
                            "EstateName"});
                table38.AddRow(new string[] {
                            "Test Estate 1"});
#line 69
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table38, "Given ");
#line hidden
                Reqnroll.Table table39 = new Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table39.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "True",
                            "True"});
                table39.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 2",
                            "True",
                            "True"});
#line 72
 await testRunner.AndAsync("I have created the following operators", ((string)(null)), table39, "And ");
#line hidden
                Reqnroll.Table table40 = new Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName"});
                table40.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1"});
                table40.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 2"});
#line 76
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table40, "And ");
#line hidden
                Reqnroll.Table table41 = new Reqnroll.Table(new string[] {
                            "EmailAddress",
                            "Password",
                            "GivenName",
                            "FamilyName",
                            "EstateName"});
                table41.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User1",
                            "Test Estate 1"});
                table41.AddRow(new string[] {
                            "estateuser2@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User2",
                            "Test Estate 1"});
#line 80
 await testRunner.AndAsync("I have created the following security users", ((string)(null)), table41, "And ");
#line hidden
#line 84
 await testRunner.WhenAsync("I remove the operator \'Test Operator 1\' from estate \'Test Estate 1\' the operator " +
                        "is removed", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion

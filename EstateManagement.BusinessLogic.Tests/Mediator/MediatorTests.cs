﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.Mediator
{
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using BusinessLogic.Services;
    using Lamar;
    using Manger;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Moq;
    using Testing;
    using Xunit;

    public class MediatorTests
    {
        private readonly List<IBaseRequest> Requests = new();

        private IMediator mediator;

        public MediatorTests() {
            this.Requests.Add(TestData.AddMerchantDeviceCommand);
            this.Requests.Add(TestData.CreateMerchantCommand);
            this.Requests.Add(TestData.AssignOperatorToMerchantCommand);
            this.Requests.Add(TestData.AddMerchantContractCommand);
            this.Requests.Add(TestData.CreateMerchantUserCommand);
            this.Requests.Add(TestData.MakeMerchantDepositCommand);
            this.Requests.Add(TestData.MakeMerchantWithdrawalCommand);
            this.Requests.Add(TestData.SwapMerchantDeviceCommand);
            this.Requests.Add(TestData.GenerateMerchantStatementCommand);
            this.Requests.Add(TestData.UpdateMerchantCommand);
            this.Requests.Add(TestData.AddMerchantAddressCommand);
            this.Requests.Add(TestData.UpdateMerchantAddressCommand);
            this.Requests.Add(TestData.AddMerchantContactCommand);
            this.Requests.Add(TestData.UpdateMerchantContactCommand);
            this.Requests.Add(TestData.RemoveOperatorFromMerchantCommand);
            this.Requests.Add(TestData.RemoveMerchantContractCommand);
            this.Requests.Add(TestData.CreateEstateCommand);
            this.Requests.Add(TestData.CreateEstateUserCommand);
            this.Requests.Add(TestData.CreateOperatorCommand);
            this.Requests.Add(TestData.RemoveOperatorFromEstateCommand);
            this.Requests.Add(TestData.AddOperatorToEstateCommand);
            this.Requests.Add(TestData.UpdateOperatorCommand);

            this.Requests.Add(TestData.GetMerchantsQuery);
            this.Requests.Add(TestData.GetMerchantQuery);
            this.Requests.Add(TestData.GetMerchantContractsQuery);
            this.Requests.Add(TestData.GetTransactionFeesForProductQuery);
            this.Requests.Add(TestData.GetEstateQuery);
            this.Requests.Add(TestData.GetEstatesQuery);
            this.Requests.Add(TestData.GetOperatorQuery);
            this.Requests.Add(TestData.GetOperatorsQuery);


            //this.Requests.Add(TestData.AddProductToContractRequest);
            //this.Requests.Add(TestData.AddSettledFeeToMerchantStatementRequest);
            //this.Requests.Add(TestData.AddTransactionFeeForProductToContractRequest);
            //this.Requests.Add(TestData.AddTransactionToMerchantStatementRequest);
            //this.Requests.Add(TestData.CreateContractRequest);
            //this.Requests.Add(TestData.DisableTransactionFeeForProductRequest);
            //this.Requests.Add(TestData.EmailMerchantStatementRequest);
            //this.Requests.Add(TestData.GenerateMerchantStatementRequest);
            //this.Requests.Add(TestData.SetMerchantSettlementScheduleRequest);

            Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");
            hostingEnvironment.Setup(he => he.ContentRootPath).Returns("/home");
            hostingEnvironment.Setup(he => he.ApplicationName).Returns("Test Application");

            ServiceRegistry services = new ServiceRegistry();
            Startup s = new Startup(hostingEnvironment.Object);
            Startup.Configuration = this.SetupMemoryConfiguration();

            this.AddTestRegistrations(services, hostingEnvironment.Object);
            services.AddSingleton<IFileSystem, MockFileSystem>();
            s.ConfigureContainer(services);
            Startup.Container.AssertConfigurationIsValid(AssertMode.Full);
            
            this.mediator = Startup.Container.GetService<IMediator>();
        }

        [Fact]
        public async Task Mediator_Send_RequestHandled() {

            List<String> errors = new List<String>();

            foreach (IBaseRequest baseRequest in this.Requests) {
                try {
                    await mediator.Send(baseRequest);
                }
                catch(Exception ex) {
                    errors.Add($"{ex.Message} Request type {baseRequest.GetType()}");
                }
            }

            if (errors.Any() == true) {
                String errorMessage = String.Join(Environment.NewLine, errors);
                throw new Exception(errorMessage);
            }
        }

        private IConfigurationRoot SetupMemoryConfiguration()
        {
            Dictionary<String, String> configuration = new Dictionary<String, String>();

            IConfigurationBuilder builder = new ConfigurationBuilder();

            configuration.Add("ConnectionStrings:HealthCheck", "HeathCheckConnString");
            configuration.Add("SecurityConfiguration:Authority", "https://127.0.0.1");
            configuration.Add("EventStoreSettings:ConnectionString", "esdb://127.0.0.1:2113");
            configuration.Add("EventStoreSettings:ConnectionName", "UnitTestConnection");
            configuration.Add("EventStoreSettings:UserName", "admin");
            configuration.Add("EventStoreSettings:Password", "changeit");
            configuration.Add("AppSettings:UseConnectionStringConfig", "false");
            configuration.Add("AppSettings:SecurityService", "http://127.0.0.1");
            configuration.Add("AppSettings:MessagingServiceApi", "http://127.0.0.1");
            configuration.Add("AppSettings:TransactionProcessorApi", "http://127.0.0.1");
            configuration.Add("AppSettings:DatabaseEngine", "SqlServer");

            builder.AddInMemoryCollection(configuration);

            return builder.Build();
        }

        private void AddTestRegistrations(ServiceRegistry services,
                                          IWebHostEnvironment hostingEnvironment)
        {
            services.AddLogging();
            DiagnosticListener diagnosticSource = new DiagnosticListener(hostingEnvironment.ApplicationName);
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddSingleton<DiagnosticListener>(diagnosticSource);
            services.AddSingleton<IWebHostEnvironment>(hostingEnvironment);
            services.AddSingleton<IHostEnvironment>(hostingEnvironment);
            services.AddSingleton<IConfiguration>(Startup.Configuration);
            
            services.OverrideServices(s => {
                                          s.AddSingleton<IMerchantDomainService, DummyMerchantDomainService>();
                                          s.AddSingleton<IEstateDomainService, DummyEstateDomainService>();
                                          s.AddSingleton<IContractDomainService, DummyContractDomainService>();
                                          s.AddSingleton<IMerchantStatementDomainService, DummyMerchantStatementDomainService>();
                                          s.AddSingleton<IEstateManagementManager, DummyEstateManagementManager>();
                                          s.AddSingleton<IOperatorDomainService, DummyOperatorDomainService>();
            });
        }
    }
}

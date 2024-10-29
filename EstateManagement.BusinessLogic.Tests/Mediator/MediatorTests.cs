using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstateManagement.DataTransferObjects.Responses.Contract;

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
            this.Requests.Add(TestData.Commands.AddMerchantDeviceCommand);
            this.Requests.Add(TestData.Commands.CreateMerchantCommand);
            this.Requests.Add(TestData.Commands.AssignOperatorToMerchantCommand);
            this.Requests.Add(TestData.Commands.AddMerchantContractCommand);
            this.Requests.Add(TestData.Commands.CreateMerchantUserCommand);
            this.Requests.Add(TestData.Commands.MakeMerchantDepositCommand);
            this.Requests.Add(TestData.Commands.MakeMerchantWithdrawalCommand);
            this.Requests.Add(TestData.Commands.SwapMerchantDeviceCommand);
            this.Requests.Add(TestData.GenerateMerchantStatementCommand);
            this.Requests.Add(TestData.Commands.UpdateMerchantCommand);
            this.Requests.Add(TestData.Commands.AddMerchantAddressCommand);
            this.Requests.Add(TestData.Commands.UpdateMerchantAddressCommand);
            this.Requests.Add(TestData.Commands.AddMerchantContactCommand);
            this.Requests.Add(TestData.Commands.UpdateMerchantContactCommand);
            this.Requests.Add(TestData.Commands.RemoveOperatorFromMerchantCommand);
            this.Requests.Add(TestData.Commands.RemoveMerchantContractCommand);
            this.Requests.Add(TestData.Commands.CreateEstateCommand);
            this.Requests.Add(TestData.Commands.CreateEstateUserCommand);
            this.Requests.Add(TestData.Commands.CreateOperatorCommand);
            this.Requests.Add(TestData.Commands.RemoveOperatorFromEstateCommand);
            this.Requests.Add(TestData.Commands.AddOperatorToEstateCommand);
            this.Requests.Add(TestData.Commands.UpdateOperatorCommand);
            this.Requests.Add(TestData.Commands.CreateContractCommand);
            this.Requests.Add(TestData.Commands.AddProductToContractCommand_VariableValue);
            this.Requests.Add(TestData.Commands.AddProductToContractCommand_FixedValue);
            this.Requests.Add(TestData.Commands.AddTransactionFeeForProductToContractCommand(CalculationType.Fixed, FeeType.Merchant));
            this.Requests.Add(TestData.Commands.DisableTransactionFeeForProductCommand);

            this.Requests.Add(TestData.Queries.GetMerchantsQuery);
            this.Requests.Add(TestData.Queries.GetMerchantQuery);
            this.Requests.Add(TestData.Queries.GetMerchantContractsQuery);
            this.Requests.Add(TestData.Queries.GetTransactionFeesForProductQuery);
            this.Requests.Add(TestData.Queries.GetEstateQuery);
            this.Requests.Add(TestData.Queries.GetEstatesQuery);
            this.Requests.Add(TestData.Queries.GetOperatorQuery);
            this.Requests.Add(TestData.Queries.GetOperatorsQuery);
            this.Requests.Add(TestData.Queries.GetContractQuery);
            this.Requests.Add(TestData.Queries.GetContractsQuery);
            this.Requests.Add(TestData.Queries.GetSettlementQuery);
            this.Requests.Add(TestData.Queries.GetSettlementsQuery);

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
            configuration.Add("ConnectionStrings:EstateReportingReadModel", "");
            
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
                                          s.AddSingleton<IReportingManager, DummyReportingManager>();
            });
        }
    }
}

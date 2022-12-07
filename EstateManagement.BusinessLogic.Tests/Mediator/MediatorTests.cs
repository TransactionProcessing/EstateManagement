using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.Mediator
{
    using System.Diagnostics;
    using System.Threading;
    using BusinessLogic.Services;
    using Lamar;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Models;
    using Models.Contract;
    using Moq;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MediatorTests
    {
        private List<IBaseRequest> Requests = new List<IBaseRequest>();
        public MediatorTests() {
            this.Requests.Add(TestData.AddMerchantDeviceRequest);
            this.Requests.Add(TestData.AddOperatorToEstateRequest);
            this.Requests.Add(TestData.AddProductToContractRequest);
            this.Requests.Add(TestData.AddSettledFeeToMerchantStatementRequest);
            this.Requests.Add(TestData.AddTransactionFeeForProductToContractRequest);
            this.Requests.Add(TestData.AddTransactionToMerchantStatementRequest);
            this.Requests.Add(TestData.AssignOperatorToMerchantRequest);
            this.Requests.Add(TestData.CreateContractRequest);
            this.Requests.Add(TestData.CreateEstateRequest);
            this.Requests.Add(TestData.CreateEstateUserRequest);
            this.Requests.Add(TestData.CreateMerchantRequest);
            this.Requests.Add(TestData.CreateMerchantUserRequest);
            this.Requests.Add(TestData.DisableTransactionFeeForProductRequest);
            this.Requests.Add(TestData.EmailMerchantStatementRequest);
            this.Requests.Add(TestData.GenerateMerchantStatementRequest);
            this.Requests.Add(TestData.MakeMerchantDepositRequest);
            this.Requests.Add(TestData.MakeMerchantWithdrawalRequest);
            this.Requests.Add(TestData.SetMerchantSettlementScheduleRequest);
            this.Requests.Add(TestData.SwapMerchantDeviceRequest);
        }

        [Fact]
        public async Task Mediator_Send_RequestHandled() {

            Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");
            hostingEnvironment.Setup(he => he.ContentRootPath).Returns("/home");
            hostingEnvironment.Setup(he => he.ApplicationName).Returns("Test Application");

            ServiceRegistry services = new ServiceRegistry();
            Startup s = new Startup(hostingEnvironment.Object);
            Startup.Configuration = this.SetupMemoryConfiguration();

            this.AddTestRegistrations(services, hostingEnvironment.Object);
            s.ConfigureContainer(services);
            Startup.Container.AssertConfigurationIsValid(AssertMode.Full);

            List<String> errors = new List<String>();
            IMediator mediator = Startup.Container.GetService<IMediator>();
            foreach (IBaseRequest baseRequest in this.Requests) {
                try {
                    await mediator.Send(baseRequest);
                }
                catch(Exception ex) {
                    errors.Add(ex.Message);
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
            configuration.Add("EventStoreSettings:ConnectionString", "https://127.0.0.1:2113");
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
                                          s.AddSingleton<IMerchantDomainService, DummyDomainService>();
                                          s.AddSingleton<IEstateDomainService, DummyEstateDomainService>();
                                          s.AddSingleton<IContractDomainService, DummyContractDomainService>();
                                          s.AddSingleton<IMerchantStatementDomainService, DummyMerchantStatementDomainService>();
                                      });
        }
    }

    public class DummyContractDomainService : IContractDomainService
    {
        public async Task AddProductToContract(Guid productId,
                                               Guid contractId,
                                               String productName,
                                               String displayText,
                                               Decimal? value,
                                               CancellationToken cancellationToken) {
            
        }

        public async Task AddTransactionFeeForProductToContract(Guid transactionFeeId,
                                                                Guid contractId,
                                                                Guid productId,
                                                                String description,
                                                                CalculationType calculationType,
                                                                FeeType feeType,
                                                                Decimal value,
                                                                CancellationToken cancellationToken) {
            
        }

        public async Task DisableTransactionFeeForProduct(Guid transactionFeeId,
                                                          Guid contractId,
                                                          Guid productId,
                                                          CancellationToken cancellationToken) {
            
        }

        public async Task CreateContract(Guid contractId,
                                         Guid estateId,
                                         Guid operatorId,
                                         String description,
                                         CancellationToken cancellationToken) {
            
        }
    }

    public class DummyEstateDomainService : IEstateDomainService
    {
        public async Task CreateEstate(Guid estateId,
                                       String estateName,
                                       CancellationToken cancellationToken) {
            
        }

        public async Task AddOperatorToEstate(Guid estateId,
                                              Guid operatorId,
                                              String operatorName,
                                              Boolean requireCustomMerchantNumber,
                                              Boolean requireCustomTerminalNumber,
                                              CancellationToken cancellationToken) {
            
        }

        public async Task<Guid> CreateEstateUser(Guid estateId,
                                                 String emailAddress,
                                                 String password,
                                                 String givenName,
                                                 String middleName,
                                                 String familyName,
                                                 CancellationToken cancellationToken) {
            return Guid.NewGuid();
        }
    }


    public class DummyDomainService : IMerchantDomainService
    {
        public async Task CreateMerchant(Guid estateId,
                                         Guid merchantId,
                                         String name,
                                         Guid addressId,
                                         String addressLine1,
                                         String addressLine2,
                                         String addressLine3,
                                         String addressLine4,
                                         String town,
                                         String region,
                                         String postalCode,
                                         String country,
                                         Guid contactId,
                                         String contactName,
                                         String contactPhoneNumber,
                                         String contactEmailAddress,
                                         SettlementSchedule settlementSchedule,
                                         DateTime createDateTime,
                                         CancellationToken cancellationToken) {
            
        }

        public async Task AssignOperatorToMerchant(Guid estateId,
                                             Guid merchantId,
                                             Guid operatorId,
                                             String merchantNumber,
                                             String terminalNumber,
                                             CancellationToken cancellationToken) {
            
        }

        public async Task<Guid> CreateMerchantUser(Guid estateId,
                                             Guid merchantId,
                                             String emailAddress,
                                             String password,
                                             String givenName,
                                             String middleName,
                                             String familyName,
                                             CancellationToken cancellationToken) {
            return Guid.NewGuid();

        }

        public async Task AddDeviceToMerchant(Guid estateId,
                                              Guid merchantId,
                                              Guid deviceId,
                                              String deviceIdentifier,
                                              CancellationToken cancellationToken) {
            
        }

        public async Task SwapMerchantDevice(Guid estateId,
                                             Guid merchantId,
                                             Guid deviceId,
                                             String originalDeviceIdentifier,
                                             String newDeviceIdentifier,
                                             CancellationToken cancellationToken) {
            
        }

        public async Task<Guid> MakeMerchantDeposit(Guid estateId,
                                                    Guid merchantId,
                                                    MerchantDepositSource source,
                                                    String reference,
                                                    DateTime depositDateTime,
                                                    Decimal amount,
                                                    CancellationToken cancellationToken) {
            return Guid.NewGuid();
        }

        public async Task<Guid> MakeMerchantWithdrawal(Guid estateId,
                                                       Guid merchantId,
                                                       DateTime withdrawalDateTime,
                                                       Decimal amount,
                                                       CancellationToken cancellationToken) {
            return Guid.NewGuid();
        }

        public async Task SetMerchantSettlementSchedule(Guid estateId,
                                                        Guid merchantId,
                                                        SettlementSchedule settlementSchedule,
                                                        CancellationToken cancellationToken) {
            
        }
    }

    public class DummyMerchantStatementDomainService : IMerchantStatementDomainService{
        public async Task AddTransactionToStatement(Guid estateId,
                                                    Guid merchantId,
                                                    DateTime transactionDateTime,
                                                    Decimal? transactionAmount,
                                                    Boolean isAuthorised,
                                                    Guid transactionId,
                                                    CancellationToken cancellationToken) {
            
        }

        public async Task AddSettledFeeToStatement(Guid estateId,
                                                   Guid merchantId,
                                                   DateTime settledDateTime,
                                                   Decimal settledAmount,
                                                   Guid transactionId,
                                                   Guid settledFeeId,
                                                   CancellationToken cancellationToken) {
            
        }

        public async Task<Guid> GenerateStatement(Guid estateId,
                                                  Guid merchantId,
                                                  DateTime statementDate,
                                                  CancellationToken cancellationToken) {
            return Guid.NewGuid();
        }

        public async Task EmailStatement(Guid estateId,
                                         Guid merchantId,
                                         Guid merchantStatementId,
                                         CancellationToken cancellationToken) {
            
        }
    }
}

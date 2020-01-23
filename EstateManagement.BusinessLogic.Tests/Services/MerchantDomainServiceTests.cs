using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services;
    using EstateAggregate;
    using MerchantAggregate;
    using Moq;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantDomainServiceTests
    {
        [Fact]
        public async Task MerchantDomainService_CreateMerchant_MerchantIsCreated()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.NotThrow( async () =>
                            {
                                await domainService.CreateMerchant(TestData.EstateId,
                                                                   TestData.MerchantId,
                                                                   TestData.MerchantName,
                                                                   TestData.MerchantAddressId,
                                                                   TestData.MerchantAddressLine1,
                                                                   TestData.MerchantAddressLine2,
                                                                   TestData.MerchantAddressLine3,
                                                                   TestData.MerchantAddressLine4,
                                                                   TestData.MerchantTown,
                                                                   TestData.MerchantRegion,
                                                                   TestData.MerchantPostalCode,
                                                                   TestData.MerchantCountry,
                                                                   TestData.MerchantContactId,
                                                                   TestData.MerchantContactName,
                                                                   TestData.MerchantContactPhoneNumber,
                                                                   TestData.MerchantContactEmailAddress,
                                                                   CancellationToken.None);
                            });
        }

        [Fact]
        public void MerchantDomainService_CreateMerchant_EstateNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new 
                MerchantDomainService(aggregateRepositoryManager.Object,securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.CreateMerchant(TestData.EstateId,
                                                   TestData.MerchantId,
                                                   TestData.MerchantName,
                                                   TestData.MerchantAddressId,
                                                   TestData.MerchantAddressLine1,
                                                   TestData.MerchantAddressLine2,
                                                   TestData.MerchantAddressLine3,
                                                   TestData.MerchantAddressLine4,
                                                   TestData.MerchantTown,
                                                   TestData.MerchantRegion,
                                                   TestData.MerchantPostalCode,
                                                   TestData.MerchantCountry,
                                                   TestData.MerchantContactId,
                                                   TestData.MerchantContactName,
                                                   TestData.MerchantContactPhoneNumber,
                                                   TestData.MerchantContactEmailAddress,
                                                   CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AssignOperatorToMerchant_OperatorAssigned()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                         TestData.MerchantId,
                                                         TestData.OperatorId,
                                                         TestData.OperatorMerchantNumber,
                                                         TestData.OperatorTerminalNumber,
                                                         CancellationToken.None);
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_MerchantNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator());

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                                                                     TestData.MerchantId,
                                                                                                     TestData.OperatorId,
                                                                                                     TestData.OperatorMerchantNumber,
                                                                                                     TestData.OperatorTerminalNumber,
                                                                                                     CancellationToken.None);
                                                    });
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_EstateNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                             TestData.MerchantId,
                                                             TestData.OperatorId,
                                                             TestData.OperatorMerchantNumber,
                                                             TestData.OperatorTerminalNumber,
                                                             CancellationToken.None);
            });
        }

        [Fact]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorNotFoundForEstate_ErrorThrown()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                             TestData.MerchantId,
                                                             TestData.OperatorId,
                                                             TestData.OperatorMerchantNumber,
                                                             TestData.OperatorTerminalNumber,
                                                             CancellationToken.None);
            });
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorRequiresMerchantNumber_MerchantNumberNotSet_ErrorThrown(String merchantNumber)
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberFalse));

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                             TestData.MerchantId,
                                                             TestData.OperatorId,
                                                             merchantNumber,
                                                             TestData.OperatorTerminalNumber,
                                                             CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MerchantDomainService_AssignOperatorToMerchant_OperatorRequiresTerminalNumber_TerminalNumberNotSet_ErrorThrown(String terminalNumber)
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await domainService.AssignOperatorToMerchant(TestData.EstateId,
                                                                                                     TestData.MerchantId,
                                                                                                     TestData.OperatorId,
                                                                                                     TestData.OperatorMerchantNumber,
                                                                                                     terminalNumber,
                                                                                                     CancellationToken.None);
                                                    });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_MerchantUserIsCreated()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EstateAggregateWithOperator(TestData.RequireCustomMerchantNumberTrue, TestData.RequireCustomTerminalNumberTrue));

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.NotThrow(async () =>
            {
                await domainService.CreateMerchantUser(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.MerchantUserEmailAddress,
                                                        TestData.MerchantUserPassword,
                                                        TestData.MerchantUserGivenName,
                                                        TestData.MerchantUserMiddleName,
                                                        TestData.MerchantUserFamilyName,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_EstateNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.CreateMerchantUser(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.MerchantUserEmailAddress,
                                                        TestData.MerchantUserPassword,
                                                        TestData.MerchantUserGivenName,
                                                        TestData.MerchantUserMiddleName,
                                                        TestData.MerchantUserFamilyName,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_MerchantNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
            {
                UserId = Guid.NewGuid()
            });

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.CreateMerchantUser(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.MerchantUserEmailAddress,
                                                        TestData.MerchantUserPassword,
                                                        TestData.MerchantUserGivenName,
                                                        TestData.MerchantUserMiddleName,
                                                        TestData.MerchantUserFamilyName,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_CreateMerchantUser_ErrorCreatingUser_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<Exception>(async () =>
            {
                await domainService.CreateMerchantUser(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.MerchantUserEmailAddress,
                                                        TestData.MerchantUserPassword,
                                                        TestData.MerchantUserGivenName,
                                                        TestData.MerchantUserMiddleName,
                                                        TestData.MerchantUserFamilyName,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_MerchantDeviceIsAdded()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.NotThrow(async () =>
            {
                await domainService.AddDeviceToMerchant(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.DeviceId,
                                                        TestData.DeviceIdentifier,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_EstateNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedMerchantAggregate);
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AddDeviceToMerchant(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.DeviceId,
                                                        TestData.DeviceIdentifier,
                                                        CancellationToken.None);
            });
        }

        [Fact]
        public async Task MerchantDomainService_AddDeviceToMerchant_MerchantNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<MerchantAggregate>(It.IsAny<Guid>())).Returns(merchantAggregateRepository.Object);
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            MerchantDomainService domainService = new MerchantDomainService(aggregateRepositoryManager.Object, securityServiceClient.Object);

            Should.Throw<InvalidOperationException>(async () =>
            {
                await domainService.AddDeviceToMerchant(TestData.EstateId,
                                                       TestData.MerchantId,
                                                        TestData.DeviceId,
                                                        TestData.DeviceIdentifier,
                                                        CancellationToken.None);
            });
        }
    }
}

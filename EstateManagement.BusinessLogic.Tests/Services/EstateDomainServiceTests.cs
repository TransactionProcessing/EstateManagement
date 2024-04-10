﻿using System;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services;
    using EstateAggregate;
    using Moq;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateDomainServiceTests
    {
        [Fact]
        public async Task EstateDomainService_CreateEstate_EstateIsCreated()
        {
            Mock<IAggregateRepository<EstateAggregate, DomainEvent>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            estateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EstateAggregate());
            estateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            EstateDomainService domainService = new EstateDomainService(estateAggregateRepository.Object, securityServiceClient.Object);

            Should.NotThrow(async () =>
            {
                await domainService.CreateEstate(TestData.EstateId,
                                                   TestData.EstateName,
                                                   CancellationToken.None);
            });
        }

        [Fact]
        public async Task EstateDomainService_AddOperatorEstate_OperatorIsAdded()
        {
            Mock<IAggregateRepository<EstateAggregate, DomainEvent>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            estateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            estateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();

            EstateDomainService domainService = new EstateDomainService(estateAggregateRepository.Object, securityServiceClient.Object);

            Should.NotThrow(async () =>
                            {
                                await domainService.AddOperatorToEstate(TestData.EstateId,
                                                                 TestData.OperatorId,
                                                                 TestData.OperatorName,
                                                                 TestData.RequireCustomMerchantNumberFalse,
                                                                 TestData.RequireCustomTerminalNumberFalse,
                                                                 CancellationToken.None);
                            });
        }

        [Fact]
        public async Task EstateDomainService_CreateEstateUser_EstateUserIsCreated()
        {
            Mock<IAggregateRepository<EstateAggregate, DomainEvent>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            estateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            estateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CreateUserResponse
                                                                                                                                      {
                                                                                                                                          UserId = Guid.NewGuid()
                                                                                                                                      });

            EstateDomainService domainService = new EstateDomainService(estateAggregateRepository.Object, securityServiceClient.Object);

            Should.NotThrow(async () =>
                            {
                                await domainService.CreateEstateUser(TestData.EstateId,
                                                                        TestData.EstateUserEmailAddress,
                                                                        TestData.EstateUserPassword,
                                                                        TestData.EstateUserGivenName,
                                                                        TestData.EstateUserMiddleName,
                                                                        TestData.EstateUserFamilyName,
                                                                        CancellationToken.None);
                            });
        }
    }
}

﻿using System;
using System.Collections.Generic;
using SecurityService.DataTransferObjects.Responses;
using SimpleResults;

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

    public class EstateDomainServiceTests {
        private EstateDomainService DomainService;
        private Mock<IAggregateRepository<EstateAggregate, DomainEvent>> EstateAggregateRepository;
        private Mock<ISecurityServiceClient> SecurityServiceClient;
        public EstateDomainServiceTests() {
            this.EstateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate, DomainEvent>>();
            this.SecurityServiceClient = new Mock<ISecurityServiceClient>();
            this.DomainService = new EstateDomainService(EstateAggregateRepository.Object, this.SecurityServiceClient.Object);
        }

        [Fact]
        public async Task EstateDomainService_CreateEstate_EstateIsCreated() {
            
            this.EstateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SimpleResults.Result.Success(new EstateAggregate()));
            this.EstateAggregateRepository
                .Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SimpleResults.Result.Success());
            
            Result result = await this.DomainService.CreateEstate(TestData.Commands.CreateEstateCommand, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        
        [Fact]
        public async Task EstateDomainService_AddOperatorEstate_OperatorIsAdded()
        {
            this.EstateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            this.EstateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(SimpleResults.Result.Success());

            Result result = await this.DomainService.AddOperatorToEstate(TestData.Commands.AddOperatorToEstateCommand, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Fact]
        public async Task EstateDomainService_RemoveOperatorFromEstate_OperatorIsRemoved()
        {
            this.EstateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.Aggregates.EstateAggregateWithOperator()));
            this.EstateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            Result result = await this.DomainService.RemoveOperatorFromEstate(TestData.Commands.RemoveOperatorFromEstateCommand, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task EstateDomainService_CreateEstateUser_EstateUserIsCreated() {
            this.EstateAggregateRepository
                .Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(TestData.Aggregates.CreatedEstateAggregate()));
            this.EstateAggregateRepository
                .Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SimpleResults.Result.Success());

            this.SecurityServiceClient
                .Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);
            this.SecurityServiceClient
                .Setup(s => s.GetUsers(It.IsAny<String>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(new List<UserDetails>() {
                    new UserDetails {
                        UserId = Guid.Parse("FA077CE3-B915-4048-88E3-9B500699317F")
                    }
                }));

            Result result = await this.DomainService.CreateEstateUser(TestData.Commands.CreateEstateUserCommand, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        // TODO: EstateDomainServiceTests - CreateEstateUser - failed creating user test
        // TODO: EstateDomainServiceTests - Estate Not Created tests missing
        // TODO: EstateDomainServiceTests - Save Changes failed
    }
}

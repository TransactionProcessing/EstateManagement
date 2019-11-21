using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services;
    using EstateAggregate;
    using Moq;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateDomainServiceTests
    {
        [Fact]
        public async Task EstateDomainService_CreateEstate_EstateIsCreated()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EstateAggregate());
            estateAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<EstateAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            EstateDomainService domainService = new EstateDomainService(aggregateRepositoryManager.Object);

            Should.NotThrow(async () =>
            {
                await domainService.CreateEstate(TestData.EstateId,
                                                   TestData.EstateName,
                                                   CancellationToken.None);
            });
        }
    }
}

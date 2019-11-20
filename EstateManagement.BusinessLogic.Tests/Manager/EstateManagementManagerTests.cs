using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Tests.Manager
{
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using Manger;
    using Models.Factories;
    using Moq;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class EstateManagementManagerTests
    {
        [Fact]
        public async Task EstateManagementManager_GetEstate_EstateIsReturned()
        {
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();
            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);

            Mock<IModelFactory> modelFactory = new Mock<IModelFactory>();
            modelFactory.Setup(m => m.ConvertFrom(It.IsAny<EstateAggregate>())).Returns(TestData.EstateModel);

            Mock<IAggregateRepositoryManager> aggregateRepositoryManager = new Mock<IAggregateRepositoryManager>();
            aggregateRepositoryManager.Setup(x => x.GetAggregateRepository<EstateAggregate>(It.IsAny<Guid>())).Returns(estateAggregateRepository.Object);

            EstateManagementManager estateManagementManager = new EstateManagementManager(aggregateRepositoryManager.Object, modelFactory.Object);

            var estateModel =  await estateManagementManager.GetEstate(TestData.EstateId, CancellationToken.None);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(TestData.EstateModel.EstateId);
            estateModel.Name.ShouldBe(TestData.EstateModel.Name);
        }
    }
}

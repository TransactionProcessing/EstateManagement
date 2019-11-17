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
    using Shared.DomainDrivenDesign.EventStore;
    using Shouldly;
    using Testing;
    using Xunit;

    public class MerchantDomainServiceTests
    {
        [Fact]
        public async Task MerchantDomainService_CreateMerchant_MerchantIsCreated()
        {
            Mock<IAggregateRepository<MerchantAggregate>> merchantAggregateRepository = new Mock<IAggregateRepository<MerchantAggregate>>();
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();

            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.CreatedEstateAggregate);
            MerchantDomainService domainService = new MerchantDomainService(merchantAggregateRepository.Object, estateAggregateRepository.Object);

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
            Mock<IAggregateRepository<EstateAggregate>> estateAggregateRepository = new Mock<IAggregateRepository<EstateAggregate>>();

            merchantAggregateRepository.Setup(m => m.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MerchantAggregate());
            merchantAggregateRepository.Setup(m => m.SaveChanges(It.IsAny<MerchantAggregate>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            estateAggregateRepository.Setup(e => e.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.EmptyEstateAggregate);
            MerchantDomainService domainService = new MerchantDomainService(merchantAggregateRepository.Object, estateAggregateRepository.Object);

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
    }
}

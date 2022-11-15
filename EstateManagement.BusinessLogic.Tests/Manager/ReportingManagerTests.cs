namespace EstateManagement.BusinessLogic.Tests.Manager;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manger;
using Models;
using Moq;
using Repository;
using Shouldly;
using Testing;
using Xunit;

public class ReportingManagerTests
{
    [Fact]
    public async Task ReportingManager_GetSettlement_DataReturned()
    {
        Mock<IEstateReportingRepository> repository = new Mock<IEstateReportingRepository>();
        Mock<IEstateReportingRepositoryForReports> repositoryForReports = new Mock<IEstateReportingRepositoryForReports>();
        repositoryForReports.Setup(r => r.GetSettlement(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(TestData.SettlementModel);

        ReportingManager manager = new ReportingManager(repository.Object, repositoryForReports.Object);

        SettlementModel model = await manager.GetSettlement(TestData.EstateId, TestData.MerchantId, TestData.SettlementId, CancellationToken.None);

        model.ShouldNotBeNull();
        model.ShouldSatisfyAllConditions(p => p.SettlementDate.ShouldBe(TestData.SettlementModel.SettlementDate),
                                         p => p.IsCompleted.ShouldBe(TestData.SettlementModel.IsCompleted),
                                         p => p.NumberOfFeesSettled.ShouldBe(TestData.SettlementModel.NumberOfFeesSettled),
                                         p => p.SettlementId.ShouldBe(TestData.SettlementModel.SettlementId),
                                         p => p.ValueOfFeesSettled.ShouldBe(TestData.SettlementModel.ValueOfFeesSettled));
    }

    [Fact]
    public async Task ReportingManager_GetSettlements_DataReturned()
    {
        Mock<IEstateReportingRepository> repository = new Mock<IEstateReportingRepository>();
        Mock<IEstateReportingRepositoryForReports> repositoryForReports = new Mock<IEstateReportingRepositoryForReports>();
        repositoryForReports.Setup(r => r.GetSettlements(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(TestData.SettlementModels);

        ReportingManager manager = new ReportingManager(repository.Object, repositoryForReports.Object);

        List<SettlementModel> model = await manager.GetSettlements(TestData.EstateId, TestData.MerchantId, TestData.StartDate, TestData.EndDate, CancellationToken.None);

        model.ShouldNotBeNull();
        model.ShouldNotBeEmpty();
        model.ShouldNotBeEmpty();
        model.Count.ShouldBe(TestData.SettlementModels.Count);
    }
}
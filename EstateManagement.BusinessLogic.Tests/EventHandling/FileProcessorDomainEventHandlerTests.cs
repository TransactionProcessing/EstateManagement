namespace EstateManagement.BusinessLogic.Tests.EventHandling;

using System.Threading;
using BusinessLogic.EventHandling;
using FileProcessor.File.DomainEvents;
using FileProcessor.FileImportLog.DomainEvents;
using Moq;
using Repository;
using Shared.Logger;
using Shouldly;
using Testing;
using Xunit;

public class FileProcessorDomainEventHandlerTests
{
    #region Methods

    private Mock<IEstateReportingRepository> EstateReportingRepository;

    private FileProcessorDomainEventHandler DomainEventHandler;
    public FileProcessorDomainEventHandlerTests() {
        Logger.Initialise(NullLogger.Instance);
        this.EstateReportingRepository = new Mock<IEstateReportingRepository>();

        this.DomainEventHandler = new FileProcessorDomainEventHandler(this.EstateReportingRepository.Object);
    }
        
    [Fact]
    public void FileProcessorDomainEventHandler_FileAddedToImportLogEvent_EventIsHandled()
    {
        FileAddedToImportLogEvent domainEvent = TestData.FileAddedToImportLogEvent;
            
        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileCreatedEvent_EventIsHandled()
    {
        FileCreatedEvent domainEvent = TestData.FileCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileLineAddedEvent_EventIsHandled()
    {
        FileLineAddedEvent domainEvent = TestData.FileLineAddedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileLineProcessingFailedEvent_EventIsHandled()
    {
        FileLineProcessingFailedEvent domainEvent = TestData.FileLineProcessingFailedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileLineProcessingIgnoredEvent_EventIsHandled()
    {
        FileLineProcessingIgnoredEvent domainEvent = TestData.FileLineProcessingIgnoredEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileLineProcessingSuccessfulEvent_EventIsHandled()
    {
        FileLineProcessingSuccessfulEvent domainEvent = TestData.FileLineProcessingSuccessfulEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_FileProcessingCompletedEvent_EventIsHandled()
    {
        FileProcessingCompletedEvent domainEvent = TestData.FileProcessingCompletedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    [Fact]
    public void FileProcessorDomainEventHandler_ImportLogCreatedEvent_EventIsHandled()
    {
        ImportLogCreatedEvent domainEvent = TestData.ImportLogCreatedEvent;

        Should.NotThrow(async () => { await this.DomainEventHandler.Handle(domainEvent, CancellationToken.None); });
    }

    #endregion
}
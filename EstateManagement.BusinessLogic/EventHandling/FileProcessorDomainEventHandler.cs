namespace EstateManagement.BusinessLogic.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using FileProcessor.File.DomainEvents;
using FileProcessor.FileImportLog.DomainEvents;
using Repository;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;

public class FileProcessorDomainEventHandler : IDomainEventHandler
{
    #region Fields

    /// <summary>
    /// The estate reporting repository
    /// </summary>
    private readonly IEstateReportingRepository EstateReportingRepository;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MerchantDomainEventHandler" /> class.
    /// </summary>
    /// <param name="estateReportingRepository">The estate reporting repository.</param>
    public FileProcessorDomainEventHandler(IEstateReportingRepository estateReportingRepository)
    {
        this.EstateReportingRepository = estateReportingRepository;
    }

    #endregion

    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task Handle(IDomainEvent domainEvent,
                             CancellationToken cancellationToken)
    {
        await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(ImportLogCreatedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.AddFileImportLog(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileAddedToImportLogEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.AddFileToImportLog(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileCreatedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.AddFile(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileLineAddedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.AddFileLineToFile(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileLineProcessingSuccessfulEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.UpdateFileLine(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileLineProcessingFailedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.UpdateFileLine(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileLineProcessingIgnoredEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.UpdateFileLine(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleSpecificDomainEvent(FileProcessingCompletedEvent domainEvent,
                                                 CancellationToken cancellationToken)
    {
        await this.EstateReportingRepository.UpdateFileAsComplete(domainEvent, cancellationToken);
    }
}
using SimpleResults;

namespace EstateManagement.BusinessLogic.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using Estate.DomainEvents;
using Repository;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;

public class EstateDomainEventHandler : IDomainEventHandler
{
    #region Fields

    /// <summary>
    /// The estate reporting repository
    /// </summary>
    private readonly IEstateReportingRepository EstateReportingRepository;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EstateDomainEventHandler"/> class.
    /// </summary>
    /// <param name="estateReportingRepository">The estate reporting repository.</param>
    public EstateDomainEventHandler(IEstateReportingRepository estateReportingRepository)
    {
        this.EstateReportingRepository = estateReportingRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<Result> Handle(IDomainEvent domainEvent,
                                     CancellationToken cancellationToken)
    {
        return await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(EstateCreatedEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        Result createResult = await this.EstateReportingRepository.CreateReadModel(domainEvent, cancellationToken);
        if (createResult.IsFailed)
            return createResult;

        return await this.EstateReportingRepository.AddEstate(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(SecurityUserAddedToEstateEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.AddEstateSecurityUser(domainEvent, cancellationToken);
    }

    ///// <summary>
    ///// Handles the specific domain event.
    ///// </summary>
    ///// <param name="domainEvent">The domain event.</param>
    ///// <param name="cancellationToken">The cancellation token.</param>
    //private async Task HandleSpecificDomainEvent(OperatorAddedToEstateEvent domainEvent,
    //                                             CancellationToken cancellationToken)
    //{
    //    await this.EstateReportingRepository.AddEstateOperator(domainEvent, cancellationToken);
    //}


    private async Task<Result> HandleSpecificDomainEvent(EstateReferenceAllocatedEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.UpdateEstate(domainEvent, cancellationToken);
    }

    #endregion
}
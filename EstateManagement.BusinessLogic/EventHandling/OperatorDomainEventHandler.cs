using SimpleResults;

namespace EstateManagement.BusinessLogic.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using Estate.DomainEvents;
using EstateManagement.Merchant.DomainEvents;
using Operator.DomainEvents;
using Repository;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;

public class OperatorDomainEventHandler : IDomainEventHandler
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
    public OperatorDomainEventHandler(IEstateReportingRepository estateReportingRepository)
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
                                     CancellationToken cancellationToken){
        Task<Result> t = domainEvent switch{
            OperatorCreatedEvent oce => this.EstateReportingRepository.AddOperator(oce, cancellationToken),
            OperatorNameUpdatedEvent onue => this.EstateReportingRepository.UpdateOperator(onue, cancellationToken),
            OperatorRequireCustomMerchantNumberChangedEvent oprcmnce => this.EstateReportingRepository.UpdateOperator(oprcmnce, cancellationToken),
            OperatorRequireCustomTerminalNumberChangedEvent oprctnce => this.EstateReportingRepository.UpdateOperator(oprctnce, cancellationToken),
            _ => null
        };
        if (t != null)
            return await t;

        return Result.Success();
    }
    
    #endregion
}
﻿using SimpleResults;

namespace EstateManagement.BusinessLogic.EventHandling;

using System.Threading;
using System.Threading.Tasks;
using Contract.DomainEvents;
using Repository;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;

public class ContractDomainEventHandler : IDomainEventHandler
{
    #region Fields

    /// <summary>
    /// The estate reporting repository
    /// </summary>
    private readonly IEstateReportingRepository EstateReportingRepository;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EstateDomainEventHandler" /> class.
    /// </summary>
    /// <param name="estateReportingRepository">The estate reporting repository.</param>
    public ContractDomainEventHandler(IEstateReportingRepository estateReportingRepository)
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
    private async Task<Result> HandleSpecificDomainEvent(ContractCreatedEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.AddContract(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(FixedValueProductAddedToContractEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.AddContractProduct(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(VariableValueProductAddedToContractEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.AddContractProduct(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(TransactionFeeForProductAddedToContractEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.AddContractProductTransactionFee(domainEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<Result> HandleSpecificDomainEvent(TransactionFeeForProductDisabledEvent domainEvent,
                                                         CancellationToken cancellationToken)
    {
        return await this.EstateReportingRepository.DisableContractProductTransactionFee(domainEvent, cancellationToken);
    }

    #endregion
}
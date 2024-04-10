namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests.Merchant;
    using Events;
    using MediatR;
    using Merchant.DomainEvents;
    using MerchantAggregate;
    using MerchantStatement.DomainEvents;
    using Models.Merchant;
    using Newtonsoft.Json;
    using Repository;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using TransactionProcessor.Transaction.DomainEvents;
    using Deposit = CallbackHandler.DataTransferObjects.Deposit;
    using MerchantDepositSource = Models.MerchantDepositSource;

    public class MerchantDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        private readonly IEstateManagementRepository EstateManagementRepository;

        private readonly IMediator Mediator;

        private readonly IAggregateRepository<MerchantAggregate, DomainEvent> MerchantAggregateRepository;

        private readonly IEstateReportingRepository EstateReportingRepository;
        #endregion

        #region Constructors

        public MerchantDomainEventHandler(IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                          IEstateManagementRepository estateManagementRepository,
                                          IEstateReportingRepository estateReportingRepository,
                                          IMediator mediator)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.EstateManagementRepository = estateManagementRepository;
            this.EstateReportingRepository = estateReportingRepository;
            this.Mediator = mediator;
        }

        #endregion

        #region Methods

        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(CallbackReceivedEnrichedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            if (domainEvent.TypeString == typeof(Deposit).ToString())
            {
                // Work out the merchant id from the reference field (second part, split on hyphen)
                String merchantReference = domainEvent.Reference.Split("-")[1];

                Merchant merchant = await this.EstateManagementRepository.GetMerchantFromReference(domainEvent.EstateId, merchantReference, cancellationToken);

                // We now need to deserialise the message from the callback
                Deposit callbackMessage = JsonConvert.DeserializeObject<Deposit>(domainEvent.CallbackMessage);

                MerchantCommands.MakeMerchantDepositCommand command = new(domainEvent.EstateId,
                                                                          merchant.MerchantId,
                                                                          MerchantDepositSource.Automatic,
                                                                          new MakeMerchantDepositRequest{
                                                                                                            DepositDateTime = callbackMessage.DateTime,
                                                                                                            Reference = callbackMessage.Reference,
                                                                                                            Amount = callbackMessage.Amount,
                                                                                                        });
                await this.Mediator.Send(command, cancellationToken);
            }
        }
        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(MerchantCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchant(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(AddressAddedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchantAddress(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ContactAddedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchantContact(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(SecurityUserAddedToMerchantEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchantSecurityUser(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(DeviceAddedToMerchantEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchantDevice(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(OperatorAssignedToMerchantEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddMerchantOperator(domainEvent, cancellationToken);
        }
        
        private async Task HandleSpecificDomainEvent(SettlementScheduleChangedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateMerchant(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(MerchantReferenceAllocatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateMerchant(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateMerchant(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(TransactionHasBeenCompletedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateMerchant(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(ContractAddedToMerchantEvent domainEvent, CancellationToken cancellationToken){
            await this.EstateReportingRepository.AddContractToMerchant(domainEvent, cancellationToken);
        }

        #endregion
    }
}
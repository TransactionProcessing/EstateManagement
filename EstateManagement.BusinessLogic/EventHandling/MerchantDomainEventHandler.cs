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
            Task t = domainEvent switch{
                MerchantCreatedEvent de => this.EstateReportingRepository.AddMerchant(de, cancellationToken),
                MerchantNameUpdatedEvent de => this.EstateReportingRepository.UpdateMerchant(de, cancellationToken),
                AddressAddedEvent de => this.EstateReportingRepository.AddMerchantAddress(de, cancellationToken),
                ContactAddedEvent de => this.EstateReportingRepository.AddMerchantContact(de, cancellationToken),
                SecurityUserAddedToMerchantEvent de => this.EstateReportingRepository.AddMerchantSecurityUser(de, cancellationToken),
                DeviceAddedToMerchantEvent de => this.EstateReportingRepository.AddMerchantDevice(de, cancellationToken),
                OperatorAssignedToMerchantEvent de => this.EstateReportingRepository.AddMerchantOperator(de, cancellationToken),
                SettlementScheduleChangedEvent de => this.EstateReportingRepository.UpdateMerchant(de, cancellationToken),
                MerchantReferenceAllocatedEvent de => this.EstateReportingRepository.UpdateMerchant(de, cancellationToken),
                StatementGeneratedEvent de => this.EstateReportingRepository.UpdateMerchant(de, cancellationToken),
                TransactionHasBeenCompletedEvent de => this.EstateReportingRepository.UpdateMerchant(de, cancellationToken),
                ContractAddedToMerchantEvent de => this.EstateReportingRepository.AddContractToMerchant(de, cancellationToken),
                CallbackReceivedEnrichedEvent de => this.HandleSpecificDomainEvent(de, cancellationToken),
                DeviceSwappedForMerchantEvent de => this.EstateReportingRepository.SwapMerchantDevice(de, cancellationToken),
                OperatorRemovedFromMerchantEvent de => this.EstateReportingRepository.RemoveOperatorFromMerchant(de, cancellationToken),
                MerchantAddressLine1UpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de,cancellationToken),
                MerchantAddressLine2UpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantAddressLine3UpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantAddressLine4UpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantCountyUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantRegionUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantTownUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantPostalCodeUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantAddress(de, cancellationToken),
                MerchantContactNameUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantContact(de, cancellationToken),
                MerchantContactEmailAddressUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantContact(de, cancellationToken),
                MerchantContactPhoneNumberUpdatedEvent de => this.EstateReportingRepository.UpdateMerchantContact(de, cancellationToken),
                _ => null
            };
            if (t != null)
                await t;
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
        
        
        

        #endregion
    }
}
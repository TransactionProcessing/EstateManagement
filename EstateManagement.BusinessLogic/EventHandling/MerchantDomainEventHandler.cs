namespace EstateManagement.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using MediatR;
    using MerchantAggregate;
    using Models;
    using Models.Merchant;
    using Newtonsoft.Json;
    using Repository;
    using Requests;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.Aggregate;
    using Shared.EventStore.EventHandling;
    using Deposit = CallbackHandler.DataTransferObjects.Deposit;

    public class MerchantDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        private readonly IEstateManagementRepository EstateManagementRepository;

        private readonly IMediator Mediator;

        private readonly IAggregateRepository<MerchantAggregate, DomainEvent> MerchantAggregateRepository;

        #endregion

        #region Constructors

        public MerchantDomainEventHandler(IAggregateRepository<MerchantAggregate, DomainEvent> merchantAggregateRepository,
                                          IEstateManagementRepository estateManagementRepository,
                                          IMediator mediator)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.EstateManagementRepository = estateManagementRepository;
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

                MakeMerchantDepositRequest makeMerchantDepositRequest =
                    MakeMerchantDepositRequest.Create(domainEvent.EstateId,
                                                      merchant.MerchantId,
                                                      MerchantDepositSource.Automatic,
                                                      callbackMessage.DepositId.ToString(),
                                                      callbackMessage.DateTime,
                                                      callbackMessage.Amount);

                await this.Mediator.Send(makeMerchantDepositRequest, cancellationToken);
            }
        }

        #endregion
    }
}
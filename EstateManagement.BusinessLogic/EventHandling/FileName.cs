using EstateManagement.Contract.DomainEvents;
using EstateManagement.Estate.DomainEvents;
using EstateManagement.MerchantStatement.DomainEvents;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EventStore.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionProcessor.Settlement.DomainEvents;
using TransactionProcessor.Transaction.DomainEvents;

namespace EstateManagement.BusinessLogic.EventHandling
{
    using FileProcessor.File.DomainEvents;
    using FileProcessor.FileImportLog.DomainEvents;
    using Repository;

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
        private async Task HandleSpecificDomainEvent(ContractCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddContract(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(FixedValueProductAddedToContractEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddContractProduct(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(VariableValueProductAddedToContractEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddContractProduct(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionFeeForProductAddedToContractEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddContractProductTransactionFee(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(TransactionFeeForProductDisabledEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.DisableContractProductTransactionFee(domainEvent, cancellationToken);
        }

        #endregion
    }

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
        private async Task HandleSpecificDomainEvent(EstateCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateReadModel(domainEvent, cancellationToken);

            await this.EstateReportingRepository.AddEstate(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(SecurityUserAddedToEstateEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddEstateSecurityUser(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(OperatorAddedToEstateEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddEstateOperator(domainEvent, cancellationToken);
        }


        private async Task HandleSpecificDomainEvent(EstateReferenceAllocatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.UpdateEstate(domainEvent, cancellationToken);
        }

        #endregion
    }

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

    public class MerchantStatementDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The estate reporting repository
        /// </summary>
        private readonly IEstateReportingRepository EstateReportingRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDomainEventHandler" /> class.
        /// </summary>
        /// <param name="estateReportingRepository">The estate reporting repository.</param>
        public MerchantStatementDomainEventHandler(IEstateReportingRepository estateReportingRepository)
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
        public async Task Handle(IDomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(StatementCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.CreateStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(TransactionAddedToStatementEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddTransactionToStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(SettledFeeAddedToStatementEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.AddSettledFeeToStatement(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(StatementGeneratedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.EstateReportingRepository.MarkStatementAsGenerated(domainEvent, cancellationToken);
        }
        #endregion
    }
}

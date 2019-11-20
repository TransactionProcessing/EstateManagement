namespace EstateManagement.BusinessLogic.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using EstateAggregate;
    using Services;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.EventStore.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.ICommandRouter" />
    public class CommandRouter : ICommandRouter
    {
        #region Fields

        /// <summary>
        /// The aggregate repository manager
        /// </summary>
        private readonly IAggregateRepositoryManager AggregateRepositoryManager;

        /// <summary>
        /// The merchant domain service
        /// </summary>
        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRouter" /> class.
        /// </summary>
        /// <param name="estateAggregateRepositoryManager">The estate aggregate repository.</param>
        /// <param name="merchantDomainService">The merchant domain service.</param>
        public CommandRouter(IAggregateRepositoryManager estateAggregateRepositoryManager,
                             IMerchantDomainService merchantDomainService)
        {
            this.AggregateRepositoryManager = estateAggregateRepositoryManager;
            this.MerchantDomainService = merchantDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Routes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Route(ICommand command,
                                CancellationToken cancellationToken)
        {
            ICommandHandler commandHandler = CreateHandler((dynamic)command);

            await commandHandler.Handle(command, cancellationToken);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateEstateCommand command)
        {
            return new EstateCommandHandler(this.AggregateRepositoryManager);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateMerchantCommand command)
        {
            return new MerchantCommandHandler(this.MerchantDomainService);
        }

        #endregion
    }
}
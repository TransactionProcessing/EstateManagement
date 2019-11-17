namespace EstateManagement.BusinessLogic.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using EstateAggregate;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.DomainDrivenDesign.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.ICommandHandler" />
    public class EstateCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate> EstateAggregateRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCommandHandler"/> class.
        /// </summary>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        public EstateCommandHandler(IAggregateRepository<EstateAggregate> estateAggregateRepository)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Handle(ICommand command,
                                 CancellationToken cancellationToken)
        {
            await this.HandleCommand((dynamic)command, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleCommand(CreateEstateCommand command,
                                         CancellationToken cancellationToken)
        {
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(command.EstateId, cancellationToken);

            estateAggregate.Create(command.Name);

            await this.EstateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
        }

        #endregion
    }
}
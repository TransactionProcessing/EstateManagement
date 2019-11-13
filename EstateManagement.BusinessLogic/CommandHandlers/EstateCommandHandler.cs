namespace EstateManagement.BusinessLogic.CommandHandlers
{
    using System;
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
        private readonly IAggregateRepository<EstateAggregate> EstateAggregateRepository;

        public EstateCommandHandler(IAggregateRepository<EstateAggregate> estateAggregateRepository)
        {
            this.EstateAggregateRepository = estateAggregateRepository;
        }

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

            estateAggregate.Register(command.Name);

            try
            {
                await this.EstateAggregateRepository.SaveChanges(estateAggregate, cancellationToken);
            }
            catch(Exception e)
            {
                throw;
            }
            
        }

        #endregion
    }
}
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
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.ICommandHandler" />
    public class EstateCommandHandler : ICommandHandler
    {
        private readonly IEstateDomainService EstateDomainService;

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCommandHandler" /> class.
        /// </summary>
        /// <param name="estateDomainService">The estate domain service.</param>
        public EstateCommandHandler(IEstateDomainService estateDomainService)
        {
            this.EstateDomainService = estateDomainService;
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
            await this.EstateDomainService.CreateEstate(command.EstateId, command.Name, cancellationToken);
        }

        private async Task HandleCommand(AddOperatorToEstateCommand command, CancellationToken cancellationToken)
        {
            await this.EstateDomainService.AddOperatorToEstate(command.EstateId,
                                                               command.OperatorId,
                                                               command.Name,
                                                               command.RequireCustomMerchantNumber,
                                                               command.RequireCustomTerminalNumber,
                                                               cancellationToken);
        }

        #endregion
    }
}
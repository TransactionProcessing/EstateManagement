namespace EstateManagement.BusinessLogic.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Services;
    using Shared.DomainDrivenDesign.CommandHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.ICommandHandler" />
    public class MerchantCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The merchant domain service
        /// </summary>
        private readonly IMerchantDomainService MerchantDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCommandHandler" /> class.
        /// </summary>
        /// <param name="merchantDomainService">The merchant domain service.</param>
        public MerchantCommandHandler(IMerchantDomainService merchantDomainService)
        {
            this.MerchantDomainService = merchantDomainService;
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
        /// <exception cref="NotImplementedException"></exception>
        private async Task HandleCommand(CreateMerchantCommand command,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.CreateMerchant(command.EstateId,
                                                            command.MerchantId,
                                                            command.Name,
                                                            command.AddressId,
                                                            command.AddressLine1,
                                                            command.AddressLine2,
                                                            command.AddressLine3,
                                                            command.AddressLine4,
                                                            command.Town,
                                                            command.Region,
                                                            command.PostalCode,
                                                            command.Country,
                                                            command.ContactId,
                                                            command.ContactName,
                                                            command.ContactPhoneNumber,
                                                            command.ContactEmailAddress,
                                                            cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleCommand(AssignOperatorToMerchantCommand command,
                                         CancellationToken cancellationToken)
        {
            await this.MerchantDomainService.AssignOperatorToMerchant(command.EstateId,
                                                                      command.MerchantId,
                                                                      command.OperatorId,
                                                                      command.MerchantNumber,
                                                                      command.TerminalNumber,
                                                                      cancellationToken);
        }

        #endregion
    }
}
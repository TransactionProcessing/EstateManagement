namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Guid" />
    /// <seealso cref="DataTransferObjects.Requests.Estate.CreateEstateRequest.String}" />
    /// <seealso cref="AddOperatorToEstateRequest.String}" />
    public class EstateRequestHandler : IRequestHandler<EstateCommands.CreateEstateCommand>,
                                        IRequestHandler<EstateCommands.AddOperatorToEstateCommand>,
                                        IRequestHandler<EstateCommands.CreateEstateUserCommand>
    {
        #region Fields

        /// <summary>
        /// The estate domain service
        /// </summary>
        private readonly IEstateDomainService EstateDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateRequestHandler" /> class.
        /// </summary>
        /// <param name="estateDomainService">The estate domain service.</param>
        public EstateRequestHandler(IEstateDomainService estateDomainService)
        {
            this.EstateDomainService = estateDomainService;
        }

        #endregion

        #region Methods

        public async Task Handle(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken)
        {
            await this.EstateDomainService.CreateEstate(command, cancellationToken);
        }

        public async Task Handle(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken)
        {
            await this.EstateDomainService.CreateEstateUser(command, cancellationToken);
        }

        public async Task Handle(EstateCommands.AddOperatorToEstateCommand command,
                                         CancellationToken cancellationToken)
        {
            await this.EstateDomainService.AddOperatorToEstate(command, cancellationToken);
        }

        #endregion
    }
}
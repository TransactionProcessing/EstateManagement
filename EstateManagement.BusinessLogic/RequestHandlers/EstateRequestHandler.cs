namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.BusinessLogic.Manger;
    using MediatR;
    using Models.Estate;
    using Requests;
    using Services;
    using Shared.Exceptions;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Guid" />
    /// <seealso cref="DataTransferObjects.Requests.Estate.CreateEstateRequest.String}" />
    /// <seealso cref="AddOperatorToEstateRequest.String}" />
    public class EstateRequestHandler : IRequestHandler<EstateCommands.CreateEstateCommand>,
                                        IRequestHandler<EstateCommands.AddOperatorToEstateCommand>,
                                        IRequestHandler<EstateCommands.CreateEstateUserCommand>,
                                        IRequestHandler<EstateQueries.GetEstateQuery, Estate>,
                                        IRequestHandler<EstateQueries.GetEstatesQuery, List<Estate>>
    {
        #region Fields

        /// <summary>
        /// The estate domain service
        /// </summary>
        private readonly IEstateDomainService EstateDomainService;

        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateRequestHandler" /> class.
        /// </summary>
        /// <param name="estateDomainService">The estate domain service.</param>
        public EstateRequestHandler(IEstateDomainService estateDomainService,
                                    IEstateManagementManager estateManagementManager){
            this.EstateDomainService = estateDomainService;
            this.EstateManagementManager = estateManagementManager;
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

        public async Task<Estate> Handle(EstateQueries.GetEstateQuery query, CancellationToken cancellationToken){
            Estate estate = await this.EstateManagementManager.GetEstate(query.EstateId, cancellationToken);
            return estate;
        }

        public async Task<List<Estate>> Handle(EstateQueries.GetEstatesQuery query, CancellationToken cancellationToken){
            List<Estate> estates = await this.EstateManagementManager.GetEstates(query.EstateId, cancellationToken);
            return estates;
        }
    }
}
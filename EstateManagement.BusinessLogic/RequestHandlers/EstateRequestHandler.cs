using SimpleResults;

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
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Guid" />
    /// <seealso cref="DataTransferObjects.Requests.Estate.CreateEstateRequest.String}" />
    /// <seealso cref="AddOperatorToEstateRequest.String}" />
    public class EstateRequestHandler : IRequestHandler<EstateCommands.CreateEstateCommand, Result>,
                                        IRequestHandler<EstateCommands.AddOperatorToEstateCommand, Result>,
                                        IRequestHandler<EstateCommands.RemoveOperatorFromEstateCommand, Result>,
                                        IRequestHandler<EstateCommands.CreateEstateUserCommand, Result>,
                                        IRequestHandler<EstateQueries.GetEstateQuery, Result<Estate>>,
                                        IRequestHandler<EstateQueries.GetEstatesQuery, Result<List<Estate>>>
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

        public async Task<Result> Handle(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken)
        {
            return await this.EstateDomainService.CreateEstate(command, cancellationToken);
        }

        public async Task<Result> Handle(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken)
        {
            return await this.EstateDomainService.CreateEstateUser(command, cancellationToken);
        }

        public async Task<Result> Handle(EstateCommands.AddOperatorToEstateCommand command,
                                         CancellationToken cancellationToken)
        {
            return await this.EstateDomainService.AddOperatorToEstate(command, cancellationToken);
        }

        #endregion

        public async Task<Result<Estate>> Handle(EstateQueries.GetEstateQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetEstate(query.EstateId, cancellationToken);
        }

        public async Task<Result<List<Estate>>> Handle(EstateQueries.GetEstatesQuery query, CancellationToken cancellationToken){
            return await this.EstateManagementManager.GetEstates(query.EstateId, cancellationToken);
        }

        public async Task<Result> Handle(EstateCommands.RemoveOperatorFromEstateCommand command, CancellationToken cancellationToken){
            return await this.EstateDomainService.RemoveOperatorFromEstate(command, cancellationToken);
        }
    }
}
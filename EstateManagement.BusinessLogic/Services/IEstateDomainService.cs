using SimpleResults;

namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Requests;

    public interface IEstateDomainService
    {
        #region Methods

        Task<Result> CreateEstate(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken);

        Task<Result> AddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command, CancellationToken cancellationToken);

        Task<Result> CreateEstateUser(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken);

        Task<Result> RemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
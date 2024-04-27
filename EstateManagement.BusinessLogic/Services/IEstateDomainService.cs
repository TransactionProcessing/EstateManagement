namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Requests;

    public interface IEstateDomainService
    {
        #region Methods

        Task CreateEstate(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken);

        Task AddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command, CancellationToken cancellationToken);

        Task CreateEstateUser(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken);

        Task RemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand command, CancellationToken cancellationToken);

        #endregion
    }
}
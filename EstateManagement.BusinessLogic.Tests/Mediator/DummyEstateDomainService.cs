using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Requests;

public class DummyEstateDomainService : IEstateDomainService
{
    public async Task<Result> CreateEstate(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken) => Result.Success();
        
    public async Task<Result> AddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command, CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> CreateEstateUser(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> RemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand command, CancellationToken cancellationToken) => Result.Success();
}
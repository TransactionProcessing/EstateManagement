namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Requests;

public class DummyEstateDomainService : IEstateDomainService
{
    public async Task CreateEstate(EstateCommands.CreateEstateCommand command, CancellationToken cancellationToken){
        
    }

    public async Task AddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command, CancellationToken cancellationToken) {
    }

    public async Task CreateEstateUser(EstateCommands.CreateEstateUserCommand command, CancellationToken cancellationToken) {
    }

    public async Task RemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand command, CancellationToken cancellationToken){
        
    }
}
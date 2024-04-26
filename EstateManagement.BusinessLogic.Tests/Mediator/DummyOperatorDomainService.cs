namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Requests;

public class DummyOperatorDomainService : IOperatorDomainService{
    public async Task CreateOperator(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken){
        
    }
}
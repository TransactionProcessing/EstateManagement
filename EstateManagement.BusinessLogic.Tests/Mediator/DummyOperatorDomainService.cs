using SimpleResults;

namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Requests;

public class DummyOperatorDomainService : IOperatorDomainService{
    public async Task<Result> CreateOperator(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> UpdateOperator(OperatorCommands.UpdateOperatorCommand command, CancellationToken cancellationToken) => Result.Success();
}
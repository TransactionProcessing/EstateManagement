namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Contract;

    [ExcludeFromCodeCoverage]
    public class CreateContractRequestExample : IExamplesProvider<CreateContractRequest>
    {
        public CreateContractRequest GetExamples()
        {
            return new CreateContractRequest
                   {
                       Description = ExampleData.ContractDescription,
                       OperatorId = ExampleData.OperatorId
                   };
        }
    }
}
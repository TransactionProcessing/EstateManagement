namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

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
namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateContractResponseExample : IExamplesProvider<CreateContractResponse>
    {
        public CreateContractResponse GetExamples()
        {
            return new CreateContractResponse
                   {
                       EstateId = ExampleData.EstateId,
                       OperatorId = ExampleData.OperatorId,
                       ContractId = ExampleData.ContractId
                   };
        }
    }
}
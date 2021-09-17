namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddProductToContractResponseExample : IExamplesProvider<AddProductToContractResponse>
    {
        public AddProductToContractResponse GetExamples()
        {
            return new AddProductToContractResponse
                   {
                       EstateId = ExampleData.EstateId,
                       ContractId = ExampleData.ContractId,
                       ProductId = ExampleData.ProductId
                   };
        }
    }
}
namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

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
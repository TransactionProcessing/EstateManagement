namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Contract;

    [ExcludeFromCodeCoverage]
    public class AddTransactionFeeForProductToContractResponseExample : IExamplesProvider<AddTransactionFeeForProductToContractResponse>
    {
        public AddTransactionFeeForProductToContractResponse GetExamples()
        {
            return new AddTransactionFeeForProductToContractResponse
                   {
                       EstateId = ExampleData.EstateId,
                       ContractId = ExampleData.ContractId,
                       ProductId = ExampleData.ProductId,
                       TransactionFeeId = ExampleData.TransactionFeeId
                   };
        }
    }
}
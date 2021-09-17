namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateMerchantResponseExample : IExamplesProvider<CreateMerchantResponse>
    {
        public CreateMerchantResponse GetExamples()
        {
            return new CreateMerchantResponse
                   {
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId,
                       AddressId = ExampleData.AddressId,
                       ContactId = ExampleData.ContactId
                   };
        }
    }
}
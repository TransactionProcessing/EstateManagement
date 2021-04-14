namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateMerchantUserResponseExample : IExamplesProvider<CreateMerchantUserResponse>
    {
        public CreateMerchantUserResponse GetExamples()
        {
            return new CreateMerchantUserResponse
                   {
                       MerchantId = ExampleData.MerchantId,
                       EstateId = ExampleData.EstateId,
                       UserId = ExampleData.UserId
                   };
        }
    }
}
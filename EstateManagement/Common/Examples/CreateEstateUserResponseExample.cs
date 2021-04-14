namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateEstateUserResponseExample : IExamplesProvider<CreateEstateUserResponse>
    {
        public CreateEstateUserResponse GetExamples()
        {
            return new CreateEstateUserResponse
                   {
                       EstateId = ExampleData.EstateId,
                       UserId = ExampleData.UserId
                   };
        }
    }
}
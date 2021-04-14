namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class SecurityUserResponseExample : IExamplesProvider<SecurityUserResponse>
    {
        public SecurityUserResponse GetExamples()
        {
            return new SecurityUserResponse
                   {
                       EmailAddress = ExampleData.EstateUserEmailAddress,
                       SecurityUserId = ExampleData.UserId
                   };
        }
    }
}
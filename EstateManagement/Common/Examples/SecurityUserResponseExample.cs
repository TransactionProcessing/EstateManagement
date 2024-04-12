namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Estate;

    [ExcludeFromCodeCoverage]
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
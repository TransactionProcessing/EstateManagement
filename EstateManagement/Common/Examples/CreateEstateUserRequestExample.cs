namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Estate;

    [ExcludeFromCodeCoverage]
    public class CreateEstateUserRequestExample : IExamplesProvider<CreateEstateUserRequest>
    {
        public CreateEstateUserRequest GetExamples()
        {
            return new CreateEstateUserRequest
                   {
                       EmailAddress = ExampleData.EstateUserEmailAddress,
                       FamilyName = ExampleData.EstateUserFamilyName,
                       GivenName = ExampleData.EstateUserGivenName,
                       MiddleName = ExampleData.EstateUserMiddleName,
                       Password = ExampleData.EstateUserPassword
                   };
        }
    }
}
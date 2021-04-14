namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

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
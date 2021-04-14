namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateMerchantUserRequestExample : IExamplesProvider<CreateMerchantUserRequest>
    {
        public CreateMerchantUserRequest GetExamples()
        {
            return new CreateMerchantUserRequest
                   {
                       EmailAddress = ExampleData.MerchantUserEmailAddress,
                       FamilyName = ExampleData.EstateUserFamilyName,
                       GivenName = ExampleData.EstateUserGivenName,
                       MiddleName = ExampleData.EstateUserMiddleName,
                       Password = ExampleData.EstateUserPassword
                   };
        }
    }
}
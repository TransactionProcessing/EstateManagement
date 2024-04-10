namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;

    [ExcludeFromCodeCoverage]
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
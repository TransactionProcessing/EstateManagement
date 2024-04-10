namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;

    [ExcludeFromCodeCoverage]
    public class ContactExample : IExamplesProvider<Contact>
    {
        public Contact GetExamples()
        {
            return new Contact
                   {
                       ContactName = ExampleData.ContactName,
                       EmailAddress = ExampleData.ContactEmailAddress,
                       PhoneNumber = ExampleData.ContactPhoneNumber
                   };
        }
    }
}
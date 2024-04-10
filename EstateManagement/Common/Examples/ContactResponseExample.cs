namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Contract;

    [ExcludeFromCodeCoverage]
    public partial class ContactResponseExample : IExamplesProvider<ContactResponse>
    {
        public ContactResponse GetExamples()
        {
            return new ContactResponse
                   {
                       ContactName = ExampleData.ContactName,
                       ContactPhoneNumber = ExampleData.ContactPhoneNumber,
                       ContactEmailAddress = ExampleData.ContactEmailAddress,
                       ContactId = ExampleData.ContactId
                   };
        }
    }
}
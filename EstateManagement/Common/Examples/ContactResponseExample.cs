namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

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
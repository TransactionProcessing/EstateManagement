namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;

    [ExcludeFromCodeCoverage]
    public class CreateMerchantRequestExample : IExamplesProvider<CreateMerchantRequest>
    {
        public CreateMerchantRequest GetExamples()
        {
            return new CreateMerchantRequest
                   {
                       Contact = new Contact
                                 {
                                     EmailAddress = ExampleData.ContactEmailAddress,
                                     PhoneNumber = ExampleData.ContactPhoneNumber,
                                     ContactName = ExampleData.ContactName
                                 },
                       Address = new Address
                                 {
                                     AddressLine1 = ExampleData.AddressLine1,
                                     AddressLine2 = ExampleData.AddressLine2,
                                     AddressLine3 = ExampleData.AddressLine3,
                                     AddressLine4 = ExampleData.AddressLine4,
                                     Country = ExampleData.Country,
                                     PostalCode = ExampleData.PostalCode,
                                     Region = ExampleData.Region,
                                     Town = ExampleData.Town
                                 },
                       Name = ExampleData.MerchantName
                   };
        }
    }
}
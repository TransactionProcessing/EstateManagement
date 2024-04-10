namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddressExample : IExamplesProvider<Address>
    {
        public Address GetExamples()
        {
            return new Address
                   {
                       AddressLine1 = ExampleData.AddressLine1,
                       AddressLine2 = ExampleData.AddressLine2,
                       AddressLine3 = ExampleData.AddressLine3,
                       AddressLine4 = ExampleData.AddressLine4,
                       Country = ExampleData.Country,
                       PostalCode = ExampleData.PostalCode,
                       Region = ExampleData.Region,
                       Town = ExampleData.Town
                   };
        }
    }
}
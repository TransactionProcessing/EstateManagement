namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Merchant;
    using Swashbuckle.AspNetCore.Filters;
    using AddressResponse = DataTransferObjects.Responses.Merchant.AddressResponse;

    [ExcludeFromCodeCoverage]
    public class AddressResponseExample : IExamplesProvider<AddressResponse>
    {
        public AddressResponse GetExamples()
        {
            return new AddressResponse
                   {
                       AddressLine1 = ExampleData.AddressLine1,
                       AddressLine2 = ExampleData.AddressLine2,
                       AddressLine3 = ExampleData.AddressLine3,
                       AddressLine4 = ExampleData.AddressLine4,
                       Country = ExampleData.Country,
                       PostalCode = ExampleData.PostalCode,
                       Region = ExampleData.Region,
                       Town = ExampleData.Town,
                       AddressId = ExampleData.AddressId
                   };
        }
    }
}
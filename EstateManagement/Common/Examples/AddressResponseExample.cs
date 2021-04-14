namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

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
namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Microsoft.Rest;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateEstateResponseExample : IExamplesProvider<CreateEstateResponse>
    {
        public CreateEstateResponse GetExamples()
        {
            return new CreateEstateResponse
                   {
                       EstateId = ExampleData.EstateId
                   };
        }
    }
}
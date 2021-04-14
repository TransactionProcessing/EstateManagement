namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Microsoft.Rest;
    using Swashbuckle.AspNetCore.Filters;

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
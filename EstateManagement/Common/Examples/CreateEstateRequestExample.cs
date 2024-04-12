namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Estate;

    [ExcludeFromCodeCoverage]
    public class CreateEstateRequestExample : IExamplesProvider<CreateEstateRequest>
    {
        public CreateEstateRequest GetExamples()
        {
            return new CreateEstateRequest
                   {
                       EstateId = ExampleData.EstateId,
                       EstateName = ExampleData.EstateName
                   };
        }
    }
}
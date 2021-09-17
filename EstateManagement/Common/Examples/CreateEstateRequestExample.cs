namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

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
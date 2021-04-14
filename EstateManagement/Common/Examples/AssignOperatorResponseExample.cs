namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class AssignOperatorResponseExample : IExamplesProvider<AssignOperatorResponse>
    {
        public AssignOperatorResponse GetExamples()
        {
            return new AssignOperatorResponse
                   {
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId,
                       OperatorId = ExampleData.OperatorId
                   };
        }
    }
}
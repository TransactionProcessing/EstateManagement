namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Operator;

    [ExcludeFromCodeCoverage]
    public class CreateOperatorResponseExample : IExamplesProvider<CreateOperatorResponse>
    {
        public CreateOperatorResponse GetExamples()
        {
            return new CreateOperatorResponse
                   {
                       EstateId = ExampleData.EstateId,
                       OperatorId = ExampleData.OperatorId
                   };
        }
    }

    [ExcludeFromCodeCoverage]
    public class OperatorResponseExample : IExamplesProvider<OperatorResponse>{
        public OperatorResponse GetExamples(){
            return new OperatorResponse{
                                           OperatorId = ExampleData.OperatorId,
                                           Name = ExampleData.OperatorName,
                                           RequireCustomMerchantNumber = true,
                                           RequireCustomTerminalNumber = true
                                       };
        }
    }
}
namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AssignOperatorRequestExample : IExamplesProvider<AssignOperatorRequest>
    {
        public AssignOperatorRequest GetExamples()
        {
            return new AssignOperatorRequest
                   {
                       MerchantNumber = ExampleData.OperatorMerchantNumber,
                       OperatorId = ExampleData.OperatorId,
                       TerminalNumber = ExampleData.OperatorTerminalNumber
                   };
        }
    }
}
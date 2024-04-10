namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;

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
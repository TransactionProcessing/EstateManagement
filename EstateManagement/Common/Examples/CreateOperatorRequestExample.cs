namespace EstateManagement.Common.Examples
{
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Operator;

    [ExcludeFromCodeCoverage]
    public class CreateOperatorRequestExample : IExamplesProvider<CreateOperatorRequest>
    {
        public CreateOperatorRequest GetExamples()
        {
            return new CreateOperatorRequest
                   {
                       Name = ExampleData.OperatorName,
                       RequireCustomMerchantNumber = true,
                       RequireCustomTerminalNumber = true
                   };
        }
    }
}
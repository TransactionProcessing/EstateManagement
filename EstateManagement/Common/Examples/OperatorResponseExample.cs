namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class OperatorResponseExample : IExamplesProvider<EstateOperatorResponse>
    {
        public EstateOperatorResponse GetExamples()
        {
            return new EstateOperatorResponse
                   {
                       OperatorId = ExampleData.OperatorId,
                       Name = ExampleData.OperatorName,
                       RequireCustomMerchantNumber = true,
                       RequireCustomTerminalNumber = true
                   };
        }
    }
}
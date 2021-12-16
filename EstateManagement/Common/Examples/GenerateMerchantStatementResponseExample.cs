namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GenerateMerchantStatementResponseExample : IExamplesProvider<GenerateMerchantStatementResponse>
    {
        public GenerateMerchantStatementResponse GetExamples()
        {
            return new GenerateMerchantStatementResponse
                   {
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId,
                       MerchantStatementId = ExampleData.MerchantStatementId
                   };
        }
    }
}

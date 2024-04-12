namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests.Merchant;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GenerateMerchantStatementRequestExample : IExamplesProvider<GenerateMerchantStatementRequest>
    {
        public GenerateMerchantStatementRequest GetExamples()
        {
            return new GenerateMerchantStatementRequest
                   {
                       MerchantStatementDate = ExampleData.MerchantStatementDateTime
                   };
        }
    }
}

namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Merchant;
    using MerchantOperatorResponse = DataTransferObjects.Responses.Merchant.MerchantOperatorResponse;

    [ExcludeFromCodeCoverage]
    public class MerchantOperatorResponseExample : IExamplesProvider<MerchantOperatorResponse>
    {
        public MerchantOperatorResponse GetExamples()
        {
            return new MerchantOperatorResponse
                   {
                       OperatorId = ExampleData.OperatorId,
                       MerchantNumber = ExampleData.OperatorMerchantNumber,
                       Name = ExampleData.OperatorName,
                       TerminalNumber = ExampleData.OperatorTerminalNumber
                   };
        }
    }
}
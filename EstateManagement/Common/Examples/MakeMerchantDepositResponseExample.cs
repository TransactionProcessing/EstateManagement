namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Merchant;

    [ExcludeFromCodeCoverage]
    public class MakeMerchantDepositResponseExample : IExamplesProvider<MakeMerchantDepositResponse>
    {
        public MakeMerchantDepositResponse GetExamples()
        {
            return new MakeMerchantDepositResponse
                   {
                       DepositId = ExampleData.DepositId,
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId
                   };
        }
    }
}
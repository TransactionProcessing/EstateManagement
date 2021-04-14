namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

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
namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class MerchantBalanceResponseExample : IExamplesProvider<MerchantBalanceResponse>
    {
        public MerchantBalanceResponse GetExamples()
        {
            return new MerchantBalanceResponse
                   {
                       AvailableBalance = ExampleData.AvailableBalance,
                       Balance = ExampleData.Balance,
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId
                   };
        }
    }
}
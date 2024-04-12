using System.Diagnostics.CodeAnalysis;
using EstateManagement.DataTransferObjects.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses.Merchant;

    [ExcludeFromCodeCoverage]
    public class SwapMerchantDeviceResponseExample : IExamplesProvider<SwapMerchantDeviceResponse>
    {
        public SwapMerchantDeviceResponse GetExamples()
        {
            return new SwapMerchantDeviceResponse
            {
                DeviceId = ExampleData.DeviceId,
                EstateId = ExampleData.EstateId,
                MerchantId = ExampleData.MerchantId
            };
        }
    }
}
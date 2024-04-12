namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Merchant;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddMerchantDeviceResponseExample : IExamplesProvider<AddMerchantDeviceResponse>
    {
        public AddMerchantDeviceResponse GetExamples()
        {
            return new AddMerchantDeviceResponse
                   {
                       DeviceId = ExampleData.DeviceId,
                       EstateId = ExampleData.EstateId,
                       MerchantId = ExampleData.MerchantId
                   };
        }
    }
}
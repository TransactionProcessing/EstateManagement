namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

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
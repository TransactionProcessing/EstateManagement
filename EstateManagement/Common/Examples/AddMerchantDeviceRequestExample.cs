namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    public class AddMerchantDeviceRequestExample : IExamplesProvider<AddMerchantDeviceRequest>
    {
        public AddMerchantDeviceRequest GetExamples()
        {
            return new AddMerchantDeviceRequest
                   {
                       DeviceIdentifier = ExampleData.DeviceIdentifier
                   };
        }
    }
}
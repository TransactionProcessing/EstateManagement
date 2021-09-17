namespace EstateManagement.Common.Examples
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
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
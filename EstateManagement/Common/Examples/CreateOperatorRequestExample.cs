namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateOperatorRequestExample : IExamplesProvider<CreateOperatorRequest>
    {
        public CreateOperatorRequest GetExamples()
        {
            return new CreateOperatorRequest
                   {
                       Name = ExampleData.OperatorName,
                       RequireCustomMerchantNumber = true,
                       RequireCustomTerminalNumber = true
                   };
        }
    }
}
namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class EstateResponseExample : IExamplesProvider<EstateResponse>
    {
        public EstateResponse GetExamples()
        {
            return new EstateResponse
                   {
                       EstateId = ExampleData.EstateId,
                       EstateName = ExampleData.EstateName,
                       Operators = new List<EstateOperatorResponse>
                                   {
                                       new EstateOperatorResponse
                                       {
                                           OperatorId = ExampleData.OperatorId,
                                           Name = ExampleData.OperatorName,
                                           RequireCustomMerchantNumber = true,
                                           RequireCustomTerminalNumber = true
                                       }
                                   },
                       SecurityUsers = new List<SecurityUserResponse>
                                       {
                                           new SecurityUserResponse
                                           {
                                               EmailAddress = ExampleData.EstateUserEmailAddress,
                                               SecurityUserId = ExampleData.UserId
                                           }
                                       }
                   };
        }
    }
}
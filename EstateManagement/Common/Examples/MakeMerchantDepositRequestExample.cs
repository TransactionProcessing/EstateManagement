namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MakeMerchantDepositRequestExample : IMultipleExamplesProvider<MakeMerchantDepositRequest>
    {
        public IEnumerable<SwaggerExample<MakeMerchantDepositRequest>> GetExamples()
        {
            SwaggerExample<MakeMerchantDepositRequest> manualDeposit = new SwaggerExample<MakeMerchantDepositRequest>
                                                                       {
                                                                           Name = "Merchant Deposit",
                                                                           Value = new MakeMerchantDepositRequest
                                                                                   {
                                                                                       Amount = ExampleData.DepositAmount,
                                                                                       DepositDateTime = ExampleData.DepositDateTime,
                                                                                       Reference = ExampleData.DepositReference
                                                                                   }
                                                                      };

            return new List<SwaggerExample<MakeMerchantDepositRequest>>
                   {
                       manualDeposit
                   };
        }
    }
}
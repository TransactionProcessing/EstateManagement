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
                                                                           Name = "Manual Merchant Deposit",
                                                                           Value = new MakeMerchantDepositRequest
                                                                                   {
                                                                                       Amount = ExampleData.DepositAmount,
                                                                                       DepositDateTime = ExampleData.DepositDateTime,
                                                                                       Reference = ExampleData.DepositReference,
                                                                                       Source = ExampleData.MerchantDepositSourceManual
                                                                                   }
                                                                       };

            SwaggerExample<MakeMerchantDepositRequest> automaticDeposit = new SwaggerExample<MakeMerchantDepositRequest>
                                                                          {
                                                                              Name = "Automatic Merchant Deposit",
                                                                              Value = new MakeMerchantDepositRequest
                                                                                      {
                                                                                          Amount = ExampleData.DepositAmount,
                                                                                          DepositDateTime = ExampleData.DepositDateTime,
                                                                                          Reference = ExampleData.DepositReference,
                                                                                          Source = ExampleData.MerchantDepositSourceAutomatic
                                                                                      }
                                                                          };

            return new List<SwaggerExample<MakeMerchantDepositRequest>>
                   {
                       manualDeposit,
                       automaticDeposit
                   };
        }
    }
}
namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddTransactionFeeForProductToContractRequestExample : IMultipleExamplesProvider<AddTransactionFeeForProductToContractRequest>
    {
        public IEnumerable<SwaggerExample<AddTransactionFeeForProductToContractRequest>> GetExamples()
        {
            var merchantFixedFee = new SwaggerExample<AddTransactionFeeForProductToContractRequest>
                                   {
                                       Value = new AddTransactionFeeForProductToContractRequest
                                               {
                                                   Value = 0.5m,
                                                   CalculationType = CalculationType.Fixed,
                                                   Description = ExampleData.MerchantFixedFeeDescription,
                                                   FeeType = FeeType.Merchant
                                               },
                                       Name = "Merchant Fixed Fee"
                                   };

            var merchantPercentageFee = new SwaggerExample<AddTransactionFeeForProductToContractRequest>
                                        {
                                            Value = new AddTransactionFeeForProductToContractRequest
                                                    {
                                                        Value = 0.05m,
                                                        CalculationType = CalculationType.Percentage,
                                                        Description = ExampleData.MerchantPercentageFeeDescription,
                                                        FeeType = FeeType.Merchant
                                                    },
                                            Name = "Merchant Percentage Fee"
                                        };

            var serviceProviderFixedFee = new SwaggerExample<AddTransactionFeeForProductToContractRequest>
                                          {
                                              Value = new AddTransactionFeeForProductToContractRequest
                                                      {
                                                          Value = 0.5m,
                                                          CalculationType = CalculationType.Fixed,
                                                          Description = ExampleData.ServiceProviderFixedFeeDescription,
                                                          FeeType = FeeType.Merchant
                                                      },
                                              Name = "Service Provider Fixed Fee"
                                          };

            var serviceProviderPercentageFee = new SwaggerExample<AddTransactionFeeForProductToContractRequest>
                                               {
                                                   Value = new AddTransactionFeeForProductToContractRequest
                                                           {
                                                               Value = 0.05m,
                                                               CalculationType = CalculationType.Percentage,
                                                               Description = ExampleData.ServiceProviderPercentageFeeDescription,
                                                               FeeType = FeeType.Merchant
                                                           },
                                                   Name = "Service Provider Percentage Fee"
                                               };

            return new List<SwaggerExample<AddTransactionFeeForProductToContractRequest>>
                   {
                       merchantFixedFee,
                       merchantPercentageFee,
                       serviceProviderFixedFee,
                       serviceProviderPercentageFee
                   };
        }
    }
}
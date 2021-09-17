namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ContractResponseExample : IExamplesProvider<ContractResponse>
    {
        public ContractResponse GetExamples()
        {
            return new ContractResponse
                   {
                       EstateId = ExampleData.EstateId,
                       Description = ExampleData.ContractDescription,
                       OperatorId = ExampleData.OperatorId,
                       ContractId = ExampleData.ContractId,
                       OperatorName = ExampleData.OperatorName,
                       Products = new List<ContractProduct>
                                  {
                                      new ContractProduct
                                      {
                                          Value = ExampleData.ProductValue,
                                          Name = ExampleData.ProductName,
                                          ProductId = ExampleData.ProductId,
                                          DisplayText = ExampleData.ProductDisplayText,
                                          TransactionFees = new List<ContractProductTransactionFee>
                                                            {
                                                                new ContractProductTransactionFee
                                                                {
                                                                    Description = ExampleData.MerchantFixedFeeDescription,
                                                                    TransactionFeeId = ExampleData.TransactionFeeId,
                                                                    Value = 0.05m,
                                                                    CalculationType = CalculationType.Fixed,
                                                                    FeeType = FeeType.Merchant
                                                                }
                                                            }
                                      }
                                  }
                   };
        }
    }
}
namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Contract;

    [ExcludeFromCodeCoverage]
    public class ContractProductResponseExample : IExamplesProvider<ContractProduct>
    {
        public ContractProduct GetExamples()
        {
            return new ContractProduct
                   {
                       Name = ExampleData.ProductName,
                       ProductId = ExampleData.ProductId,
                       Value = ExampleData.ProductValue,
                       DisplayText = ExampleData.ProductDisplayText,
                       TransactionFees = new List<ContractProductTransactionFee>
                                         {
                                             new ContractProductTransactionFee
                                             {
                                                 TransactionFeeId = ExampleData.TransactionFeeId,
                                                 Description = ExampleData.MerchantFixedFeeDescription,
                                                 Value = 0.05m,
                                                 CalculationType = CalculationType.Fixed,
                                                 FeeType = FeeType.Merchant
                                             }
                                         }
                   };
        }
    }
}
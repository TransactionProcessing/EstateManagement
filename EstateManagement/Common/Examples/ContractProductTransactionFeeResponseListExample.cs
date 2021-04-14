namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class ContractProductTransactionFeeResponseListExample : IExamplesProvider<List<ContractProductTransactionFee>>
    {
        public List<ContractProductTransactionFee> GetExamples()
        {
            return new List<ContractProductTransactionFee>
                   {

                       new ContractProductTransactionFee
                       {
                           Value = 0.05m,
                           Description = ExampleData.MerchantFixedFeeDescription,
                           TransactionFeeId = ExampleData.TransactionFeeId,
                           CalculationType = CalculationType.Fixed,
                           FeeType = FeeType.Merchant
                       }
                   };
        }
    }
}
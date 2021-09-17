namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MerchantBalanceHistoryResponseExample : IMultipleExamplesProvider<MerchantBalanceHistoryResponse>
    {
        public IEnumerable<SwaggerExample<MerchantBalanceHistoryResponse>> GetExamples()
        {
            var balanceHistoryIn = new SwaggerExample<MerchantBalanceHistoryResponse>
                                   {
                                       Name = "Merchant Balance In",
                                       Value = new MerchantBalanceHistoryResponse
                                               {
                                                   EstateId = ExampleData.EstateId,
                                                   MerchantId = ExampleData.MerchantId,
                                                   Balance = ExampleData.Balance,
                                                   Reference = ExampleData.BalanceHistoryReference,
                                                   ChangeAmount = ExampleData.BalanceHistoryChangeAmount,
                                                   EntryDateTime = ExampleData.BalanceHistoryEntryDateTime,
                                                   EntryType = ExampleData.BalanceHistoryEntryTypeCredit,
                                                   EventId = ExampleData.EventId,
                                                   In = ExampleData.BalanceHistoryChangeAmount,
                                                   Out = null,
                                                   TransactionId = ExampleData.BalanceHistoryTransactionId
                                               }
                                   };

            var balanceHistoryOut = new SwaggerExample<MerchantBalanceHistoryResponse>
                                    {
                                        Name = "Merchant Balance Out",
                                        Value = new MerchantBalanceHistoryResponse
                                                {
                                                    EstateId = ExampleData.EstateId,
                                                    MerchantId = ExampleData.MerchantId,
                                                    Balance = ExampleData.Balance,
                                                    Reference = ExampleData.BalanceHistoryReference,
                                                    ChangeAmount = ExampleData.BalanceHistoryChangeAmount,
                                                    EntryDateTime = ExampleData.BalanceHistoryEntryDateTime,
                                                    EntryType = ExampleData.BalanceHistoryEntryTypeDebit,
                                                    EventId = ExampleData.EventId,
                                                    In = null,
                                                    Out = ExampleData.BalanceHistoryChangeAmount,
                                                    TransactionId = ExampleData.BalanceHistoryTransactionId
                                                }

                                    };

            return new List<SwaggerExample<MerchantBalanceHistoryResponse>>
                   {
                       balanceHistoryIn,
                       balanceHistoryOut
                   };


        }
    }
}
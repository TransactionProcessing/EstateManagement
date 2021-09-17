namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class MerchantBalanceHistoryResponseListExample : IExamplesProvider<List<MerchantBalanceHistoryResponse>>
    {
        public List<MerchantBalanceHistoryResponse> GetExamples()
        {
            var balanceHistoryIn = new MerchantBalanceHistoryResponse
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
                                   };

            var balanceHistoryOut = new MerchantBalanceHistoryResponse
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
                                    };

            return new List<MerchantBalanceHistoryResponse>
                   {
                       balanceHistoryIn,
                       balanceHistoryOut
                   };


        }
    }
}
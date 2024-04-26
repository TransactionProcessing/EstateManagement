namespace EstateManagement.IntegrationTesting.Helpers;

public static class SubscriptionsHelper
{
    public static List<(String streamName, String groupName, Int32 maxRetries)> GetSubscriptions()
    {
        List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new(){
                                                                                               ("$ce-CallbackMessageAggregate", "Estate Management", 0),
                                                                                               ("$ce-ContractAggregate", "Estate Management", 0),
                                                                                               ("$ce-EstateAggregate", "Estate Management", 0),
                                                                                               ("$ce-EstateAggregate", "Estate Management - Ordered", 0),
                                                                                               ("$ce-FileAggregate", "Estate Management", 0),
                                                                                               ("$ce-FloatAggregate", "Estate Management", 0),
                                                                                               ("$ce-MerchantAggregate", "Estate Management", 0),
                                                                                               ("$ce-MerchantStatementAggregate", "Estate Management", 0),
                                                                                               ("$ce-MerchantStatementAggregate", "Estate Management - Ordered", 0),
                                                                                               ("$ce-ReconciliationAggregate", "Estate Management", 0),
                                                                                               ("$ce-SettlementAggregate", "Estate Management", 0),
                                                                                               ("$ce-TransactionAggregate", "Estate Management", 0),
                                                                                               ("$ce-TransactionAggregate", "Estate Management - Ordered", 0),
                                                                                               ("$ce-VoucherAggregate", "Estate Management", 0),
                                                                                               ("$ce-OperatorAggregate", "Estate Management", 0),
                                                                                           };
        return subscriptions;
    }
}
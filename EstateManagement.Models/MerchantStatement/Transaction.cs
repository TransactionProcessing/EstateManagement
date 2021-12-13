namespace EstateManagement.Models.MerchantStatement
{
    using System;

    public class Transaction
    {
        public Guid TransactionId { get; set; }

        public DateTime DateTime { get; set; }
        
        public Decimal Amount { get; set; }

        public Guid OperatorId { get; set; }
    }
}
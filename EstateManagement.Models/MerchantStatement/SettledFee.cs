namespace EstateManagement.Models.MerchantStatement
{
    using System;

    public class SettledFee
    {
        public DateTime DateTime { get; set; }

        public Decimal Amount { get; set; }

        public Guid TransactionId { get; set; }
        public Guid SettledFeeId { get; set; }
    }
}
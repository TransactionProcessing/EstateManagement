namespace EstateManagement.Models.MerchantStatement
{
    using System;

    public class StatementLine
    {
        public DateTime DateTime { get; set; }

        public String Description { get; set; }

        public Decimal Amount { get; set; }

        // TODO: maybe make this an enum type
        public Int32 LineType { get; set; } 
    }
}
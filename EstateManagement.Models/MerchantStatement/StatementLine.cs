namespace EstateManagement.Models.MerchantStatement
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class StatementLine
    {
        #region Properties

        public String StatementLineAmountDisplay { get; set; }

        public Decimal StatementLineAmount { get; set; }

        public String StatementLineDate { get; set; }

        public String StatementLineDescription { get; set; }

        public String StatementLineNumber { get; set; }

        #endregion
    }
}
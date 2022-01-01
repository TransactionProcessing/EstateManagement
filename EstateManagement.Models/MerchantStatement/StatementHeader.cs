namespace EstateManagement.Models.MerchantStatement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class StatementHeader
    {
        #region Properties

        public String EstateName { get; set; }

        public String MerchantAddressLine1 { get; set; }

        public String MerchantContactNumber { get; set; }

        public String MerchantCountry { get; set; }

        public String MerchantEmail { get; set; }

        public String MerchantName { get; set; }

        public String MerchantPostcode { get; set; }

        public String MerchantRegion { get; set; }

        public String MerchantTown { get; set; }

        public String StatementDate { get; set; }

        public String StatementId { get; set; }

        public List<StatementLine> StatementLines { get; set; }

        public String StatementTotal { get; set; }

        public String TransactionFeesValue { get; set; }

        public String TransactionsValue { get; set; }

        #endregion
    }
}
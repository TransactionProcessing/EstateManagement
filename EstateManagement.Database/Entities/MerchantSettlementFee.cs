namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("merchantsettlementfee")]
    public class MerchantSettlementFee
    {
        #region Properties

        public Decimal CalculatedValue { get; set; }

        public DateTime FeeCalculatedDateTime { get; set; }

        public Int32 TransactionFeeReportingId { get; set; }

        public Decimal FeeValue { get; set; }

        public Boolean IsSettled { get; set; }

        public Int32 MerchantReportingId { get; set; }

        public Int32 SettlementReportingId { get; set; }

        public Int32 TransactionReportingId { get; set; }

        #endregion
    }
}
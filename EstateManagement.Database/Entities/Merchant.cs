namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchant")]
    public class Merchant
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }

        public Int32 EstateReportingId { get; set; }

        public Guid MerchantId { get; set; }

        public String Name { get; set; }

        public Int32 SettlementSchedule { get; set; }

        public String? Reference { get; set; }

        public DateTime LastStatementGenerated { get; set; }

        #endregion
    }
}
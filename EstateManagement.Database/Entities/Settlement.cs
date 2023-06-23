namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("settlement")]
    public class Settlement
    {
        #region Properties

        public Int32 EstateReportingId { get; set; }

        public Boolean IsCompleted { get; set; }

        public DateTime SettlementDate { get; set; }

        public Guid SettlementId { get; set; }

        #endregion
    }
}
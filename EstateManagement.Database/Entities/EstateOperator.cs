namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("estateoperator")]
    public class EstateOperator
    {
        #region Properties

        public Int32 EstateReportingId { get; set; }

        public String Name { get; set; }

        public Guid OperatorId { get; set; }

        public Boolean RequireCustomMerchantNumber { get; set; }

        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }
}
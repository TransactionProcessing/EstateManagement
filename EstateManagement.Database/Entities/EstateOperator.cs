namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("estateoperator")]
    public class EstateOperator
    {
        #region Properties

        public Int32 OperatorReportingId { get; set; }

        public Int32 EstateReportingId { get; set; }

        #endregion
    }
}
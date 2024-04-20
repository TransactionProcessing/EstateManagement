namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantoperator")]
    public class MerchantOperator
    {
        #region Properties
        public Int32 MerchantReportingId { get; set; }

        public String? MerchantNumber { get; set; }

        public String Name { get; set; }

        public Guid OperatorId { get; set; }

        public String? TerminalNumber { get; set; }

        public Boolean IsDeleted { get; set; }

        #endregion
    }
}
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
        public Guid MerchantId { get; set; }

        public String? MerchantNumber { get; set; }

        public String Name { get; set; }

        public Guid OperatorId { get; set; }

        public String? TerminalNumber { get; set; }

        #endregion
    }
}
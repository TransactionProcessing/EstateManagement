namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("transactionfee")]
    public class TransactionFee
    {
        #region Properties

        public Decimal CalculatedValue { get; set; }

        public Int32 CalculationType { get; set; }

        public Guid EventId { get; set; }
        public Guid FeeId { get; set; }

        public Int32 FeeType { get; set; }

        public Decimal FeeValue { get; set; }

        public Guid TransactionId { get; set; }

        #endregion
    }
}
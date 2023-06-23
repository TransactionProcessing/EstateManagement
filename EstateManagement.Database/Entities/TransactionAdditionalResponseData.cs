namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("transactionadditionalresponsedata")]
    public class TransactionAdditionalResponseData
    {
        #region Properties

        public Guid MerchantId { get; set; }

        [Key]
        public Guid TransactionId { get; set; }

        #endregion
    }
}
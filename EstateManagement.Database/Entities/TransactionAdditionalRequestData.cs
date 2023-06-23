namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("transactionadditionalrequestdata")]
    public class TransactionAdditionalRequestData
    {
        #region Properties

        public String? Amount { get; set; }


        public String? CustomerAccountNumber { get; set; }

        public Guid MerchantId { get; set; }

        [Key]
        public Guid TransactionId { get; set; }

        #endregion
    }
}
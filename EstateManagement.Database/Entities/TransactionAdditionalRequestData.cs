namespace EstateManagement.Database.Entities
{
    using System;
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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TransactionId { get; set; }

        #endregion
    }
}
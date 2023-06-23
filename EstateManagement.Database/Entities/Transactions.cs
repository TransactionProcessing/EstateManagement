﻿namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("transaction")]
    public class Transaction
    {
        #region Properties
        
        public String? AuthorisationCode { get; set; }
        
        public Guid ContractId { get; set; }
        
        public String? DeviceIdentifier { get; set; }
        
        public Boolean IsAuthorised { get; set; }
        
        public Boolean IsCompleted { get; set; }
        
        public Guid MerchantId { get; set; }

        public String? OperatorIdentifier { get; set; }
        
        public Guid ProductId { get; set; }
        
        public String? ResponseCode { get; set; }
        
        public String? ResponseMessage { get; set; }
        
        public DateTime TransactionDate { get; set; }
        
        public DateTime TransactionDateTime { get; set; }
        
        [Key]
        public Guid TransactionId { get; set; }
        
        public String? TransactionNumber { get; set; }
        
        public String? TransactionReference { get; set; }
        
        public TimeSpan TransactionTime { get; set; }
        
        public String? TransactionType { get; set; }

        public Int32 TransactionSource { get; set; }

        #endregion
    }
}
﻿namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("voucher")]
    public class Voucher
    {
        #region Properties
        
        public DateTime ExpiryDate { get; set; }
        public DateTime ExpiryDateTime { get; set; }

        public Boolean IsGenerated { get; set; }
        
        public Boolean IsIssued { get; set; }

        public Boolean IsRedeemed { get; set; }

        public String? OperatorIdentifier { get; set; }

        public String? RecipientEmail { get; set; }

        public String? RecipientMobile { get; set; }

        public Decimal Value { get; set; }

        public String VoucherCode { get; set; }

        public Guid VoucherId { get; set; }

        public Guid TransactionId { get; set; }

        public DateTime GenerateDateTime { get; set; }

        public DateTime IssuedDateTime { get; set; }

        public DateTime RedeemedDateTime { get; set; }

        public DateTime GenerateDate { get; set; }

        public DateTime IssuedDate { get; set; }

        public DateTime RedeemedDate { get; set; }

        #endregion
    }
}
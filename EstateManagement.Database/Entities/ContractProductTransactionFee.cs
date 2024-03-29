﻿namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("contractproducttransactionfee")]
    public class ContractProductTransactionFee
    {
        #region Properties

        public Int32 ContractProductReportingId { get; set; }

        public Int32 CalculationType { get; set; }

        public Int32 FeeType { get; set; }

        public Boolean IsEnabled { get; set; }

        public Guid TransactionFeeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 TransactionFeeReportingId { get; set; }

        public String Description { get; set; }

        public Decimal Value { get; set; }

        #endregion
    }
}
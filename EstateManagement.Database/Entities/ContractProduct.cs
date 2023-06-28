﻿namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("contractproduct")]
    public class ContractProduct
    {
        #region Properties

        public Int32 ContractReportingId { get; set; }

        public String DisplayText { get; set; }
        
        public Guid ProductId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 ContractProductReportingId { get; set; }

        public String ProductName { get; set; }
        
        public Decimal? Value { get; set; }

        public Int32 ProductType { get; set; }

        #endregion
    }
}
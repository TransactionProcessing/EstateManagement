namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("contractproduct")]
    public class ContractProduct
    {
        #region Properties

        public Guid ContractId { get; set; }

        public String DisplayText { get; set; }

        public Guid EstateId { get; set; }
        
        public Guid ProductId { get; set; }
        
        public String ProductName { get; set; }
        
        public Decimal? Value { get; set; }

        public Int32 ProductType { get; set; }

        #endregion
    }
}
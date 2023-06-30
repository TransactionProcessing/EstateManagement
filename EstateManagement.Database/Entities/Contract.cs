namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("contract")]
    public class Contract
    {
        #region Properties
        
        public Guid ContractId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 ContractReportingId { get; set; }

        public Int32 EstateReportingId { get; set; }

        public Guid OperatorId { get; set; }

        public String Description { get; set; }
        #endregion
    }
}
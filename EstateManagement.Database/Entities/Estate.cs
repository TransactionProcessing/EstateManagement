namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("estate")]
    public class Estate
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }
        
        public Guid EstateId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 EstateReportingId { get; set; }

        public String Name { get; set; }

        public String? Reference { get; set; }

        #endregion
    }
}
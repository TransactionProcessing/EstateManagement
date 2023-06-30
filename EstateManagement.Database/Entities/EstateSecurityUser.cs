namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("estatesecurityuser")]
    public class EstateSecurityUser
    {
        #region Properties
        public DateTime CreatedDateTime { get; set; }

        public String EmailAddress { get; set; }

        public Int32 EstateReportingId { get; set; }
        
        public Guid SecurityUserId { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int32 EstateSecurityUserReportingId { get; set; }

        #endregion
    }
}
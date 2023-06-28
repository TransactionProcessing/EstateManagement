namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantsecurityuser")]
    public class MerchantSecurityUser
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }

        public String EmailAddress { get; set; }

        public Int32 MerchantReportingId { get; set; }

        public Guid SecurityUserId { get; set; }

        #endregion
    }
}
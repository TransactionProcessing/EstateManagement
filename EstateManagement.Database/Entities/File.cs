namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("file")]
    public class File
    {
        public Int32 EstateReportingId { get; set; }

        public Guid FileId { get; set; }

        public Guid FileImportLogId { get; set; }

        public string FileLocation { get; set; }

        public Guid FileProfileId { get; set; }

        public DateTime FileReceivedDateTime { get; set; }

        public Guid MerchantId { get; set; }

        public Guid UserId { get; set; }

        public Boolean IsCompleted { get; set; }
    }
}
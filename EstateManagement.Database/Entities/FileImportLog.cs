namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("fileimportlog")]
    public class FileImportLog
    {
        public Int32 EstateReportingId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 FileImportLogReportingId { get; set; }

        public Guid FileImportLogId { get; set; }

        public DateTime ImportLogDateTime { get; set; }
    }
}

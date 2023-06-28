namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("fileimportlogfile")]
    public class FileImportLogFile
    {
        public Int32 FileReportingId { get; set; }

        public Int32 FileImportLogReportingId { get; set; }

        public string FilePath { get; set; }

        public Guid FileProfileId { get; set; }

        public DateTime FileUploadedDateTime { get; set; }

        public Int32 MerchantReportingId { get; set; }

        public string OriginalFileName { get; set; }
        
        public Guid UserId { get; set; }
    }
}
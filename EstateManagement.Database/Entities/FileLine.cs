namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("fileline")]
    public class FileLine
    {
        public string FileLineData { get; set; }

        public int LineNumber { get; set; }

        public string Status { get; set; } // Success/Failed/Ignored (maybe first char?)

        public Int32 TransactionReportingId { get; set; }

        public Int32 FileReportingId { get; set; }
    }
}
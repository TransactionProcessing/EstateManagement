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
        public Guid FileId { get; set; }
        
        public string FileLineData { get; set; }

        public int LineNumber { get; set; }

        public string Status { get; set; } // Success/Failed/Ignored (maybe first char?)

        public Guid TransactionId { get; set; }
    }
}
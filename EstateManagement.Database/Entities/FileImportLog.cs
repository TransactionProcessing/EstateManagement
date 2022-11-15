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
        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the file import log identifier.
        /// </summary>
        /// <value>
        /// The file import log identifier.
        /// </value>
        public Guid FileImportLogId { get; set; }

        /// <summary>
        /// Gets or sets the import log date time.
        /// </summary>
        /// <value>
        /// The import log date time.
        /// </value>
        public DateTime ImportLogDateTime { get; set; }
    }
}

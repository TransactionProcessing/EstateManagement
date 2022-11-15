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
        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>
        /// The file identifier.
        /// </value>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the file import log identifier.
        /// </summary>
        /// <value>
        /// The file import log identifier.
        /// </value>
        public Guid FileImportLogId { get; set; }

        /// <summary>
        /// Gets or sets the file location.
        /// </summary>
        /// <value>
        /// The file location.
        /// </value>
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the file profile identifier.
        /// </summary>
        /// <value>
        /// The file profile identifier.
        /// </value>
        public Guid FileProfileId { get; set; }

        /// <summary>
        /// Gets or sets the file received date time.
        /// </summary>
        /// <value>
        /// The file received date time.
        /// </value>
        public DateTime FileReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCompleted { get; set; }
    }
}
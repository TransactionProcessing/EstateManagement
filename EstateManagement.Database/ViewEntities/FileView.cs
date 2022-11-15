namespace EstateManagement.Database.ViewEntities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("uvwFileView")]
    public class FileView
    {
        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>
        /// The file identifier.
        /// </value>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the file received date time.
        /// </summary>
        /// <value>
        /// The file received date time.
        /// </value>
        public DateTime FileReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the file received date.
        /// </summary>
        /// <value>
        /// The file received date.
        /// </value>
        public DateTime FileReceivedDate { get; set; }

        /// <summary>
        /// Gets or sets the file received time.
        /// </summary>
        /// <value>
        /// The file received time.
        /// </value>
        public TimeSpan FileReceivedTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }
        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        public String MerchantName { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the line count.
        /// </summary>
        /// <value>
        /// The line count.
        /// </value>
        public Int32 LineCount { get; set; }
        /// <summary>
        /// Gets or sets the pending count.
        /// </summary>
        /// <value>
        /// The pending count.
        /// </value>
        public Int32 PendingCount { get; set; }
        /// <summary>
        /// Gets or sets the failed count.
        /// </summary>
        /// <value>
        /// The failed count.
        /// </value>
        public Int32 FailedCount { get; set; }
        /// <summary>
        /// Gets or sets the success count.
        /// </summary>
        /// <value>
        /// The success count.
        /// </value>
        public Int32 SuccessCount { get; set; }
    }
}
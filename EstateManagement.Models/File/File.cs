namespace EstateManagement.Models.File{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Merchant;

    [ExcludeFromCodeCoverage]
    public class File{
        #region Properties

        public Guid FileId{ get; set; }

        public List<FileLineDetails> FileLineDetails{ get; set; }

        public DateTime FileReceivedDate{ get; set; }

        public DateTime FileReceivedDateTime{ get; set; }

        public Merchant Merchant{ get; set; }

        #endregion
    }
}
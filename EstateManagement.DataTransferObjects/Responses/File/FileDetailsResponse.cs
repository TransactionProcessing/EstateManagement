namespace EstateManagement.DataTransferObjects.Responses.File{
    using System;
    using System.Collections.Generic;
    using Merchant;

    public class FileDetailsResponse{
        public Guid FileId{ get; set; }

        public DateTime FileReceivedDate{ get; set; }

        public DateTime FileReceivedDateTime{ get; set; }

        public List<FileLineDetailsResponse> FileLineDetails{ get; set; }

        public MerchantResponse Merchant{ get; set; }
    }
}
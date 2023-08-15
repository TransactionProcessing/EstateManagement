namespace EstateManagement.DataTransferObjects.Responses{

    using System;
    using System.Collections.Generic;

    public class FileDetailsResponse{
        public Guid FileId{ get; set; }

        public DateTime FileReceivedDate{ get; set; }

        public DateTime FileReceivedDateTime{ get; set; }

        public List<FileLineDetailsResponse> FileLineDetails{ get; set; }

        public MerchantResponse Merchant{ get; set; }
    }
}
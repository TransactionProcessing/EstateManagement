namespace EstateManagement.DataTransferObjects.Requests.Merchant
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Responses.Merchant;

    public class UpdateMerchantRequest
    {
        #region Properties

        [JsonProperty("name")]
        public String Name { get; set; }
        
        [JsonProperty("settlment_schedule")]
        public SettlementSchedule SettlementSchedule { get; set; }

        #endregion
    }
}
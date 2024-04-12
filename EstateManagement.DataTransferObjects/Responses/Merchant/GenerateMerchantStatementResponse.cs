namespace EstateManagement.DataTransferObjects.Responses.Merchant
{
    using System;
    using Newtonsoft.Json;

    public class GenerateMerchantStatementResponse
    {
        [JsonProperty("merchant_statement_id")]
        public Guid MerchantStatementId { get; set; }
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("estate_id")]
        public Guid EstateId { get; set; }
    }
}
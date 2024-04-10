namespace EstateManagement.DataTransferObjects.Requests.Merchant{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public class AddMerchantContractRequest
    {
        [JsonProperty("contract_id")]
        public Guid ContractId { get; set; }
    }
}
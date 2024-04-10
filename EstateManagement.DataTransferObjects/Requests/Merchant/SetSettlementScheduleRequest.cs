namespace EstateManagement.DataTransferObjects.Requests.Merchant
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Responses.Merchant;

    [ExcludeFromCodeCoverage]
    public class SetSettlementScheduleRequest
    {
        [JsonProperty("settlment_schedule")]
        public SettlementSchedule SettlementSchedule { get; set; }

    }
}

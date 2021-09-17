using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.DataTransferObjects.Requests
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public class SetSettlementScheduleRequest
    {
        [JsonProperty("settlment_schedule")]
        public SettlementSchedule SettlementSchedule { get; set; }

    }
    public enum SettlementSchedule
    {
        NotSet,
        Immediate,
        Weekly,
        Monthly
    }
}

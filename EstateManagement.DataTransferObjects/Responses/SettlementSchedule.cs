using System;

namespace EstateManagement.DataTransferObjects.Responses
{

    [Obsolete("Use enum in EstateManagement.DataTransferObjects.Responses.Merchant namespace")]
    public enum SettlementSchedule
    {
        NotSet,
        Immediate,
        Weekly,
        Monthly
    }
}
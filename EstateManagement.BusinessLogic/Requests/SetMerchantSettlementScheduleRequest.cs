namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models;

    public class SetMerchantSettlementScheduleRequest : IRequest<String>
    {
        public Guid EstateId { get; }

        public Guid MerchantId { get; }

        public SettlementSchedule SettlementSchedule { get; }

        private SetMerchantSettlementScheduleRequest(Guid estateId,
                                                     Guid merchantId,
                                                     SettlementSchedule settlementSchedule)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.SettlementSchedule = settlementSchedule;
        }

        public static SetMerchantSettlementScheduleRequest Create(Guid estateId,
                                                                  Guid merchantId,
                                                                  SettlementSchedule settlementSchedule)
        {
            return new SetMerchantSettlementScheduleRequest(estateId, merchantId, settlementSchedule);
        }
    }
}
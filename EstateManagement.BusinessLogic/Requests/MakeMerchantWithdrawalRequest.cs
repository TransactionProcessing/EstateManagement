namespace EstateManagement.BusinessLogic.Requests;

using System;
using MediatR;

//public class MakeMerchantWithdrawalRequest : IRequest<Guid>
//{
//    #region Constructors

//    public MakeMerchantWithdrawalRequest(Guid estateId,
//                                         Guid merchantId,
//                                         DateTime withdrawalDateTime,
//                                         Decimal amount) {
//        this.EstateId = estateId;
//        this.MerchantId = merchantId;
//        this.WithdrawalDateTime = withdrawalDateTime;
//        this.Amount = amount;
//    }

//    #endregion

//    #region Properties

//    public Decimal Amount { get; }

//    public Guid EstateId { get; }

//    public Guid MerchantId { get; }

//    public DateTime WithdrawalDateTime { get; }

//    #endregion

//    #region Methods

//    public static MakeMerchantWithdrawalRequest Create(Guid estateId,
//                                                       Guid merchantId,
//                                                       DateTime withdrawalDateTime,
//                                                       Decimal amount) {
//        return new MakeMerchantWithdrawalRequest(estateId, merchantId, withdrawalDateTime, amount);
//    }

//    #endregion
//}
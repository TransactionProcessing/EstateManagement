using System;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

public record MerchantStatementCommands {
    public record AddTransactionToMerchantStatementCommand(Guid EstateId,
                                                           Guid MerchantId,
                                                           DateTime TransactionDateTime,
                                                           Decimal? TransactionAmount,
                                                           Boolean IsAuthorised,
                                                           Guid TransactionId) : IRequest<Result>;

    public record EmailMerchantStatementCommand(Guid EstateId,
                                                Guid MerchantId,
                                                Guid MerchantStatementId) : IRequest<Result>;

    public record AddSettledFeeToMerchantStatementCommand(Guid EstateId,
                                                          Guid MerchantId,
                                                          DateTime SettledDateTime,
                                                          Decimal SettledAmount,
                                                          Guid TransactionId,
                                                          Guid SettledFeeId) : IRequest<Result>;
}
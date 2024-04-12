namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Requests;

public class DummyMerchantStatementDomainService : IMerchantStatementDomainService{
    public async Task AddTransactionToStatement(Guid estateId,
                                                Guid merchantId,
                                                DateTime transactionDateTime,
                                                Decimal? transactionAmount,
                                                Boolean isAuthorised,
                                                Guid transactionId,
                                                CancellationToken cancellationToken) {
    }

    public async Task AddSettledFeeToStatement(Guid estateId,
                                               Guid merchantId,
                                               DateTime settledDateTime,
                                               Decimal settledAmount,
                                               Guid transactionId,
                                               Guid settledFeeId,
                                               CancellationToken cancellationToken) {
    }

    public async Task<Guid> GenerateStatement(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken){
        return Guid.NewGuid();
    }
    
    public async Task EmailStatement(Guid estateId,
                                     Guid merchantId,
                                     Guid merchantStatementId,
                                     CancellationToken cancellationToken) {
    }
}
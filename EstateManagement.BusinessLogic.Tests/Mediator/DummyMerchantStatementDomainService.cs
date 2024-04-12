namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Manger;
using Models.Contract;
using Models.Estate;
using Models.File;
using Models.Merchant;
using Requests;
using Contract = Models.Contract.Contract;

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

public class DummyEstateManagementManager : IEstateManagementManager{
    public async Task<List<Contract>> GetMerchantContracts(Guid estateId, Guid merchantId, CancellationToken cancellationToken){
        return new List<Contract>();
    }

    public async Task<List<Contract>> GetContracts(Guid estateId, CancellationToken cancellationToken){
        return new List<Contract>();
    }

    public async Task<Contract> GetContract(Guid estateId, Guid contractId, CancellationToken cancellationToken){
        return new Contract();
    }

    public async Task<Estate> GetEstate(Guid estateId, CancellationToken cancellationToken){
        return new Estate();
    }

    public async Task<List<Estate>> GetEstates(Guid estateId, CancellationToken cancellationToken){
        return new List<Estate>();
    }

    public async Task<Merchant> GetMerchant(Guid estateId, Guid merchantId, CancellationToken cancellationToken){
        return new Merchant();
    }

    public async Task<List<Merchant>> GetMerchants(Guid estateId, CancellationToken cancellationToken){
        return new List<Merchant>{
                                     new Merchant()
                                 };
    }

    public async Task<List<TransactionFee>> GetTransactionFeesForProduct(Guid estateId, Guid merchantId, Guid contractId, Guid productId, CancellationToken cancellationToken){
        return new List<TransactionFee>();
    }

    public async Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken){
        return new File();
    }
}
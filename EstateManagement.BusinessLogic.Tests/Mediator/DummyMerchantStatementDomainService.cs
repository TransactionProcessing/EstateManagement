using EstateManagement.Models;
using SimpleResults;

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
using Operator = Models.Operator.Operator;

public class DummyMerchantStatementDomainService : IMerchantStatementDomainService{
    public async Task<Result> AddTransactionToStatement(MerchantStatementCommands.AddTransactionToMerchantStatementCommand command,
                                                        CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> AddSettledFeeToStatement(MerchantStatementCommands.AddSettledFeeToMerchantStatementCommand command,
                                                       CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> GenerateStatement(MerchantCommands.GenerateMerchantStatementCommand command, CancellationToken cancellationToken) => Result.Success();

    public async Task<Result> EmailStatement(MerchantStatementCommands.EmailMerchantStatementCommand command,
                                             CancellationToken cancellationToken) => Result.Success();
}

public class DummyEstateManagementManager : IEstateManagementManager {
    public async Task<Result<List<Contract>>> GetMerchantContracts(Guid estateId,
                                                                   Guid merchantId,
                                                                   CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<List<Contract>>> GetContracts(Guid estateId,
                                                           CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<Contract>> GetContract(Guid estateId,
                                                    Guid contractId,
                                                    CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<Estate>> GetEstate(Guid estateId,
                                                CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<List<Estate>>> GetEstates(Guid estateId,
                                                       CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<Merchant>> GetMerchant(Guid estateId,
                                                    Guid merchantId,
                                                    CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<List<Merchant>>> GetMerchants(Guid estateId,
                                                           CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<List<ContractProductTransactionFee>>> GetTransactionFeesForProduct(Guid estateId,
                                                                                                Guid merchantId,
                                                                                                Guid contractId,
                                                                                                Guid productId,
                                                                                                CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<File>> GetFileDetails(Guid estateId,
                                                   Guid fileId,
                                                   CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<Operator>> GetOperator(Guid estateId,
                                                    Guid operatorId,
                                                    CancellationToken cancellationToken) =>
        Result.Success();

    public async Task<Result<List<Operator>>> GetOperators(Guid estateId,
                                                           CancellationToken cancellationToken) =>
        Result.Success();
}

public class DummyReportingManager : IReportingManager {
    public async Task<Result<SettlementModel>> GetSettlement(Guid estateId,
                                                             Guid merchantId,
                                                             Guid settlementId,
                                                             CancellationToken cancellationToken) {
        return Result.Success();
    }

    public async Task<Result<List<SettlementModel>>> GetSettlements(Guid estateId,
                                                                    Guid? merchantId,
                                                                    String startDate,
                                                                    String endDate,
                                                                    CancellationToken cancellationToken) {
        return Result.Success();
    }
}
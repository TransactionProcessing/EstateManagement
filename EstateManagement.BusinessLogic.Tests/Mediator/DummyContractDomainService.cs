namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Models.Contract;

public class DummyContractDomainService : IContractDomainService
{
    public async Task AddProductToContract(Guid productId,
                                           Guid contractId,
                                           String productName,
                                           String displayText,
                                           Decimal? value,
                                           ProductType productType,
                                           CancellationToken cancellationToken) {
    }

    public async Task AddTransactionFeeForProductToContract(Guid transactionFeeId,
                                                            Guid contractId,
                                                            Guid productId,
                                                            String description,
                                                            CalculationType calculationType,
                                                            FeeType feeType,
                                                            Decimal value,
                                                            CancellationToken cancellationToken) {
    }

    public async Task DisableTransactionFeeForProduct(Guid transactionFeeId,
                                                      Guid contractId,
                                                      Guid productId,
                                                      CancellationToken cancellationToken) {
    }

    public async Task CreateContract(Guid contractId,
                                     Guid estateId,
                                     Guid operatorId,
                                     String description,
                                     CancellationToken cancellationToken) {
    }
}
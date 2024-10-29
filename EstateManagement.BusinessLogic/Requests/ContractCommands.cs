using System;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.Requests;

public record ContractCommands {
    public record CreateContractCommand(Guid EstateId,
                                        Guid ContractId,
                                        DataTransferObjects.Requests.Contract.CreateContractRequest RequestDTO) : IRequest<Result>;

    public record AddProductToContractCommand(Guid EstateId,
                                              Guid ContractId,
                                              Guid ProductId,
                                              DataTransferObjects.Requests.Contract.AddProductToContractRequest
                                                  RequestDTO) : IRequest<Result>;

    public record AddTransactionFeeForProductToContractCommand(Guid EstateId,
                                                               Guid ContractId,
                                                               Guid ProductId,
                                                               Guid TransactionFeeId,
                                                               DataTransferObjects.Requests.Contract.
                                                                   AddTransactionFeeForProductToContractRequest
                                                                   RequestDTO) : IRequest<Result>;

    public record DisableTransactionFeeForProductCommand(Guid EstateId,
                                                         Guid ContractId,
                                                         Guid ProductId,
                                                         Guid TransactionFeeId) : IRequest<Result>;
}
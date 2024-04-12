namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MediatR;
    using Models.Contract;

    [ExcludeFromCodeCoverage]
    public class MerchantCommands{

        public record CreateMerchantCommand(Guid EstateId, CreateMerchantRequest RequestDto) : IRequest<Guid>;

        public record AssignOperatorToMerchantCommand(Guid EstateId, Guid MerchantId, AssignOperatorRequest RequestDto) : IRequest<Guid>;

        public record AddMerchantDeviceCommand(Guid EstateId, Guid MerchantId, AddMerchantDeviceRequest RequestDto) : IRequest<Guid>;

        public record AddMerchantContractCommand(Guid EstateId, Guid MerchantId, AddMerchantContractRequest RequestDto) : IRequest;

        public record CreateMerchantUserCommand(Guid EstateId, Guid MerchantId, CreateMerchantUserRequest RequestDto) : IRequest<Guid>;

        public record MakeMerchantDepositCommand(Guid EstateId, Guid MerchantId, Models.MerchantDepositSource DepositSource, MakeMerchantDepositRequest RequestDto) : IRequest<Guid>;

        public record MakeMerchantWithdrawalCommand(Guid EstateId, Guid MerchantId, MakeMerchantWithdrawalRequest RequestDto) : IRequest<Guid>;

        public record SwapMerchantDeviceCommand(Guid EstateId, Guid MerchantId, SwapMerchantDeviceRequest RequestDto): IRequest<Guid>;

        public record GenerateMerchantStatementCommand(Guid EstateId, Guid MerchantId, GenerateMerchantStatementRequest RequestDto) : IRequest<Guid>;
        
        public record UpdateMerchantCommand(Guid EstateId, Guid MerchantId, UpdateMerchantRequest RequestDto) : IRequest;
    }

    [ExcludeFromCodeCoverage]
    public class MerchantQueries{
        public record GetMerchantQuery(Guid EstateId, Guid MerchantId) : IRequest<Models.Merchant.Merchant>;

        public record GetMerchantContractsQuery(Guid EstateId, Guid MerchantId) : IRequest<List<Models.Contract.Contract>>;

        public record GetMerchantsQuery(Guid EstateId) : IRequest<List<Models.Merchant.Merchant>>;

        public record GetTransactionFeesForProductQuery(Guid EstateId, Guid MerchantId, Guid ContractId,Guid ProductId) : IRequest<List<TransactionFee>>;

    }
}
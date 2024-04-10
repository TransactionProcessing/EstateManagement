namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MediatR;

    public class MerchantCommands{

        [ExcludeFromCodeCoverage]
        public record CreateMerchantCommand(Guid EstateId, CreateMerchantRequest RequestDto) : IRequest<Guid>;

        [ExcludeFromCodeCoverage]
        public record AssignOperatorToMerchantCommand(Guid EstateId, Guid MerchantId, AssignOperatorRequest RequestDto) : IRequest<Guid>;

        [ExcludeFromCodeCoverage]
        public record AddMerchantDeviceCommand(Guid EstateId, Guid MerchantId, AddMerchantDeviceRequest RequestDto) : IRequest<Guid>;

        [ExcludeFromCodeCoverage]
        public record AddMerchantContractCommand(Guid EstateId, Guid MerchantId, AddMerchantContractRequest RequestDto) : IRequest;

        [ExcludeFromCodeCoverage]
        public record CreateMerchantUserCommand(Guid EstateId, Guid MerchantId, CreateMerchantUserRequest RequestDto) : IRequest<Guid>;

        [ExcludeFromCodeCoverage]
        public record MakeMerchantDepositCommand(Guid EstateId, Guid MerchantId, Models.MerchantDepositSource DepositSource, MakeMerchantDepositRequest RequestDto) : IRequest<Guid>;

        public record MakeMerchantWithdrawalCommand(Guid EstateId, Guid MerchantId, MakeMerchantWithdrawalRequest RequestDto) : IRequest<Guid>;
    }
}
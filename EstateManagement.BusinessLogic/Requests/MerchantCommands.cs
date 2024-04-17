namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MediatR;

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

        public record AddMerchantAddressCommand(Guid EstateId, Guid MerchantId,  Address RequestDto) : IRequest;
        public record UpdateMerchantAddressCommand(Guid EstateId, Guid MerchantId, Guid AddressId, Address RequestDto) : IRequest;

        public record AddMerchantContactCommand(Guid EstateId, Guid MerchantId, Contact RequestDto) : IRequest;

        public record UpdateMerchantContactCommand(Guid EstateId, Guid MerchantId, Guid ContactId, Contact RequestDto) : IRequest;
    }

    
}
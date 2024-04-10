namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using EstateManagement.DataTransferObjects.Requests.Merchant;
    using MediatR;
    
    [ExcludeFromCodeCoverage]
    public record CreateMerchantCommand(Guid EstateId, CreateMerchantRequest RequestDto) : IRequest<Guid>{}
}
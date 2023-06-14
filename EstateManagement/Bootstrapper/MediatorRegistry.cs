namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using Lamar;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class MediatorRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MediatorRegistry"/> class.
        /// </summary>
        public MediatorRegistry()
        {
            this.AddTransient<IMediator, Mediator>();

            // request & notification handlers
            this.AddSingleton<IRequestHandler<CreateEstateRequest>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateEstateUserRequest, Guid>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<AddOperatorToEstateRequest>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateMerchantRequest>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AssignOperatorToMerchantRequest>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateMerchantUserRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AddMerchantDeviceRequest>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MakeMerchantDepositRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MakeMerchantWithdrawalRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SetMerchantSettlementScheduleRequest>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SwapMerchantDeviceRequest>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddProductToContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddTransactionFeeForProductToContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<DisableTransactionFeeForProductRequest>, ContractRequestHandler>();

            this.AddSingleton<IRequestHandler<AddTransactionToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<AddSettledFeeToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<GenerateMerchantStatementRequest, Guid>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<EmailMerchantStatementRequest>, MerchantStatementRequestHandler>();
        }

        #endregion
    }
}
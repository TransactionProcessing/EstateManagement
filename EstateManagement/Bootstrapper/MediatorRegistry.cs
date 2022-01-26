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
            this.AddTransient<ServiceFactory>(context => { return t => context.GetService(t); });

            this.AddSingleton<IRequestHandler<CreateEstateRequest,Unit>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateEstateUserRequest, Guid>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<AddOperatorToEstateRequest, Unit>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateMerchantRequest, Unit>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AssignOperatorToMerchantRequest, Unit>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateMerchantUserRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AddMerchantDeviceRequest, Unit>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MakeMerchantDepositRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SetMerchantSettlementScheduleRequest, Unit>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SwapMerchantDeviceRequest, Unit>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateContractRequest, Unit>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddProductToContractRequest, Unit>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddTransactionFeeForProductToContractRequest, Unit>, ContractRequestHandler>();

            this.AddSingleton<IRequestHandler<AddTransactionToMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<AddSettledFeeToMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<GenerateMerchantStatementRequest, Guid>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<EmailMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
        }

        #endregion
    }
}
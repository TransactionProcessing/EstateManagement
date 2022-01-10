namespace EstateManagement.Bootstrapper
{
    using System;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using Lamar;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
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

            this.AddSingleton<IRequestHandler<CreateEstateRequest, String>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateEstateUserRequest, Guid>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<AddOperatorToEstateRequest, String>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateMerchantRequest, String>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AssignOperatorToMerchantRequest, String>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<CreateMerchantUserRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<AddMerchantDeviceRequest, String>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MakeMerchantDepositRequest, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SetMerchantSettlementScheduleRequest, String>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<SwapMerchantDeviceRequest, String>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateContractRequest, String>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddProductToContractRequest, String>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddTransactionFeeForProductToContractRequest, String>, ContractRequestHandler>();

            this.AddSingleton<IRequestHandler<AddTransactionToMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<AddSettledFeeToMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<GenerateMerchantStatementRequest, Guid>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<EmailMerchantStatementRequest, Unit>, MerchantStatementRequestHandler>();
        }

        #endregion
    }
}
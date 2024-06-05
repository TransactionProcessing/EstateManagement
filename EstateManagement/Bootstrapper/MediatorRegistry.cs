namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using Lamar;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Estate;
    using Models.Merchant;
    using Operator = Models.Operator.Operator;

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
            this.AddSingleton<IRequestHandler<EstateCommands.CreateEstateCommand>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.CreateEstateUserCommand>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.AddOperatorToEstateCommand>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.RemoveOperatorFromEstateCommand>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<EstateQueries.GetEstateQuery, Estate>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateQueries.GetEstatesQuery, List<Estate>>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<OperatorCommands.CreateOperatorCommand>, OperatorRequestHandler>();
            this.AddSingleton<IRequestHandler<OperatorCommands.UpdateOperatorCommand>, OperatorRequestHandler>();

            this.AddSingleton<IRequestHandler<OperatorQueries.GetOperatorQuery, Operator>, OperatorRequestHandler>();
            this.AddSingleton<IRequestHandler<OperatorQueries.GetOperatorsQuery, List<Operator>>, OperatorRequestHandler>();

            this.AddSingleton<IRequestHandler<MerchantCommands.CreateMerchantCommand,Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AssignOperatorToMerchantCommand, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.CreateMerchantUserCommand, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.MakeMerchantWithdrawalCommand, Guid>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantContractCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantAddressCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantAddressCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantContactCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantContactCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.RemoveMerchantContractCommand>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantQuery, Merchant>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantContractsQuery, List<Models.Contract.Contract>>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantsQuery, List<Models.Merchant.Merchant>>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetTransactionFeesForProductQuery, List<Models.Contract.ContractProductTransactionFee>>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<CreateContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddProductToContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<AddTransactionFeeForProductToContractRequest>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<DisableTransactionFeeForProductRequest>, ContractRequestHandler>();

            this.AddSingleton<IRequestHandler<AddTransactionToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<AddSettledFeeToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.GenerateMerchantStatementCommand, Guid>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<EmailMerchantStatementRequest>, MerchantStatementRequestHandler>();
        }

        #endregion
    }
}
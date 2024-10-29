using SimpleResults;

namespace EstateManagement.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using EstateManagement.Models;
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
            this.AddSingleton<IRequestHandler<EstateCommands.CreateEstateCommand,Result>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.CreateEstateUserCommand, Result>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.AddOperatorToEstateCommand, Result>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateCommands.RemoveOperatorFromEstateCommand, Result>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<EstateQueries.GetEstateQuery, Result<Estate>>, EstateRequestHandler>();
            this.AddSingleton<IRequestHandler<EstateQueries.GetEstatesQuery, Result<List<Estate>>>, EstateRequestHandler>();

            this.AddSingleton<IRequestHandler<OperatorCommands.CreateOperatorCommand, Result>, OperatorRequestHandler>();
            this.AddSingleton<IRequestHandler<OperatorCommands.UpdateOperatorCommand, Result>, OperatorRequestHandler>();

            this.AddSingleton<IRequestHandler<OperatorQueries.GetOperatorQuery, Result<Operator>>, OperatorRequestHandler>();
            this.AddSingleton<IRequestHandler<OperatorQueries.GetOperatorsQuery, Result<List<Operator>>>, OperatorRequestHandler>();

            this.AddSingleton<IRequestHandler<MerchantCommands.CreateMerchantCommand,Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AssignOperatorToMerchantCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.CreateMerchantUserCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.MakeMerchantWithdrawalCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantContractCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantAddressCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantAddressCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.AddMerchantContactCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.UpdateMerchantContactCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand, Result>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.RemoveMerchantContractCommand, Result>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantQuery, Result<Merchant>>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantContractsQuery, Result<List<Models.Contract.Contract>>>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetMerchantsQuery, Result<List<Models.Merchant.Merchant>>>, MerchantRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantQueries.GetTransactionFeesForProductQuery, Result<List<Models.Contract.ContractProductTransactionFee>>>, MerchantRequestHandler>();

            this.AddSingleton<IRequestHandler<ContractCommands.CreateContractCommand,Result>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<ContractCommands.AddProductToContractCommand, Result>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<ContractCommands.DisableTransactionFeeForProductCommand,Result>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<ContractCommands.AddTransactionFeeForProductToContractCommand,Result>, ContractRequestHandler>();
            
            this.AddSingleton<IRequestHandler<ContractQueries.GetContractQuery, Result<Models.Contract.Contract>>, ContractRequestHandler>();
            this.AddSingleton<IRequestHandler<ContractQueries.GetContractsQuery, Result<List<Models.Contract.Contract>>>, ContractRequestHandler>();

            this.AddSingleton<IRequestHandler<AddTransactionToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<AddSettledFeeToMerchantStatementRequest>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<MerchantCommands.GenerateMerchantStatementCommand, Result>, MerchantStatementRequestHandler>();
            this.AddSingleton<IRequestHandler<EmailMerchantStatementRequest>, MerchantStatementRequestHandler>();

            this.AddSingleton<IRequestHandler<SettlementQueries.GetSettlementQuery, Result<SettlementModel>>,
                SettlementRequestHandler>();
            this.AddSingleton<IRequestHandler<SettlementQueries.GetSettlementsQuery, Result<List<SettlementModel>>>,
                SettlementRequestHandler>();
        }

        #endregion
    }
}
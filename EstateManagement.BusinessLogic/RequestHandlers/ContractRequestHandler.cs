﻿using System;
using System.Collections.Generic;
using EstateManagement.BusinessLogic.Manger;
using EstateManagement.DataTransferObjects.Requests.Contract;
using SimpleResults;

namespace EstateManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CreateContractRequest.String}" />
    /// <seealso cref="AddProductToContractRequest.String}" />
    /// <seealso cref="AddTransactionFeeForProductToContractRequest.String}" />
    public class ContractRequestHandler : IRequestHandler<ContractCommands.CreateContractCommand, Result>,
                                          IRequestHandler<ContractCommands.AddProductToContractCommand, Result>,
                                          IRequestHandler<ContractCommands.AddTransactionFeeForProductToContractCommand, Result>,
                                          IRequestHandler<ContractCommands.DisableTransactionFeeForProductCommand, Result>,
                                          IRequestHandler<ContractQueries.GetContractQuery, Result<Models.Contract.Contract>>,
                                          IRequestHandler<ContractQueries.GetContractsQuery, Result<List<Models.Contract.Contract>>>
    {
        #region Fields

        private readonly IContractDomainService ContractDomainService;
        private readonly IEstateManagementManager EstateManagementManager;

        #endregion

        #region Constructors

        public ContractRequestHandler(IContractDomainService contractDomainService, IEstateManagementManager estateManagementManager) {
            this.ContractDomainService = contractDomainService;
            this.EstateManagementManager = estateManagementManager;
        }

        #endregion

        #region Methods

        public async Task<Result> Handle(ContractCommands.CreateContractCommand command,
                                 CancellationToken cancellationToken)
        {
            return await this.ContractDomainService.CreateContract(command, cancellationToken);
        }
        
        public async Task<Result> Handle(ContractCommands.AddProductToContractCommand command,
                                         CancellationToken cancellationToken)
        {
            return await this.ContractDomainService.AddProductToContract(command, cancellationToken);
        }
        
        public async Task<Result> Handle(ContractCommands.AddTransactionFeeForProductToContractCommand command,
                                 CancellationToken cancellationToken)
        {
            return await this.ContractDomainService.AddTransactionFeeForProductToContract(command, cancellationToken);
        }

        #endregion

        public async Task<Result> Handle(ContractCommands.DisableTransactionFeeForProductCommand command, CancellationToken cancellationToken)
        {
            return await this.ContractDomainService.DisableTransactionFeeForProduct(command, cancellationToken);
        }

        public async Task<Result<Models.Contract.Contract>> Handle(ContractQueries.GetContractQuery query,
                                                                   CancellationToken cancellationToken) {
            Result<Models.Contract.Contract> result =
                await this.EstateManagementManager.GetContract(query.EstateId, query.ContractId, cancellationToken);
            return result;
        }

        public async Task<Result<List<Models.Contract.Contract>>> Handle(ContractQueries.GetContractsQuery query,
                                                                         CancellationToken cancellationToken) {
            Result<List<Models.Contract.Contract>> result = await this.EstateManagementManager.GetContracts(query.EstateId, cancellationToken);
            return result;
        }
    }
}
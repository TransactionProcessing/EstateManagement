namespace EstateManagement.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Estate;
    using Models.File;
    using Models.Merchant;
    using Models.MerchantStatement;
    using Contract = Models.Contract.Contract;
    using Operator = Models.Merchant.Operator;

    /// <summary>
    /// 
    /// </summary>
    public interface IEstateManagementRepository
    {
        #region Methods

        Task<Contract> GetContract(Guid estateId,
                                   Guid contractId,
                                   Boolean includeProducts,
                                   Boolean includeProductsWithFees,
                                   CancellationToken cancellationToken);

        Task<List<Contract>> GetContracts(Guid estateId,
                                   CancellationToken cancellationToken);

        Task<Estate> GetEstate(Guid estateId,
                               CancellationToken cancellationToken);

        Task<List<Contract>> GetMerchantContracts(Guid estateId,
                                                  Guid merchantId,
                                                  CancellationToken cancellationToken);

        Task<List<Merchant>> GetMerchants(Guid estateId,
                                          CancellationToken cancellationToken);

        Task<Merchant> GetMerchant(Guid estateId,
                                         Guid merchantId,
                                         CancellationToken cancellationToken);

        Task<Merchant> GetMerchantFromReference(Guid estateId, 
                                                String reference,
                                                CancellationToken cancellationToken);

        Task<StatementHeader> GetStatement(Guid estateId,
                                           Guid merchantStatementId,
                                           CancellationToken cancellationToken);

        Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken);

        Task<List<Models.Operator.Operator>> GetOperators(Guid estateId,
                                          CancellationToken cancellationToken);

        #endregion
    }
}
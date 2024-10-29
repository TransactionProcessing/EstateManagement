using SimpleResults;

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
        
        Task<Result<List<Contract>>> GetContracts(Guid estateId,
                                   CancellationToken cancellationToken);

        Task<Result<Estate>> GetEstate(Guid estateId,
                                     CancellationToken cancellationToken);

        Task<Result<List<Contract>>> GetMerchantContracts(Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken);

        Task<Result<List<Merchant>>> GetMerchants(Guid estateId,
                                                CancellationToken cancellationToken);

        Task<Result<Merchant>> GetMerchant(Guid estateId,
                                         Guid merchantId,
                                         CancellationToken cancellationToken);

        Task<Result<Merchant>> GetMerchantFromReference(Guid estateId, 
                                                      String reference,
                                                      CancellationToken cancellationToken);

        Task<Result<StatementHeader>> GetStatement(Guid estateId,
                                                 Guid merchantStatementId,
                                                 CancellationToken cancellationToken);

        Task<Result<File>> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken);

        Task<Result<List<Models.Operator.Operator>>> GetOperators(Guid estateId,
                                                                CancellationToken cancellationToken);

        #endregion
    }
}
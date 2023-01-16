namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Contract;

    /// <summary>
    /// 
    /// </summary>
    public interface IContractDomainService
    {
        #region Methods
        
        Task AddProductToContract(Guid productId,
                                  Guid contractId,
                                  String productName,
                                  String displayText,
                                  Decimal? value,
                                  ProductType productType,
                                  CancellationToken cancellationToken);

        Task AddTransactionFeeForProductToContract(Guid transactionFeeId,
                                                   Guid contractId,
                                                   Guid productId,
                                                   String description,
                                                   CalculationType calculationType,
                                                   FeeType feeType,
                                                   Decimal value,
                                                   CancellationToken cancellationToken);
        
        Task DisableTransactionFeeForProduct(Guid transactionFeeId,
                                                   Guid contractId,
                                                   Guid productId,
                                                   CancellationToken cancellationToken);

        Task CreateContract(Guid contractId,
                            Guid estateId,
                            Guid operatorId,
                            String description,
                            CancellationToken cancellationToken);

        #endregion
    }
}
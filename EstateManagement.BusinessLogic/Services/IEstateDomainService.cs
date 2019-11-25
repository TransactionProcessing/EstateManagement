namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEstateDomainService
    {
        #region Methods

        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateEstate(Guid estateId, 
                          String estateName,
                          CancellationToken cancellationToken);

        /// <summary>
        /// Creates the operator.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddOperatorToEstate(Guid estateId,
                            Guid operatorId,
                            String operatorName,
                            Boolean requireCustomMerchantNumber,
                            Boolean requireCustomTerminalNumber,
                            CancellationToken cancellationToken);

        #endregion
    }
}
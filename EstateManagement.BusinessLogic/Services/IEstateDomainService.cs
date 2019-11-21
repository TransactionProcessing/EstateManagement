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

        #endregion
    }
}
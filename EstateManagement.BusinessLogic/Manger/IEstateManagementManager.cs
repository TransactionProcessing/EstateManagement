namespace EstateManagement.BusinessLogic.Manger
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IEstateManagementManager
    {
        #region Methods

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Estate> GetEstate(Guid estateId,
                               CancellationToken cancellationToken);

        #endregion
    }
}
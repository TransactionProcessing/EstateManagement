namespace EstateManagement.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    public interface IEstateClient
    {
        /// <summary>
        /// Creates the estate.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="createEstateRequest">The create estate request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateEstateResponse> CreateEstate(String accessToken,
                                                CreateEstateRequest createEstateRequest,
                                                CancellationToken cancellationToken);
    }
}
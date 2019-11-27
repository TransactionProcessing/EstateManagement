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
        #region Methods

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

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createMerchantRequest">The create merchant request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateMerchantResponse> CreateMerchant(String accessToken,
                                                    Guid estateId,
                                                    CreateMerchantRequest createMerchantRequest,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Adds the operator.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="createOperatorRequest">The create operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateOperatorResponse> CreateOperator(String accessToken,
                            Guid estateId,
                            CreateOperatorRequest createOperatorRequest,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<EstateResponse> GetEstate(String accessToken,
                                       Guid estateId,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<MerchantResponse> GetMerchant(String accessToken,
                                           Guid estateId,
                                           Guid merchantId,
                                           CancellationToken cancellationToken);

        /// <summary>
        /// Assigns the operator to merchant.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="assignOperatorRequest">The assign operator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<AssignOperatorResponse> AssignOperatorToMerchant(String accessToken,
                                      Guid estateId,
                                      Guid merchantId,
                                      AssignOperatorRequest assignOperatorRequest,
                                      CancellationToken cancellationToken);

        #endregion
    }
}
namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /// <summary>
    /// 
    /// </summary>
    public interface IMerchantDomainService
    {
        #region Methods

        /// <summary>
        /// Creates the merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="addressLine3">The address line3.</param>
        /// <param name="addressLine4">The address line4.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="country">The country.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="contactName">Name of the contact.</param>
        /// <param name="contactPhoneNumber">The contact phone number.</param>
        /// <param name="contactEmailAddress">The contact email address.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateMerchant(Guid estateId,
                            Guid merchantId,
                            String name,
                            Guid addressId,
                            String addressLine1,
                            String addressLine2,
                            String addressLine3,
                            String addressLine4,
                            String town,
                            String region,
                            String postalCode,
                            String country,
                            Guid contactId,
                            String contactName,
                            String contactPhoneNumber,
                            String contactEmailAddress,
                            CancellationToken cancellationToken);

        /// <summary>
        /// Assigns the operator to merchant.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AssignOperatorToMerchant(Guid estateId,
                                      Guid merchantId,
                                      Guid operatorId,
                                      String merchantNumber,
                                      String terminalNumber,
                                      CancellationToken cancellationToken);

        /// <summary>
        /// Creates the merchant user.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="familyName">Name of the family.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Guid> CreateMerchantUser(Guid estateId,
                                      Guid merchantId,
                                      String emailAddress,
                                      String password,
                                      String givenName,
                                      String middleName,
                                      String familyName,
                                      CancellationToken cancellationToken);

        #endregion
    }
}
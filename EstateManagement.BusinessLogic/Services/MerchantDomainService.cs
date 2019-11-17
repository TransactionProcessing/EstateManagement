namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateAggregate;
    using MerchantAggregate;
    using Shared.DomainDrivenDesign.EventStore;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.BusinessLogic.Services.IMerchantDomainService" />
    public class MerchantDomainService : IMerchantDomainService
    {
        #region Fields

        /// <summary>
        /// The estate aggregate repository
        /// </summary>
        private readonly IAggregateRepository<EstateAggregate> EstateAggregateRepository;

        /// <summary>
        /// The merchant aggregate repository
        /// </summary>
        private readonly IAggregateRepository<MerchantAggregate> MerchantAggregateRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDomainService"/> class.
        /// </summary>
        /// <param name="merchantAggregateRepository">The merchant aggregate repository.</param>
        /// <param name="estateAggregateRepository">The estate aggregate repository.</param>
        public MerchantDomainService(IAggregateRepository<MerchantAggregate> merchantAggregateRepository,
                                     IAggregateRepository<EstateAggregate> estateAggregateRepository)
        {
            this.MerchantAggregateRepository = merchantAggregateRepository;
            this.EstateAggregateRepository = estateAggregateRepository;
        }

        #endregion

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
        /// <exception cref="System.InvalidOperationException">Estate Id {estateId} has not been created</exception>
        public async Task CreateMerchant(Guid estateId,
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
                                         CancellationToken cancellationToken)
        {
            MerchantAggregate merchantAggregate = await this.MerchantAggregateRepository.GetLatestVersion(merchantId, cancellationToken);

            // Estate Id is a valid estate
            EstateAggregate estateAggregate = await this.EstateAggregateRepository.GetLatestVersion(estateId, cancellationToken);
            if (estateAggregate.IsCreated == false)
            {
                throw new InvalidOperationException($"Estate Id {estateId} has not been created");
            }

            // Reject Duplicate Merchant Names... is this needed ?

            // Create the merchant
            merchantAggregate.Create(estateId, name, DateTime.Now);

            // Add the address 
            merchantAggregate.AddAddress(addressId, addressLine1, addressLine2, addressLine3, addressLine4, town, region, postalCode, country);

            // Add the contact
            merchantAggregate.AddContact(contactId, contactName, contactPhoneNumber, contactEmailAddress);

            await this.MerchantAggregateRepository.SaveChanges(merchantAggregate, cancellationToken);
        }

        #endregion
    }
}
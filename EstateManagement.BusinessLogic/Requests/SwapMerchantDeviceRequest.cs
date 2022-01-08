using System;
using MediatR;

namespace EstateManagement.BusinessLogic.Requests
{
    public class SwapMerchantDeviceRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignOperatorToMerchantRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        private SwapMerchantDeviceRequest(Guid estateId,
            Guid merchantId,
            Guid deviceId,
            String originalDeviceIdentifier,
            String newDeviceIdentifier)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.DeviceId = deviceId;
            this.OriginalDeviceIdentifier = originalDeviceIdentifier;
            this.NewDeviceIdentifier = newDeviceIdentifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The device identifier
        /// </summary>
        public String OriginalDeviceIdentifier { get; }

        /// <summary>
        /// The device identifier
        /// </summary>
        public String NewDeviceIdentifier { get; }

        public Guid DeviceId { get; }

        /// <summary>
        /// The estate identifier
        /// </summary>
        public Guid EstateId { get; }

        /// <summary>
        /// The merchant identifier
        /// </summary>
        public Guid MerchantId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <returns></returns>
        public static SwapMerchantDeviceRequest Create(Guid estateId,
            Guid merchantId,
            Guid deviceId,
            String originalDeviceIdentifier,
            String newDeviceIdentifier)
        {
            return new SwapMerchantDeviceRequest(estateId, merchantId, deviceId,originalDeviceIdentifier, newDeviceIdentifier);
        }

        #endregion
    }
}
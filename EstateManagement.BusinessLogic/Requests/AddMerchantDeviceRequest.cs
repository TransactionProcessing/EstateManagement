namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.String}" />
    public class AddMerchantDeviceRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignOperatorToMerchantRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        private AddMerchantDeviceRequest(Guid estateId,
                                         Guid merchantId,
                                         Guid deviceId,
                                         String deviceIdentifier)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.DeviceId = deviceId;
            this.DeviceIdentifier = deviceIdentifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The device identifier
        /// </summary>
        public Guid DeviceId { get; }

        /// <summary>
        /// The device identifier
        /// </summary>
        public String DeviceIdentifier { get; }

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
        public static AddMerchantDeviceRequest Create(Guid estateId,
                                                      Guid merchantId,
                                                      Guid deviceId,
                                                      String deviceIdentifier)
        {
            return new AddMerchantDeviceRequest(estateId, merchantId, deviceId, deviceIdentifier);
        }

        #endregion
    }
}
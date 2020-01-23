namespace EstateManagement.Merchant.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    [JsonObject]
    public class DeviceAddedToMerchantEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public DeviceAddedToMerchantEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        private DeviceAddedToMerchantEvent(Guid aggregateId,
                                           Guid eventId,
                                           Guid estateId,
                                           Guid deviceId,
                                           String deviceIdentifier) : base(aggregateId, eventId)
        {
            this.MerchantId = aggregateId;
            this.EstateId = estateId;
            this.DeviceId = deviceId;
            this.DeviceIdentifier = deviceIdentifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [JsonProperty]
        public Guid DeviceId { get; private set; }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [JsonProperty]
        public String DeviceIdentifier { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; private set; }

        [JsonProperty]
        public Guid MerchantId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <returns></returns>
        public static DeviceAddedToMerchantEvent Create(Guid aggregateId,
                                                        Guid estateId,
                                                        Guid deviceId,
                                                        String deviceIdentifier)
        {
            return new DeviceAddedToMerchantEvent(aggregateId, Guid.NewGuid(), estateId, deviceId, deviceIdentifier);
        }

        #endregion
    }
}
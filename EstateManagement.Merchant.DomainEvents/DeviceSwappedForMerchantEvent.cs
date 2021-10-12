using System;
using Shared.DomainDrivenDesign.EventSourcing;

namespace EstateManagement.Merchant.DomainEvents
{
    public record DeviceSwappedForMerchantEvent : DomainEventRecord.DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        public DeviceSwappedForMerchantEvent(Guid aggregateId,
            Guid estateId,
            Guid deviceId,
            String originalDeviceIdentifier,
            String newDeviceIdentifier) : base(aggregateId, Guid.NewGuid())
        {
            this.MerchantId = aggregateId;
            this.EstateId = estateId;
            this.DeviceId = deviceId;
            this.OriginalDeviceIdentifier = originalDeviceIdentifier;
            this.NewDeviceIdentifier=newDeviceIdentifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public Guid DeviceId { get; init; }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public String OriginalDeviceIdentifier { get; init; }

        public String NewDeviceIdentifier { get; init; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; init; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; init; }

        #endregion
    }
}
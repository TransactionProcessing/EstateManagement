namespace EstateManagement.DataTransferObjects.Responses
{
    using System;

    public class MerchantBalanceResponse
    {
        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the available balance.
        /// </summary>
        /// <value>
        /// The available balance.
        /// </value>
        public Decimal AvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public Decimal Balance { get; set; }
    }
}
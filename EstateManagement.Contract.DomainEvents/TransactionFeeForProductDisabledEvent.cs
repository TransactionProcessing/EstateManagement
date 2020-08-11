using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.Contract.DomainEvents
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    public class TransactionFeeForProductDisabledEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductDisabledEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TransactionFeeForProductDisabledEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFeeForProductDisabledEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        private TransactionFeeForProductDisabledEvent(Guid aggregateId,
                                                      Guid eventId,
                                                      Guid estateId,
                                                      Guid productId,
                                                      Guid transactionFeeId) : base(aggregateId, eventId)
        {
            this.ContractId = aggregateId;
            this.EstateId = estateId;
            this.ProductId = productId;
            this.TransactionFeeId = transactionFeeId;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        [JsonProperty]
        public Guid ContractId { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty]
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the transaction fee identifier.
        /// </summary>
        /// <value>
        /// The transaction fee identifier.
        /// </value>
        [JsonProperty]
        public Guid TransactionFeeId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="transactionFeeId">The transaction fee identifier.</param>
        /// <returns></returns>
        public static TransactionFeeForProductDisabledEvent Create(Guid aggregateId,
                                                                   Guid estateId,
                                                                   Guid productId,
                                                                   Guid transactionFeeId)
        {
            return new TransactionFeeForProductDisabledEvent(aggregateId, Guid.NewGuid(), estateId, productId, transactionFeeId);
        }

        #endregion
    }
}

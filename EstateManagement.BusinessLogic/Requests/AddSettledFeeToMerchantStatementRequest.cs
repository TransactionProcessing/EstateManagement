namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    public class AddSettledFeeToMerchantStatementRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTransactionToMerchantStatementRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settledDateTime">The settled date time.</param>
        /// <param name="settledAmount">The settled amount.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="settledFeeId">The settled fee identifier.</param>
        private AddSettledFeeToMerchantStatementRequest(Guid estateId,
                                                        Guid merchantId, 
                                                        DateTime settledDateTime, 
                                                        Decimal settledAmount, 
                                                        Guid transactionId,
                                                        Guid settledFeeId)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.SettledDateTime = settledDateTime;
            this.SettledAmount = settledAmount;
            this.TransactionId = transactionId;
            this.SettledFeeId = settledFeeId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the settled fee identifier.
        /// </summary>
        /// <value>
        /// The settled fee identifier.
        /// </value>
        public Guid SettledFeeId { get; }
        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; }

        /// <summary>
        /// Gets the settled date time.
        /// </summary>
        /// <value>
        /// The settled date time.
        /// </value>
        public DateTime SettledDateTime { get; }

        /// <summary>
        /// Gets the settled amount.
        /// </summary>
        /// <value>
        /// The settled amount.
        /// </value>
        public Decimal SettledAmount { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; }

        #endregion

        #region Methods


        /// <summary>
        /// Creates the specified merchant statement identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="settledDateTime">The settled date time.</param>
        /// <param name="settledAmount">The settled amount.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="settledFeeId">The settled fee identifier.</param>
        /// <returns></returns>
        public static AddSettledFeeToMerchantStatementRequest Create(Guid estateId,
                                                                     Guid merchantId,
                                                                     DateTime settledDateTime,
                                                                     Decimal settledAmount,
                                                                     Guid transactionId,
                                                                     Guid settledFeeId)
        {
            return new AddSettledFeeToMerchantStatementRequest(estateId, merchantId, settledDateTime,settledAmount,transactionId,settledFeeId);
        }

        #endregion

    }
}
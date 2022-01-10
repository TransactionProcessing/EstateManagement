namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    public class AddTransactionToMerchantStatementRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTransactionToMerchantStatementRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transactionDateTime">The transaction date time.</param>
        /// <param name="transactionAmount">The transaction amount.</param>
        /// <param name="isAuthorised">if set to <c>true</c> [is authorised].</param>
        /// <param name="transactionId">The transaction identifier.</param>
        private AddTransactionToMerchantStatementRequest(Guid estateId,
                                                         Guid merchantId, 
                                                         DateTime transactionDateTime, 
                                                         Decimal? transactionAmount, 
                                                         Boolean isAuthorised,
                                                         Guid transactionId)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.TransactionDateTime = transactionDateTime;
            this.TransactionAmount = transactionAmount;
            this.TransactionId = transactionId;
            this.IsAuthorised = isAuthorised;
        }

        #endregion

        #region Properties
        
        public Guid TransactionId { get; }

        public DateTime TransactionDateTime { get; }

        public Decimal? TransactionAmount { get; }

        public Boolean IsAuthorised { get; }

        public Guid EstateId { get; }

        public Guid MerchantId { get; }

        #endregion

        #region Methods


        /// <summary>
        /// Creates the specified merchant statement identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="transactionDateTime">The transaction date time.</param>
        /// <param name="transactionAmount">The transaction amount.</param>
        /// <param name="isAuthorised">if set to <c>true</c> [is authorised].</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns></returns>
        public static AddTransactionToMerchantStatementRequest Create(Guid estateId,
                                                                      Guid merchantId, 
                                                                      DateTime transactionDateTime, 
                                                                      Decimal? transactionAmount,
                                                                      Boolean isAuthorised,
                                                                      Guid transactionId)
        {
            return new AddTransactionToMerchantStatementRequest(estateId, merchantId, transactionDateTime, transactionAmount, isAuthorised, transactionId);
        }

        #endregion

    }
}
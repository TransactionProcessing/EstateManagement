namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.Guid}" />
    public class MakeMerchantDepositRequest : IRequest<Guid>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MakeMerchantDepositRequest"/> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        public MakeMerchantDepositRequest(Guid estateId,
                                          Guid merchantId,
                                          MerchantDepositSource source,
                                          String reference,
                                          DateTime depositDateTime,
                                          Decimal amount)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.Source = source;
            this.Reference = reference;
            this.DepositDateTime = depositDateTime;
            this.Amount = amount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public Decimal Amount { get; }

        /// <summary>
        /// Gets the deposit date time.
        /// </summary>
        /// <value>
        /// The deposit date time.
        /// </value>
        public DateTime DepositDateTime { get; }

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

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public String Reference { get; }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public MerchantDepositSource Source { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="depositDateTime">The deposit date time.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static MakeMerchantDepositRequest Create(Guid estateId,
                                                        Guid merchantId,
                                                        MerchantDepositSource source,
                                                        String reference,
                                                        DateTime depositDateTime,
                                                        Decimal amount)
        {
            return new MakeMerchantDepositRequest(estateId, merchantId, source, reference, depositDateTime, amount);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    public class EmailMerchantStatementRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignOperatorToMerchantRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="merchantStatementId">The merchant statement identifier.</param>
        private EmailMerchantStatementRequest(Guid estateId,
                                              Guid merchantId,
                                              Guid merchantStatementId)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.MerchantStatementId = merchantStatementId;
        }

        #endregion

        #region Properties
        
        public Guid MerchantStatementId { get; }

        public Guid EventId { get; }

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
        /// <param name="merchantStatementId">The merchant statement identifier.</param>
        /// <returns></returns>
        public static EmailMerchantStatementRequest Create(Guid estateId,
                                                           Guid merchantId,
                                                           Guid merchantStatementId)
        {
            return new EmailMerchantStatementRequest(estateId, merchantId, merchantStatementId);
        }

        #endregion
    }
}

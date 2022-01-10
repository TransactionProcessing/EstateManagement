using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    public class AssignOperatorToMerchantRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignOperatorToMerchantRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        private AssignOperatorToMerchantRequest(Guid estateId,
                                                Guid merchantId,
                                                Guid operatorId,
                                                String merchantNumber,
                                                String terminalNumber)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.OperatorId = operatorId;
            this.MerchantNumber = merchantNumber;
            this.TerminalNumber = terminalNumber;
        }

        #endregion

        #region Properties

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
        /// Gets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        public String MerchantNumber { get; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; }

        /// <summary>
        /// Gets the terminal number.
        /// </summary>
        /// <value>
        /// The terminal number.
        /// </value>
        public String TerminalNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        /// <returns></returns>
        public static AssignOperatorToMerchantRequest Create(Guid estateId,
                                                             Guid merchantId,
                                                             Guid operatorId,
                                                             String merchantNumber,
                                                             String terminalNumber)
        {
            return new AssignOperatorToMerchantRequest(estateId, merchantId, operatorId, merchantNumber, terminalNumber);
        }

        #endregion
    }
}

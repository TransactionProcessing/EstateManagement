using System;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    public class AddOperatorToEstateRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOperatorToEstateRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        private AddOperatorToEstateRequest(Guid estateId,
                                           Guid operatorId,
                                           String name,
                                           Boolean requireCustomMerchantNumber,
                                           Boolean requireCustomTerminalNumber)
        {
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.RequireCustomMerchantNumber = requireCustomMerchantNumber;
            this.RequireCustomTerminalNumber = requireCustomTerminalNumber;
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
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; }

        /// <summary>
        /// Gets a value indicating whether [require custom merchant number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom merchant number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomMerchantNumber { get; }

        /// <summary>
        /// Gets a value indicating whether [require custom terminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom terminal number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomTerminalNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <returns></returns>
        public static AddOperatorToEstateRequest Create(Guid estateId,
                                                        Guid operatorId,
                                                        String name,
                                                        Boolean requireCustomMerchantNumber,
                                                        Boolean requireCustomTerminalNumber)
        {
            return new AddOperatorToEstateRequest(estateId, operatorId, name, requireCustomMerchantNumber, requireCustomTerminalNumber);
        }

        #endregion
    }
}

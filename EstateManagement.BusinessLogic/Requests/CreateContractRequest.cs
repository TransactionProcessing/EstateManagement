namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.String}" />
    public class CreateContractRequest : IRequest<Unit>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContractRequest" /> class.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        private CreateContractRequest(Guid contractId,
                                      Guid estateId,
                                      Guid operatorId,
                                      String description)
        {
            this.ContractId = contractId;
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; }

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static CreateContractRequest Create(Guid contractId, 
                                                   Guid estateId,
                                                   Guid operatorId,
                                                   String description)
        {
            return new CreateContractRequest(contractId, estateId, operatorId, description);
        }

        #endregion
    }
}
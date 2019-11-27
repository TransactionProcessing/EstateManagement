namespace EstateManagement.BusinessLogic.Commands
{
    using System;
    using Shared.DomainDrivenDesign.CommandHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.Command{System.String}" />
    public class AssignOperatorToMerchantCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignOperatorToMerchantCommand"/> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        /// <param name="commandId">The command identifier.</param>
        private AssignOperatorToMerchantCommand(Guid estateId,
                                                Guid merchantId,
                                                Guid operatorId,
                                                String merchantNumber,
                                                String terminalNumber,
                                                Guid commandId) : base(commandId)
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
        public static AssignOperatorToMerchantCommand Create(Guid estateId,
                                                             Guid merchantId,
                                                             Guid operatorId,
                                                             String merchantNumber,
                                                             String terminalNumber)
        {
            return new AssignOperatorToMerchantCommand(estateId, merchantId, operatorId, merchantNumber, terminalNumber, Guid.NewGuid());
        }

        #endregion
    }
}
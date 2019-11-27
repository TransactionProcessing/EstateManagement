namespace EstateManagement.MerchantAggregate
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class Operator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        private Operator(Guid operatorId,
                         String name,
                         String merchantNumber,
                         String terminalNumber)
        {
            this.OperatorId = operatorId;
            this.Name = name;
            this.MerchantNumber = merchantNumber;
            this.TerminalNumber = terminalNumber;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        internal String MerchantNumber { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        internal String Name { get; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        internal Guid OperatorId { get; }

        /// <summary>
        /// Gets the terminal number.
        /// </summary>
        /// <value>
        /// The terminal number.
        /// </value>
        internal String TerminalNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="merchantNumber">The merchant number.</param>
        /// <param name="terminalNumber">The terminal number.</param>
        /// <returns></returns>
        internal static Operator Create(Guid operatorId,
                                        String name,
                                        String merchantNumber,
                                        String terminalNumber)
        {
            return new Operator(operatorId, name, merchantNumber, terminalNumber);
        }

        #endregion
    }
}
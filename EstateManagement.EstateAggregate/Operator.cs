namespace EstateManagement.EstateAggregate
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
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomterminalNumber">if set to <c>true</c> [require customterminal number].</param>
        private Operator(Guid operatorId,
                         String name,
                         Boolean requireCustomMerchantNumber,
                         Boolean requireCustomterminalNumber)
        {
            this.OperatorId = operatorId;
            this.Name = name;
            this.RequireCustomMerchantNumber = requireCustomMerchantNumber;
            this.RequireCustomterminalNumber = requireCustomterminalNumber;
        }

        #endregion

        #region Properties

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
        /// Gets a value indicating whether [require customterminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require customterminal number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomterminalNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <returns></returns>
        internal static Operator Create(Guid operatorId,
                                        String name,
                                        Boolean requireCustomMerchantNumber,
                                        Boolean requireCustomTerminalNumber)
        {
            return new Operator(operatorId, name, requireCustomMerchantNumber, requireCustomTerminalNumber);
        }

        #endregion
    }
}
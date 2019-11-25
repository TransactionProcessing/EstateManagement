using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.BusinessLogic.Commands
{
    using Shared.DomainDrivenDesign.CommandHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.CommandHandling.Command{System.String}" />
    public class AddOperatorToEstateCommand : Command<String>
    {
        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [require custom merchant number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom merchant number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomMerchantNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [require custom terminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom terminal number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomTerminalNumber { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEstateCommand" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        /// <param name="commandId">The command identifier.</param>
        private AddOperatorToEstateCommand(Guid estateId, 
                                      Guid operatorId,
                                    String name,
                                    Boolean requireCustomMerchantNumber,
                                    Boolean requireCustomTerminalNumber,
                                    Guid commandId) : base(commandId)
        {
            this.EstateId = estateId;
            this.OperatorId = operatorId;
            this.Name = name;
            this.RequireCustomMerchantNumber = requireCustomMerchantNumber;
            this.RequireCustomTerminalNumber = requireCustomTerminalNumber;
        }

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
        public static AddOperatorToEstateCommand Create(Guid estateId,
                                                   Guid operatorId,
                                                   String name,
                                                   Boolean requireCustomMerchantNumber,
                                                   Boolean requireCustomTerminalNumber)
        {
            return new AddOperatorToEstateCommand(estateId, operatorId, name, requireCustomMerchantNumber, requireCustomTerminalNumber, Guid.NewGuid());
        }

        #endregion
    }
}

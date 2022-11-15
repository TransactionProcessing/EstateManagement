namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("estateoperator")]
    public class EstateOperator
    {
        #region Properties

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [require custom merchant number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom merchant number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomMerchantNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [require custom terminal number].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require custom terminal number]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }
}
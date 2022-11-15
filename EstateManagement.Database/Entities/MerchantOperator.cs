namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantoperator")]
    public class MerchantOperator
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
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the merchant number.
        /// </summary>
        /// <value>
        /// The merchant number.
        /// </value>
        public String? MerchantNumber { get; set; }

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
        /// Gets or sets the terminal number.
        /// </summary>
        /// <value>
        /// The terminal number.
        /// </value>
        public String? TerminalNumber { get; set; }

        #endregion
    }
}
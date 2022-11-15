namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("contract")]
    public class Contract
    {
        #region Properties

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid OperatorId { get; set; }

        #endregion
    }
}
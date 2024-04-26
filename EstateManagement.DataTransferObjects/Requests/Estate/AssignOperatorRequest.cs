namespace EstateManagement.DataTransferObjects.Requests.Estate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AssignOperatorRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }

        #endregion
    }
}
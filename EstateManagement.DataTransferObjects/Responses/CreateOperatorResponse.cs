using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.DataTransferObjects.Responses
{
    using Newtonsoft.Json;

    public class CreateOperatorResponse
    {
        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty("estate_id")]
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }
    }
}

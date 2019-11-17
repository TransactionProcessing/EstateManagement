namespace EstateManagement.DataTransferObjects.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class CreateEstateRequest
    {
        /// <summary>
        /// Gets or sets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        [Required]
        [JsonProperty("estate_name")]
        public String EstateName { get; set; }
    }
}
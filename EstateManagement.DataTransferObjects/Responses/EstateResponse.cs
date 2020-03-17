using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.DataTransferObjects.Responses
{
    /// <summary>
    /// 
    /// </summary>
    public class EstateResponse
    {
        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        public String EstateName { get; set; }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        public List<OperatorResponse> Operators { get; set; }

        /// <summary>
        /// Gets or sets the security users.
        /// </summary>
        /// <value>
        /// The security users.
        /// </value>
        public List<SecurityUserResponse> SecurityUsers { get; set; }
    }
}

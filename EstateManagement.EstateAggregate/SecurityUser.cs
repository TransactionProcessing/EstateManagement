using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.EstateAggregate
{
    /// <summary>
    /// 
    /// </summary>
    internal class SecurityUser
    {
        /// <summary>
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        internal Guid SecurityUserId { get; }
        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        internal String EmailAddress { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUser"/> class.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        private SecurityUser(Guid securityUserId,String emailAddress)
        {
            this.SecurityUserId = securityUserId;
            this.EmailAddress = emailAddress;
        }

        /// <summary>
        /// Creates the specified security user identifier.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        internal static SecurityUser Create(Guid securityUserId, String emailAddress)
        {
            return new SecurityUser(securityUserId, emailAddress);
        }
    }
}

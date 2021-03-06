﻿namespace EstateManagement.BusinessLogic.Requests
{
    using System;
    using MediatR;

    public class CreateMerchantUserRequest : IRequest<Guid>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEstateUserRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="familyName">Name of the family.</param>
        private CreateMerchantUserRequest(Guid estateId,
                                          Guid merchantId,
                                          String emailAddress,
                                          String password,
                                          String givenName,
                                          String middleName,
                                          String familyName)
        {
            this.EstateId = estateId;
            this.MerchantId = merchantId;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.GivenName = givenName;
            this.MiddleName = middleName;
            this.FamilyName = familyName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public Guid MerchantId { get; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; }

        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>
        /// The name of the family.
        /// </value>
        public String FamilyName { get; }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public String GivenName { get; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public String MiddleName { get; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String Password { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified email address.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="givenName">Name of the given.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="familyName">Name of the family.</param>
        /// <returns></returns>
        public static CreateMerchantUserRequest Create(Guid estateId,
                                                       Guid merchantId,
                                                       String emailAddress,
                                                       String password,
                                                       String givenName,
                                                       String middleName,
                                                       String familyName)
        {
            return new CreateMerchantUserRequest(estateId, merchantId, emailAddress, password, givenName, middleName, familyName);
        }

        #endregion
    }
}
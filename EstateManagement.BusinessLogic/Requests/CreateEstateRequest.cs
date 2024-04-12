using System;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    public class CreateEstateRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEstateRequest" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="name">The name.</param>
        private CreateEstateRequest(Guid estateId,
                                    String name)
        {
            this.EstateId = estateId;
            this.Name = name;
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
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static CreateEstateRequest Create(Guid estateId,
                                                 String name)
        {
            return new CreateEstateRequest(estateId, name);
        }

        #endregion
    }
}

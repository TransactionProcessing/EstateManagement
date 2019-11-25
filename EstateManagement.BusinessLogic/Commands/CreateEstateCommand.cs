namespace EstateManagement.BusinessLogic.Commands
{
    using System;
    using Shared.DomainDrivenDesign.CommandHandling;

    public class CreateEstateCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEstateCommand" /> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateEstateCommand(Guid estateId,
                                      String name,
                                      Guid commandId) : base(commandId)
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
        public static CreateEstateCommand Create(Guid estateId,
                                                   String name)
        {
            return new CreateEstateCommand(estateId, name, Guid.NewGuid());
        }

        #endregion
    }
}
namespace EstateManagement.EstateAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Estate.DomainEvents;
    using Models;
    using Models.Estate;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.DomainDrivenDesign.EventStore;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventStore.Aggregate" />
    public class EstateAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The operators
        /// </summary>
        private readonly List<Operator> Operators;

        /// <summary>
        /// The security users
        /// </summary>
        private readonly List<SecurityUser> SecurityUsers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public EstateAggregate()
        {
            // Nothing here
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private EstateAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Operators = new List<Operator>();
            this.SecurityUsers = new List<SecurityUser>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        public String EstateName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsCreated { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="requireCustomMerchantNumber">if set to <c>true</c> [require custom merchant number].</param>
        /// <param name="requireCustomTerminalNumber">if set to <c>true</c> [require custom terminal number].</param>
        public void AddOperator(Guid operatorId,
                                String operatorName,
                                Boolean requireCustomMerchantNumber,
                                Boolean requireCustomTerminalNumber)
        {
            Guard.ThrowIfNullOrEmpty(operatorName, typeof(ArgumentNullException), "Operator name must be provided when adding a new operator");

            this.CheckEstateHasBeenCreated();
            this.CheckOperatorHasNotAlreadyBeenCreated(operatorId, operatorName);

            OperatorAddedToEstateEvent operatorAddedToEstateEvent =
                OperatorAddedToEstateEvent.Create(this.AggregateId, operatorId, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);

            this.ApplyAndPend(operatorAddedToEstateEvent);
        }

        /// <summary>
        /// Adds the security user.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="emailAddress">The email address.</param>
        public void AddSecurityUser(Guid securityUserId,
                                    String emailAddress)
        {
            this.CheckEstateHasBeenCreated();

            SecurityUserAddedEvent securityUserAddedEvent = SecurityUserAddedEvent.Create(this.AggregateId, securityUserId, emailAddress);

            this.ApplyAndPend(securityUserAddedEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static EstateAggregate Create(Guid aggregateId)
        {
            return new EstateAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the specified estate name.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        public void Create(String estateName)
        {
            Guard.ThrowIfNullOrEmpty(estateName, typeof(ArgumentNullException), "Estate name must be provided when registering a new estate");

            this.CheckEstateHasNotAlreadyBeenCreated();

            EstateCreatedEvent estateCreatedEvent = EstateCreatedEvent.Create(this.AggregateId, estateName);

            this.ApplyAndPend(estateCreatedEvent);
        }

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <returns></returns>
        public Estate GetEstate()
        {
            Estate estateModel = new Estate();

            estateModel.EstateId = this.AggregateId;
            estateModel.Name = this.EstateName;

            if (this.Operators.Any())
            {
                estateModel.Operators = new List<Models.Estate.Operator>();

                foreach (Operator @operator in this.Operators)
                {
                    estateModel.Operators.Add(new Models.Estate.Operator
                                              {
                                                  OperatorId = @operator.OperatorId,
                                                  Name = @operator.Name,
                                                  RequireCustomMerchantNumber = @operator.RequireCustomMerchantNumber,
                                                  RequireCustomTerminalNumber = @operator.RequireCustomterminalNumber
                                              });
                }
            }

            if (this.SecurityUsers.Any())
            {
                estateModel.SecurityUsers = new List<Models.SecurityUser>();

                foreach (SecurityUser securityUser in this.SecurityUsers)
                {
                    estateModel.SecurityUsers.Add(new Models.SecurityUser
                                                  {
                                                      EmailAddress = securityUser.EmailAddress,
                                                      SecurityUserId = securityUser.SecurityUserId
                                                  });
                }
            }

            return estateModel;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       EstateId = this.AggregateId
                   };
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        private void CheckEstateHasBeenCreated()
        {
            if (this.IsCreated == false)
            {
                throw new InvalidOperationException("Estate has not been created");
            }
        }

        /// <summary>
        /// Checks the estate has not already been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">Estate with name {this.EstateName} has already been created</exception>
        private void CheckEstateHasNotAlreadyBeenCreated()
        {
            if (this.IsCreated)
            {
                throw new InvalidOperationException($"Estate with name {this.EstateName} has already been created");
            }
        }

        /// <summary>
        /// Checks the operator has not already been created.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Duplicate operator details are not allowed, an operator already exists on this estate with Id [{operatorId}]
        /// or
        /// Duplicate operator details are not allowed, an operator already exists on this estate with Name [{operatorName}]
        /// </exception>
        private void CheckOperatorHasNotAlreadyBeenCreated(Guid operatorId,
                                                           String operatorName)
        {
            Operator operatorWithId = this.Operators.SingleOrDefault(o => o.OperatorId == operatorId);

            if (operatorWithId != null)
            {
                throw new InvalidOperationException($"Duplicate operator details are not allowed, an operator already exists on this estate with Id [{operatorId}]");
            }

            Operator operatorWithName = this.Operators.SingleOrDefault(o => o.Name == operatorName);

            if (operatorWithName != null)
            {
                throw new InvalidOperationException($"Duplicate operator details are not allowed, an operator already exists on this estate with Name [{operatorName}]");
            }
        }

        private void PlayEvent(SecurityUserAddedEvent domainEvent)
        {
            SecurityUser securityUser = SecurityUser.Create(domainEvent.SecurityUserId, domainEvent.EmailAddress);

            this.SecurityUsers.Add(securityUser);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(EstateCreatedEvent domainEvent)
        {
            this.EstateName = domainEvent.EstateName;
            this.IsCreated = true;
        }

        /// <summary>
        /// Operators the added to estate event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(OperatorAddedToEstateEvent domainEvent)
        {
            Operator @operator = Operator.Create(domainEvent.OperatorId,
                                                 domainEvent.Name,
                                                 domainEvent.RequireCustomMerchantNumber,
                                                 domainEvent.RequireCustomMerchantNumber);

            this.Operators.Add(@operator);
        }

        #endregion
    }
}